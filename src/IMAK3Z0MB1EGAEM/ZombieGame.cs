using System.Threading;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.character;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.hud;
using IMAK3Z0MB1EGAEM.map;
using IMAK3Z0MB1EGAEM.menu;
using IMAK3Z0MB1EGAEM.particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using Viking_x86.director;
using Viking_x86.zomb1es.endless;
using ZombiesWP7;
using ZombiesWP7.xdktools;

namespace IMAK3Z0MB1EGAEM;

public class ZombieGame
{
	public static Texture2D spritesTex;

	public static Texture2D grassTex;

	public static Texture2D skaTex;

	public static Texture2D spaceTex;

	public static Texture2D concreteTex;

	public static Texture2D gridTex;

	public static Texture2D fireTex;

	public static Texture2D nekoTex;

	public static Texture2D psychoNekoTex;

	private static EndlessUpdate endlessUpdate;

	public static int mainPlayerIndex = -1;

	private ContentManager Content;

	private bool loadcomplete;

	public int LeaderMgr { get; set; }

	public ZombieGame(ContentManager Content)
	{
		this.Content = Content;
	}

	public static EndlessNode GetEndlessRoom(int x, int y)
	{
		return endlessUpdate.GetRoom(x, y);
	}

	public static int GetEndlessRound()
	{
		return endlessUpdate.round;
	}

	public void Update(GameTime gameTime)
	{
		_ = Menu.needsQuit;
		if (!loadcomplete)
		{
			Game1.loader.Update();
			return;
		}
		FMan.frameTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
		if (mainPlayerIndex < 0)
		{
			for (int i = 0; i < 4; i++)
			{
				if (Menu.playerState[i] != Menu.PlayerState.Out)
				{
					mainPlayerIndex = i;
				}
			}
		}
		switch (GameState.state)
		{
		case 2:
		case 6:
			Menu.Update();
			break;
		case 3:
		case 7:
		{
			bool flag = GameState.state == 7;
			if (flag)
			{
				MapMan.map.Update(gameTime);
			}
			if (Menu.GetActive())
			{
				Menu.Update();
				break;
			}
			CharMan.Update();
			if (Menu.menuLevel[2].active || Menu.menuLevel[2].alpha > 0f)
			{
				break;
			}
			ParticleMan.Update();
			CamMan.Update();
			if (flag)
			{
				Music.Update(2);
				endlessUpdate.Update();
			}
			else
			{
				Music.Update(0);
				TimeMgr.ZombieTMgr().Update();
				MapMan.Update();
				if (TrialMgr.IsTrial() && TimeMgr.ZombieTMgr().time > 300.0)
				{
					EndTrial();
				}
			}
			bool flag2 = true;
			for (int j = 0; j < CharMan.hero.Length; j++)
			{
				if (CharMan.hero[j].exists)
				{
					flag2 = false;
				}
			}
			if (flag2)
			{
				if (flag)
				{
					Game1.leaderMgr.Write(2, CharMan.hero[0].score);
					GameState.state = 6;
				}
				else
				{
					Game1.leaderMgr.Write(0, CharMan.hero[0].score);
					GameState.state = 2;
				}
				for (int k = 0; k < 4; k++)
				{
					Menu.playerState[k] = Menu.PlayerState.Out;
				}
				Menu.timeGo = 0f;
				Music.Stop();
				Menu.scoreMode = 0;
			}
			break;
		}
		}
		if (!HUD.playersInited)
		{
			HUD.InitPlayers();
			HUD.playersInited = true;
		}
	}

	private void EndTrial()
	{
		TimeMgr.ZombieTMgr().Pause(0);
		Menu.menuLevel[2].active = false;
		Menu.menuLevel[11].active = true;
		((Upsell)Menu.menuLevel[11]).quitUponExit = false;
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch, GraphicsDevice dev)
	{
		dev.Clear(Color.Black);
		if (!loadcomplete)
		{
			Game1.loader.Draw(spriteBatch);
			return;
		}
		ScrollMan.Prepare();
		switch (GameState.state)
		{
		case 2:
		case 6:
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			Menu.Draw(spriteBatch);
			spriteBatch.End();
			break;
		case 3:
		case 7:
		{
			bool flag = GameState.state == 7;
			if (flag)
			{
				MapMan.DrawClassicMapFloor(spriteBatch);
			}
			else
			{
				MapMan.DrawMap(spriteBatch);
			}
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			CharMan.Draw(spriteBatch);
			ParticleMan.Draw(spriteBatch, alpha: false);
			DrawDebug(spriteBatch);
			spriteBatch.End();
			spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			if (CharMan.areAlphas)
			{
				CharMan.DrawAlphas(spriteBatch);
			}
			ParticleMan.Draw(spriteBatch, alpha: true);
			for (int i = 0; i < 4; i++)
			{
				if (!CharMan.hero[i].exists || !(CharMan.hero[i].spawnFrame > 0f))
				{
					continue;
				}
				for (int j = 0; j < 2; j++)
				{
					float num;
					for (num = CharMan.hero[i].spawnFrame + (float)j * 0.2f; num > 0.4f; num -= 0.4f)
					{
					}
					CharMan.DrawUnderglow(CharMan.hero[i].loc, spriteBatch, i, num);
				}
			}
			if (flag)
			{
				spriteBatch.End();
				MapMan.DrawClassicMapOverlay(spriteBatch);
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			}
			else
			{
				MapMan.DrawOverMap(spriteBatch);
			}
			HUD.Draw(spriteBatch);
			spriteBatch.End();
			break;
		}
		}
		ScrollMan.Reset();
	}

	private void DrawDebug(SpriteBatch spriteBatch)
	{
	}

	internal void Init()
	{
		Init(endless: false);
	}

	internal void Init(bool endless)
	{
		loadcomplete = false;
		if (endless)
		{
			Thread thread = new Thread(ThreadedLoadEndless);
			thread.Start();
		}
		else
		{
			Thread thread2 = new Thread(ThreadedLoadNormal);
			thread2.Start();
		}
	}

	public void ThreadedLoadNormal()
	{
		Game1.loader.Reset();
		MapMan.Init();
		ScrollMan.screenSize = Game1.VIEWPORT;
		Music.LoadSong(0, Content, "sfx/music/epicopus");
		Game1.loader.SetText("B3ATS OPT1M1ZED!");
		SharedLoad(normal: true);
		loadcomplete = true;
	}

	public void ThreadedLoadEndless()
	{
		Game1.loader.Reset();
		MapMan.Init();
		ScrollMan.screenSize = Game1.VIEWPORT;
		Music.LoadSong(2, Content, "sfx/music/endless");
		Game1.loader.SetText("B3ATS OPT1M1ZED!");
		endlessUpdate = new EndlessUpdate();
		MapMan.ReadMap(Content);
		MapMan.mapSize = new Vector2(3686.4001f, 2764.8f);
		Game1.loader.SetText("Datamap LOADZORZ!");
		SharedLoad(normal: false);
		loadcomplete = true;
	}

	private void SharedLoad(bool normal)
	{
		ParticleMan.Init();
		spritesTex = Content.Load<Texture2D>("gfx_2/zombie/sprites");
		Game1.loader.SetText("Readz zombie/sprites.png!");
		if (normal)
		{
			grassTex = Content.Load<Texture2D>("gfx_2/zombie/grass");
			Game1.loader.SetText("Readz zombie/grass.png!");
			skaTex = Content.Load<Texture2D>("gfx_2/zombie/ska");
			Game1.loader.SetText("Readz zombie/ska.png!");
			spaceTex = Content.Load<Texture2D>("gfx_2/zombie/space");
			Game1.loader.SetText("Readz zombie/space.png!");
			concreteTex = Content.Load<Texture2D>("gfx_2/zombie/concrete");
			Game1.loader.SetText("Readz zombie/concrete.png!");
			gridTex = Content.Load<Texture2D>("gfx_2/zombie/grid");
			Game1.loader.SetText("Readz zombie/grid.png!");
			fireTex = Content.Load<Texture2D>("gfx_2/zombie/fire");
			Game1.loader.SetText("Readz zombie/fire.png!");
			nekoTex = Content.Load<Texture2D>("gfx_2/zombie/neko");
			Game1.loader.SetText("Readz zombie/neko.png!");
			psychoNekoTex = Content.Load<Texture2D>("gfx_2/zombie/psychoneko");
			Game1.loader.SetText("Readz zombie/psychoneko.png!");
		}
		Menu.Reset();
	}

	internal void Play(bool endless)
	{
		for (int i = 0; i < CharMan.monster.Length; i++)
		{
			CharMan.monster[i].exists = false;
		}
		for (int j = 0; j < ParticleMan.particle.Length; j++)
		{
			ParticleMan.particle[j].exists = false;
		}
		for (int k = 0; k < 4; k++)
		{
			CharMan.hero[k].exists = false;
			if (Menu.playerState[k] == Menu.PlayerState.Ready)
			{
				CharMan.hero[k].Init(MapMan.mapSize / 2f + Rand.GetRandomVec2(-200f, 200f, -200f, 200f));
			}
		}
		if (endless)
		{
			GameState.state = 7;
			endlessUpdate.Init();
			Music.song = 2;
			Music.Start();
		}
		else
		{
			TimeMgr.ZombieTMgr().phase = 0;
			TimeMgr.time = 0;
			TimeMgr.ZombieTMgr().playNum = 0;
			GameState.state = 3;
			Music.song = 0;
			Music.Start();
		}
	}

	internal void Play()
	{
		Play(endless: false);
	}

	internal static void UpdateEndlessScroll(Vector2 scrollGoal)
	{
		endlessUpdate.UpdateScroll(scrollGoal);
	}

	internal static int GetEndlessRoomX()
	{
		return endlessUpdate.x_room;
	}

	internal static int GetEndlessRoomY()
	{
		return endlessUpdate.y_room;
	}

	internal static Vector2 GetEndlessRoomTL()
	{
		return new Vector2((float)endlessUpdate.x_room * 64f * 16f * 1.2f, (float)endlessUpdate.y_room * 64f * 12f * 1.2f) + new Vector2(64f, 64f);
	}

	internal static Vector2 GetEndlessRoomBR()
	{
		return new Vector2((float)(endlessUpdate.x_room + 1) * 64f * 16f * 1.2f, (float)(endlessUpdate.y_room + 1) * 64f * 12f * 1.2f) + new Vector2(64f, 64f);
	}

	internal static bool GetEndlessDrawAdditiveFloor()
	{
		return endlessUpdate.GetDrawAdditiveFloor();
	}

	internal static void UpdateEndlessDrawableBounds()
	{
		endlessUpdate.UpdateDrawableRect();
	}
}
