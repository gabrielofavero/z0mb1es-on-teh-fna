using System.Collections.Generic;
using IMAK3Z0MB1EGAEM;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SceneEdit.scene;
using ZombiesWP7;
using ZombiesWP7.xdktools;

namespace Viking_x86.main;

public class MainMenu
{
	public const int OUT_DEST_Z0MB1ES = 0;

	public const int OUT_DEST_VIKING = 1;

	public const int OUT_DEST_ENDL3SS = 2;

	private const string demoMode = "trial mode!";

	private const string lockstr = "Ł";

	private const string NEXT = ">";

	private const string PREV = "<";

	public float frame;

	public float scroll;

	public float inFrame;

	public float outFrame;

	public int outDest;

	private GamePadState pgs;

	public float selected;

	private SceneMgr sceneMgr;

	private Vector2 swipeCoast;

	private float arrowFrame;

	private float playSwingFrame;

	// --- Keyboard/Controller page navigation ---
	private float _navPageTimer = 0f;
	private const float NAV_PAGE_DELAY = 0.25f;
	private bool _navLeftPrev = false;
	private bool _navRightPrev = false;

	// Exit is the last tab in the carousel
	private const int EXIT_TAB_INDEX = 7;

	private const float PAGE_SWIPE_AMOUNT = 60f;

	public int Selected()
	{
		return (int)(selected + 0.5f);
	}

	public MainMenu(ContentManager Content)
	{
		inFrame = 1f;
		sceneMgr = new SceneMgr();
		sceneMgr.Read("scene/data/menu.zcx");
		Game1.loader.SetText("Menu data initialized.");
		sceneMgr.texture.Add("diamond", Content.Load<Texture2D>("gfx_2/scenes/diamond"));
		Game1.loader.SetText("Loadzored: gfx/scenes/diamond!");
		sceneMgr.texture.Add("face1", Content.Load<Texture2D>("gfx_2/scenes/face1"));
		Game1.loader.SetText("Loadzored: gfx/scenes/face1!");
		sceneMgr.texture.Add("face2", Content.Load<Texture2D>("gfx_2/scenes/face2"));
		Game1.loader.SetText("Loadzored: gfx/scenes/face2!");
		sceneMgr.texture.Add("heart", Content.Load<Texture2D>("gfx_2/scenes/heart"));
		Game1.loader.SetText("Loadzored: gfx/scenes/heart!");
		sceneMgr.texture.Add("viking", Content.Load<Texture2D>("gfx_2/scenes/viking"));
		Game1.loader.SetText("Loadzored: gfx/scenes/viking!");
	}

	public void Update()
	{
		float num = selected;
		if (playSwingFrame > 0f)
		{
			playSwingFrame -= Game1.frameTime;
		}
		if (inFrame > 0f)
		{
			inFrame -= Game1.frameTime * 10f;
		}
		if (outFrame > 0f)
		{
			outFrame -= Game1.frameTime * 10f;
			if (outFrame <= 0f)
			{
				switch (outDest)
				{
				case 0:
					GameState.state = 2;
					Game1.zgame.Init();
					break;
				case 1:
					GameState.state = 4;
					Game1.vgame.Init();
					break;
				case 2:
					GameState.state = 6;
					Game1.zgame.Init(endless: true);
					break;
				}
			}
		}
		SceneCam.Update(sceneMgr.video, sceneMgr.video.scenes[sceneMgr.curScene], Game1.frameTime);
		if (outFrame > 0f)
		{
			SceneCam.location.Z += 1f - outFrame;
			SceneCam.location.X -= (1f - outFrame) * 1.6f * 10f;
			SceneCam.location.Y -= (1f - outFrame) * 10f;
		}
		frame += Game1.frameTime;
		if (frame > 6.28f)
		{
			frame -= 6.28f;
		}
		arrowFrame += Game1.frameTime * 0.6f;
		if (arrowFrame > 0.5f)
		{
			arrowFrame -= 0.5f;
		}
		scroll += Game1.frameTime * 40f;
		if (scroll > 1439.9999f)
		{
			scroll -= 1439.9999f;
		}
		GamePadState state = GamePad.GetState(PlayerIndex.One);
		if (inFrame <= 0f && outFrame <= 0f)
		{
			if (Menu.GetActive())
			{
				Menu.Update();
				CoastLock();
			}
			else
			{
				// --- Keyboard/Controller page navigation ---
				KeyboardState ks = Keyboard.GetState();
				float stickX = state.ThumbSticks.Left.X;
				bool stickLeft = stickX < -0.5f;
				bool stickRight = stickX > 0.5f;
				bool navLeft = ks.IsKeyDown(Keys.Left) || ks.IsKeyDown(Keys.A) || state.DPad.Left == ButtonState.Pressed || state.Buttons.LeftShoulder == ButtonState.Pressed || stickLeft;
				bool navRight = ks.IsKeyDown(Keys.Right) || ks.IsKeyDown(Keys.D) || state.DPad.Right == ButtonState.Pressed || state.Buttons.RightShoulder == ButtonState.Pressed || stickRight;

				if (navLeft || navRight)
				{
					if (_navLeftPrev || _navRightPrev)
					{
						_navPageTimer += Game1.frameTime;
					}
					else
					{
						_navPageTimer = NAV_PAGE_DELAY; // trigger immediately on first press
					}

					if (_navPageTimer >= NAV_PAGE_DELAY)
					{
						_navPageTimer = 0f;
						float direction = navRight ? -1f : 1f;
						swipeCoast.X = direction * PAGE_SWIPE_AMOUNT;

						if (playSwingFrame <= 0f)
						{
							Sound.Play("swing");
							playSwingFrame = 0.15f;
						}
					}

					_navLeftPrev = navLeft;
					_navRightPrev = navRight;

					// Simulate swipe for the page movement
					InputMgr.swiping = true;
					InputMgr.swipe.X = swipeCoast.X;
				}
				else
				{
					_navLeftPrev = false;
					_navRightPrev = false;
					_navPageTimer = 0f;
				}

				if (InputMgr.swiping)
				{
					swipeCoast.X = InputMgr.swipe.X;
				}
				else if (swipeCoast.X > 0f || swipeCoast.X < 0f)
				{
					InputMgr.swiping = true;
					InputMgr.swipe.X = swipeCoast.X;
					CoastLock();
				}
				if (swipeCoast.X > 0f)
				{
					swipeCoast.X -= Game1.frameTime * 150f;
					if (swipeCoast.X < 0f)
					{
						swipeCoast.X = 0f;
					}
				}
				if (swipeCoast.X < 0f)
				{
					swipeCoast.X += Game1.frameTime * 150f;
					if (swipeCoast.X > 0f)
					{
						swipeCoast.X = 0f;
					}
				}
				if (InputMgr.swiping)
				{
					sceneMgr.video.time += InputMgr.swipe.X * Game1.frameTime * -0.05f;
				}
				else
				{
					CoastLock();
				}
				if (sceneMgr.video.time < 0f)
				{
					sceneMgr.video.time = 0f;
				}
				if (TrialMgr.IsTrial())
				{
					if (sceneMgr.video.time > 7f)
					{
						sceneMgr.video.time = 7f;
					}
				}
				else if (sceneMgr.video.time > 7f)
				{
					sceneMgr.video.time = 7f;
				}
				selected = sceneMgr.video.time;
				int num2 = (int)(num + 0.5f);
				int num3 = Selected();
				if (num2 != num3 && playSwingFrame <= 0f)
				{
					Sound.Play("swing");
					playSwingFrame = 0.1f;
				}
				if (InputMgr.IsClick())
				{
					Vector2 clickVec = InputMgr.GetClickVec();
					float num4 = 100f;
					float num5 = 60f;
					if (clickVec.X < num4)
					{
						if (playSwingFrame <= 0f)
						{
							swipeCoast.X = num5;
							Sound.Play("swing");
							playSwingFrame = 0.5f;
						}
					}
					else if (clickVec.X > ScrollMan.screenSize.X - num4)
					{
						if (playSwingFrame <= 0f)
						{
							swipeCoast.X = 0f - num5;
							Sound.Play("swing");
							playSwingFrame = 0.5f;
						}
					}
					else
					{
						Sound.PlayHit();
						switch (Selected())
						{
						case 0:
							outDest = 0;
							outFrame = 1f;
							break;
						case 1:
							if (TrialMgr.IsTrial())
							{
								ShowUnlock();
								break;
							}
							outDest = 1;
							outFrame = 1f;
							break;
						case 2:
							if (TrialMgr.IsTrial())
							{
								ShowUnlock();
								break;
							}
							outDest = 2;
							outFrame = 1f;
							break;
						case 3:
							Menu.menuLevel[3].active = true;
							break;
						case 4:
						{
							Achievements achievements = (Achievements)Menu.menuLevel[7];
							achievements.Init();
							achievements.active = true;
							break;
						}
						case 5:
						{
							Leaderboards leaderboards = (Leaderboards)Menu.menuLevel[8];
							leaderboards.Init();
							leaderboards.active = true;
							break;
						}
						case 6:
						{
							if (TrialMgr.IsTrial())
							{
								ShowUnlock();
								break;
							}
							GameCreator gameCreator = (GameCreator)Menu.menuLevel[10];
							gameCreator.SetFromPlayer();
							Menu.menuLevel[10].active = true;
							break;
						}
						case 7:
							Game1.needsQuit = true;
							break;
						}
					}
				}
				if (InputMgr.isBack)
				{
					Game1.needsQuit = true;
				}
		}
	}
	pgs = state;
}

private void ShowUnlock()
{
	Menu.menuLevel[11].active = true;
	((Upsell)Menu.menuLevel[11]).quitUponExit = false;
}

	private void CoastLock()
	{
		float num = 5f;
		if (swipeCoast.X > 0f || swipeCoast.X < 0f)
		{
			float num2 = ((swipeCoast.X > 0f) ? swipeCoast.X : (0f - swipeCoast.X)) / 5f;
			num -= num2;
			if (num < 0f)
			{
				return;
			}
		}
		float num3 = sceneMgr.video.time - (float)(int)sceneMgr.video.time;
		if (num3 < 0.5f)
		{
			sceneMgr.video.time -= num3 * Game1.frameTime * num;
		}
		else
		{
			sceneMgr.video.time += (1f - num3) * Game1.frameTime * num;
		}
	}

	private void DrawArrows(SpriteBatch sprite)
	{
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		float num = 0.05f;
		float size = 2f;
		float y = 0.9f;
		float num2 = 7f;
		if (TrialMgr.IsTrial())
		{
			num2 += 1f;
		}
		for (int i = 0; i < 2; i++)
		{
			float num3;
			for (num3 = (float)i * 0.25f + arrowFrame; num3 > 0.5f; num3 -= 0.5f)
			{
			}
			num3 -= 0.25f;
			float num4 = 0.1f;
			float num5 = num3;
			if (num5 < 0f)
			{
				num5 = 0f - num5;
			}
			num5 = (0.25f - num5) * 4f;
			if (selected < num2 - 0.5f)
			{
				Text.DrawString(sprite, ">", ScrollMan.screenSize * new Vector2(1f - num + num3 * num4, y), size, new Color(num5, num5, num5, 1f), Text.Justify.Center);
			}
			if (selected > 0.5f)
			{
				Text.DrawString(sprite, "<", ScrollMan.screenSize * new Vector2(num - num3 * num4, y), size, new Color(num5, num5, num5, 1f), Text.Justify.Center);
			}
		}
		sprite.End();
	}

	public void Draw(SpriteBatch sprite)
	{
		SceneMaster.Update(sceneMgr.video, sceneMgr.video.scenes[sceneMgr.curScene]);
		sceneMgr.Draw(sprite);
		if (swipeCoast.X < 1f && swipeCoast.X > -1f)
		{
			DrawArrows(sprite);
		}
		if (TrialMgr.IsTrial())
		{
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			float y = 0.95f;
			sprite.Draw(Game1.nullTex, VScroll.screenSize * new Vector2(0.5f, y), new Rectangle(0, 0, 1, 1), new Color(0f, 0f, 0f, 0.9f), 0f, new Vector2(0.5f, 0.5f), new Vector2(150f, 25f), SpriteEffects.None, 1f);
			Text.DrawString(sprite, "trial mode!", VScroll.screenSize * new Vector2(0.5f, y), 3f, new Color(1f, 1f, 1f, 1f), Text.Justify.Center);
			if (TrialMgr.IsTrial())
			{
				switch (Selected())
				{
				case 1:
				case 2:
				case 6:
					sprite.Draw(Game1.nullTex, VScroll.screenSize * new Vector2(0.5f, 0.525f), new Rectangle(0, 0, 1, 1), new Color(0f, 0f, 0f, 0.3f), 0f, new Vector2(0.5f, 0.5f), new Vector2(20f, 20f) * 7.5f, SpriteEffects.None, 1f);
					sprite.Draw(Game1.nullTex, VScroll.screenSize * new Vector2(0.5f, 0.525f), new Rectangle(0, 0, 1, 1), new Color(0f, 0f, 0f, 0.7f), 0f, new Vector2(0.5f, 0.5f), new Vector2(20f, 20f) * 7f, SpriteEffects.None, 1f);
					Text.DrawString(sprite, "Ł", VScroll.screenSize * new Vector2(0.5f, 0.5f), 20f, new Color(1f, 1f, 1f, 1f), Text.Justify.Center);
					break;
				}
			}
			sprite.End();
		}
		if (Menu.GetActive())
		{
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			Menu.Draw(sprite);
			sprite.End();
		}
		if (outFrame > 0f || inFrame > 0f)
		{
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			if (inFrame > 0f)
			{
				sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color(0f, 0f, 0f, inFrame));
			}
			else
			{
				sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color(0f, 0f, 0f, 1f - outFrame));
			}
			sprite.End();
		}

		// Draw "3XIT" label with slide-in animation as carousel approaches the exit tab
		if (!Menu.GetActive() && inFrame <= 0f && outFrame <= 0f)
		{
			float exitT = selected - 6f; // 0 at position 6, 1 at position 7
			if (exitT > 0f)
			{
				if (exitT > 1f) exitT = 1f;
				float slideX = (1f - exitT) * VScroll.screenSize.X * 0.5f;
				sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
				Text.DrawString(sprite, "3XIT",
					new Vector2(VScroll.screenSize.X / 2f + slideX, VScroll.screenSize.Y * 0.5f),
					8f, new Color(1f, 1f, 1f, exitT), Text.Justify.Center);
				sprite.End();
			}
		}

		// Draw mouse cursor on main menu
		sprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		InputMgr.DrawMouseCursor(sprite);
		sprite.End();
	}
}
