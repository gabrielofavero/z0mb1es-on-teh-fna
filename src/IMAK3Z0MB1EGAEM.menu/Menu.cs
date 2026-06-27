using System.Collections.Generic;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.menu;

internal class Menu
{
	public enum PlayerState
	{
		Out,
		In,
		Ready
	}

	public const int LEVEL_QUIT_YOU_SURE = 0;

	public const int LEVEL_HIGH_SCORE = 1;

	public const int LEVEL_PAUSE = 2;

	public const int LEVEL_HELP_AND_OPTIONS = 3;

	public const int LEVEL_END_GAME_YOU_SURE = 4;

	public const int LEVEL_SETTINGS = 5;

	public const int LEVEL_DEBUG = 6;

	public const int LEVEL_ACHIEVEMENTS = 7;

	public const int LEVEL_LEADERBOARDS = 8;

	public const int LEVEL_CUSTOM = 9;

	public const int LEVEL_GAMECREATOR = 10;

	public const int LEVEL_UNLOCK = 11;

	public const int LEVEL_THANKSUPSELL = 12;

	public const int LEVEL_CREDITS = 13;

	public const int LEVEL_HOW_TO_PLAY = 14;

	public const int LEVEL_NOT_SIGNED_IN_TO_LIVE = 15;

	public const int LEVEL_ACHIEVE_UNLOCK = 16;

	private const string JOIN1 = "j0in";

	private const string JOIN2 = "game!1";

	private const string READY = "ready!";

	private const string A_GO = "(a) go";

	private const string B_CANCEL = "(B) cancel";

	private const string Y_SCORES = "(Y) scores";

	private const string READY1 = "ready??";

	private const string READYA1 = "press (A)!";

	private const string READY2 = "READY!!!1";

	private const string IMADEAGAME = "I MAED A GAM3 W1TH";

	private const string ZOMBIESINIT = "Z0MBIES 1N IT!!!1";

	private const string TWINSTICKSHOOTER = "(IT'S A TW1N ST1CK SH00T3R)";

	private const string TIME = "TIME";

	private const string VIKING = "VIKING";

	private const string TIME_VIKING = "TIME VIKING";

	private const string ENDLESS = "ENDL3SS";

	private const string ZOMBIES = "Z0MB1ES!!!1";

	private const string TAP_TO_START = "tap to start!";

	public static PlayerState[] playerState = new PlayerState[4];

	public static int quitYouSure = -1;

	public static bool needsQuit = false;

	public static Dictionary<int, MenuLevel> menuLevel;

	public static float timeGo = 0f;

	public static int grace;

	public static int scoreMode = -1;

	private static float inFrame = 0f;

	// Back arrow animation (same style as MenuLevel.DrawBackArrow)
	private static float _backArrowFrame = 0f;
	private const float BACK_ARROW_X = 0.05f;
	private const float BACK_ARROW_Y = 0.9f;
	private const float BACK_ARROW_SIZE = 2f;

	public static void Initialize()
	{
		menuLevel = new Dictionary<int, MenuLevel>();
		menuLevel.Add(1, new HighScoreMenu());
		menuLevel.Add(0, new QuitYouSure());
		menuLevel.Add(2, new Pause());
		menuLevel.Add(3, new HelpAndOptions());
		menuLevel.Add(4, new EndGameYouSure());
		menuLevel.Add(5, new Settings());
		menuLevel.Add(6, new Debug());
		menuLevel.Add(7, new Achievements());
		menuLevel.Add(8, new Leaderboards());
		menuLevel.Add(9, new CustomMapper());
		menuLevel.Add(10, new GameCreator());
		menuLevel.Add(11, new Upsell());
		menuLevel.Add(12, new ThanksUpsell());
		menuLevel.Add(13, new Credits());
		menuLevel.Add(14, new HowToPlay());
		menuLevel.Add(15, new NotSignedInToLive());
		menuLevel.Add(16, new AchieveUpsell());
	}

	public static void Reset()
	{
		scoreMode = -1;
		quitYouSure = -1;
		for (int i = 0; i < playerState.Length; i++)
		{
			playerState[i] = PlayerState.Out;
		}
		timeGo = 0f;
		inFrame = 1f;
	}

	public static bool GetActive()
	{
		foreach (MenuLevel value in menuLevel.Values)
		{
			if (value.active || value.alpha > 0f)
			{
				return true;
			}
		}
		return false;
	}

	public static void Update()
	{
		// Animate back arrow (same rhythm as MenuLevel / MainMenu arrows)
		_backArrowFrame += Game1.frameTime * 0.6f;
		if (_backArrowFrame > 0.5f)
		{
			_backArrowFrame -= 0.5f;
		}

		foreach (MenuLevel value in menuLevel.Values)
		{
			value.Update();
		}
		switch (GameState.state)
		{
		case 2:
		case 4:
		case 6:
		{
			if (inFrame > 0f)
			{
				inFrame -= Game1.frameTime * 4f;
				break;
			}
			int num = 0;
			if (grace <= 0)
			{
				bool clickHandled = false;
				if (InputMgr.IsClick())
				{
					// Check if click is on the back arrow first
					Vector2 arrowPos = VScroll.screenSize * new Vector2(BACK_ARROW_X, BACK_ARROW_Y);
					float arrowHitSize = 40f;
					Vector2 cv = InputMgr.GetClickVec();
					if (cv.X > arrowPos.X - arrowHitSize * 0.5f && cv.X < arrowPos.X + arrowHitSize * 0.5f &&
						cv.Y > arrowPos.Y - arrowHitSize * 0.5f && cv.Y < arrowPos.Y + arrowHitSize * 0.5f)
					{
						GameState.state = 1;
						Reset();
						clickHandled = true;
					}
				}
				if (!clickHandled && InputMgr.IsClick())
				{
					switch (playerState[num])
					{
					case PlayerState.Out:
						playerState[num] = PlayerState.In;
						playerState[num] = PlayerState.Ready;
						break;
					case PlayerState.In:
						playerState[num] = PlayerState.Ready;
						break;
					}
				}
				if (InputMgr.isBack)
				{
					GameState.state = 1;
					Reset();
				}
			}
			if (grace > 0)
			{
				grace--;
			}
			else if (playerState[0] == PlayerState.Ready)
			{
				timeGo += Game1.frameTime;
				if (timeGo > 1f)
				{
					switch (GameState.state)
					{
					case 6:
						Game1.zgame.Play(endless: true);
						break;
					case 2:
						Game1.zgame.Play();
						break;
					case 4:
						Game1.vgame.Play();
						break;
					case 3:
					case 5:
						break;
					}
				}
			}
			else
			{
				timeGo = 0f;
			}
			break;
		}
		case 3:
		case 5:
			break;
		}
	}

	public static void Draw(SpriteBatch sprite)
	{
		foreach (MenuLevel value in menuLevel.Values)
		{
			if (value.alpha > 0f)
			{
				value.Draw(sprite);
			}
		}
		float num = 1f;
		if (timeGo > 0f)
		{
			num -= timeGo * 2f;
		}
		switch (GameState.state)
		{
		case 4:
			Text.DrawString(sprite, "TIME VIKING", new Vector2(VScroll.screenSize.X / 2f, 120f), 16f * Game1.VIEWSCALE, new Color(Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), num), Text.Justify.Center, glow: true);
			break;
		case 2:
			Text.DrawString(sprite, "I MAED A GAM3 W1TH", new Vector2(VScroll.screenSize.X / 2f, 120f * Game1.VIEWSCALE), 11f * Game1.VIEWSCALE, new Color(Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), num), Text.Justify.Center, glow: true);
			Text.DrawString(sprite, "Z0MBIES 1N IT!!!1", new Vector2(VScroll.screenSize.X / 2f, 180f * Game1.VIEWSCALE), 11f * Game1.VIEWSCALE, new Color(Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), num), Text.Justify.Center, glow: true);
			break;
		case 6:
			Text.DrawString(sprite, "ENDL3SS", new Vector2(VScroll.screenSize.X / 2f, 100f * Game1.VIEWSCALE), 22f * Game1.VIEWSCALE, new Color(Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), num), Text.Justify.Center, glow: true);
			Text.DrawString(sprite, "Z0MB1ES!!!1", new Vector2(VScroll.screenSize.X / 2f, 200f * Game1.VIEWSCALE), 16f * Game1.VIEWSCALE, new Color(Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), num), Text.Justify.Center, glow: true);
			break;
		}
		switch (GameState.state)
		{
		case 2:
		case 4:
		case 6:
			if (timeGo <= 0f)
			{
				Text.DrawString(sprite, "tap to start!", new Vector2(VScroll.screenSize.X / 2f, VScroll.screenSize.Y - 50f), 6f * Game1.VIEWSCALE, new Color(0f, 1f, 0f, num), Text.Justify.Center, glow: true);

				// Draw animated "<" back arrow (matches MenuLevel.DrawBackArrow style)
				DrawBackArrow(sprite, num);
			}
			break;
		case 3:
		case 5:
			break;
		}

		// Draw mouse cursor on top of menus
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
		InputMgr.DrawMouseCursor(sprite);
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
	}

	public static void DrawOk(SpriteBatch sprite)
	{
		Text.DrawString(sprite, "(a) go", new Vector2(VScroll.screenSize.X / 2f, VScroll.screenSize.Y - 100f), 6f, Color.Lime, Text.Justify.Center);
	}

	public static void DrawReady(SpriteBatch sprite)
	{
		Text.DrawString(sprite, "ready!", new Vector2(VScroll.screenSize.X / 2f, VScroll.screenSize.Y - 100f), 6f, Color.Lime, Text.Justify.Center);
	}

	public static void DrawOkCancel(SpriteBatch sprite)
	{
		Text.DrawString(sprite, "(a) go", new Vector2(VScroll.screenSize.X / 2f - 150f, VScroll.screenSize.Y - 100f), 6f, Color.Lime, Text.Justify.Center);
		Text.DrawString(sprite, "(B) cancel", new Vector2(VScroll.screenSize.X / 2f + 150f, VScroll.screenSize.Y - 100f), 6f, Color.Red, Text.Justify.Center);
	}

	public static void DrawOkCancelScores(SpriteBatch sprite)
	{
		Text.DrawString(sprite, "(a) go", new Vector2(VScroll.screenSize.X / 2f, VScroll.screenSize.Y - 100f), 6f, Color.Lime, Text.Justify.Center);
		Text.DrawString(sprite, "(B) cancel", new Vector2(VScroll.screenSize.X / 2f + 200f, VScroll.screenSize.Y - 100f), 6f, Color.Red, Text.Justify.Center);
		Text.DrawString(sprite, "(Y) scores", new Vector2(VScroll.screenSize.X / 2f - 200f, VScroll.screenSize.Y - 100f), 6f, Color.Yellow, Text.Justify.Center);
	}

	internal static void SetLevel(int p)
	{
		foreach (KeyValuePair<int, MenuLevel> item in menuLevel)
		{
			item.Value.active = false;
			if (item.Key == p)
			{
				item.Value.active = true;
			}
		}
	}

	/// <summary>
	/// Draws an animated "\u003c" back arrow on the left side of the lobby screen,
	/// matching the MenuLevel.DrawBackArrow and MainMenu.DrawArrows style.
	/// </summary>
	private static void DrawBackArrow(SpriteBatch sprite, float alpha)
	{
		float t;
		for (t = _backArrowFrame; t > 0.5f; t -= 0.5f)
		{
		}
		t -= 0.25f;
		float speed = 0.1f;
		float pulse = t;
		if (pulse < 0f)
		{
			pulse = 0f - pulse;
		}
		pulse = (0.25f - pulse) * 4f;

		Vector2 arrowPos = VScroll.screenSize * new Vector2(BACK_ARROW_X - t * speed, BACK_ARROW_Y);
		Color arrowColor = new Color(pulse, pulse, pulse, alpha);
		Text.DrawString(sprite, "<", arrowPos, BACK_ARROW_SIZE, arrowColor, Text.Justify.Center);
	}
}
