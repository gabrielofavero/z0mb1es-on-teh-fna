using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class UrbanTools
{
	private struct Splat
	{
		public Vector3 loc;

		public float size;

		public void Update()
		{
			if (loc.Y == 0f || loc.Y > VScroll.scroll.Y + 500f)
			{
				if (loc.X == 0f)
				{
					loc.Y = VScroll.scroll.Y + Rand.GetRandomFloat(-500f, 500f);
				}
				else
				{
					loc.Y = VScroll.scroll.Y - 700f - Rand.GetRandomFloat(0f, 600f);
				}
				loc.X = Game1.vgame.world.GetBase().X + Rand.GetRandomFloat(-400f, 400f);
				loc.Z = Rand.GetRandomFloat(0.3f, 1f);
				size = Rand.GetRandomFloat(2f, 2.5f);
			}
			loc.Y += Game1.frameTime * 60f;
		}

		public void Draw(SpriteBatch sprite, int type, int color)
		{
			float num = 1f;
			if (TimeMgr.CurTMgr().trackTime < 11.0)
			{
				num = (float)TimeMgr.CurTMgr().trackTime / 11f;
			}
			Rectangle r = default(Rectangle);
			Color color2 = Color.White;
			switch (type)
			{
			case 0:
				r = new Rectangle(0, 0, 256, 256);
				break;
			case 1:
				r = new Rectangle(256, 0, 320, 256);
				break;
			case 2:
				r = new Rectangle(576, 0, 448, 256);
				break;
			case 3:
				r = new Rectangle(0, 256, 384, 256);
				break;
			}
			num *= 0.75f;
			switch (color)
			{
			case 0:
				color2 = new Color(num * 0.25f, num * 0.25f, 0f, 1f);
				break;
			case 1:
				color2 = new Color(num * 0.25f, num * 0.5f, 0f, 1f);
				break;
			case 2:
				color2 = new Color(num * 0.5f, num * 0.5f, 0f, 1f);
				break;
			case 3:
				color2 = new Color(0f, 0f, 0f, 0.75f);
				break;
			case 4:
				color2 = new Color(0f, num * 0.5f, 0f, 0.75f);
				break;
			}
			Vector2 vector = new Vector2(loc.X, loc.Y);
			float num2;
			for (num2 = (float)TimeMgr.CurTMgr().pulse * 2f; num2 > 1f; num2 -= 1f)
			{
			}
			num2 -= 0.6f;
			if (num2 > 0f)
			{
				vector += Rand.GetRandomVec2(-1f, 1f, -1f, 1f) * 10f * num2;
			}
			sprite.Draw(Game1.vgame.urbanTex, VScroll.GetScreenLoc(vector, loc.Z), ScaleTools.ScaledRect(r), color2, VScroll.angle, new Vector2((float)r.Width / 2f, (float)r.Height / 2f) / 2f, size * VScroll.zoom * loc.Z * 2f, SpriteEffects.None, 1f);
		}
	}

	private Splat[] splat = new Splat[12];

	private float streamFrame;

	public void Update()
	{
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
		for (int i = 0; i < splat.Length; i++)
		{
			splat[i].Update();
		}
	}

	public void Draw(SpriteBatch sprite)
	{
		for (float num = (float)TimeMgr.CurTMgr().pulse * 2f; num > 1f; num -= 1f)
		{
		}
		float num2 = 1f;
		if (TimeMgr.CurTMgr().trackTime < 11.0)
		{
			num2 = (float)TimeMgr.CurTMgr().trackTime / 11f;
		}
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(num2 * 0.2f, num2 * 0.4f, num2 * 0.1f, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.urbanTex, VScroll.GetScreenLoc(new Vector2(Game1.vgame.world.GetBase().X, VScroll.scroll.Y), 0.2f), ScaleTools.ScaledRect(512 - (int)streamFrame, 576, 512, 448), new Color(num2 * 0.5f, num2 * 0.5f, 0f, 1f), VScroll.angle + 1.57f, new Vector2(256f, 224f) / 2f, new Vector2(1.5f, 1.5f) * VScroll.zoom * 2f, SpriteEffects.None, 1f);
		for (int i = 0; i < splat.Length; i++)
		{
			splat[i].Draw(sprite, i % 4, i % 5);
		}
		sprite.Draw(Game1.vgame.heartTex, VScroll.GetScreenLoc(new Vector2(Game1.vgame.world.GetBase().X, VScroll.scroll.Y), 0.2f), ScaleTools.ScaledRect(0, 512 - (int)streamFrame, 512, 512), new Color(0f, 0f, 0f, 0.65f), VScroll.angle, new Vector2(256f, 256f) / 2f, new Vector2(1f, 1.5f) * VScroll.zoom * 2f, SpriteEffects.None, 1f);
	}
}
