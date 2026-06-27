using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.text;

public class Shape
{
	private bool[,] dot;

	public int width;

	public int idx = -1;

	public Shape(int[] src)
	{
		dot = new bool[5, 5];
		width = 0;
		for (int i = 0; i < src.Length; i++)
		{
			int num = 0;
			while (src[i] > 0)
			{
				dot[i, num] = src[i] % 10 == 1;
				src[i] /= 10;
				num++;
				if (num > width)
				{
					width = num;
				}
			}
		}
	}

	public void Bake(SpriteBatch sprite, Vector2 loc, float scale, Color c, Texture2D nullTex)
	{
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				if (dot[i, j])
				{
					sprite.Draw(nullTex, loc + new Vector2((float)j * (0f - scale), (float)i * scale), new Rectangle(0, 0, 1, 1), c, 0f, default(Vector2), scale, SpriteEffects.None, 1f);
				}
			}
		}
	}

	public void Draw(SpriteBatch sprite, Vector2 loc, float scale, Color c, Texture2D nullTex)
	{
		Draw(sprite, loc, scale, c, nullTex, glow: false);
	}

	public void Draw(SpriteBatch sprite, Vector2 loc, float scale, Color c, Texture2D nullTex, bool glow)
	{
		if (loc.X < ((float)width + 1f) * (0f - scale) || loc.X > VScroll.screenSize.X || idx <= -1)
		{
			return;
		}
		sprite.Draw(Game1.textTarg, loc, new Rectangle(idx % 16 * 32, idx / 16 * 32, 32, 32), c, 0f, default(Vector2), scale / 5.25f, SpriteEffects.None, 1f);
		if (!glow)
		{
			return;
		}
		float num = (float)(int)c.A / 256f;
		num /= 4f;
		if (num > 0.15f)
		{
			Vector2 position = loc + new Vector2(0.5f, 0.5f) * new Vector2(width, 5f) * scale;
			if (position.X < 50f)
			{
				num *= position.X / 50f;
			}
			if (position.X > Game1.VIEWPORT.X - 50f)
			{
				num *= (Game1.VIEWPORT.X - position.X) / 50f;
			}
			Color color = c;
			if (num > 1f)
			{
				num = 1f;
			}
			if (num < 0f)
			{
				num = 0f;
			}
			num *= 0.5f;
			color.A = (byte)(num * 256f);
			sprite.Draw(Game1.glowTex, position, new Rectangle(0, 0, 128, 128), color, 0f, new Vector2(64f, 64f), scale / 10f, SpriteEffects.None, 1f);
		}
	}

	public void Draw(SpriteBatch sprite, Vector2 loc, float scale, Color c, float angle, Texture2D nullTex)
	{
		loc += new Vector2((float)width * scale * VScroll.xVec.X, (float)width * scale * VScroll.xVec.Y);
		for (int i = 0; i < 5; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				while (dot[i, j])
				{
					sprite.Draw(Game1.nullTex, loc + new Vector2(VScroll.xVec.X * (0f - (float)j) * scale, VScroll.xVec.Y * (0f - (float)j) * scale) + new Vector2(VScroll.yVec.X * (0f - (float)i) * scale, VScroll.yVec.Y * (0f - (float)i) * scale), new Rectangle(0, 0, 1, 1), c, angle, default(Vector2), scale, SpriteEffects.None, 1f);
					j++;
					if (j >= 5)
					{
						break;
					}
				}
			}
		}
	}
}
