using System;
using System.Collections.Generic;
using MapEdit;
using MapEdit.glows;
using MapEdit.map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SheetEdit.TextureSheet;
using Viking_x86;
using Viking_x86.director;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.map;

internal class MapMan
{
	public enum LightMode
	{
		None,
		SlowWhite,
		BlinkWhite,
		RainbowBlink,
		BlinkBlue,
		SlowRed,
		SlowGoo,
		BlinkRed,
		SoftWhite,
		SlowBlue,
		RedLasers,
		BlueLasers,
		RainbowLasers,
		GreenLasers
	}

	private const float DOWN = 1.57f;

	private const float RIGHT = 0f;

	private const float LEFT = 3.14f;

	private const float UP = 4.71f;

	private const string ZOMBIES = "Z0MB1ES!!1";

	private const string PANIC = "PAN1C!!!!11";

	private const string I = "I";

	private const string MADE = "MAD3";

	private const string A = "A";

	private const string GAME = "GAEM";

	private const string WITH = "W1TH";

	private const string ZOM = "Z0M";

	private const string BIES = "B1ES";

	private const string INIT = "IN 1T!!!1";

	private const string KARATE = "KARATE";

	private const string PUNCH = "PUNCH!!1";

	private const string DIE_IN = "D1E 1N";

	private const string A_FIRE = "A F1RE!!1";

	private const string TEH_GAME = "TEH GAEM";

	private const string IZ_ANGER = "IZ ANGRY!1";

	private const string MASSIVE = "MASS1VE";

	private const string OUTRO = "OUTRO!!1";

	public const float ENDLESS_SCALE = 1.2f;

	public static Map map;

	private static GlowMgr glowMgr;

	private static Dictionary<string, XTexture> textures;

	public static Vector2 mapSize;

	public static float mapScale;

	private static float frame;

	private static float brite;

	private static float goalBrite;

	public static bool CheckHeroCol(Vector2 t)
	{
		float num = 12f;
		t /= 1.2f;
		if (!map.CheckCol(t + new Vector2(0f - num, 0f - num)) && !map.CheckCol(t + new Vector2(num, 0f - num)) && !map.CheckCol(t + new Vector2(0f - num, num)))
		{
			return map.CheckCol(t + new Vector2(num, num));
		}
		return true;
	}

	public static void Update()
	{
		frame += FMan.frameTime;
		if (frame > 125.600006f)
		{
			frame -= 125.600006f;
		}
		if (brite > goalBrite)
		{
			brite -= FMan.frameTime;
			if (brite <= goalBrite)
			{
				brite = goalBrite;
			}
		}
		if (brite < goalBrite)
		{
			brite += FMan.frameTime;
			if (brite >= goalBrite)
			{
				brite = goalBrite;
			}
		}
	}

	public static void Init()
	{
		mapScale = 1.6f;
		mapSize = new Vector2(1280f, 720f) * mapScale;
	}

	public static void DrawClassicMapFloor(SpriteBatch sprite)
	{
		ScrollManager.screenSize = ScrollMan.screenSize;
		ScrollManager.scroll = ScrollMan.scroll;
		ScrollManager.zoom = ScrollMan.zoom;
		ZombieGame.UpdateEndlessDrawableBounds();
		if (ZombieGame.GetEndlessDrawAdditiveFloor())
		{
			sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			map.Draw(3, 4, sprite, textures, glowMgr, game: true, 1.2f);
			sprite.End();
		}
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		map.Draw(0, 1, sprite, textures, glowMgr, game: true, 1.2f);
		sprite.End();
	}

	public static void DrawMap(SpriteBatch sprite)
	{
		double num = 0.0;
		switch (TimeMgr.ZombieTMgr().phase)
		{
		case 0:
			if (TimeMgr.ZombieTMgr().trackLeft < 8.0 && TimeMgr.ZombieTMgr().beat > 0)
			{
				float num4 = (float)(TimeMgr.ZombieTMgr().trackLeft / 8.0) + 0.2f;
				DrawMapTrans(sprite, ZombieGame.grassTex, ZombieGame.skaTex, 1f - num4);
			}
			else
			{
				DrawJustMap(sprite, ZombieGame.grassTex);
			}
			break;
		case 1:
			if (TimeMgr.ZombieTMgr().trackLeft < 8.0)
			{
				float num6 = (float)(TimeMgr.ZombieTMgr().trackLeft / 8.0) + 0.2f;
				DrawMapTrans(sprite, ZombieGame.skaTex, ZombieGame.spaceTex, 1f - num6);
			}
			else
			{
				DrawJustMap(sprite, ZombieGame.skaTex);
			}
			break;
		case 2:
			if (TimeMgr.ZombieTMgr().trackLeft < 8.0)
			{
				float num3 = (float)(TimeMgr.ZombieTMgr().trackLeft / 8.0) + 0.2f;
				DrawMapTrans(sprite, ZombieGame.spaceTex, ZombieGame.concreteTex, 1f - num3);
				break;
			}
			DrawJustMap(sprite, ZombieGame.spaceTex);
			num = TimeMgr.ZombieTMgr().trackTime;
			if (num > 1.0)
			{
				num = 1.0;
				if (TimeMgr.ZombieTMgr().trackLeft < 10.0)
				{
					num = (TimeMgr.ZombieTMgr().trackLeft - 8.0) / 2.0;
				}
			}
			sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			sprite.Draw(ZombieGame.spaceTex, ScrollMan.GetScreenLoc(mapSize / 2f, 1f), ScaleTools.ScaledRect(280, 0, 720, 720), new Color(1f, 1f, 1f, (float)num), frame / 10f, new Vector2(360f, 360f) / 2f, 3.3f * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
			sprite.End();
			break;
		case 3:
			if (TimeMgr.ZombieTMgr().trackLeft < 8.0)
			{
				float num5 = (float)(TimeMgr.ZombieTMgr().trackLeft / 8.0) + 0.2f;
				DrawMapTrans(sprite, ZombieGame.concreteTex, ZombieGame.gridTex, 1f - num5);
			}
			else
			{
				DrawJustMap(sprite, ZombieGame.concreteTex);
			}
			break;
		case 4:
			DrawJustMap(sprite, ZombieGame.gridTex);
			num = TimeMgr.ZombieTMgr().trackTime;
			if (num > 1.0)
			{
				num = 1.0;
				if (TimeMgr.ZombieTMgr().trackLeft < 10.0)
				{
					num = (TimeMgr.ZombieTMgr().trackLeft - 8.0) / 2.0;
				}
			}
			sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			sprite.Draw(ZombieGame.gridTex, ScrollMan.GetScreenLoc(mapSize / 2f, 0.8f), ScaleTools.ScaledRect(280, 0, 720, 720), new Color((float)Math.Sin(1f + frame) * 0.5f + 0.5f, (float)Math.Sin(1f + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin(1f + frame + 2f) * 0.5f + 0.5f, (float)num), frame / 10f, new Vector2(360f, 360f) / 2f, mapScale * ScrollMan.zoom * 3.5f * 2f, SpriteEffects.FlipHorizontally, 1f);
			sprite.Draw(ZombieGame.gridTex, ScrollMan.GetScreenLoc(mapSize / 2f, 0.6f), ScaleTools.ScaledRect(280, 0, 720, 720), new Color((float)Math.Sin(2f + frame) * 0.5f + 0.5f, (float)Math.Sin(2f + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin(2f + frame + 2f) * 0.5f + 0.5f, (float)num), frame / -15f, new Vector2(360f, 360f) / 2f, mapScale * ScrollMan.zoom * 3.3f * 2f, SpriteEffects.FlipVertically, 1f);
			sprite.End();
			break;
		case 5:
			if (TimeMgr.ZombieTMgr().beat < 32 || (TimeMgr.ZombieTMgr().beat >= 64 && TimeMgr.ZombieTMgr().beat < 96))
			{
				if (TimeMgr.ZombieTMgr().beat / 2 % 2 == 0)
				{
					if (TimeMgr.ZombieTMgr().octobeat % 2 == 0)
					{
						DrawJustMap(sprite, ZombieGame.nekoTex);
						break;
					}
					sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
					Text.DrawString(sprite, "TEH GAEM", ScrollMan.GetScreenLoc(mapSize / 2f + new Vector2(0f, -150f), 1f), 50f * ScrollMan.zoom, Color.White, Text.Justify.Center);
					Text.DrawString(sprite, "IZ ANGRY!1", ScrollMan.GetScreenLoc(mapSize / 2f + new Vector2(0f, 150f), 1f), 50f * ScrollMan.zoom, Color.White, Text.Justify.Center);
					sprite.End();
				}
				else
				{
					DrawJustMap(sprite, ZombieGame.fireTex);
				}
			}
			else if (TimeMgr.ZombieTMgr().octobeat % 2 == 0)
			{
				DrawJustMap(sprite, ZombieGame.psychoNekoTex);
			}
			else
			{
				DrawJustMap(sprite, ZombieGame.concreteTex);
			}
			break;
		case 6:
			if (TimeMgr.ZombieTMgr().trackTime < 2.0)
			{
				float num2 = (float)(1.0 - TimeMgr.ZombieTMgr().trackLeft / 2.0) + 0.2f;
				DrawMapTrans(sprite, ZombieGame.concreteTex, ZombieGame.grassTex, 1f - num2);
			}
			else
			{
				DrawJustMap(sprite, ZombieGame.grassTex);
			}
			break;
		}
	}

	private static void DrawIMadeZomb(SpriteBatch sprite, int prog)
	{
		switch (prog)
		{
		case -1:
			DrawLightText(sprite, mapSize / 2f, "I", 110f);
			break;
		case 0:
		case 1:
		case 2:
			DrawLightText(sprite, mapSize / 2f, "MAD3", 70f);
			break;
		case 3:
			DrawLightText(sprite, mapSize / 2f, "A", 110f);
			break;
		case 4:
		case 5:
		case 6:
			DrawLightText(sprite, mapSize / 2f, "GAEM", 80f);
			break;
		case 7:
			DrawLightText(sprite, mapSize / 2f, "W1TH", 60f);
			break;
		case 8:
		case 9:
			DrawLightText(sprite, mapSize / 2f, "Z0M", 70f);
			break;
		case 10:
		case 11:
			DrawLightText(sprite, mapSize / 2f, "B1ES", 60f);
			break;
		case 12:
		case 13:
			DrawLightText(sprite, mapSize / 2f, "IN 1T!!!1", 50f);
			break;
		}
	}

	public static void DrawOverMap(SpriteBatch sprite)
	{
		goalBrite = 1f;
		switch (TimeMgr.ZombieTMgr().phase)
		{
		case 0:
			if (TimeMgr.ZombieTMgr().beat < 64)
			{
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 64);
			}
			else if (TimeMgr.ZombieTMgr().beat < 80)
			{
				DrawLights(sprite, LightMode.BlinkRed);
				DrawLights(sprite, LightMode.BlinkWhite);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 64);
			}
			else if (TimeMgr.ZombieTMgr().beat < 144)
			{
				DrawLights(sprite, LightMode.SlowRed);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 144);
			}
			else if (TimeMgr.ZombieTMgr().beat < 160)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.BlinkWhite);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 144);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 160);
			}
			else if (TimeMgr.ZombieTMgr().beat < 176)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.BlinkBlue);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 160);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 176);
			}
			else if (TimeMgr.ZombieTMgr().beat < 192)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.BlinkRed);
				DrawLights(sprite, LightMode.BlinkBlue);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 176);
			}
			else if (TimeMgr.ZombieTMgr().beat < 224)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				DrawLights(sprite, LightMode.BlinkRed);
				DrawLights(sprite, LightMode.BlinkBlue);
				if (TimeMgr.ZombieTMgr().beat >= 192)
				{
					DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "PAN1C!!!!11" : "Z0MB1ES!!1", 50f);
					DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "Z0MB1ES!!1" : "PAN1C!!!!11", 50f);
				}
			}
			else
			{
				DrawLights(sprite, LightMode.SlowBlue);
			}
			break;
		case 1:
			if (TimeMgr.ZombieTMgr().beat < 16)
			{
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 96)
			{
				DrawLights(sprite, LightMode.SlowGoo);
			}
			else if (TimeMgr.ZombieTMgr().beat < 112)
			{
				if ((TimeMgr.ZombieTMgr().beat - 96) / 2 % 2 == 0)
				{
					DrawLights(sprite, LightMode.RedLasers);
					DrawLights(sprite, LightMode.BlueLasers);
					DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().quadbeat / 2 % 2 == 0) ? "KARATE" : "PUNCH!!1", 50f);
					DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().quadbeat / 2 % 2 == 0) ? "PUNCH!!1" : "KARATE", 50f);
				}
				else
				{
					DrawLights(sprite, LightMode.SlowRed);
				}
			}
			else if (TimeMgr.ZombieTMgr().beat < 160)
			{
				DrawLights(sprite, LightMode.SlowGoo);
			}
			else if ((TimeMgr.ZombieTMgr().beat - 160) / 2 % 2 == 0)
			{
				DrawLights(sprite, LightMode.RedLasers);
				DrawLights(sprite, LightMode.BlueLasers);
				DrawLights(sprite, LightMode.GreenLasers);
				DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().quadbeat / 2 % 2 == 0) ? "D1E 1N" : "A F1RE!!1", 50f);
				DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().quadbeat / 2 % 2 == 0) ? "A F1RE!!1" : "D1E 1N", 50f);
			}
			else
			{
				DrawLights(sprite, LightMode.SlowRed);
			}
			break;
		case 2:
			goalBrite = 0.5f;
			break;
		case 3:
			if (TimeMgr.ZombieTMgr().beat >= 64 && TimeMgr.ZombieTMgr().beat < 128)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "PAN1C!!!!11" : "Z0MB1ES!!1", 50f);
				DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "Z0MB1ES!!1" : "PAN1C!!!!11", 50f);
			}
			if (TimeMgr.ZombieTMgr().beat >= 192)
			{
				if (TimeMgr.ZombieTMgr().beat < 256)
				{
					DrawLights(sprite, LightMode.SoftWhite);
					DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "PAN1C!!!!11" : "Z0MB1ES!!1", 50f);
					DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "Z0MB1ES!!1" : "PAN1C!!!!11", 50f);
				}
				else
				{
					DrawLights(sprite, LightMode.SoftWhite);
					DrawLights(sprite, LightMode.RainbowBlink);
					DrawLights(sprite, LightMode.BlinkRed);
					DrawLights(sprite, LightMode.BlinkBlue);
				}
			}
			break;
		case 4:
			if (TimeMgr.ZombieTMgr().beat < 32)
			{
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 64)
			{
				DrawLights(sprite, LightMode.RedLasers);
			}
			else
			{
				if (TimeMgr.ZombieTMgr().beat < 96)
				{
					break;
				}
				if (TimeMgr.ZombieTMgr().beat < 126)
				{
					DrawLights(sprite, LightMode.BlueLasers);
				}
				else
				{
					if (TimeMgr.ZombieTMgr().beat < 128)
					{
						break;
					}
					if (TimeMgr.ZombieTMgr().beat < 158)
					{
						DrawLights(sprite, LightMode.GreenLasers);
					}
					else if (TimeMgr.ZombieTMgr().beat >= 160)
					{
						if (TimeMgr.ZombieTMgr().beat < 190)
						{
							DrawLights(sprite, LightMode.GreenLasers);
							DrawLights(sprite, LightMode.BlueLasers);
							DrawLights(sprite, LightMode.RedLasers);
						}
						else if (TimeMgr.ZombieTMgr().beat >= 192 && TimeMgr.ZombieTMgr().beat < 222)
						{
							DrawLights(sprite, LightMode.GreenLasers);
							DrawLights(sprite, LightMode.BlueLasers);
							DrawLights(sprite, LightMode.RedLasers);
						}
					}
				}
			}
			break;
		case 5:
			if (TimeMgr.ZombieTMgr().beat < 32)
			{
				if (TimeMgr.ZombieTMgr().beat / 2 % 2 == 0)
				{
					DrawLights(sprite, LightMode.RedLasers);
					DrawLights(sprite, LightMode.GreenLasers);
				}
			}
			else if (TimeMgr.ZombieTMgr().beat < 64)
			{
				DrawLights(sprite, LightMode.RedLasers);
				DrawLights(sprite, LightMode.GreenLasers);
				DrawLights(sprite, LightMode.BlueLasers);
			}
			else if (TimeMgr.ZombieTMgr().beat < 96)
			{
				if (TimeMgr.ZombieTMgr().beat / 2 % 2 == 0)
				{
					DrawLights(sprite, LightMode.RedLasers);
					DrawLights(sprite, LightMode.GreenLasers);
				}
			}
			else
			{
				DrawLights(sprite, LightMode.RedLasers);
				DrawLights(sprite, LightMode.GreenLasers);
				DrawLights(sprite, LightMode.BlueLasers);
			}
			break;
		case 6:
			if (TimeMgr.ZombieTMgr().beat < 4)
			{
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 4);
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 52)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 4);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 20);
				DrawIMadeZomb(sprite, TimeMgr.ZombieTMgr().beat - 36);
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 100)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "PAN1C!!!!11" : "Z0MB1ES!!1", 50f);
				DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "Z0MB1ES!!1" : "PAN1C!!!!11", 50f);
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 196)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 212)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				DrawLights(sprite, LightMode.BlinkRed);
				DrawLights(sprite, LightMode.BlinkBlue);
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 220)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				DrawLights(sprite, LightMode.BlueLasers);
				break;
			}
			if (TimeMgr.ZombieTMgr().beat < 228)
			{
				DrawLights(sprite, LightMode.SoftWhite);
				DrawLights(sprite, LightMode.RainbowBlink);
				DrawLights(sprite, LightMode.RedLasers);
				DrawLights(sprite, LightMode.BlinkRed);
				break;
			}
			switch (TimeMgr.ZombieTMgr().beat % 3)
			{
			case 0:
				DrawLights(sprite, LightMode.GreenLasers);
				DrawLights(sprite, LightMode.BlinkRed);
				break;
			case 1:
				DrawLights(sprite, LightMode.RedLasers);
				DrawLights(sprite, LightMode.BlinkBlue);
				break;
			case 2:
				DrawLights(sprite, LightMode.BlueLasers);
				DrawLights(sprite, LightMode.BlinkWhite);
				break;
			}
			DrawLightText(sprite, mapSize / 2f + new Vector2(0f, -150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "MASS1VE" : "OUTRO!!1", 50f);
			DrawLightText(sprite, mapSize / 2f + new Vector2(0f, 150f), (TimeMgr.ZombieTMgr().beat % 2 == 0) ? "OUTRO!!1" : "MASS1VE", 50f);
			break;
		}
	}

	private static void DrawLights(SpriteBatch sprite, LightMode light)
	{
		goalBrite = 0.5f;
		switch (light)
		{
		case LightMode.SlowWhite:
		{
			for (int n = 1; n < 6; n += 2)
			{
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.25f), new Vector2((float)n * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)n + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.25f), new Vector2((float)n * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)n + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.25f), new Vector2(0f, (float)n * mapSize.Y / 6f), 0f + (float)Math.Cos((float)n + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.25f), new Vector2(mapSize.X, (float)n * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)n + frame) * 0.5f);
			}
			break;
		}
		case LightMode.SlowBlue:
		{
			for (int num8 = 1; num8 < 6; num8 += 2)
			{
				DrawLight(sprite, new Color(0.35f, 0.35f, 1f, 0.25f), new Vector2((float)num8 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num8 + frame) * 0.5f);
				DrawLight(sprite, new Color(0.35f, 0.35f, 1f, 0.25f), new Vector2((float)num8 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num8 + frame) * 0.5f);
				DrawLight(sprite, new Color(0.35f, 0.35f, 1f, 0.25f), new Vector2(0f, (float)num8 * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num8 + frame) * 0.5f);
				DrawLight(sprite, new Color(0.35f, 0.35f, 1f, 0.25f), new Vector2(mapSize.X, (float)num8 * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num8 + frame) * 0.5f);
			}
			break;
		}
		case LightMode.SoftWhite:
		{
			for (int num14 = 1; num14 < 6; num14 += 2)
			{
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.125f), new Vector2((float)num14 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num14 + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.25f), new Vector2((float)num14 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num14 + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.125f), new Vector2(0f, (float)num14 * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num14 + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 1f, 1f, 0.25f), new Vector2(mapSize.X, (float)num14 * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num14 + frame) * 0.5f);
			}
			break;
		}
		case LightMode.SlowRed:
		{
			for (int num6 = 1; num6 < 6; num6 += 2)
			{
				DrawLight(sprite, new Color(1f, 0.1f, 0.1f, 0.25f), new Vector2((float)num6 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num6 + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 0.1f, 0.1f, 0.25f), new Vector2((float)num6 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num6 + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 0.1f, 0.1f, 0.25f), new Vector2(0f, (float)num6 * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num6 + frame) * 0.5f);
				DrawLight(sprite, new Color(1f, 0.1f, 0.1f, 0.25f), new Vector2(mapSize.X, (float)num6 * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num6 + frame) * 0.5f);
			}
			break;
		}
		case LightMode.BlinkWhite:
		{
			for (int num15 = 1; num15 < 6; num15 += 2)
			{
				if (num15 % 2 * 2 == TimeMgr.ZombieTMgr().octobeat % 4)
				{
					DrawLight(sprite, new Color(1f, 1f, 1f, 0.125f), new Vector2((float)num15 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num15 + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(1f, 1f, 1f, 0.125f), new Vector2((float)num15 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num15 + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(1f, 1f, 1f, 0.125f), new Vector2(0f, (float)num15 * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num15 + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(1f, 1f, 1f, 0.125f), new Vector2(mapSize.X, (float)num15 * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num15 + frame * 2f) * 0.5f);
				}
			}
			break;
		}
		case LightMode.BlinkBlue:
		{
			for (int num13 = 1; num13 < 6; num13 += 2)
			{
				if (num13 % 2 * 2 == (TimeMgr.ZombieTMgr().octobeat + 2) % 4)
				{
					DrawLight(sprite, new Color(0.2f, 0.2f, 1f, 0.125f), new Vector2((float)num13 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num13 + 1f + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(0.2f, 0.2f, 1f, 0.125f), new Vector2((float)num13 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num13 + 1f + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(0.2f, 0.2f, 1f, 0.125f), new Vector2(0f, (float)num13 * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num13 + 1f + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(0.2f, 0.2f, 1f, 0.125f), new Vector2(mapSize.X, (float)num13 * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num13 + 1f + frame * 2f) * 0.5f);
				}
			}
			break;
		}
		case LightMode.BlinkRed:
		{
			for (int num7 = 1; num7 < 6; num7 += 2)
			{
				if (num7 / 2 % 2 * 2 == (TimeMgr.ZombieTMgr().octobeat + 4) % 4)
				{
					DrawLight(sprite, new Color(1f, 0.2f, 0.2f, 0.125f), new Vector2((float)num7 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num7 + 2f + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(1f, 0.2f, 0.2f, 0.125f), new Vector2((float)num7 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num7 + 2f + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(1f, 0.2f, 0.2f, 0.125f), new Vector2(0f, (float)num7 * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num7 + 2f + frame * 2f) * 0.5f);
					DrawLight(sprite, new Color(1f, 0.2f, 0.2f, 0.125f), new Vector2(mapSize.X, (float)num7 * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num7 + 2f + frame * 2f) * 0.5f);
				}
			}
			break;
		}
		case LightMode.RainbowBlink:
		{
			for (int num = 1; num < 6; num += 2)
			{
				if (num / 2 % 2 * 2 == TimeMgr.ZombieTMgr().octobeat % 4)
				{
					DrawLight(sprite, new Color((float)Math.Sin((float)num + frame) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 2f) * 0.5f + 0.5f, 0.125f), new Vector2((float)num * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num + 3f + frame) * 0.5f);
					DrawLight(sprite, new Color((float)Math.Sin((float)num + frame) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 2f) * 0.5f + 0.5f, 0.125f), new Vector2((float)num * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num + 3f + frame) * 0.5f);
					DrawLight(sprite, new Color((float)Math.Sin((float)num + frame) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 2f) * 0.5f + 0.5f, 0.125f), new Vector2(0f, (float)num * mapSize.Y / 6f), 0f + (float)Math.Cos((float)num + 3f + frame) * 0.5f);
					DrawLight(sprite, new Color((float)Math.Sin((float)num + frame) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)num + frame + 2f) * 0.5f + 0.5f, 0.125f), new Vector2(mapSize.X, (float)num * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)num + 3f + frame) * 0.5f);
				}
			}
			break;
		}
		case LightMode.SlowGoo:
		{
			for (int m = 1; m < 6; m += 2)
			{
				DrawLight(sprite, new Color((float)Math.Sin((float)m + frame) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 2f) * 0.5f + 0.5f, 0.25f), new Vector2((float)m * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)m + frame) * 0.5f);
				DrawLight(sprite, new Color((float)Math.Sin((float)m + frame) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 2f) * 0.5f + 0.5f, 0.25f), new Vector2((float)m * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)m + frame) * 0.5f);
				DrawLight(sprite, new Color((float)Math.Sin((float)m + frame) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 2f) * 0.5f + 0.5f, 0.25f), new Vector2(0f, (float)m * mapSize.Y / 6f), 0f + (float)Math.Cos((float)m + frame) * 0.5f);
				DrawLight(sprite, new Color((float)Math.Sin((float)m + frame) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 1f) * 0.5f + 0.5f, (float)Math.Sin((float)m + frame + 2f) * 0.5f + 0.5f, 0.25f), new Vector2(mapSize.X, (float)m * mapSize.Y / 6f), 3.14f + (float)Math.Cos((float)m + frame) * 0.5f);
			}
			break;
		}
		case LightMode.RedLasers:
		{
			for (int num9 = 1; num9 < 6; num9++)
			{
				if (num9 % 3 == TimeMgr.ZombieTMgr().quadbeat / 2 % 3)
				{
					for (int num10 = 0; num10 < 5; num10 += 2)
					{
						DrawLaser(sprite, new Color(1f, 0.1f, 0.1f, 0.2f), new Vector2((float)num9 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos((float)num9 + (float)num10 / 4f + frame) * 0.5f);
						DrawLaser(sprite, new Color(1f, 0.1f, 0.1f, 0.2f), new Vector2((float)num9 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos((float)num9 + (float)num10 / 4f + frame) * 0.5f);
					}
				}
			}
			for (int num11 = 1; num11 < 5; num11++)
			{
				if (num11 % 3 == TimeMgr.ZombieTMgr().quadbeat / 2 % 3)
				{
					for (int num12 = 0; num12 < 5; num12 += 2)
					{
						DrawLaser(sprite, new Color(1f, 0.1f, 0.1f, 0.2f), new Vector2(0f, (float)num11 * mapSize.Y / 5f), 0f + (float)Math.Cos((float)num11 + (float)num12 / 4f + frame) * 0.5f);
						DrawLaser(sprite, new Color(1f, 0.1f, 0.1f, 0.2f), new Vector2(mapSize.X, (float)num11 * mapSize.Y / 5f), 3.14f + (float)Math.Cos((float)num11 + (float)num12 / 4f + frame) * 0.5f);
					}
				}
			}
			break;
		}
		case LightMode.GreenLasers:
		{
			for (int num2 = 1; num2 < 6; num2++)
			{
				if (num2 % 5 == TimeMgr.ZombieTMgr().quadbeat % 5)
				{
					for (int num3 = 0; num3 < 5; num3 += 2)
					{
						DrawLaser(sprite, new Color(0.1f, 1f, 0.1f, 0.2f), new Vector2((float)num2 * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos(1f + (float)num2 + (float)num3 / 4f + frame) * 0.5f);
						DrawLaser(sprite, new Color(0.1f, 1f, 0.1f, 0.2f), new Vector2((float)num2 * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos(1f + (float)num2 + (float)num3 / 4f + frame) * 0.5f);
					}
				}
			}
			for (int num4 = 1; num4 < 5; num4++)
			{
				if (num4 % 5 == TimeMgr.ZombieTMgr().quadbeat % 5)
				{
					for (int num5 = 0; num5 < 5; num5 += 2)
					{
						DrawLaser(sprite, new Color(0.1f, 1f, 0.1f, 0.2f), new Vector2(0f, (float)num4 * mapSize.Y / 5f), 0f + (float)Math.Cos(1f + (float)num4 + (float)num5 / 4f + frame) * 0.5f);
						DrawLaser(sprite, new Color(0.1f, 1f, 0.1f, 0.2f), new Vector2(mapSize.X, (float)num4 * mapSize.Y / 5f), 3.14f + (float)Math.Cos(1f + (float)num4 + (float)num5 / 4f + frame) * 0.5f);
					}
				}
			}
			break;
		}
		case LightMode.BlueLasers:
		{
			for (int i = 1; i < 6; i++)
			{
				if (i % 4 == TimeMgr.ZombieTMgr().quadbeat / 2 % 4)
				{
					for (int j = 0; j < 5; j += 2)
					{
						DrawLaser(sprite, new Color(0.1f, 0.1f, 1f, 0.25f), new Vector2((float)i * mapSize.X / 6f, 0f), 1.57f + (float)Math.Cos(2f + (float)i + (float)j / 4f + frame) * 0.5f);
						DrawLaser(sprite, new Color(0.1f, 0.1f, 1f, 0.25f), new Vector2((float)i * mapSize.X / 6f, mapSize.Y), 4.71f + (float)Math.Cos(2f + (float)i + (float)j / 4f + frame) * 0.5f);
					}
				}
			}
			for (int k = 1; k < 5; k++)
			{
				if (k % 4 == TimeMgr.ZombieTMgr().quadbeat / 2 % 4)
				{
					for (int l = 0; l < 5; l += 2)
					{
						DrawLaser(sprite, new Color(0.1f, 0.1f, 1f, 0.25f), new Vector2(0f, (float)k * mapSize.Y / 5f), 0f + (float)Math.Cos(2f + (float)k + (float)l / 4f + frame) * 0.5f);
						DrawLaser(sprite, new Color(0.1f, 0.1f, 1f, 0.25f), new Vector2(mapSize.X, (float)k * mapSize.Y / 5f), 3.14f + (float)Math.Cos(2f + (float)k + (float)l / 4f + frame) * 0.5f);
					}
				}
			}
			break;
		}
		case LightMode.RainbowLasers:
			break;
		}
	}

	private static void DrawLightText(SpriteBatch sprite, Vector2 loc, string text, float size)
	{
		Text.DrawString(c: new Color(Rand.GetRandomFloat(0.5f, 1f), Rand.GetRandomFloat(0.5f, 1f), Rand.GetRandomFloat(0.5f, 1f), 0.2f), sprite: sprite, str: text, loc: ScrollMan.GetScreenLoc(loc, 1f), size: size * ScrollMan.zoom, jus: Text.Justify.Center);
	}

	private static void DrawLightText(SpriteBatch sprite, Color c, Vector2 loc, string text, float size)
	{
		Text.DrawString(sprite, text, ScrollMan.GetScreenLoc(loc, 1f), size * ScrollMan.zoom, c, Text.Justify.Center);
	}

	private static void DrawLight(SpriteBatch sprite, Color c, Vector2 loc, float angle)
	{
		sprite.Draw(ZombieGame.spritesTex, ScrollMan.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(0, 448, 768, 256), c, angle, new Vector2(64f, 128f) / 2f, ScrollMan.zoom * 1.1f * 2f, SpriteEffects.None, 1f);
	}

	private static void DrawLaser(SpriteBatch sprite, Color c, Vector2 loc, float angle)
	{
		sprite.Draw(ZombieGame.spritesTex, ScrollMan.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(0, 704, 1024, 64), c, angle, new Vector2(64f, 32f) / 2f, ScrollMan.zoom * new Vector2(3f, 1f) * 2f, SpriteEffects.None, 1f);
	}

	private static void DrawJustMap(SpriteBatch sprite, Texture2D tex)
	{
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Opaque);
		Vector2 realLoc = ScrollMan.GetRealLoc(default(Vector2), 1f);
		Vector2 realLoc2 = ScrollMan.GetRealLoc(ScrollMan.screenSize, 1f);
		realLoc /= mapScale;
		realLoc /= 2f;
		realLoc2 /= mapScale;
		realLoc2 /= 2f;
		sprite.Draw(tex, ScrollMan.screenSize / 2f, new Rectangle((int)realLoc.X, (int)realLoc.Y, (int)(realLoc2.X - realLoc.X) + 4, (int)(realLoc2.Y - realLoc.Y) + 4), new Color(brite, brite, brite, 1f), 0f, (realLoc + realLoc2) / 2f - new Vector2((int)realLoc.X, (int)realLoc.Y), mapScale * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
		sprite.End();
	}

	private static void DrawMapTrans(SpriteBatch sprite, Texture2D tex1, Texture2D tex2, float v)
	{
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		float num = (1f - v) * brite;
		Vector2 realLoc = ScrollMan.GetRealLoc(default(Vector2), 1f);
		Vector2 realLoc2 = ScrollMan.GetRealLoc(ScrollMan.screenSize, 1f);
		realLoc /= mapScale;
		realLoc /= 2f;
		realLoc2 /= mapScale;
		realLoc2 /= 2f;
		sprite.Draw(tex1, ScrollMan.screenSize / 2f, new Rectangle((int)realLoc.X, (int)realLoc.Y, (int)(realLoc2.X - realLoc.X) + 4, (int)(realLoc2.Y - realLoc.Y) + 4), new Color(num, num, num, 1f), 0f, (realLoc + realLoc2) / 2f - new Vector2((int)realLoc.X, (int)realLoc.Y), mapScale * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
		sprite.Draw(tex2, ScrollMan.screenSize / 2f, new Rectangle((int)realLoc.X, (int)realLoc.Y, (int)(realLoc2.X - realLoc.X) + 4, (int)(realLoc2.Y - realLoc.Y) + 4), new Color(v, v, v, v), 0f, (realLoc + realLoc2) / 2f - new Vector2((int)realLoc.X, (int)realLoc.Y), mapScale * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
		sprite.End();
	}

	internal static void DrawClassicMapOverlay(SpriteBatch sprite)
	{
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		map.Draw(1, 3, sprite, textures, glowMgr, game: true, 1.2f);
		for (int i = -1; i < 4; i++)
		{
			for (int j = -1; j < 4; j++)
			{
				float num = 1f;
				if (i >= 0 && j >= 0 && i < 3 && j < 3)
				{
					num = 1f - ZombieGame.GetEndlessRoom(i, j).alpha;
				}
				if (num > 0f)
				{
					Vector2 loc = new Vector2((float)i * 64f * 16f, (float)j * 64f * 12f) * 1.2f;
					Vector2 loc2 = new Vector2((float)(i + 1) * 64f * 16f, (float)(j + 1) * 64f * 12f) * 1.2f;
					loc = ScrollMan.GetScreenLoc(loc, 1f);
					loc2 = ScrollMan.GetScreenLoc(loc2, 1f);
					if (loc.X < 0f)
					{
						loc.X = 0f;
					}
					if (loc.Y < 0f)
					{
						loc.Y = 0f;
					}
					if (loc2.X > 800f)
					{
						loc2.X = 800f;
					}
					if (loc2.Y > 480f)
					{
						loc2.Y = 480f;
					}
					sprite.Draw(Game1.nullTex, loc, new Rectangle(0, 0, 1, 1), new Color(0f, 0f, 0f, num), 0f, default(Vector2), loc2 - loc, SpriteEffects.None, 1f);
				}
			}
		}
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		glowMgr.Draw(sprite, ZombieGame.spritesTex);
		sprite.End();
	}

	internal static void ReadMap(ContentManager Content)
	{
		textures = new Dictionary<string, XTexture>();
		textures.Add("maps", new XTexture(Content, "gfx/maps"));
		Game1.loader.SetText("Readz zombie/maps.png!");
		textures.Add("maps2", new XTexture(Content, "gfx/maps2"));
		Game1.loader.SetText("Readz zombie/maps2.png!");
		map = new Map();
		glowMgr = new GlowMgr();
		map.Read("maps/data/map.zax", textures);
	}
}
