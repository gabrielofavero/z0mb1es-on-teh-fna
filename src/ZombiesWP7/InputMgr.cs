using System;
using IMAK3Z0MB1EGAEM;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.character;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Viking_x86;
using Viking_x86.character;
using Viking_x86.director;
using Yuki_Win;
using ZombiesWP7.debug;

namespace ZombiesWP7;

/// <summary>
/// Multi-source input aggregator.
/// Reads touch, gamepad, keyboard, and mouse every frame and exposes unified state.
/// Touch is first-class — all existing touch logic is preserved.
/// Gamepad, keyboard, and mouse are added as parallel input sources.
/// Priority: touch (when active) > gamepad > keyboard/mouse.
/// </summary>
public class InputMgr
{
	public enum ActiveInputSource
	{
		None,
		Touch,
		Mouse,
		Keyboard,
		Gamepad
	}

	private const float BUFFER = 120f;
	private const float INPUT_DEADZONE = 0.15f;
	private const float SCROLL_SPEED = 8f;

	private const int MOVE = 0;
	private const int SHOOT = 1;

	// Previous-frame state for edge detection
	private static TouchCollection p;
	private static GamePadState prevGamePad;
	private static MouseState prevMouse;
	private static KeyboardState prevKeyboard;
	private static int prevScrollWheel;

	// --- Active input source tracking ---
	private static ActiveInputSource _activeSource = ActiveInputSource.None;
	private static float _nonTouchIdleTimer = 0f;
	private const float AUTO_SHOW_TOUCH_DELAY = 3f; // seconds before touch overlays reappear after non-touch input stops

	/// <summary>The most recently used input source.</summary>
	public static ActiveInputSource ActiveSource => _activeSource;

	/// <summary>
	/// True when mouse, keyboard, or gamepad is being actively used.
	/// When true, touch overlays should be hidden.
	/// </summary>
	public static bool IsNonTouchActive => _nonTouchIdleTimer < AUTO_SHOW_TOUCH_DELAY &&
		(_activeSource == ActiveInputSource.Mouse ||
		 _activeSource == ActiveInputSource.Keyboard ||
		 _activeSource == ActiveInputSource.Gamepad);

	/// <summary>Current mouse position in render-target (600×360) coordinates (for menu cursor rendering).</summary>
	public static Vector2 MousePosition { get; private set; }

	// --- Public unified input state ---

	/// <summary>Movement direction (left stick). Normalized with magnitude 0–1.</summary>
	public static Vector2 lVec;

	/// <summary>Aim/shoot direction (right stick). Normalized with magnitude 0–1.</summary>
	public static Vector2 rVec;

	/// <summary>Scroll/swipe delta for this frame. Y component used by menus.</summary>
	public static Vector2 swipe;

	/// <summary>True while a drag/swipe is in progress.</summary>
	public static bool swiping;

	/// <summary>Current drag position (screen coords).</summary>
	public static Vector2 dragVec;

	/// <summary>True when a confirm action (tap/click/A/Enter) occurred this frame.</summary>
	private static bool isClick;

	/// <summary>Screen position of the confirm action.</summary>
	private static Vector2 clickVec;

	/// <summary>True when back/cancel was pressed this frame (Back, B, Escape).</summary>
	public static bool isBack;

	/// <summary>True when pause was pressed this frame (Start, Escape in-game).</summary>
	public static bool isPause;

	// --- Keyboard/Controller navigation state (edge-triggered, for discrete menu steps) ---
	/// <summary>True this frame when Up was pressed (keyboard Up, D-pad Up, or left stick up &gt; 0.5).</summary>
	public static bool NavUpPressed { get; private set; }
	/// <summary>True this frame when Down was pressed.</summary>
	public static bool NavDownPressed { get; private set; }
	/// <summary>True this frame when Left was pressed.</summary>
	public static bool NavLeftPressed { get; private set; }
	/// <summary>True this frame when Right was pressed.</summary>
	public static bool NavRightPressed { get; private set; }

	/// <summary>True while Up is held (keyboard, D-pad, or left stick).</summary>
	public static bool NavUpHeld { get; private set; }
	/// <summary>True while Down is held.</summary>
	public static bool NavDownHeld { get; private set; }
	/// <summary>True while Left is held.</summary>
	public static bool NavLeftHeld { get; private set; }
	/// <summary>True while Right is held.</summary>
	public static bool NavRightHeld { get; private set; }

	/// <summary>
	/// True when the confirm action came from a non-touch source (gamepad A / keyboard Enter/Space).
	/// Menu code should use this to decide whether to invoke the highlighted item directly
	/// instead of doing a hit-test against clickVec.
	/// </summary>
	public static bool IsNonTouchConfirm { get; private set; }

	// --- Touch-specific state (preserved for thumbpad rendering) ---

	private static Vector2 L_CENTER = new Vector2(120f, 360f);
	private static Vector2 R_CENTER = new Vector2(680f, 360f);

	private static bool lOrig;
	private static bool rOrig;

	private static Vector2 lDrawVec;
	private static Vector2 rDrawVec;

	private static Vector2 autoShootVec;

	private static string[] msStr = new string[2] { "M0VE!", "SH00T!" };
	private static string PAUSE = "¾";

	public static bool IsClick()
	{
		return isClick;
	}

	public static Vector2 GetClickVec()
	{
		return clickVec;
	}

	private static void DrawThumbpad(SpriteBatch sprite, Vector2 loc, float b, float a, int idx, bool clicked, Vector2 mVec)
	{
		a = (clicked ? (a * 0.65f) : (a * 0.5f));
		sprite.Draw(Game1.thumpadTex[clicked ? 1 : 0], loc, new Rectangle(0, 0, 128, 128), new Color(b, b, b, a), clicked ? (Trig.GetAngle(new Vector2(mVec.X, mVec.Y), default(Vector2)) + (float)Math.PI / 4f) : 0f, new Vector2(64f, 64f), 2f * Game1.VIEWSCALE, SpriteEffects.None, 1f);
		if (!clicked)
		{
			Text.DrawString(sprite, msStr[idx], loc, 5f, new Color(b, b, b, a), Text.Justify.Center);
		}
	}

	public static void DrawThumbpads(SpriteBatch sprite, float a)
	{
		// Hide touch overlays when mouse/keyboard/controller is actively used
		if (!Game1.player.settings.showTouchPads || IsNonTouchActive)
		{
			return;
		}
		a *= 0.5f;
		float num = 1f;
		Text.DrawString(sprite, PAUSE, new Vector2(ScrollMan.screenSize.X - 40f, 30f), 6f, new Color(1f, 1f, 1f, 1f), Text.Justify.Center);
		if (Game1.player.settings.relativeControls)
		{
			if (lOrig)
			{
				DrawThumbpad(sprite, L_CENTER, num, a, 0, lDrawVec.X > -1f, lVec);
			}
			if (rOrig)
			{
				DrawThumbpad(sprite, R_CENTER, num, a, 1, rDrawVec.X > -1f, rVec);
			}
		}
		else
		{
			DrawThumbpad(sprite, L_CENTER, num, a, 0, lDrawVec.X > -1f, lVec);
			DrawThumbpad(sprite, R_CENTER, num, a, 1, rDrawVec.X > -1f, rVec);
		}
		float num2 = 0.75f;
		Vector2 origin = new Vector2(64f, 64f) + new Vector2(4f, 4f) * new Vector2(-1f, 1f);
		if (lDrawVec.X > -1f)
		{
			sprite.Draw(Game1.thumclickTex, lDrawVec, new Rectangle(0, 0, 128, 128), new Color(num, num, num, a * 0.5f), Trig.GetAngle(new Vector2(lVec.X, lVec.Y), default(Vector2)) + (float)Math.PI / 4f, origin, num2 * Game1.VIEWSCALE, SpriteEffects.None, 1f);
		}
		if (rDrawVec.X > -1f)
		{
			sprite.Draw(Game1.thumclickTex, rDrawVec, new Rectangle(0, 0, 128, 128), new Color(num, num, num, a * 0.5f), Trig.GetAngle(new Vector2(rVec.X, rVec.Y), default(Vector2)) + (float)Math.PI / 4f, origin, num2 * Game1.VIEWSCALE, SpriteEffects.None, 1f);
		}
	}

	public static void Update()
	{
		// --- Read all input sources once ---
		GamePadState gs = GamePad.GetState(PlayerIndex.One);
		KeyboardState ks = Keyboard.GetState();
		MouseState ms = Mouse.GetState();
		TouchCollection touchState = TouchPanel.GetState();

		// --- Track mouse position for cursor (scale from backbuffer to VIEWPORT space, accounting for letterbox) ---
		MousePosition = new Vector2((ms.X - Game1.DestOffsetX) / Game1.RenderScale, (ms.Y - Game1.DestOffsetY) / Game1.RenderScale);

		// --- Reset per-frame state ---
		bool relativeControls = Game1.player.settings.relativeControls;
		lDrawVec.X = -1f;
		rDrawVec.X = -1f;
		if (!relativeControls)
		{
			L_CENTER = new Vector2(120f * Game1.VIEWSCALE, Game1.VIEWPORT.Y - 120f * Game1.VIEWSCALE);
			R_CENTER = new Vector2(Game1.VIEWPORT.X - 120f * Game1.VIEWSCALE, Game1.VIEWPORT.Y - 120f * Game1.VIEWSCALE);
		}
		isClick = false;
		clickVec = default(Vector2);
		swipe = default(Vector2);
		swiping = false;
		dragVec = default(Vector2);
		lVec = default(Vector2);
		rVec = default(Vector2);
		isBack = false;
		isPause = false;
		NavUpPressed = false;
		NavDownPressed = false;
		NavLeftPressed = false;
		NavRightPressed = false;
		NavUpHeld = false;
		NavDownHeld = false;
		NavLeftHeld = false;
		NavRightHeld = false;
		IsNonTouchConfirm = false;

		// =====================================================================
		// ACTIVE INPUT SOURCE DETECTION
		// =====================================================================
		bool mouseActive = ms.LeftButton == ButtonState.Pressed ||
			ms.RightButton == ButtonState.Pressed ||
			ms.MiddleButton == ButtonState.Pressed ||
			ms.ScrollWheelValue != prevScrollWheel ||
			(ms.X != prevMouse.X || ms.Y != prevMouse.Y);

		bool keyboardActive = ks.GetPressedKeys().Length > 0;

		bool gamepadActive = gs.ThumbSticks.Left.Length() > INPUT_DEADZONE ||
			gs.ThumbSticks.Right.Length() > INPUT_DEADZONE ||
			gs.Buttons.A == ButtonState.Pressed ||
			gs.Buttons.B == ButtonState.Pressed ||
			gs.Buttons.X == ButtonState.Pressed ||
			gs.Buttons.Y == ButtonState.Pressed ||
			gs.Buttons.Start == ButtonState.Pressed ||
			gs.Buttons.Back == ButtonState.Pressed ||
			gs.DPad.Up == ButtonState.Pressed ||
			gs.DPad.Down == ButtonState.Pressed ||
			gs.DPad.Left == ButtonState.Pressed ||
			gs.DPad.Right == ButtonState.Pressed;

		bool touchActive = touchState.Count > 0;

		// Determine active source (last-used wins)
		if (touchActive)
			_activeSource = ActiveInputSource.Touch;
		else if (gamepadActive)
			_activeSource = ActiveInputSource.Gamepad;
		else if (mouseActive)
			_activeSource = ActiveInputSource.Mouse;
		else if (keyboardActive)
			_activeSource = ActiveInputSource.Keyboard;

		// Manage non-touch idle timer for auto-showing touch overlays
		if (mouseActive || keyboardActive || gamepadActive)
		{
			_nonTouchIdleTimer = 0f;
		}
		else
		{
			_nonTouchIdleTimer += Game1.frameTime;
		}

		// =====================================================================
		// BACK / CANCEL — GamePad.Back, GamePad.B, Keyboard Escape
		// =====================================================================
		// Back button must be edge-triggered to avoid sound spam when held during gameplay.
		bool backEdge = (gs.Buttons.Back == ButtonState.Pressed && prevGamePad.Buttons.Back == ButtonState.Released);
		bool bEdge = (gs.Buttons.B == ButtonState.Pressed && prevGamePad.Buttons.B == ButtonState.Released);
		bool escEdge = (ks.IsKeyDown(Keys.Escape) && prevKeyboard.IsKeyUp(Keys.Escape));

		if (backEdge || bEdge || escEdge)
		{
			if (backEdge && Game1.tUTest)
			{
				// Original debug shortcut — only on physical Back button
				Game1.debugNeedsTitleUpdate = true;
				Game1.tUTest = false;
			}
			else
			{
				isBack = true;
				Sound.Play("back");
			}
		}

		// =====================================================================
		// PAUSE / UNPAUSE — GamePad.Start
		// =====================================================================
		if (gs.Buttons.Start == ButtonState.Pressed && prevGamePad.Buttons.Start == ButtonState.Released)
		{
			Sound.Play("back");

			// If already paused, unpause instead
			if (Menu.menuLevel[2].active && TimeMgr.CurTMgr().playMode == BaseTimeMgr.PlayMode.Paused)
			{
				Menu.menuLevel[2].active = false;
				TimeMgr.CurTMgr().UnPause();
			}
			else
			{
				isPause = true;
				switch (GameState.state)
				{
				case 7:
					if (!Menu.menuLevel[2].active)
					{
						TimeMgr.CurTMgr().Pause(0);
					}
					break;
				case 3:
				case 5:
					if (TimeMgr.CurTMgr().playMode != BaseTimeMgr.PlayMode.Paused)
					{
						TimeMgr.CurTMgr().Pause(0);
					}
					break;
				}
			}
		}

		// =====================================================================
		// TOUCH — Left/Right thumbsticks, swipe, tap (PRESERVED)
		// =====================================================================
		bool touchMoveActive = false;
		bool touchShootActive = false;
		float deadzoneSq = 0f;

		if (touchState.Count > 0)
		{
			// Swipe detection (first touch vs previous frame)
			if (p.Count == touchState.Count && touchState[0].State != TouchLocationState.Released && p[0].State != TouchLocationState.Released)
			{
				swiping = true;
				swipe = touchState[0].Position - p[0].Position;
				dragVec = touchState[0].Position;
			}

			for (int i = 0; i < touchState.Count; i++)
			{
				bool inRange = false;

				if (relativeControls)
				{
					deadzoneSq = 5f * 5f;

					if (touchState[i].State == TouchLocationState.Pressed)
					{
						if (touchState[i].Position.X < Game1.VIEWPORT.X / 2f)
						{
							L_CENTER = touchState[i].Position;
							touchMoveActive = true;
							lOrig = true;
							lDrawVec = touchState[i].Position;
						}
						else
						{
							R_CENTER = touchState[i].Position;
							touchShootActive = true;
							rOrig = true;
							rDrawVec = touchState[i].Position;
						}
					}

					if (touchState[i].State == TouchLocationState.Moved)
					{
						if (touchState[i].Position.X < Game1.VIEWPORT.X / 2f)
						{
							if (lOrig)
							{
								touchMoveActive = true;
								inRange = true;
							}
						}
						else if (rOrig)
						{
							touchShootActive = true;
							inRange = true;
						}
					}
				}
				else if (touchState[i].State != TouchLocationState.Released)
				{
					inRange = true;
				}

				if (!inRange)
					continue;

				// Left thumbstick
				Vector2 vec = touchState[i].Position - L_CENTER;
				float thumbRadius = 170f * Game1.VIEWSCALE;

				if (((relativeControls && touchState[i].Position.X < Game1.VIEWPORT.X / 2f) ||
					 (!relativeControls && vec.LengthSquared() < thumbRadius * thumbRadius)) &&
					vec.LengthSquared() > deadzoneSq)
				{
					float mag = vec.Length() / thumbRadius;
					mag *= 3f;
					if (mag > 1f) mag = 1f;

					lVec = vec;
					lVec.Normalize();
					lVec *= mag;
					lDrawVec = touchState[i].Position;
					touchMoveActive = true;
				}
				else
				{
					// Right thumbstick
					vec = touchState[i].Position - R_CENTER;
					if (((relativeControls && touchState[i].Position.X >= Game1.VIEWPORT.X / 2f) ||
						 (!relativeControls && vec.LengthSquared() < thumbRadius * thumbRadius)) &&
						vec.LengthSquared() > deadzoneSq)
					{
						rVec = vec;
						rVec.Normalize();
						rDrawVec = touchState[i].Position;
						touchShootActive = true;
					}
				}
			}
		}

		// =====================================================================
		// AUTO-GUN (debug) — preserved
		// =====================================================================
		if (DebugMgr.autoGun)
		{
			int bestIdx = -1;
			float bestDist = 0f;
			CharMan.hero[0].standTime = 0f;

			switch (GameState.state)
			{
			case 3:
			case 7:
			{
				for (int k = 0; k < CharMan.monster.Length; k++)
				{
					if (!CharMan.monster[k].exists)
						continue;

					float dist = (CharMan.monster[k].loc - CharMan.hero[0].loc).Length();
					if (dist < 800f && (bestIdx == -1 || dist < bestDist))
					{
						bestIdx = k;
						bestDist = dist;
					}
				}
				if (bestIdx != -1)
				{
					Vector2 toTarget = CharMan.monster[bestIdx].loc - CharMan.hero[0].loc;
					toTarget.Normalize();
					autoShootVec += (toTarget - autoShootVec) * Game1.frameTime;
					rVec = autoShootVec;
					touchShootActive = true;
					rDrawVec = R_CENTER + rVec * 120f * 0.5f;
				}
				else
				{
					rVec = default(Vector2);
					touchShootActive = false;
				}
				break;
			}
			case 5:
			{
				Character character = Game1.vgame.charMgr.character[0];
				if (character.lives < 5) character.lives = 5;

				for (int j = 1; j < Game1.vgame.charMgr.character.Length; j++)
				{
					Character other = Game1.vgame.charMgr.character[j];
					if (!other.exists || other.team != 1)
						continue;

					float dist = (other.loc - character.loc).Length();
					if (dist < 800f && (bestIdx == -1 || dist < bestDist))
					{
						bestIdx = j;
						bestDist = dist;
					}
				}
				if (bestIdx != -1)
				{
					Vector2 toTarget = Game1.vgame.charMgr.character[bestIdx].loc - character.loc;
					toTarget.Normalize();
					autoShootVec += (toTarget - autoShootVec) * Game1.frameTime;
					rVec = autoShootVec;
					touchShootActive = true;
					rDrawVec = R_CENTER + rVec * 120f * 0.5f;
				}
				else
				{
					rVec = default(Vector2);
					touchShootActive = false;
				}
				break;
			}
			}
		}

		// =====================================================================
		// GAMEPAD / KEYBOARD MOVEMENT — when touch left is not active
		// =====================================================================
		if (!touchMoveActive)
		{
			// Gamepad left stick only — movement
			Vector2 stick = gs.ThumbSticks.Left;
			stick.Y = -stick.Y; // invert: gamepad +Y=up, screen +Y=down
			if (stick.Length() > INPUT_DEADZONE)
			{
				float mag = stick.Length();
				if (mag > 1f) mag = 1f;
				lVec = stick;
				lVec.Normalize();
				lVec *= mag;
			}
			else
			{
				// Keyboard WASD / Arrow keys
				Vector2 keyDir = Vector2.Zero;
				if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left))  keyDir.X -= 1f;
				if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right)) keyDir.X += 1f;
				if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up))    keyDir.Y -= 1f;
				if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down))  keyDir.Y += 1f;
				if (keyDir != Vector2.Zero)
				{
					keyDir.Normalize();
					lVec = keyDir;
				}
			}
		}

		// =====================================================================
		// GAMEPAD / MOUSE AIMING — when touch right is not active
		// =====================================================================
		if (!touchShootActive)
		{
			// Gamepad right stick
			Vector2 rightStick = gs.ThumbSticks.Right;
			rightStick.Y = -rightStick.Y; // invert Y
			if (rightStick.Length() > INPUT_DEADZONE)
			{
				rVec = rightStick;
				rVec.Normalize();
			}
			// Mouse: only aim/shoot while left button is held (avoids auto-shooting)
			else if (ms.LeftButton == ButtonState.Pressed)
			{
				Vector2 mouseScreen = new Vector2((ms.X - Game1.DestOffsetX) / Game1.RenderScale, (ms.Y - Game1.DestOffsetY) / Game1.RenderScale);
				Vector2 playerPos = Vector2.Zero;
				Vector2 mouseWorld = Vector2.Zero;
				bool hasPlayer = false;

				// Zombies / Zombies Survival modes (GameState 3, 7)
				if ((GameState.state == 3 || GameState.state == 7) &&
					CharMan.hero.Length > 0 && CharMan.hero[0] != null && CharMan.hero[0].exists)
				{
					playerPos = CharMan.hero[0].loc;
					mouseWorld = ScrollMan.GetRealLoc(mouseScreen, 1f);
					hasPlayer = true;
				}
				// Time Viking mode (GameState 5) — uses VScroll with rotation
				else if (GameState.state == 5 &&
					Game1.vgame != null && Game1.vgame.charMgr != null &&
					Game1.vgame.charMgr.character.Length > 0 &&
					Game1.vgame.charMgr.character[0] != null &&
					Game1.vgame.charMgr.character[0].exists)
				{
					playerPos = Game1.vgame.charMgr.character[0].loc;
					mouseWorld = VScroll.GetRealLoc(mouseScreen, 1f);
					hasPlayer = true;
				}

				if (hasPlayer)
				{
					Vector2 aimDir = mouseWorld - playerPos;
					if (aimDir.LengthSquared() > 4f) // small deadzone to avoid jitter
					{
						aimDir.Normalize();
						rVec = aimDir;
					}
				}
			}
		}

		// =====================================================================
		// CONFIRM / CLICK
		// =====================================================================
		// Touch tap (existing)
		if (TouchPanel.IsGestureAvailable)
		{
			GestureSample gestureSample = TouchPanel.ReadGesture();
			isClick = true;
			clickVec = gestureSample.Position;

			// In-game pause button tap handling
			switch (GameState.state)
			{
			case 7:
			{
				Vector2 dist = clickVec - new Vector2(ScrollMan.screenSize.X - 40f, 30f);
				if (dist.X > -30f && dist.X < 30f && dist.Y > -30f && dist.Y < 30f && !Menu.menuLevel[2].active)
				{
					TimeMgr.CurTMgr().Pause(0);
				}
				break;
			}
			case 3:
			case 5:
			{
				Vector2 dist = clickVec - new Vector2(ScrollMan.screenSize.X - 40f, 30f);
				if (dist.X > -30f && dist.X < 30f && dist.Y > -30f && dist.Y < 30f && TimeMgr.CurTMgr().playMode != BaseTimeMgr.PlayMode.Paused)
				{
					TimeMgr.CurTMgr().Pause(0);
				}
				break;
			}
			}
		}

		// Mouse left click (edge: pressed this frame, not held)
		if (ms.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
		{
			isClick = true;
			clickVec = new Vector2((ms.X - Game1.DestOffsetX) / Game1.RenderScale, (ms.Y - Game1.DestOffsetY) / Game1.RenderScale);

			// In-game pause button mouse-click handling
			switch (GameState.state)
			{
			case 7:
			{
				Vector2 dist = clickVec - new Vector2(ScrollMan.screenSize.X - 40f, 30f);
				if (dist.X > -30f && dist.X < 30f && dist.Y > -30f && dist.Y < 30f && !Menu.menuLevel[2].active)
				{
					TimeMgr.CurTMgr().Pause(0);
				}
				break;
			}
			case 3:
			case 5:
			{
				Vector2 dist = clickVec - new Vector2(ScrollMan.screenSize.X - 40f, 30f);
				if (dist.X > -30f && dist.X < 30f && dist.Y > -30f && dist.Y < 30f && TimeMgr.CurTMgr().playMode != BaseTimeMgr.PlayMode.Paused)
				{
					TimeMgr.CurTMgr().Pause(0);
				}
				break;
			}
			}
		}

		// Gamepad A button (edge-triggered)
		if (gs.Buttons.A == ButtonState.Pressed && prevGamePad.Buttons.A == ButtonState.Released)
		{
			isClick = true;
			IsNonTouchConfirm = true;
			clickVec = ScrollMan.screenSize / 2f; // center of screen for menu hit-test
		}

		// Keyboard Enter / Space (edge-triggered)
		if ((ks.IsKeyDown(Keys.Enter) && prevKeyboard.IsKeyUp(Keys.Enter)) ||
			(ks.IsKeyDown(Keys.Space) && prevKeyboard.IsKeyUp(Keys.Space)))
		{
			isClick = true;
			IsNonTouchConfirm = true;
			clickVec = ScrollMan.screenSize / 2f; // center of screen for menu hit-test
		}

		// =====================================================================
		// SCROLLING — augment swipe from non-touch sources
		// =====================================================================
		// Mouse scroll wheel
		int wheelDelta = ms.ScrollWheelValue - prevScrollWheel;
		if (wheelDelta != 0)
		{
			swipe.Y += wheelDelta / 120f * SCROLL_SPEED; // each notch = 120 units
		}

		// =====================================================================
		// NAVIGATION DIRECTIONS — unified keyboard/D-pad/left-analog for menus
		// =====================================================================
		{
			// Keyboard: arrow keys + WASD (mirrors game movement bindings)
			bool keyLeft  = ks.IsKeyDown(Keys.Left)  || ks.IsKeyDown(Keys.A);
			bool keyRight = ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D);
			bool keyUp    = ks.IsKeyDown(Keys.Up)    || ks.IsKeyDown(Keys.W);
			bool keyDown  = ks.IsKeyDown(Keys.Down)  || ks.IsKeyDown(Keys.S);

			bool padLeft  = gs.DPad.Left  == ButtonState.Pressed;
			bool padRight = gs.DPad.Right == ButtonState.Pressed;
			bool padUp    = gs.DPad.Up    == ButtonState.Pressed;
			bool padDown  = gs.DPad.Down  == ButtonState.Pressed;

			bool padLB    = gs.Buttons.LeftShoulder  == ButtonState.Pressed;
			bool padRB    = gs.Buttons.RightShoulder == ButtonState.Pressed;

			// Left analog stick as D-pad (threshold 0.5 for deliberate direction)
			float stickX = gs.ThumbSticks.Left.X;
			float stickY = -gs.ThumbSticks.Left.Y; // invert: gamepad +Y=up
			bool stickLeft  = stickX < -0.5f;
			bool stickRight = stickX >  0.5f;
			bool stickUp    = stickY >  0.5f;
			bool stickDown  = stickY < -0.5f;

			bool prevKeyLeft  = prevKeyboard.IsKeyDown(Keys.Left)  || prevKeyboard.IsKeyDown(Keys.A);
			bool prevKeyRight = prevKeyboard.IsKeyDown(Keys.Right) || prevKeyboard.IsKeyDown(Keys.D);
			bool prevKeyUp    = prevKeyboard.IsKeyDown(Keys.Up)    || prevKeyboard.IsKeyDown(Keys.W);
			bool prevKeyDown  = prevKeyboard.IsKeyDown(Keys.Down)  || prevKeyboard.IsKeyDown(Keys.S);

			bool prevPadLeft  = prevGamePad.DPad.Left  == ButtonState.Pressed;
			bool prevPadRight = prevGamePad.DPad.Right == ButtonState.Pressed;
			bool prevPadUp    = prevGamePad.DPad.Up    == ButtonState.Pressed;
			bool prevPadDown  = prevGamePad.DPad.Down  == ButtonState.Pressed;

			bool prevPadLB    = prevGamePad.Buttons.LeftShoulder  == ButtonState.Pressed;
			bool prevPadRB    = prevGamePad.Buttons.RightShoulder == ButtonState.Pressed;

			float prevStickX = prevGamePad.ThumbSticks.Left.X;
			float prevStickY = -prevGamePad.ThumbSticks.Left.Y;
			bool prevStickLeft  = prevStickX < -0.5f;
			bool prevStickRight = prevStickX >  0.5f;
			bool prevStickUp    = prevStickY >  0.5f;
			bool prevStickDown  = prevStickY < -0.5f;

			// Held (any source)
			NavLeftHeld  = keyLeft  || padLeft  || stickLeft  || padLB;
			NavRightHeld = keyRight || padRight || stickRight || padRB;
			NavUpHeld    = keyUp    || padUp    || stickUp;
			NavDownHeld  = keyDown  || padDown  || stickDown;

			// Edge-triggered (pressed this frame from any source)
			NavLeftPressed  = (keyLeft  && !prevKeyLeft)  || (padLeft  && !prevPadLeft)  || (stickLeft  && !prevStickLeft)  || (padLB  && !prevPadLB);
			NavRightPressed = (keyRight && !prevKeyRight) || (padRight && !prevPadRight) || (stickRight && !prevStickRight) || (padRB  && !prevPadRB);
			NavUpPressed    = (keyUp    && !prevKeyUp)    || (padUp    && !prevPadUp)    || (stickUp    && !prevStickUp);
			NavDownPressed  = (keyDown  && !prevKeyDown)  || (padDown  && !prevPadDown)  || (stickDown  && !prevStickDown);
		}

		// =====================================================================
		// CLEANUP: update previous-state trackers
		// =====================================================================
		if (!touchMoveActive) lOrig = false;
		if (!touchShootActive) rOrig = false;

		p = touchState;
		prevGamePad = gs;
		prevMouse = ms;
		prevKeyboard = ks;
		prevScrollWheel = ms.ScrollWheelValue;
	}

	/// <summary>
	/// Draw a simple mouse cursor when mouse/keyboard input is active.
	/// </summary>
	public static void DrawMouseCursor(SpriteBatch sprite)
	{
		if (!IsNonTouchActive || _activeSource == ActiveInputSource.Gamepad)
			return;

		// Draw a simple crosshair cursor at mouse position
		Vector2 pos = MousePosition;
		float size = 8f;
		Color cursorColor = new Color(1f, 1f, 1f, 0.8f);

		// Horizontal line
		sprite.Draw(Game1.nullTex, new Rectangle((int)(pos.X - size), (int)pos.Y, (int)(size * 2f), 1), cursorColor);
		// Vertical line
		sprite.Draw(Game1.nullTex, new Rectangle((int)pos.X, (int)(pos.Y - size), 1, (int)(size * 2f)), cursorColor);
	}
}
