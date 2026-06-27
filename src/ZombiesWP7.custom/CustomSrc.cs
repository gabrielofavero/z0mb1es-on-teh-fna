using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;

namespace ZombiesWP7.custom;

public class CustomSrc
{
	private Texture2D texture;

	public string ID;

	public int imgIdx = -1;

	public bool cached;

	private GraphicsDevice gd;

	private Picture pic;

	public Texture2D GetTexture()
	{
		if (!cached)
		{
			texture = Texture2D.FromStream(gd, pic.GetThumbnail());
		}
		return texture;
	}

	public CustomSrc(string ID, GraphicsDevice gd, Picture pic)
	{
		this.gd = gd;
		this.pic = pic;
		this.ID = ID;
		cached = false;
	}

	public CustomSrc(string ID, Texture2D texture)
	{
		this.ID = ID;
		this.texture = texture;
		cached = true;
	}
}
