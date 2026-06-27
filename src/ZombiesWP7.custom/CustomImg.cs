using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombiesWP7.custom;

public class CustomImg
{
	public Vector2 point = new Vector2(50f, 50f);

	public float size = 0.2f;

	public Texture2D texture;

	private Color[] bits;

	public bool enabled;

	public int srcIdx = -1;

	public string ID;

	public float GetScale()
	{
		return 100f / ((size + 0.1f) * 200f);
	}

	public void Process(CustomSrc src, GraphicsDevice gd)
	{
		ID = src.ID;
		Texture2D texture2D = src.GetTexture();
		if (texture != null && !texture.IsDisposed)
		{
			texture.Dispose();
			while (!texture.IsDisposed)
			{
			}
		}
		texture = new Texture2D(gd, texture2D.Width, texture2D.Height, mipMap: false, texture2D.Format);
		if (bits == null)
		{
			bits = new Color[texture2D.Width * texture2D.Height];
		}
		texture2D.GetData(bits, 0, bits.Length);
		float num = (0.1f + size) * 100f;
		for (int i = 0; i < texture2D.Height; i++)
		{
			for (int j = 0; j < texture2D.Width; j++)
			{
				int num2 = j + i * texture2D.Width;
				float num3 = new Vector2((float)j - point.X, (float)i - point.Y).Length();
				if (num3 > num - 2f)
				{
					float num4 = num3 - (num - 2f);
					num4 = (2f - num4) / 2f;
					if (num4 < 0f)
					{
						bits[num2].A = 0;
					}
					else
					{
						bits[num2].A = (byte)(num4 * 255f);
					}
				}
			}
		}
		texture.SetData(bits, 0, bits.Length);
		enabled = true;
	}

	internal void Read(BinaryReader r)
	{
		if (r.ReadByte() == 1)
		{
			enabled = true;
			point.X = r.ReadByte();
			point.Y = r.ReadByte();
			size = (float)r.ReadByte() / 200f;
			int num = r.ReadByte();
			byte[] array = r.ReadBytes(num);
			ID = Encoding.UTF8.GetString(array, 0, num);
			srcIdx = -1;
		}
		else
		{
			enabled = false;
		}
	}

	internal void Write(BinaryWriter w)
	{
		if (enabled)
		{
			w.Write((byte)1);
			if (point.X < 0f)
			{
				point.X = 0f;
			}
			if (point.Y < 0f)
			{
				point.Y = 0f;
			}
			if (point.X > 100f)
			{
				point.X = 100f;
			}
			if (point.Y > 100f)
			{
				point.Y = 100f;
			}
			if (size < 0f)
			{
				size = 0f;
			}
			if (size > 1f)
			{
				size = 1f;
			}
			w.Write((byte)point.X);
			w.Write((byte)point.Y);
			w.Write((byte)(size * 200f));
			w.Write((byte)ID.Length);
			w.Write(Encoding.UTF8.GetBytes(ID));
		}
		else
		{
			w.Write((byte)0);
		}
	}
}
