using System;
using System.Collections.Generic;
using IMAK3Z0MB1EGAEM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace Viking_x86.loader;

public class Loader
{
	private float frame;

	private List<string> data;

	public Loader()
	{
		data = new List<string>();
	}

	public void Reset()
	{
		data.Clear();
	}

	public void Update()
	{
		frame += Game1.frameTime;
	}

	public void Draw(SpriteBatch sprite)
	{
		Draw(sprite, 0f);
	}

	public void Draw(SpriteBatch sprite, float yOff)
	{
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		float num = 1f;
		for (int i = 0; i < 8; i++)
		{
			float num2 = (float)i * 0.785f + frame * 2f;
			float num3 = ((i == 0) ? 0f : 1f);
			sprite.Draw(Game1.nullTex, ScrollMan.screenSize / 2f + new Vector2((float)Math.Cos(num2), (float)Math.Sin(num2)) * 16f + new Vector2(0f, yOff), new Rectangle(0, 0, 1, 1), new Color(1f, num3, num3, num), 0f, new Vector2(0.5f, 0.5f), 4f, SpriteEffects.None, 1f);
			sprite.Draw(Game1.nullTex, ScrollMan.screenSize / 2f + new Vector2((float)Math.Cos(0f - num2), (float)Math.Sin(0f - num2)) * 48f + new Vector2(0f, yOff), new Rectangle(0, 0, 1, 1), new Color(1f, num3, num3, 0.2f * num), 0f, new Vector2(0.5f, 0.5f), 8f, SpriteEffects.None, 1f);
		}
		for (int j = 0; j < data.Count; j++)
		{
			float num4 = data.Count - j;
			float num5 = 1f - num4 / 10f;
			if (num5 < 0.5f)
			{
				num5 = 0.5f;
			}
			try
			{
				Text.DrawString(sprite, data[j], new Vector2(2f, 5f) + new Vector2(0f, (float)j * 6f), 1f, new Color(num5, num5, num5, num5), Text.Justify.Left);
			}
			catch
			{
			}
		}
		sprite.End();
	}

	internal void SetText(string p)
	{
		data.Add(p);
	}
}
