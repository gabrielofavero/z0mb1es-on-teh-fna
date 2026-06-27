using System;
using System.Collections.Generic;
using System.IO;
using MapEdit.glows;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SheetEdit.TextureSheet;

namespace MapEdit.map;

public class Map
{
	public const int COL_NONE = 0;

	public const int COL_FULL = 1;

	public const string WEAPONS = "weapons";

	public const string SMASHABLES = "smashables";

	public const int COL_SIZE = 64;

	public const int MAP_WIDTH = 16;

	public const int MAP_HEIGHT = 12;

	public Layer[] layer;

	public int xSize = 1;

	public int ySize = 1;

	public int[,] col;

	public string[] script;

	public float delta;

	public float brite;

	public float glowBrite;

	public float[] doorAlpha = new float[4];

	public bool[] doorLocked = new bool[4];

	public Rectangle drawableRect;

	public Map()
	{
		layer = new Layer[4];
		for (int i = 0; i < layer.Length; i++)
		{
			layer[i] = new Layer();
		}
		layer[0].depth = 1f;
		layer[1].depth = 1f;
		layer[2].depth = 1.1f;
		layer[3].depth = 0.5f;
		script = new string[128];
		for (int j = 0; j < script.Length; j++)
		{
			script[j] = "";
		}
		col = new int[48, 36];
		brite = 1f;
	}

	public void RefreshDepths(Dictionary<string, XTexture> textures)
	{
		for (int i = 0; i < 4; i++)
		{
			layer[i].RefreshDepths(textures);
		}
	}

	public bool CheckCol(Vector2 t)
	{
		int num = (int)(t.X / 64f);
		int num2 = (int)(t.Y / 64f);
		if (num < 0 || num2 < 0 || num >= 48 || num2 >= 36)
		{
			return true;
		}
		if (col[num, num2] == 1)
		{
			return true;
		}
		return false;
	}

	public void Draw(int startLayer, int endLayer, SpriteBatch sprite, Dictionary<string, XTexture> textures, GlowMgr glowMgr, bool game, float drawScale)
	{
		bool flag = false;
		if (!game)
		{
			doorAlpha[0] = (doorAlpha[1] = (doorAlpha[2] = (doorAlpha[3] = 1f)));
		}
		for (int i = startLayer; i < endLayer; i++)
		{
			Layer layer = this.layer[i];
			float num = brite;
			if (i == 2)
			{
				num *= 0.2f;
			}
			Color color = new Color(num, num, num, 1f);
			for (int j = 0; j < layer.seg.Length; j++)
			{
				Seg seg = layer.seg[j];
				if (seg == null)
				{
					continue;
				}
				XSprite xSprite = textures[seg.texture].cell[seg.idx];
				Vector2 vector = seg.scaling * drawScale;
				color = new Color(num, num, num, 1f);
				bool flag2 = false;
				flag2 = ((!game) ? GetMapNoDraw(textures[seg.texture], seg, i) : GetNoDraw(textures[seg.texture], seg, i));
				if (seg.segRect.Right < drawableRect.Left || seg.segRect.Bottom < drawableRect.Top || seg.segRect.Left > drawableRect.Right || seg.segRect.Top > drawableRect.Bottom)
				{
					flag2 = true;
				}
				if (flag2)
				{
					continue;
				}
				Vector2 screenLoc = ScrollManager.GetScreenLoc(seg.loc * drawScale, seg.depth);
				float num2 = seg.rotation;
				switch (xSprite.flags)
				{
				case 6:
					num2 += (float)Math.Cos(delta * 2f + seg.loc.X / 5f + seg.loc.Y / 5f) * 0.05f;
					break;
				case 7:
					num2 += (float)Math.Cos(delta * 5f + seg.loc.X / 5f + seg.loc.Y / 5f) * 0.15f;
					break;
				case 5:
					glowMgr.Add(1, screenLoc, 0.15f, 0.14f, 0.1f, 5f * ScrollManager.zoom);
					break;
				case 8:
					num2 = (((int)seg.loc.X % 2 != 0) ? (num2 - delta * 0.2f) : (num2 + delta * 0.2f));
					color = new Color(0.65f, 0.65f, 0.65f, 0.55f);
					break;
				case 10:
				{
					float num3 = delta / 6.28f + seg.loc.X * 0.5f + seg.loc.Y * 0.5f;
					num3 -= (float)(int)num3;
					screenLoc = ScrollManager.GetScreenLoc(seg.loc + new Vector2((float)Math.Cos(seg.rotation), (float)Math.Sin(seg.rotation)) * (num3 - 0.5f) * 100f * ScrollManager.zoom, seg.depth);
					num2 = (((int)(seg.loc.X * 0.5f) % 2 != 0) ? (num2 - num3) : (num2 + num3));
					float num4 = (float)Math.Sin(num3 * 3.14f) * 0.5f;
					color = new Color(num4, num4, num4, num4);
					break;
				}
				case 9:
					num2 = (((int)seg.loc.X % 2 != 0) ? (num2 - delta * 0.2f) : (num2 + delta * 0.2f));
					color = new Color((float)Math.Cos(delta + seg.loc.X + seg.loc.Y) * 0.25f + 0.5f, (float)Math.Cos(delta + seg.loc.X * 0.5f + seg.loc.Y) * 0.25f + 0.5f, (float)Math.Cos(delta + seg.loc.X + seg.loc.Y * 0.5f) * 0.25f + 0.5f, 1f);
					break;
				}
				switch (xSprite.flags)
				{
				case 11:
				case 12:
				case 13:
				case 14:
				{
					Vector2 vector2 = default(Vector2);
					float num5 = 0f;
					switch (xSprite.flags)
					{
					case 11:
						vector2.Y = -1f;
						num5 = doorAlpha[0];
						break;
					case 12:
						vector2.X = -1f;
						num5 = doorAlpha[1];
						break;
					case 13:
						num5 = doorAlpha[2];
						vector2.X = 1f;
						break;
					case 14:
						num5 = doorAlpha[3];
						vector2.Y = 1f;
						break;
					}
					if (num5 < 1f)
					{
						glowMgr.Add(1, screenLoc, 0f, 0.4f * (1f - num5), 0f, 2f * ScrollManager.zoom);
					}
					glowMgr.Add(1, screenLoc, 0.4f * num5, 0f, 0f, 2f * ScrollManager.zoom);
					for (int k = 0; k < 3; k++)
					{
						float num6 = delta - (float)(int)delta;
						num6 += (float)k;
						float num7 = 1f;
						num7 = ((!(num6 < 1f)) ? ((!(num6 > 2f)) ? num5 : (num5 * (3f - num6))) : (num6 * num5));
						color = new Color(num7, num7, num7, num7);
						Vector2 position = screenLoc + new Vector2(num6, num6) * vector2 * 8f * vector * ScrollManager.zoom;
						Vector2 vector3 = vector + num6 * vector * ScrollManager.zoom * 0.01f;
						sprite.Draw(textures[seg.texture].texture, position, xSprite.srcRect, color, num2, xSprite.origin - new Vector2(xSprite.srcRect.X, xSprite.srcRect.Y), vector3 * ScrollManager.zoom * seg.depth, SpriteEffects.None, 1f);
					}
					break;
				}
				default:
					if (flag)
					{
						sprite.End();
						sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
						flag = false;
					}
					sprite.Draw(textures[seg.texture].texture, screenLoc, xSprite.srcRect, color, num2, xSprite.origin - new Vector2(xSprite.srcRect.X, xSprite.srcRect.Y), vector * ScrollManager.zoom * seg.depth, SpriteEffects.None, 1f);
					break;
				}
			}
		}
		if (flag)
		{
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		}
	}

	private bool GetMapNoDraw(XTexture xTexture, Seg seg, int l)
	{
		return false;
	}

	private bool GetNoDraw(XTexture xTexture, Seg seg, int l)
	{
		switch (xTexture.cell[seg.idx].flags)
		{
		case 4:
		case 5:
			return true;
		default:
			return false;
		}
	}

	public void Update(GameTime gameTime)
	{
		float num = (float)gameTime.ElapsedGameTime.TotalSeconds;
		delta += num;
		if (delta > 62.82f)
		{
			delta -= 62.82f;
		}
	}

	public void DrawFloor(SpriteBatch sprite)
	{
	}

	public void DrawFore(SpriteBatch sprite)
	{
	}

	public void Write(string path, Dictionary<string, XTexture> textures)
	{
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.Write));
		binaryWriter.Write(xSize);
		binaryWriter.Write(brite);
		binaryWriter.Write(glowBrite);
		for (int i = 0; i < layer.Length; i++)
		{
			layer[i].Write(binaryWriter, textures);
		}
		for (int j = 0; j < 48; j++)
		{
			for (int k = 0; k < 36; k++)
			{
				binaryWriter.Write(col[j, k]);
			}
		}
		int num = 0;
		for (int l = 0; l < script.Length; l++)
		{
			if (script[l] != "")
			{
				num = l + 1;
			}
		}
		binaryWriter.Write(num);
		for (int m = 0; m < num; m++)
		{
			binaryWriter.Write(script[m]);
		}
		binaryWriter.Close();
	}

	public bool TestCol(Vector3 loc)
	{
		int num = (int)(loc.X / 128f);
		int num2 = (int)(loc.Y / 128f);
		if (num >= 0 && num2 >= 0 && num < xSize * 10 && num2 < 10)
		{
			if (col[num, num2] == 1)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	public void Read(string path, Dictionary<string, XTexture> texture)
	{
		BinaryReader binaryReader = new BinaryReader(TitleContainer.OpenStream(path));
		xSize = binaryReader.ReadInt32();
		brite = binaryReader.ReadSingle();
		glowBrite = binaryReader.ReadSingle();
		for (int i = 0; i < 4; i++)
		{
			layer[i].Read(binaryReader, texture);
		}
		for (int j = 0; j < 48; j++)
		{
			for (int k = 0; k < 36; k++)
			{
				col[j, k] = binaryReader.ReadInt32();
			}
		}
		int num = binaryReader.ReadInt32();
		for (int l = 0; l < script.Length; l++)
		{
			if (l < num)
			{
				script[l] = binaryReader.ReadString();
			}
			else
			{
				script[l] = "";
			}
		}
		binaryReader.Close();
		RefreshDepths(texture);
	}

	internal void DrawSeg(SpriteBatch sprite, Dictionary<string, XTexture> textures, int idx, float brite)
	{
		Layer layer = this.layer[0];
		Seg seg = layer.seg[idx];
		XTexture xTexture = textures[seg.texture];
		XSprite xSprite = xTexture.cell[seg.idx];
		sprite.Draw(xTexture.texture, ScrollManager.GetScreenLoc(seg.loc, seg.depth), xSprite.srcRect, new Color(brite, brite, brite, 1f), seg.rotation, xSprite.origin - new Vector2(xSprite.srcRect.X, xSprite.srcRect.Y), seg.scaling * ScrollManager.zoom * seg.depth, SpriteEffects.None, 1f);
	}
}
