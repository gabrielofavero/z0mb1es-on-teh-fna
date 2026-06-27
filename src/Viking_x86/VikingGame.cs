using System.Collections.Generic;
using System.Threading;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.hud;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SheetEdit.TextureSheet;
using Viking_x86.character;
using Viking_x86.director;
using Viking_x86.particles;
using Viking_x86.vikinggame;
using Viking_x86.vikinggame.hud;
using Viking_x86.world;
using ZombiesWP7;

namespace Viking_x86;

public class VikingGame
{
	public static Dictionary<string, XTexture> textures;

	public float initFrame;

	public bool paused;

	public CharacterMgr charMgr;

	public World world;

	public ParticleMgr pMgr;

	public Texture2D spritesTex;

	public Viking_x86.vikinggame.SpawnMgr spawnMgr;

	public Texture2D grassTex;

	public Texture2D blueTex;

	public Texture2D cityTex;

	public Texture2D grayTex;

	public Texture2D heartTex;

	public Texture2D urbanTex;

	public Texture2D[] punkTex;

	public Texture2D atmosTex;

	public Texture2D[] moonTex;

	public VikingHUD hud;

	public bool diedOnce;

	public VikingDirector vikingDirector;

	private ContentManager Content;

	private bool loadComplete;

	public VikingGame(ContentManager Content)
	{
		this.Content = Content;
	}

	public void Update(GameTime gameTime)
	{
		if (!loadComplete)
		{
			Game1.loader.Update();
			return;
		}
		if (initFrame > 0f)
		{
			initFrame -= Game1.frameTime;
		}
		switch (GameState.state)
		{
		case 4:
			Menu.Update();
			break;
		case 5:
		{
			if (Menu.GetActive())
			{
				Menu.Update();
				break;
			}
			vikingDirector.Update();
			charMgr.Update();
			world.Update();
			pMgr.Update();
			spawnMgr.Update();
			hud.Update();
			GamePadState state = GamePad.GetState(PlayerIndex.One);
			if (state.Buttons.LeftShoulder == ButtonState.Pressed)
			{
				VScroll.angle += Game1.frameTime * -1f;
			}
			if (state.Buttons.RightShoulder == ButtonState.Pressed)
			{
				VScroll.angle += Game1.frameTime * 1f;
			}
			Vector2 vector = charMgr.character[0].loc + new Vector2(0f, -100f);
			float num = 1.4f;
			switch (vikingDirector.phase)
			{
			case 0:
				num = 2f;
				break;
			case 1:
				num = 3f;
				break;
			case 2:
				if (charMgr.moon.active && charMgr.moon.GetDif() >= 500f)
				{
					num = 1.1f;
					float num2 = 1.4f - VScroll.zoom;
					vector.Y -= 200f * num2;
				}
				break;
			}
			if (VScroll.zoom < num)
			{
				VScroll.zoom += Game1.frameTime * 0.05f;
				if (VScroll.zoom > num)
				{
					VScroll.zoom = num;
				}
			}
			else if (VScroll.zoom > num)
			{
				VScroll.zoom -= Game1.frameTime * 0.05f;
				if (VScroll.zoom < num)
				{
					VScroll.zoom = num;
				}
			}
			if (VScroll.scroll.X < world.towerX - 50f)
			{
				VScroll.angle = (world.towerX - 50f - VScroll.scroll.X) / -4000f;
				vector.Y += (world.towerX - 50f - VScroll.scroll.X) * 0.12f;
			}
			else
			{
				VScroll.angle += (world.goalRotation - VScroll.angle) * Game1.frameTime * 0.1f;
			}
			VScroll.scroll += (vector - VScroll.scroll) * Game1.frameTime * 10f;
			if (VScroll.scroll.X < 100f)
			{
				VScroll.scroll.X = 100f;
			}
			VikingQuake.Update();
			if (VScroll.zoom < 0.1f)
			{
				VScroll.zoom = 0.1f;
			}
			VScroll.Bake();
			if (!HUD.playersInited)
			{
				HUD.InitPlayers();
				HUD.playersInited = true;
			}
			if (!charMgr.character[0].exists)
			{
				GameState.state = 4;
				Menu.Reset();
				Music.Stop();
				Game1.leaderMgr.Write(1, charMgr.character[0].score);
			}
			break;
		}
		}
	}

	public void Draw(SpriteBatch spriteBatch, GraphicsDevice dev)
	{
		dev.Clear(Color.Black);
		if (!loadComplete)
		{
			Game1.loader.Draw(spriteBatch);
			return;
		}
		VScroll.Prepare();
		switch (GameState.state)
		{
		case 4:
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			Menu.Draw(spriteBatch);
			spriteBatch.End();
			break;
		case 5:
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			pMgr.FLICKR = 1 - pMgr.FLICKR;
			world.DrawBack(spriteBatch);
			charMgr.Draw(spriteBatch);
			pMgr.Draw(spriteBatch, alpha: false);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			pMgr.Draw(spriteBatch, alpha: true);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			world.Draw(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			hud.Draw(spriteBatch);
			HUD.Draw(spriteBatch);
			spriteBatch.End();
			if (initFrame > 0f)
			{
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
				spriteBatch.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color(0f, 0f, 0f, initFrame));
				spriteBatch.End();
			}
			break;
		}
		VScroll.Reset();
	}

	internal void Init()
	{
		loadComplete = false;
		Thread thread = new Thread(ThreadedLoad);
		thread.Start();
	}

	public void ThreadedLoad()
	{
		Game1.loader.Reset();
		initFrame = 1f;
		Music.LoadSong(1, Content, "sfx/music/timeviking");
		Game1.loader.SetText("1nit v1k1ng B3ATZ!");
		textures = new Dictionary<string, XTexture>();
		textures.Add("robot", new XTexture(Content, "gfx_2/robot"));
		Game1.loader.SetText("Readz viking/robot.png!");
		textures.Add("viking", new XTexture(Content, "gfx_2/viking"));
		Game1.loader.SetText("Readz viking/viking.png!");
		textures.Add("scrap", new XTexture(Content, "gfx_2/scrap"));
		Game1.loader.SetText("Readz viking/scrap.png!");
		CharDefMgr.Initialize();
		vikingDirector = new VikingDirector();
		charMgr = new CharacterMgr();
		world = new World();
		pMgr = new ParticleMgr();
		spritesTex = Content.Load<Texture2D>("gfx_2/sprites");
		Game1.loader.SetText("Readz viking/sprites.png!");
		grassTex = Content.Load<Texture2D>("gfx_2/grass");
		Game1.loader.SetText("Readz viking/grass.png!");
		blueTex = Content.Load<Texture2D>("gfx_2/blue");
		Game1.loader.SetText("Readz viking/blue.png!");
		cityTex = Content.Load<Texture2D>("gfx_2/city");
		Game1.loader.SetText("Readz viking/city.png!");
		grayTex = Content.Load<Texture2D>("gfx_2/gray");
		Game1.loader.SetText("Readz viking/gray.png!");
		heartTex = Content.Load<Texture2D>("gfx_2/hearts");
		Game1.loader.SetText("Readz viking/hearts.png!");
		urbanTex = Content.Load<Texture2D>("gfx_2/urban");
		Game1.loader.SetText("Readz viking/urban.png!");
		atmosTex = Content.Load<Texture2D>("gfx_2/atmos");
		Game1.loader.SetText("Readz viking/atmos.png!");
		moonTex = new Texture2D[3];
		for (int i = 0; i < 3; i++)
		{
			moonTex[i] = Content.Load<Texture2D>("gfx_2/moon" + (i + 1));
		}
		Game1.loader.SetText("Readz viking/moon.png!");
		punkTex = new Texture2D[2]
		{
			Content.Load<Texture2D>("gfx_2/punk"),
			Content.Load<Texture2D>("gfx_2/punk2")
		};
		Game1.loader.SetText("Readz viking/punkrawk.png!");
		hud = new VikingHUD();
		spawnMgr = new Viking_x86.vikinggame.SpawnMgr();
		Menu.Reset();
		loadComplete = true;
	}

	internal void Play()
	{
		diedOnce = false;
		pMgr.Reset();
		charMgr.Reset();
		world.Reset();
		paused = false;
		VScroll.scroll = default(Vector2);
		VScroll.zoom = 2.5f;
		vikingDirector.Init();
		TimeMgr.time = 1;
		TimeMgr.CurTMgr().playMode = BaseTimeMgr.PlayMode.Stopped;
		Music.Stop();
		TimeMgr.CurTMgr().playNum = 0;
		GameState.state = 5;
	}
}
