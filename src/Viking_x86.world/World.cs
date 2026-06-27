using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Viking_x86.character;
using Viking_x86.director;
using Viking_x86.vikinggame;
using Viking_x86.vikinggame.world;
using ZombiesWP7;

namespace Viking_x86.world;

public class World
{
	public struct Collision
	{
		public const int COL_NONE = 0;

		public const int COL_FULL = 1;

		public int col;

		public int idx;

		public float frame;
	}

	public const int WIDTH = 10;

	public const int HEIGHT = 40;

	public const float CEL_WIDTH = 32f;

	public const float CEL_HEIGHT = 16f;

	public const string SCRAP = "scrap";

	public const int PHASE_GRASS = 0;

	public const int PHASE_CITY = 1;

	public const int PHASE_HEARTS = 2;

	public const int PHASE_LIMBO = 3;

	public const int PHASE_NERDCORE = 4;

	public const int PHASE_PUNKOUT = 5;

	public const int PHASE_SPACE = 6;

	public Collision[,] col;

	public float height;

	public float towerX = 500f;

	public float floor = 1000f;

	private GrassTools grassTools;

	private CityTools cityTools;

	private EmoTools emoTools;

	private SpaceTools spaceTools;

	private UrbanTools urbanTools;

	private PunkTools punkTools;

	private OrbitTools orbitTools;

	public float pickupFrame;

	public int phase;

	public WorldTools worldTools;

	public Vector2 risingBaseVec;

	public Vector2 risingTrackBase;

	private int pBeat;

	private GamePadState pTestingState;

	private double testingTick;

	public float goalRotation;

	public int AchivementMgr { get; set; }

	internal void Reset()
	{
		height = 0f;
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 40; j++)
			{
				col[i, j].col = 0;
			}
		}
	}

	public World()
	{
		grassTools = new GrassTools();
		cityTools = new CityTools();
		worldTools = new WorldTools();
		emoTools = new EmoTools();
		spaceTools = new SpaceTools();
		urbanTools = new UrbanTools();
		punkTools = new PunkTools();
		orbitTools = new OrbitTools();
		col = new Collision[10, 40];
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 40; j++)
			{
				col[i, j].idx = (i + j) % 4;
			}
		}
		risingTrackBase = new Vector2(GetBase().X, VScroll.scroll.Y);
	}

	public void ShiftCells()
	{
		for (int i = 0; i < 10; i++)
		{
			for (int num = 39; num > 0; num--)
			{
				col[i, num].col = col[i, num - 1].col;
				col[i, num].idx = col[i, num - 1].idx;
				col[i, num].frame = col[i, num - 1].frame;
			}
		}
		for (int j = 0; j < 10; j++)
		{
			col[j, 0].col = 0;
		}
		float num2 = height;
		height += 16f;
		if (num2 < 6000f && height >= 6000f)
		{
			Game1.achievementMgr.AwardAchievement(10);
		}
	}

	public bool TestCollision(Vector2 loc)
	{
		int num = (int)((loc.X - towerX) / 32f);
		int num2 = (int)((loc.Y + height) / 16f);
		if (loc.X >= towerX && num >= 0 && num < 10 && num2 >= 0)
		{
			if (num2 < 40)
			{
				return col[num, num2].col == 1;
			}
			return true;
		}
		return false;
	}

	public Vector2 GetBase()
	{
		return new Vector2(towerX + 160f, 640f);
	}

	public void AddDebris(Vector2 loc, int idx)
	{
		int num = (int)((loc.X - towerX) / 32f);
		int num2 = (int)((loc.Y + height) / 16f);
		if ((Game1.vgame.charMgr.moon.active && Game1.vgame.charMgr.moon.GetDif() > 500f) || num < 0 || num >= 10 || num2 <= 0)
		{
			return;
		}
		if (num2 < 40)
		{
			bool flag = false;
			bool flag2 = false;
			if (num > 0)
			{
				if (col[num - 1, num2].col == 1)
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (num < 9)
			{
				if (col[num + 1, num2].col == 1)
				{
					flag2 = true;
				}
			}
			else
			{
				flag2 = true;
			}
			if (flag && flag2)
			{
				col[num, num2 - 1].col = 1;
				col[num, num2 - 1].frame = 0.1f;
				col[num, num2 - 1].idx = idx;
			}
		}
		else
		{
			col[num, 39].col = 1;
			col[num, 39].frame = 0.1f;
			col[num, 39].idx = idx;
		}
	}

	public void DrawBack(SpriteBatch sprite)
	{
		VikingDirector vikingDirector = Game1.vgame.vikingDirector;
		switch (vikingDirector.phase)
		{
		case 0:
			DrawBG(sprite, 1f);
			grassTools.DrawBack(sprite, 1f);
			goalRotation = 0f;
			break;
		case 1:
			if (vikingDirector.frame < 0.95f)
			{
				DrawBG(sprite, 1f - vikingDirector.frame);
				grassTools.DrawBack(sprite, 1f - vikingDirector.frame);
			}
			goalRotation = 0f;
			break;
		case 2:
			goalRotation = 0f;
			switch (TimeMgr.VikingTMgr().phase)
			{
			case 0:
				DrawBG(sprite, 1f);
				cityTools.DrawBack(sprite);
				if (TimeMgr.VikingTMgr().beat > 64)
				{
					float num2 = ((float)TimeMgr.VikingTMgr().beat - 64f) / 10f;
					if (num2 > 1f)
					{
						num2 = 1f;
					}
					goalRotation = (float)Math.Sin((540f - risingBaseVec.Y) / 100f) * 0.2f * num2;
				}
				break;
			case 1:
				if (TimeMgr.CurTMgr().beat / 8 < 16)
				{
					emoTools.DrawBack(sprite);
					goalRotation = -0.2f;
				}
				else if (TimeMgr.CurTMgr().beat / 8 < 20)
				{
					emoTools.DrawEmoBlast(sprite);
					goalRotation = 0.7f;
				}
				else
				{
					emoTools.DrawEmoRed(sprite);
				}
				break;
			case 2:
				if (TimeMgr.CurTMgr().beat / 8 < 28)
				{
					spaceTools.DrawBack(sprite);
				}
				else
				{
					spaceTools.DrawTechnoBack(sprite);
				}
				break;
			case 3:
			{
				urbanTools.Draw(sprite);
				goalRotation = (float)Math.Sin((540f - risingBaseVec.Y) / 100f) * 0.2f;
				for (int i = 0; i < Game1.vgame.charMgr.character.Length; i++)
				{
					Character character = Game1.vgame.charMgr.character[i];
					if (character != null && character.exists && character.defID == 4)
					{
						if (character.face == 1)
						{
							goalRotation = 0.5f;
						}
						else
						{
							goalRotation = -0.5f;
						}
					}
				}
				break;
			}
			case 4:
			{
				punkTools.Draw(sprite);
				float num = risingBaseVec.Y * -13.328032f;
				num -= (float)(int)num;
				goalRotation = (num - 0.5f) * 2f;
				break;
			}
			case 5:
				orbitTools.Draw(sprite);
				break;
			case 6:
				sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), Color.Black);
				break;
			}
			DrawLightShow(sprite);
			break;
		}
	}

	private void DrawLightShow(SpriteBatch sprite)
	{
		int beat = TimeMgr.CurTMgr().beat;
		new Vector3(GetBase().X, GetBase().Y, 1f);
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		switch (TimeMgr.CurTMgr().phase)
		{
		case 0:
			switch (beat)
			{
			case 33:
			case 37:
			case 49:
			case 53:
			case 97:
			case 101:
			case 113:
			case 117:
			case 129:
			case 133:
			case 145:
			case 149:
				worldTools.DrawText(sprite, VScroll.GetScreenLoc(risingBaseVec, 0.8f), 40f, 0);
				break;
			case 34:
			case 35:
			case 38:
			case 39:
			case 50:
			case 51:
			case 54:
			case 55:
			case 98:
			case 99:
			case 102:
			case 103:
			case 114:
			case 115:
			case 118:
			case 119:
			case 130:
			case 131:
			case 134:
			case 135:
			case 146:
			case 147:
			case 150:
			case 151:
				worldTools.DrawText(sprite, VScroll.GetScreenLoc(risingBaseVec, 0.8f), 40f, 1);
				break;
			}
			if (beat >= 32)
			{
				Color rl = new Color(1f, 0.25f, 0.25f, 0.25f);
				int freq = 0;
				switch (beat / 8)
				{
				case 2:
				case 3:
					rl = new Color(1f, 0.25f, 1f, 0.125f);
					break;
				case 4:
				case 5:
				case 6:
				case 7:
					rl = new Color(1f, 1f, 1f, 0.125f);
					break;
				case 8:
				case 10:
					rl = new Color(1f, 1f, 0.25f, 0.125f);
					break;
				case 9:
				case 11:
					rl = new Color(1f, 0.25f, 1f, 0.125f);
					freq = 1;
					break;
				case 12:
				case 13:
				case 14:
				case 15:
				case 16:
				case 17:
				case 18:
				case 19:
					rl = new Color(1f, 0.25f, 0.25f, 0.25f);
					break;
				case 20:
				case 22:
					rl = new Color(1f, 1f, 0.25f, 0.125f);
					freq = 1;
					break;
				case 21:
				case 23:
					rl = new Color(1f, 0.25f, 1f, 0.125f);
					break;
				case 24:
				case 25:
				case 26:
				case 27:
					rl = new Color(1f, 0.7f, 0.25f, 0.175f);
					freq = 2;
					break;
				case 28:
				case 29:
				case 30:
				case 31:
					rl = new Color(0.25f, 0.25f, 1f, 0.25f);
					freq = 1;
					break;
				case 32:
				case 33:
				case 34:
				case 35:
					rl = new Color(0.25f, 0.25f, 1f, 0.25f);
					freq = 2;
					break;
				case 36:
				case 37:
				case 38:
					rl = new Color(1f, 0.25f, 0.25f, 0.25f);
					freq = 1;
					break;
				case 39:
					rl = new Color(1f, 0.25f, 1f, 0.25f);
					freq = 2;
					break;
				}
				DrawLaserBrunch(sprite, rl, freq);
			}
			break;
		case 3:
			switch (beat / 8)
			{
			case 12:
			case 13:
			case 14:
			case 15:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 1f, 0.125f), 2);
				break;
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 0f, 0.125f), 0);
				break;
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 1f, 0.125f), 2);
				break;
			}
			break;
		case 5:
			switch (beat)
			{
			case 1:
			case 5:
			case 17:
			case 21:
			case 33:
			case 37:
			case 49:
			case 53:
			case 65:
			case 69:
			case 81:
			case 85:
			case 129:
			case 133:
			case 145:
			case 149:
			case 161:
			case 165:
			case 177:
			case 181:
				worldTools.DrawText(sprite, VScroll.GetScreenLoc(risingBaseVec, 0.8f), 40f, 0);
				break;
			case 2:
			case 3:
			case 6:
			case 7:
			case 18:
			case 19:
			case 22:
			case 23:
			case 34:
			case 35:
			case 38:
			case 39:
			case 50:
			case 51:
			case 54:
			case 55:
			case 66:
			case 67:
			case 70:
			case 71:
			case 82:
			case 83:
			case 86:
			case 87:
			case 130:
			case 131:
			case 134:
			case 135:
			case 146:
			case 147:
			case 150:
			case 151:
			case 162:
			case 163:
			case 166:
			case 167:
			case 178:
			case 179:
			case 182:
			case 183:
				worldTools.DrawText(sprite, VScroll.GetScreenLoc(risingBaseVec, 0.8f), 40f, 1);
				break;
			}
			switch (beat / 8)
			{
			case 2:
			case 3:
				DrawLaserBrunch(sprite, new Color(1f, 0.25f, 1f, 0.125f), 0);
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 1f, 0.125f), 0);
				break;
			case 12:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 0.25f, 0.125f), 2);
				break;
			case 13:
				DrawLaserBrunch(sprite, new Color(1f, 0.25f, 1f, 0.125f), 1);
				break;
			case 14:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 0.25f, 0.125f), 2);
				break;
			case 15:
				DrawLaserBrunch(sprite, new Color(1f, 0.25f, 1f, 0.125f), 1);
				break;
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 1f, 0.125f), 0);
				break;
			case 24:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 0.25f, 0.125f), 2);
				break;
			case 25:
				DrawLaserBrunch(sprite, new Color(1f, 0.25f, 1f, 0.125f), 1);
				break;
			case 26:
				DrawLaserBrunch(sprite, new Color(1f, 1f, 0.25f, 0.125f), 2);
				break;
			case 27:
				DrawLaserBrunch(sprite, new Color(1f, 0.25f, 1f, 0.125f), 1);
				break;
			}
			if (beat / 8 >= 28 && beat / 8 < 60)
			{
				switch (beat / 8 % 4)
				{
				case 0:
					DrawLaserBrunch(sprite, new Color(1f, 0.1f, 0.1f, 0.125f), 2);
					break;
				case 1:
					DrawLaserBrunch(sprite, new Color(0.1f, 1f, 0.1f, 0.125f), 2);
					break;
				case 2:
					DrawLaserBrunch(sprite, new Color(0.1f, 0.1f, 1f, 0.25f), 2);
					break;
				case 3:
					DrawLaserBrunch(sprite, new Color(1f, 1f, 0.1f, 0.125f), 2);
					break;
				}
			}
			break;
		}
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
	}

	private void DrawLaserBrunch(SpriteBatch sprite, Color rl, int freq)
	{
		Vector3 vector = new Vector3(risingBaseVec.X, risingBaseVec.Y + 700f, 1f);
		worldTools.DrawLaserBunch(sprite, rl, vector + new Vector3(-100f, 0f, -0.1f), -1.37f, freq);
		worldTools.DrawLaserBunch(sprite, rl, vector + new Vector3(100f, 0f, -0.1f), -1.7700001f, freq);
		worldTools.DrawLaserBunch(sprite, rl, vector + new Vector3(-200f, 0f, -0.1f), -1.37f, freq);
		worldTools.DrawLaserBunch(sprite, rl, vector + new Vector3(200f, 0f, -0.1f), -1.7700001f, freq);
		worldTools.DrawLaserBunch(sprite, rl, vector + new Vector3(-300f, 0f, -0.1f), -1.37f, freq);
		worldTools.DrawLaserBunch(sprite, rl, vector + new Vector3(300f, 0f, -0.1f), -1.7700001f, freq);
	}

	private void DrawForeLightShow(SpriteBatch sprite)
	{
		int beat = TimeMgr.CurTMgr().beat;
		new Vector3(GetBase().X, GetBase().Y, 1f);
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		switch (TimeMgr.CurTMgr().phase)
		{
		case 0:
			switch (beat / 8)
			{
			case 2:
			case 3:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 4:
			case 5:
			case 6:
			case 7:
				worldTools.DrawLightRow(sprite, 5, risingBaseVec);
				break;
			case 8:
			case 10:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 9:
			case 11:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
				worldTools.DrawLightRow(sprite, 5, risingBaseVec);
				break;
			case 20:
			case 22:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 21:
			case 23:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 24:
			case 25:
			case 26:
			case 27:
				worldTools.DrawLightRow(sprite, 6, risingBaseVec);
				break;
			case 28:
			case 29:
			case 30:
			case 31:
				worldTools.DrawLightRow(sprite, 1, risingBaseVec);
				break;
			case 32:
			case 33:
			case 34:
			case 35:
				worldTools.DrawLightRow(sprite, 2, risingBaseVec);
				break;
			case 36:
			case 37:
			case 38:
				worldTools.DrawLightRow(sprite, 6, risingBaseVec);
				break;
			case 39:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			}
			break;
		case 1:
			switch (beat)
			{
			case 124:
			case 125:
			case 126:
			case 127:
				worldTools.DrawLightRow(sprite, 8, risingBaseVec);
				break;
			}
			switch (beat / 8)
			{
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
			case 12:
			case 13:
			case 14:
			case 15:
				worldTools.DrawLightRow(sprite, 4, risingBaseVec);
				break;
			case 16:
			case 17:
			case 18:
			case 19:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			}
			break;
		case 2:
		{
			int num = beat;
			if (num == 223)
			{
				worldTools.DrawLightRow(sprite, 8, risingBaseVec);
			}
			switch (beat / 8)
			{
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
				worldTools.DrawLightRow(sprite, 4, risingBaseVec);
				break;
			case 36:
			case 37:
			case 38:
			case 39:
			case 40:
			case 41:
			case 42:
			case 43:
			case 44:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			}
			break;
		}
		case 3:
			switch (beat / 8)
			{
			case 4:
			case 5:
			case 6:
			case 7:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 8:
			case 9:
			case 10:
			case 11:
				worldTools.DrawLightRow(sprite, 6, risingBaseVec);
				break;
			case 12:
			case 13:
			case 14:
			case 15:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 16:
			case 17:
			case 18:
			case 19:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 20:
			case 21:
			case 22:
			case 23:
				worldTools.DrawLightRow(sprite, 6, risingBaseVec);
				break;
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			}
			break;
		case 4:
			switch (beat / 8)
			{
			case 4:
			case 5:
			case 6:
			case 7:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 8:
			case 9:
			case 10:
			case 11:
				worldTools.DrawLightRow(sprite, 7, risingBaseVec);
				break;
			case 12:
			case 13:
			case 14:
			case 15:
			case 16:
			case 17:
			case 18:
			case 19:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 20:
			case 21:
			case 22:
			case 23:
			case 24:
			case 25:
			case 26:
			case 27:
			case 28:
			case 29:
			case 30:
			case 31:
				worldTools.DrawLightRow(sprite, 6, risingBaseVec);
				break;
			case 32:
			case 33:
			case 34:
			case 35:
			case 36:
			case 37:
			case 38:
			case 39:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			}
			break;
		case 5:
			switch (beat / 8)
			{
			case 2:
			case 3:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 4:
			case 5:
			case 6:
			case 7:
			case 8:
			case 9:
			case 10:
			case 11:
				worldTools.DrawLightRow(sprite, 5, risingBaseVec);
				break;
			case 12:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 13:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 14:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 15:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 16:
			case 17:
			case 18:
			case 19:
			case 20:
			case 21:
			case 22:
			case 23:
				worldTools.DrawLightRow(sprite, 5, risingBaseVec);
				break;
			case 24:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 25:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 26:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 27:
				worldTools.DrawLightRow(sprite, 3, risingBaseVec);
				break;
			case 28:
			case 29:
			case 30:
			case 31:
				worldTools.DrawLightRow(sprite, 1, risingBaseVec);
				break;
			case 32:
			case 33:
			case 34:
			case 35:
				worldTools.DrawLightRow(sprite, 2, risingBaseVec);
				break;
			case 36:
			case 37:
			case 38:
			case 39:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 40:
			case 41:
			case 42:
			case 43:
				worldTools.DrawLightRow(sprite, 1, risingBaseVec);
				break;
			case 44:
			case 45:
			case 46:
			case 47:
				worldTools.DrawLightRow(sprite, 2, risingBaseVec);
				break;
			case 48:
			case 49:
			case 50:
			case 51:
				worldTools.DrawLightRow(sprite, 0, risingBaseVec);
				break;
			case 52:
			case 53:
			case 54:
			case 55:
				worldTools.DrawLightRow(sprite, 1, risingBaseVec);
				break;
			case 56:
			case 57:
			case 58:
			case 59:
				worldTools.DrawLightRow(sprite, 2, risingBaseVec);
				break;
			}
			break;
		}
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
	}

	private void DrawBG(SpriteBatch sprite, float alpha)
	{
		sprite.Draw(Game1.vgame.blueTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(alpha, alpha, alpha, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
	}

	public void Draw(SpriteBatch sprite)
	{
		float foreBright = GetForeBright();
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 40; j++)
			{
				if (col[i, j].col == 1)
				{
					Vector2 vector = new Vector2(1f, 1f);
					float num = 0f;
					if (col[i, j].frame > 0f)
					{
						vector.X += col[i, j].frame * 3f;
						vector.Y -= col[i, j].frame * 3f;
						num += col[i, j].frame * 5f;
					}
					Color color = new Color(foreBright, foreBright, foreBright, 1f);
					switch (i)
					{
					case 0:
					case 9:
						color = new Color(foreBright * 0.6f, foreBright * 0.6f, foreBright * 0.6f, 1f);
						break;
					case 1:
					case 8:
						color = new Color(foreBright * 0.85f, foreBright * 0.85f, foreBright * 0.85f, 1f);
						break;
					}
					switch (col[i, j].idx)
					{
					case 0:
					case 1:
					case 2:
					case 3:
					case 19:
					case 20:
					case 21:
					case 22:
						sprite.Draw(VikingGame.textures["scrap"].texture, VScroll.GetScreenLoc(new Vector2(towerX + (float)i * 32f, (float)j * 16f - height + num) + new Vector2(32f, 16f) / 2f, 1f), VikingGame.textures["scrap"].GetSpriteRect(col[i, j].idx), color, VScroll.angle, VikingGame.textures["scrap"].GetRelativeSpriteOrigin(col[i, j].idx), VScroll.zoom * 0.25f * vector * 2f, SpriteEffects.None, 1f);
						break;
					case 4:
					case 5:
					case 6:
					case 7:
					case 8:
					case 9:
						sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(towerX + (float)i * 32f, (float)j * 16f - height + num) + new Vector2(32f, 16f) / 2f, 1f), ScaleTools.ScaledRect(448, 64 + (col[i, j].idx - 4) * 64, 128, 64), color, VScroll.angle + ((float)col[i, j].idx - 6.5f) * 0.01f, new Vector2(64f, 32f) / 2f, VScroll.zoom * 0.25f * vector * 1.1f * 2f, SpriteEffects.None, 1f);
						break;
					}
					j++;
					sprite.Draw(Game1.nullTex, VScroll.GetScreenLoc(new Vector2(towerX + (float)i * 32f, (float)j * 16f - height + num), 1f), new Rectangle(0, 0, 1, 1), Color.Black, VScroll.angle, default(Vector2), new Vector2(132f, 64f * (float)(40 - j)) * VScroll.zoom * 0.25f, SpriteEffects.None, 1f);
					break;
				}
			}
		}
		DrawFore(sprite);
	}

	public float GetForeBright()
	{
		if (Game1.vgame.vikingDirector.phase < 2)
		{
			return 1f;
		}
		switch (TimeMgr.CurTMgr().phase)
		{
		case 1:
			if (TimeMgr.CurTMgr().trackLeft < 1.0)
			{
				return (float)TimeMgr.CurTMgr().trackLeft;
			}
			break;
		case 2:
			if (TimeMgr.CurTMgr().beat / 8 >= 28)
			{
				return 1f;
			}
			return 0f;
		case 5:
			if (TimeMgr.CurTMgr().trackLeft < 30.0)
			{
				return (float)TimeMgr.CurTMgr().trackLeft / 30f;
			}
			break;
		case 6:
			return 0f;
		}
		return 1f;
	}

	private void DrawFore(SpriteBatch sprite)
	{
		VikingDirector vikingDirector = Game1.vgame.vikingDirector;
		switch (vikingDirector.phase)
		{
		case 0:
			grassTools.DrawGrass(sprite, 1f);
			break;
		case 1:
			if (vikingDirector.frame < 0.95f)
			{
				grassTools.DrawGrass(sprite, 1f - vikingDirector.frame);
			}
			if (vikingDirector.frame > 0.95f)
			{
				sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color((vikingDirector.frame - 0.95f) * 20f, (vikingDirector.frame - 0.95f) * 20f, (vikingDirector.frame - 0.95f) * 20f, (vikingDirector.frame - 0.95f) * 20f));
			}
			break;
		case 2:
			DrawForeLightShow(sprite);
			cityTools.DrawFore(sprite);
			break;
		}
	}

	internal void Update()
	{
		worldTools.Update();
		if (pickupFrame > 0f)
		{
			pickupFrame -= Game1.frameTime;
		}
		if (TimeMgr.VikingTMgr().phase == 1)
		{
			emoTools.Update();
		}
		else if (TimeMgr.VikingTMgr().phase == 2)
		{
			spaceTools.Update();
		}
		else if (TimeMgr.VikingTMgr().phase == 3)
		{
			urbanTools.Update();
		}
		if (TimeMgr.CurTMgr().beat != pBeat)
		{
			if (TimeMgr.CurTMgr().beat / 16 != pBeat / 16)
			{
				risingBaseVec = new Vector2(GetBase().X, VScroll.scroll.Y);
			}
			if (TimeMgr.CurTMgr().beat == 0)
			{
				risingTrackBase = new Vector2(GetBase().X, VScroll.scroll.Y);
				if (TimeMgr.CurTMgr().phase == 5)
				{
					Game1.vgame.charMgr.moon.Init();
				}
			}
		}
		pBeat = TimeMgr.CurTMgr().beat;
		for (int i = 0; i < 10; i++)
		{
			for (int j = 0; j < 40; j++)
			{
				if (col[i, j].col == 1 && col[i, j].frame > 0f)
				{
					col[i, j].frame -= Game1.frameTime;
					if (col[i, j].frame < 0f)
					{
						col[i, j].frame = 0f;
					}
				}
			}
		}
		bool flag = true;
		for (int num = 39; num > 20; num--)
		{
			if (col[5, num].col != 1)
			{
				flag = false;
			}
		}
		if (flag)
		{
			ShiftCells();
		}
	}

	internal float GetMinY(float x)
	{
		int num = (int)((x - towerX) / 32f);
		if (x > towerX && num >= 0 && num < 10)
		{
			int num2 = 0;
			int num3 = 0;
			int lowestTile = GetLowestTile(num);
			if (num > 0)
			{
				int lowestTile2 = GetLowestTile(num - 1);
				num2 = lowestTile2 - lowestTile;
			}
			if (num < 9)
			{
				int lowestTile3 = GetLowestTile(num + 1);
				num3 = lowestTile3 - lowestTile;
			}
			float num4 = (float)lowestTile * 16f - height;
			float num5 = (x - towerX - (float)num * 32f) / 32f;
			if (num2 > 0 && num3 > 0)
			{
				if (num5 < 0.5f)
				{
					return num4 + 16f - num5 * 16f;
				}
				return num4 + 16f - (1f - num5) * 16f;
			}
			if (num3 > 0)
			{
				return num4 + num5 * 16f;
			}
			if (num2 > 0)
			{
				return num4 + (1f - num5) * 16f;
			}
			return num4;
		}
		return height + 640f;
	}

	private int GetLowestTile(int x)
	{
		for (int i = 0; i < 40; i++)
		{
			if (col[x, i].col == 1)
			{
				return i;
			}
		}
		return 40;
	}
}
