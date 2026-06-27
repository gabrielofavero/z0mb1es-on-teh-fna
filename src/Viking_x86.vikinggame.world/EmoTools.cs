using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class EmoTools
{
	private struct Heart
	{
		public Vector3 loc;

		public float size;

		public int type;

		public void Update()
		{
			if (loc.Y == 0f || loc.Y > VScroll.scroll.Y + 400f)
			{
				loc.Y = VScroll.scroll.Y - 500f - Rand.GetRandomFloat(0f, 600f);
				loc.X = Game1.vgame.world.GetBase().X + Rand.GetRandomFloat(-300f, 300f);
				loc.Z = Rand.GetRandomFloat(0.3f, 0.8f);
				size = Rand.GetRandomFloat(1f, 2f);
				type = Rand.GetRandomInt(0, 2);
			}
			loc.Y += Game1.frameTime * 120f;
		}

		public void Draw(SpriteBatch sprite)
		{
			type = (int)(loc.X + (float)TimeMgr.CurTMgr().beat) % 2;
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect((type == 0) ? 832 : 960, 256, (type == 0) ? 128 : 64, 128), new Color(1f, 0f, 0f, 0.125f), VScroll.angle, new Vector2((type % 2 == 0) ? 60f : 32f, 64f) / 2f, size * VScroll.zoom * loc.Z * 2f, SpriteEffects.None, 1f);
		}
	}

	private struct Word
	{
		public Vector3 loc;

		public void Update()
		{
			if (loc.Y == 0f || loc.Y > VScroll.scroll.Y + 400f)
			{
				loc.Y = VScroll.scroll.Y - 500f - Rand.GetRandomFloat(0f, 800f);
				loc.X = Game1.vgame.world.GetBase().X + Rand.GetRandomFloat(-400f, 400f);
				loc.Z = Rand.GetRandomFloat(0.3f, 0.8f);
			}
			loc.Y += Game1.frameTime * 120f;
		}

		public void Draw(SpriteBatch sprite, int type)
		{
			sprite.Draw(Game1.vgame.heartTex, VScroll.GetScreenLoc(new Vector2(loc.X, loc.Y), loc.Z), ScaleTools.ScaledRect(512, type * 256 + (int)((0f - loc.Y) / 5f) % 4 * 64, 128, 64), new Color(1f, 1f, 1f, 0.45f), VScroll.angle, new Vector2(64f, 32f) / 2f, 1f * VScroll.zoom * loc.Z * 2f, SpriteEffects.None, 1f);
		}
	}

	private Heart[] heart = new Heart[16];

	private Word[] word = new Word[4];

	private float exclamationRowFrame;

	private float streamFrame;

	public void Update()
	{
		exclamationRowFrame += Game1.frameTime * 0.5f;
		if (exclamationRowFrame > 1f)
		{
			exclamationRowFrame -= 1f;
		}
		streamFrame += Game1.frameTime * 20f;
		streamFrame -= VScroll.scrollDif.Y / 10f;
		if (streamFrame < 0f)
		{
			streamFrame += 512f;
		}
		if (streamFrame > 512f)
		{
			streamFrame -= 512f;
		}
		for (int i = 0; i < heart.Length; i++)
		{
			heart[i].Update();
		}
		for (int j = 0; j < word.Length; j++)
		{
			word[j].Update();
		}
	}

	public void DrawBack(SpriteBatch sprite)
	{
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(1f, 1f, 1f, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		float num = streamFrame / 2f;
		Rectangle value = new Rectangle(0, 256 - (int)num, 256, 256);
		sprite.Draw(origin: new Vector2(128f, 128f - (num - (float)(int)num)), texture: Game1.vgame.heartTex, position: VScroll.GetScreenLoc(new Vector2(Game1.vgame.world.GetBase().X, VScroll.scroll.Y), 0.2f), sourceRectangle: value, color: new Color(1f, 1f, 1f, 0.5f), rotation: VScroll.angle, scale: new Vector2(1f, 1.5f) * VScroll.zoom * 2f, effects: SpriteEffects.None, layerDepth: 1f);
		DrawExclamationLine(sprite, -300f, 0.35f);
		DrawExclamationLine(sprite, 300f, 0.45f);
		DrawExclamationLine(sprite, -500f, 0.25f);
		DrawExclamationLine(sprite, 500f, 0.375f);
		for (int i = 0; i < heart.Length; i++)
		{
			heart[i].Draw(sprite);
		}
		for (int j = 0; j < word.Length; j++)
		{
			word[j].Draw(sprite, j % 4);
		}
		if (TimeMgr.CurTMgr().trackTime < 2.0)
		{
			sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color(1f, 1f, 1f, 1f - (float)(TimeMgr.CurTMgr().trackTime / 2.0)));
		}
	}

	public void DrawEmoBlast(SpriteBatch sprite)
	{
		float num = (float)TimeMgr.CurTMgr().pulse;
		for (num *= 2f; num > 1f; num -= 1f)
		{
		}
		float num2;
		for (num2 = (float)TimeMgr.CurTMgr().pulse * 2f; num2 > 1f; num2 -= 1f)
		{
		}
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(0.2f + num2 / 2f, 0.2f + num2 / 2f, 0.2f + num2 / 2f, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.heartTex, VScroll.GetScreenLoc(new Vector2(Game1.vgame.world.GetBase().X, VScroll.scroll.Y), 0.2f), ScaleTools.ScaledRect(0, 512 - (int)streamFrame, 512, 512), new Color(1f, 0f, 0f, 0.5f), VScroll.angle, new Vector2(256f, 256f) / 2f, new Vector2(1f, 1.5f) * VScroll.zoom * 2f, SpriteEffects.None, 1f);
		for (int i = 0; i < 2; i++)
		{
			sprite.Draw(Game1.vgame.heartTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(640, 512 + Rand.GetRandomInt(0, 4) * 128, 384, 128), new Color(1f, Rand.GetRandomFloat(0.5f, 1f), Rand.GetRandomFloat(0.5f, 1f), 0.75f), VScroll.angle, new Vector2(192f, 64f) / 2f, VScroll.zoom * new Vector2(1.65f, (1f - num2) * 6f + 0.1f) * 2f, Rand.CoinToss(0.5f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
		}
		for (int j = 0; j < 2; j++)
		{
			float num3;
			for (num3 = num + (float)j * 0.5f; num3 > 1f; num3 -= 1f)
			{
			}
			int num4 = (int)(num3 * 32f) % 2;
			Rectangle r = new Rectangle(704, 0, 128, 192);
			switch ((TimeMgr.CurTMgr().quadbeat + j) % 3)
			{
			case 1:
				r.X = 832;
				r.Width = 192;
				break;
			case 2:
				r.X = 768;
				r.Y = 192;
				r.Width = 256;
				break;
			}
			float num5 = num4;
			Vector2 vector = new Vector2(1f, 1f);
			if (j == 1)
			{
				vector += new Vector2(0.5f, 0.5f);
			}
			vector.X += (float)Math.Cos(num3 * 6.28f) * 0.25f;
			vector.Y += (float)Math.Sin(num3 * 6.28f) * 0.25f;
			sprite.Draw(Game1.vgame.heartTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(r), new Color(num5, num5, num5, 0.4f), VScroll.angle, new Vector2((float)r.Width / 2f, (float)r.Height / 2f) / 2f, VScroll.zoom * vector * 2f * 2f, (j % 2 == 0) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
		}
	}

	public void DrawEmoRed(SpriteBatch sprite)
	{
		float num = (float)TimeMgr.CurTMgr().pulse;
		for (num *= 2f; num > 1f; num -= 1f)
		{
		}
		float num2;
		for (num2 = (float)TimeMgr.CurTMgr().pulse; num2 > 1f; num2 -= 1f)
		{
		}
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(1f, 0f, 0f, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.heartTex, VScroll.GetScreenLoc(new Vector2(Game1.vgame.world.GetBase().X, VScroll.scroll.Y), 0.2f), ScaleTools.ScaledRect(0, 512 - (int)streamFrame, 512, 512), new Color(0f, 0f, 0f, 0.5f), VScroll.angle, new Vector2(256f, 256f) / 2f, new Vector2(1f, 1.5f) * VScroll.zoom * 2f, SpriteEffects.None, 1f);
		float num3;
		for (num3 = num2 * 2f; num3 > 1f; num3 -= 1f)
		{
		}
		for (int i = 0; i < 2; i++)
		{
			sprite.Draw(Game1.vgame.heartTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(640, 512 + Rand.GetRandomInt(0, 4) * 128, 384, 128), new Color(1f, 1f, 1f, 0.35f), VScroll.angle + (Rand.CoinToss(0.5f) ? 3.14f : 0f), new Vector2(192f, 64f) / 2f, VScroll.zoom * new Vector2(1.65f, (1f - num3) * 4f + 0.3f) * 2f, Rand.CoinToss(0.5f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
		}
		if (TimeMgr.CurTMgr().trackLeft < 20.0)
		{
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(VScroll.scroll + new Vector2(0f, (8f - (float)TimeMgr.CurTMgr().trackLeft) * 40f), 0.85f), ScaleTools.ScaledRect(577, 64, 447, 192), Color.White, -1.57f + VScroll.angle, new Vector2(224f, 96f) / 2f, VScroll.zoom * 1.2f * 2f, SpriteEffects.None, 1f);
		}
		if (TimeMgr.CurTMgr().trackLeft < 13.0)
		{
			sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color(1f, 1f, 1f, (float)(1.0 - TimeMgr.CurTMgr().trackLeft / 13.0)));
		}
		if (TimeMgr.CurTMgr().trackLeft < 9.0)
		{
			VikingQuake.SetQuake((float)(1.0 - TimeMgr.CurTMgr().trackLeft / 9.0) * 0.65f);
		}
	}

	private void DrawExclamationLine(SpriteBatch sprite, float x, float z)
	{
		float y = VScroll.scroll.Y;
		float num = 140f;
		float num2 = 16f * num;
		_ = num2 / 2f;
		float num3 = y + num2 / 2f;
		for (int i = 0; i < 16; i++)
		{
			Vector2 loc = new Vector2(Game1.vgame.world.GetBase().X + x, 0f);
			loc.Y = (float)i * num + exclamationRowFrame * num * 2f;
			while (loc.Y > num3)
			{
				loc.Y -= num2;
			}
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(loc, z), ScaleTools.ScaledRect((i % 2 == 0) ? 832 : 960, 256, (i % 2 == 0) ? 128 : 64, 128), new Color(1f, 0.9f, 0.9f, 0.15f), VScroll.angle, new Vector2((i % 2 == 0) ? 60f : 32f, 64f) / 2f, z * 1.4f * VScroll.zoom * 2f, SpriteEffects.None, 1f);
		}
	}
}
