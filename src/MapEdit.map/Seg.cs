using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using SheetEdit.TextureSheet;

namespace MapEdit.map;

public class Seg
{
	public Vector2 loc;

	public int idx;

	public string texture;

	public float rotation;

	public Vector2 scaling;

	public Vector3 cVec;

	public int flag;

	public string clothes;

	public string food;

	public float depth;

	public Rectangle segRect;

	internal void CopyFrom(Seg seg)
	{
		if (seg != null)
		{
			try
			{
				loc = seg.loc;
				idx = seg.idx;
				texture = seg.texture;
				rotation = seg.rotation;
				scaling = seg.scaling;
				cVec = seg.cVec;
				clothes = seg.clothes;
				food = seg.food;
				flag = seg.flag;
			}
			catch
			{
			}
		}
	}

	internal void Read(BinaryReader reader, Dictionary<string, XTexture> t)
	{
		idx = reader.ReadInt32();
		loc = new Vector2(reader.ReadSingle(), reader.ReadSingle());
		rotation = reader.ReadSingle();
		scaling = new Vector2(reader.ReadSingle(), reader.ReadSingle());
		texture = reader.ReadString();
		cVec = new Vector3(-1f, -1f, -1f);
		GetSegRect(t);
	}

	internal void GetSegRect(Dictionary<string, XTexture> textures)
	{
		Vector2 vector = loc * 1.2f;
		XSprite xSprite = textures[texture].cell[idx];
		Vector2 vector2 = new Vector2((float)Math.Cos(rotation) * ((float)xSprite.srcRect.Right - xSprite.origin.X) * scaling.X, (float)Math.Sin(rotation) * ((float)xSprite.srcRect.Right - xSprite.origin.X) * scaling.X) * 1.2f;
		Vector2 vector3 = new Vector2((float)Math.Cos(rotation + 1.57f) * ((float)xSprite.srcRect.Bottom - xSprite.origin.Y) * scaling.Y, (float)Math.Sin(rotation + 1.57f) * ((float)xSprite.srcRect.Bottom - xSprite.origin.Y) * scaling.Y) * 1.2f;
		Vector2 vector4 = new Vector2((float)Math.Cos(rotation) * (xSprite.origin.X - (float)xSprite.srcRect.X) * scaling.X, (float)Math.Sin(rotation) * (xSprite.origin.X - (float)xSprite.srcRect.X) * scaling.X) * 1.2f;
		Vector2 vector5 = new Vector2((float)Math.Cos(rotation + 1.57f) * (xSprite.origin.Y - (float)xSprite.srcRect.Y) * scaling.Y, (float)Math.Sin(rotation + 1.57f) * (xSprite.origin.Y - (float)xSprite.srcRect.Y) * scaling.Y) * 1.2f;
		Vector2 vector6 = vector - vector4 - vector5;
		Vector2 vector7 = vector + vector2 - vector5;
		Vector2 vector8 = vector - vector4 + vector3;
		Vector2 vector9 = vector + vector2 + vector3;
		Vector2 vector10 = vector;
		Vector2 vector11 = vector;
		if (vector7.X < vector10.X)
		{
			vector10.X = vector7.X;
		}
		if (vector7.Y < vector10.Y)
		{
			vector10.Y = vector7.Y;
		}
		if (vector8.X < vector10.X)
		{
			vector10.X = vector8.X;
		}
		if (vector8.Y < vector10.Y)
		{
			vector10.Y = vector8.Y;
		}
		if (vector9.X < vector10.X)
		{
			vector10.X = vector9.X;
		}
		if (vector9.Y < vector10.Y)
		{
			vector10.Y = vector9.Y;
		}
		if (vector6.X < vector10.X)
		{
			vector10.X = vector6.X;
		}
		if (vector6.Y < vector10.Y)
		{
			vector10.Y = vector6.Y;
		}
		if (vector7.X > vector11.X)
		{
			vector11.X = vector7.X;
		}
		if (vector7.Y > vector11.Y)
		{
			vector11.Y = vector7.Y;
		}
		if (vector8.X > vector11.X)
		{
			vector11.X = vector8.X;
		}
		if (vector8.Y > vector11.Y)
		{
			vector11.Y = vector8.Y;
		}
		if (vector9.X > vector11.X)
		{
			vector11.X = vector9.X;
		}
		if (vector9.Y > vector11.Y)
		{
			vector11.Y = vector9.Y;
		}
		if (vector6.X > vector11.X)
		{
			vector11.X = vector6.X;
		}
		if (vector6.Y > vector11.Y)
		{
			vector11.Y = vector6.Y;
		}
		segRect = new Rectangle((int)vector10.X, (int)vector10.Y, (int)(vector11.X - vector10.X), (int)(vector11.Y - vector10.Y));
	}

	internal void Write(BinaryWriter writer, Dictionary<string, XTexture> t)
	{
		writer.Write(idx);
		writer.Write(loc.X);
		writer.Write(loc.Y);
		writer.Write(rotation);
		writer.Write(scaling.X);
		writer.Write(scaling.Y);
		writer.Write(texture);
	}
}
