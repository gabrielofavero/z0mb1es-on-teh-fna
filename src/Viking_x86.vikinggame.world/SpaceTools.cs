using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class SpaceTools
{
	private struct Glow
	{
		public Vector3 loc;

		public float size;

		public void Update()
		{
			if (loc.Y == 0f || loc.Y > VScroll.scroll.Y + 400f)
			{
				loc.Y = VScroll.scroll.Y - 500f - Rand.GetRandomFloat(0f, 600f);
				loc.X = Game1.vgame.world.GetBase().X + Rand.GetRandomFloat(-400f, 400f);
				loc.Z = Rand.GetRandomFloat(0.3f, 1f);
				size = Rand.GetRandomFloat(0.1f, 0.2f);
			}
			loc.Y += Game1.frameTime * 120f;
		}

		public void Draw(SpriteBatch sprite)
		{
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect(128, 64, 128, 128), new Color(1f, 1f, 1f, 0.5f), VScroll.angle, new Vector2(64f, 64f) / 2f, size * VScroll.zoom * loc.Z * 2f, SpriteEffects.None, 1f);
		}
	}

	private Glow[] glow = new Glow[16];

	public void Update()
	{
		for (int i = 0; i < glow.Length; i++)
		{
			glow[i].Update();
		}
	}

	public void DrawTechnoBack(SpriteBatch sprite)
	{
		float num;
		for (num = (float)TimeMgr.CurTMgr().pulse * 2f; num > 1f; num -= 1f)
		{
		}
		float num2 = 1f;
		if (TimeMgr.CurTMgr().trackLeft < 1.0)
		{
			num2 = (float)TimeMgr.CurTMgr().trackLeft;
		}
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(num2 * 0.2f + num * 0.2f, num2 * 0.2f, num2 * 0.2f, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		DrawTechnoLines(sprite, 1f, 0.3f, 5f);
		DrawTechnoLines(sprite, -1.2f, 0.5f, 150f);
		DrawTechnoLines(sprite, 1.4f, 0.7f, 250f);
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
	}

	private void DrawTechnoLines(SpriteBatch sprite, float s, float angle, float wid)
	{
		float num;
		for (num = (float)TimeMgr.CurTMgr().pulse * 2f; num > 1f; num -= 1f)
		{
		}
		float num2 = 1f;
		if (TimeMgr.CurTMgr().trackLeft < 1.0)
		{
			num2 = (float)TimeMgr.CurTMgr().trackLeft;
		}
		for (int i = 0; i < 4; i++)
		{
			double num3 = TimeMgr.CurTMgr().time * (double)s + (double)i * 0.785;
			Vector2 vector = new Vector2((float)Math.Cos(num3), (float)Math.Sin(num3)) * wid;
			for (int j = 0; j < 2; j++)
			{
				sprite.Draw(Game1.vgame.heartTex, new Vector2(VScroll.GetScreenLoc(new Vector2(Game1.vgame.world.GetBase().X, VScroll.scroll.Y), angle).X, VScroll.screenSize.Y / 2f) + vector * VScroll.zoom * ((j == 1) ? (-1f) : 1f), ScaleTools.ScaledRect(640, 512 + Rand.GetRandomInt(0, 4) * 128, 384, 128), new Color(Rand.GetRandomFloat(0f, 1f) * num2, Rand.GetRandomFloat(0f, 1f) * num2, Rand.GetRandomFloat(0f, 1f) * num2, 0.5f), VScroll.angle + (float)num3 + 1.57f + (Rand.CoinToss(0.5f) ? 3.14f : 0f), new Vector2(192f, 64f) / 2f, VScroll.zoom * new Vector2(1.65f * Rand.GetRandomFloat(1f, 1.2f), (1f - num) * 1.5f + 0.3f) * new Vector2(1f, angle) * 2f, Rand.CoinToss(0.5f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
			}
		}
	}

	public void DrawBack(SpriteBatch sprite)
	{
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(1f, 1f, 1f, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		for (int i = 0; i < glow.Length; i++)
		{
			glow[i].Draw(sprite);
		}
		float num;
		for (num = (float)TimeMgr.CurTMgr().pulse * 2f; num > 1f; num -= 1f)
		{
		}
		int num2 = TimeMgr.CurTMgr().beat / 8;
		if (num2 < 24 || num2 >= 28)
		{
			if (num2 >= 8)
			{
				for (int j = 0; j < 1; j++)
				{
					sprite.Draw(Game1.vgame.heartTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(640, 512 + Rand.GetRandomInt(0, 4) * 128, 384, 128), new Color(0.175f, 0.175f, 0.175f, 0.175f), VScroll.angle, new Vector2(192f, 64f) / 2f, VScroll.zoom * new Vector2(1.65f, (1f - num) * 3f + 0.3f) * 2f, Rand.CoinToss(0.5f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
				}
			}
			if (num2 >= 28)
			{
				DrawBeam(sprite, -100f, 0.5f);
				DrawBeam(sprite, 150f, 0.65f);
				DrawBeam(sprite, -350f, 0.75f);
				DrawBeam(sprite, 350f, 0.85f);
			}
			if (num2 >= 36)
			{
				DrawBeam(sprite, 0f, 0.35f);
				DrawBeam(sprite, 250f, 0.45f);
				DrawBeam(sprite, -250f, 0.125f);
				DrawBeam(sprite, 130f, 0.95f);
			}
		}
		if (TimeMgr.CurTMgr().trackTime < 2.0)
		{
			sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color(1f - (float)(TimeMgr.CurTMgr().trackTime / 2.0), 1f - (float)(TimeMgr.CurTMgr().trackTime / 2.0), 1f - (float)(TimeMgr.CurTMgr().trackTime / 2.0), 1f - (float)(TimeMgr.CurTMgr().trackTime / 2.0)));
		}
	}

	private void DrawBeam(SpriteBatch sprite, float x, float z)
	{
		float num;
		for (num = (float)TimeMgr.CurTMgr().pulse * 2f; num > 1f; num -= 1f)
		{
		}
		sprite.Draw(Game1.vgame.heartTex, new Vector2(VScroll.GetScreenLoc(Game1.vgame.world.GetBase() + new Vector2(x, 0f), z).X, VScroll.screenSize.Y / 2f), ScaleTools.ScaledRect(640, 512 + Rand.GetRandomInt(0, 4) * 128, 384, 128), new Color(0f, 0f, 0f, 0.125f), VScroll.angle + 1.57f + (Rand.CoinToss(0.5f) ? 3.14f : 0f), new Vector2(192f, 64f) / 2f, VScroll.zoom * new Vector2(1.65f * Rand.GetRandomFloat(1f, 1.2f), (1f - num) * 1.5f + 0.3f) * 2f, Rand.CoinToss(0.5f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
	}
}
