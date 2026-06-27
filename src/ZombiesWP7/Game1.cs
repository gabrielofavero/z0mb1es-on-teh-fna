using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using IMAK3Z0MB1EGAEM;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.character;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.math;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Viking_x86;
using Viking_x86.character;
using Viking_x86.director;
using Viking_x86.loader;
using Viking_x86.main;
using ZombiesWP7.custom;
using ZombiesWP7.player;
using ZombiesWP7.storage;
using ZombiesWP7.store;
using ZombiesWP7.xdktools;
using Z0MB1ES;

namespace ZombiesWP7;

public class Game1 : Game
{
	public enum SigninStatus
	{
		None,
		Local
	}

	private const float LOAD_TOTAL_TIME = 3f;

	private GraphicsDeviceManager graphics;

	private SpriteBatch spriteBatch;

	public static float frameTime;

	public static Texture2D nullTex;

	public static Texture2D[] thumpadTex = new Texture2D[2];

	public static Texture2D thumclickTex;

	public static Texture2D glowTex;

	public static ZombieGame zgame;

	public static VikingGame vgame;

	public static TimeMgr tMgr;

	public static Loader loader;

	public static MainMenu mainMenu;

	public static LeaderboardMgr leaderMgr;

	public static AchievementMgr achievementMgr;

	private bool loadcomplete;

	public static bool needsExit = false;

	private float framePerSecondTime;

	private int framesPerSecondCount;

	public static int fps;

	public static Vector2 VIEWPORT = new Vector2(600f, 360f);

	/// <summary>Integer scale factor from internal 600×360 to render target (computed from display height).</summary>
	public static int RenderScale = 2;

	/// <summary>Letterboxing offset X from backbuffer left edge to the render target.</summary>
	public static int DestOffsetX = 0;

	/// <summary>Letterboxing offset Y from backbuffer top edge to the render target.</summary>
	public static int DestOffsetY = 0;

	public static float VIEWSCALE = 0.75f;

	public static float MENUSCALE = 1.25f;

	public static RenderTarget2D textTarg;

	/// <summary>Render target that captures the game at its native 600×360 for integer upscaling.</summary>
	private RenderTarget2D gameRenderTarget;

	public static Texture2D bakedText;

	public static Dictionary<string, Texture2D> achieveTextures;

	public static SigninStatus signinStatus = SigninStatus.None;

	public static SpriteFont arial;

	public static Player player;

	public static AccelMgr accelMgr;

	public static StringBuilder gamerName;

	public static PlayerProfile profile;

	private float loadFrame;

	public static CustomMgr custom;

	public static StorageMgr storageMgr;

	private bool loadedAlready;

	public static bool needsQuit = false;

	/// <summary>Debug flag: enables title-update test on Back button press.</summary>
	public static bool tUTest = false;

	/// <summary>Debug flag: set by InputMgr when tUTest activates.</summary>
	public static bool debugNeedsTitleUpdate = false;

	public static LocStrings locStrings;

	public Game1()
	{
		graphics = new GraphicsDeviceManager(this);
		base.Content.RootDirectory = "";
		base.TargetElapsedTime = TimeSpan.FromTicks(333333L);
	}

	protected override void Initialize()
	{
		// Load the PNG asset manifest so all texture dimensions are known at runtime
		PngManifest.Load();
		AssetScaler.Initialize();

		Window.Title = "I MAED A GAM3 W1TH Z0MB1ES 1N IT!!!1";
		locStrings = new LocStrings();
		Rand.rand = new Random();
		storageMgr = new StorageMgr();
		accelMgr = new AccelMgr();
		profile = new PlayerProfile();
		custom = new CustomMgr();
		loader = new Loader();
		player = new Player();
		CharMan.Init();
		VScroll.screenSize = VIEWPORT;
		ScrollMan.screenSize = VIEWPORT;
		leaderMgr = new LeaderboardMgr();
		achievementMgr = new AchievementMgr();
		graphics.IsFullScreen = true;
		graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
		graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
		graphics.SynchronizeWithVerticalRetrace = true;
		graphics.ApplyChanges();

		// Compute integer render scale from display height for pixel-perfect upscaling
		int backW = base.GraphicsDevice.PresentationParameters.BackBufferWidth;
		int backH = base.GraphicsDevice.PresentationParameters.BackBufferHeight;
		RenderScale = backH / (int)VIEWPORT.Y;
		if (RenderScale < 1) RenderScale = 1;
		int destW = (int)VIEWPORT.X * RenderScale;
		int destH = (int)VIEWPORT.Y * RenderScale;
		DestOffsetX = (backW - destW) / 2;
		DestOffsetY = (backH - destH) / 2;

		// Render at native 600×360, upscaled with integer scaling in Draw()
		gameRenderTarget = new RenderTarget2D(base.GraphicsDevice, (int)VIEWPORT.X, (int)VIEWPORT.Y);
		TouchPanel.EnabledGestures = GestureType.Tap;
		base.IsMouseVisible = false;
		base.Initialize();
	}

	protected override void LoadContent()
	{
		spriteBatch = new SpriteBatch(base.GraphicsDevice);
		nullTex = base.Content.Load<Texture2D>("gfx/1x1");
		textTarg = new RenderTarget2D(base.GraphicsDevice, 512, 256);
		Thread thread = new Thread(ThreadedMainLoad);
		thread.Start();
	}

	private void LoadAchieveTextures()
	{
		achieveTextures = new Dictionary<string, Texture2D>();
		achieveTextures.Add("kill1000", base.Content.Load<Texture2D>("gfx/achievements/1000"));
		achieveTextures.Add("ez_100k", base.Content.Load<Texture2D>("gfx/achievements/ez_100k"));
		achieveTextures.Add("ez_oneroom", base.Content.Load<Texture2D>("gfx/achievements/ez_1room"));
		achieveTextures.Add("ez_oneround", base.Content.Load<Texture2D>("gfx/achievements/ez_1round"));
		achieveTextures.Add("ez_tworounds", base.Content.Load<Texture2D>("gfx/achievements/ez_2rounds"));
		achieveTextures.Add("ez_million", base.Content.Load<Texture2D>("gfx/achievements/ez_millonz"));
		achieveTextures.Add("im_100k", base.Content.Load<Texture2D>("gfx/achievements/im_100k"));
		achieveTextures.Add("kill10000", base.Content.Load<Texture2D>("gfx/achievements/10000"));
		achieveTextures.Add("im_invincible", base.Content.Load<Texture2D>("gfx/achievements/im_invincible"));
		achieveTextures.Add("im_asteroids", base.Content.Load<Texture2D>("gfx/achievements/im_asteroids"));
		achieveTextures.Add("im_nomove", base.Content.Load<Texture2D>("gfx/achievements/im_nomove"));
		achieveTextures.Add("im_laser", base.Content.Load<Texture2D>("gfx/achievements/im_laser"));
		achieveTextures.Add("im_million", base.Content.Load<Texture2D>("gfx/achievements/im_million"));
		achieveTextures.Add("im_win", base.Content.Load<Texture2D>("gfx/achievements/im_win"));
		achieveTextures.Add("tv_100k", base.Content.Load<Texture2D>("gfx/achievements/tv_100k2"));
		achieveTextures.Add("tv_million", base.Content.Load<Texture2D>("gfx/achievements/tv_million2"));
		achieveTextures.Add("tv_tower", base.Content.Load<Texture2D>("gfx/achievements/tv_tower2"));
		achieveTextures.Add("tv_win", base.Content.Load<Texture2D>("gfx/achievements/tv_win"));
		achieveTextures.Add("tv_invincible", base.Content.Load<Texture2D>("gfx/achievements/tv_invincible"));
		achieveTextures.Add("mega", base.Content.Load<Texture2D>("gfx/achievements/mega"));
	}

	public void ReMainLoad()
	{
		loader.Reset();
		base.Content.Unload();
		nullTex = base.Content.Load<Texture2D>("gfx/1x1");
		Text.baked = false;
		loadcomplete = false;
		Thread thread = new Thread(ThreadedMainLoad);
		thread.Start();
		GameState.state = 0;
	}

	public void ThreadedMainLoad()
	{
		loader.Reset();
		Text.Init(nullTex);
		loader.SetText("Loadered.");
		Sound.Init(base.Content);
		loader.SetText("MAXIMUM SOUNDS!!!1");
		Menu.Initialize();
		thumpadTex[0] = base.Content.Load<Texture2D>("gfx_2/thumbpad");
		thumpadTex[1] = base.Content.Load<Texture2D>("gfx_2/thumbpad2");
		thumclickTex = base.Content.Load<Texture2D>("gfx_2/thumbclick");
		glowTex = base.Content.Load<Texture2D>("gfx/glow");
		loader.SetText("MAXIMUM TWINSTICKS!!!1");
		LoadAchieveTextures();
		loader.SetText("Achievables!!!1");
		CharDefMgr.Initialize();
		arial = base.Content.Load<SpriteFont>("arial");
		loader.SetText("MAXIMUM FONTS!!!1");
		vgame = new VikingGame(base.Content);
		zgame = new ZombieGame(base.Content);
		mainMenu = new MainMenu(base.Content);
		custom.Init(base.GraphicsDevice, base.Content);
		storageMgr.Read();
		TrialMgr.CheckTrial();
		loadcomplete = true;
		loadedAlready = true;
	}

	protected override void UnloadContent()
	{
	}

	protected override void Update(GameTime gameTime)
	{
		if (needsQuit)
		{
			Exit();
		}
		frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		achievementMgr.Update();
		if (Menu.needsQuit)
		{
			Menu.needsQuit = false;
			GameState.state = 0;
			ReMainLoad();
			return;
		}
		framePerSecondTime += frameTime;
		if (framePerSecondTime > 1f)
		{
			framePerSecondTime -= 1f;
			fps = framesPerSecondCount;
			framesPerSecondCount = 0;
		}
		InputMgr.Update();
		switch (GameState.state)
		{
		case 0:
			loadFrame += frameTime;
			loader.Update();
			if (loadcomplete && loadFrame > 3f)
			{
				GameState.state = 1;
			}
			break;
		case 1:
			mainMenu.Update();
			break;
		case 2:
		case 3:
		case 6:
		case 7:
			zgame.Update(gameTime);
			break;
		case 4:
		case 5:
			vgame.Update(gameTime);
			break;
		}
		Sound.Update();
		base.Update(gameTime);
	}

	protected override void OnExiting(object sender, EventArgs args)
	{
		storageMgr.Write();
		base.OnExiting(sender, args);
	}

	protected override void Draw(GameTime gameTime)
	{
		// --- Render game at native 600×360 into the render target ---
		base.GraphicsDevice.SetRenderTarget(gameRenderTarget);
		base.GraphicsDevice.Clear(Color.Black);

		if (!Text.baked)
		{
			Text.Bake(base.GraphicsDevice, textTarg, spriteBatch);
		}
		framesPerSecondCount++;
		switch (GameState.state)
		{
		case 0:
			if (!loadedAlready || loadFrame < 3f)
			{
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
				spriteBatch.Draw(nullTex, new Rectangle(0, 0, (int)ScrollMan.screenSize.X, (int)ScrollMan.screenSize.Y), Color.Black);
				spriteBatch.End();
			}
			else
			{
				loader.Draw(spriteBatch);
			}
			break;
		case 1:
			mainMenu.Draw(spriteBatch);
			break;
		case 2:
		case 3:
		case 6:
		case 7:
			zgame.Draw(gameTime, spriteBatch, graphics.GraphicsDevice);
			break;
		case 4:
		case 5:
			vgame.Draw(spriteBatch, graphics.GraphicsDevice);
			break;
		}
		_ = Text.baked;

		// --- Scale the render target up to the backbuffer (letterboxed, integer-scaled) ---
		base.GraphicsDevice.SetRenderTarget(null);
		base.GraphicsDevice.Clear(Color.Black);

		spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise);
		spriteBatch.Draw(gameRenderTarget,
			new Rectangle(DestOffsetX, DestOffsetY,
				(int)VIEWPORT.X * RenderScale, (int)VIEWPORT.Y * RenderScale),
			Color.White);
		spriteBatch.End();

		base.Draw(gameTime);
	}
}

/// <summary>Program entry point for FNA desktop.</summary>
public static class Program
{
	private static void Main()
	{
		using (Game1 game = new Game1())
			game.Run();
	}
}
