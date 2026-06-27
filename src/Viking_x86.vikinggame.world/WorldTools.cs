using System;
using IMAK3Z0MB1EGAEM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class WorldTools
{
	public const int FREQ_BEAT = 0;

	public const int FREQ_DOUBLE = 1;

	public const int FREQ_QUAD = 2;

	public const int FREQ_OCTO = 3;

	public const int STR_TIME = 0;

	public const int STR_VIKING = 1;

	public const int LIGHTROW_RED = 0;

	public const int LIGHTROW_BLUE = 1;

	public const int LIGHTROW_GREEN = 2;

	public const int LIGHTROW_STROBE_WHITE = 3;

	public const int LIGHTROW_WHITE = 4;

	public const int LIGHTROW_PURPLE_STROBE = 5;

	public const int LIGHTROW_YELLOW_STROBE = 6;

	public const int LIGHTROW_RED_STROBE = 7;

	public const int LIGHTROW_STROBE_WHITE_FAST = 8;

	public float frame;

	private string[] STRING = new string[2] { "TIME", "VIKING" };

	public void Update()
	{
		frame += Game1.frameTime;
		if (frame > 6.28f)
		{
			frame -= 6.28f;
		}
	}

	public void DrawText(SpriteBatch sprite, Vector2 loc, Color c, float size, int idx)
	{
		Text.DrawString(sprite, STRING[idx], loc, size, c, Text.Justify.Center);
	}

	public void DrawText(SpriteBatch sprite, Vector2 loc, float size, int idx)
	{
		Text.DrawString(sprite, STRING[idx], loc, size, new Color(Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), Rand.GetRandomFloat(0.4f, 1f), 0.5f), Text.Justify.Center, VScroll.angle);
	}

	public void DrawLaserBunch(SpriteBatch sprite, Color c, Vector3 loc, float angle, int freq)
	{
		for (int i = 0; i < 4; i++)
		{
			for (int j = 0; j < 5; j += 3)
			{
				DrawLaser(sprite, c, loc, angle, freq, i, 0.02f, (float)j * 0.1f);
			}
		}
	}

	public void DrawLaser(SpriteBatch sprite, Color c, Vector3 loc, float angle, int freq, int idx, float wid)
	{
		DrawLaser(sprite, c, loc, angle, freq, idx, 0.02f, 0f);
	}

	public void DrawLaser(SpriteBatch sprite, Color c, Vector3 loc, float angle, int freq, int idx, float wid, float add)
	{
		switch (freq)
		{
		case 0:
			if (TimeMgr.CurTMgr().beat % 4 != idx)
			{
				return;
			}
			break;
		case 1:
			if (TimeMgr.CurTMgr().quadbeat / 2 % 4 != idx)
			{
				return;
			}
			break;
		case 2:
			if (TimeMgr.CurTMgr().quadbeat % 4 != idx)
			{
				return;
			}
			break;
		case 3:
			if (TimeMgr.CurTMgr().octobeat % 4 != idx)
			{
				return;
			}
			break;
		}
		angle += (float)Math.Cos(loc.X * 0.1f + loc.Y * 0.1f + frame * 2f + (float)idx * 0.5f + add) * 0.3f;
		sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect(256, 768, 768, 256), c, angle + VScroll.angle, new Vector2(64f, 128f) / 2f, VScroll.zoom * new Vector2(3f, 0.02f) * 2f, SpriteEffects.None, 1f);
	}

	public void DrawLight(SpriteBatch sprite, Color c, Vector3 loc, float angle, float size)
	{
		angle += (float)Math.Cos(loc.X * 0.1f + loc.Y * 0.1f + frame) * 0.2f;
		loc.Y += (float)Math.Sin(VScroll.angle) * 100f;
		sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect(256, 768, 768, 256), c, angle + VScroll.angle, new Vector2(64f, 128f) / 2f, size * VScroll.zoom * new Vector2(1f, 0.7f) * 2f, SpriteEffects.None, 1f);
	}

	public void DrawLightRow(SpriteBatch sprite, int lightColor, Vector2 orig)
	{
		orig.Y += 200f;
		for (int i = 1; i < 5; i += 2)
		{
			Vector2 vector = orig + new Vector2(((float)i - 2f) * 180f, 0f);
			Color c = Color.White;
			switch (lightColor)
			{
			case 1:
				c = new Color(0.2f, 0.25f, 1f, 0.35f);
				break;
			case 0:
				c = new Color(1f, 0.25f, 0.2f, 0.35f);
				break;
			case 2:
				c = new Color(0.2f, 1f, 0.2f, 0.35f);
				break;
			case 3:
				c = new Color(1f, 1f, 1f, 0.35f);
				if (TimeMgr.CurTMgr().octobeat % 2 == i % 2)
				{
					c = new Color(0f, 0f, 0f, 0f);
				}
				break;
			case 4:
				c = new Color(1f, 1f, 1f, 0.125f);
				break;
			case 5:
				c = new Color(1f, 0.2f, 1f, 0.35f);
				if (TimeMgr.CurTMgr().quadbeat % 2 == i % 2)
				{
					c = new Color(0f, 0f, 0f, 0f);
				}
				break;
			case 6:
				c = new Color(1f, 1f, 0.2f, 0.35f);
				if (TimeMgr.CurTMgr().quadbeat % 2 == i % 2)
				{
					c = new Color(0f, 0f, 0f, 0f);
				}
				break;
			case 7:
				c = new Color(1f, 0.3f, 0.2f, 0.45f);
				if (TimeMgr.CurTMgr().quadbeat % 2 == i % 2)
				{
					c = new Color(0f, 0f, 0f, 0f);
				}
				break;
			case 8:
				c = new Color(1f, 1f, 1f, 0.35f);
				if (TimeMgr.CurTMgr().hexadecobeat % 2 == i % 2)
				{
					c = new Color(0f, 0f, 0f, 0f);
				}
				break;
			}
			if (c.A > 0 && i != 2)
			{
				DrawLight(sprite, c, new Vector3(vector.X, vector.Y, 1.1f), -1.57f + (float)(i - 2) * -0.18f, 1f);
			}
		}
	}

	public void DrawBlip(SpriteBatch sprite, float r, float g, float b, Vector3 loc)
	{
		float a = (float)Math.Cos(loc.X * 0.1f + loc.Y * 0.1f + loc.Z * 0.1f + frame) * 0.5f + 0.5f;
		sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect(128, 64, 128, 128), new Color(r, g, b, a), 0f, new Vector2(64f, 64f) / 2f, 1f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect(128, 64, 128, 128), new Color(r, g, b, a), 0f, new Vector2(64f, 64f) / 2f, 0.5f, SpriteEffects.None, 1f);
	}
}
