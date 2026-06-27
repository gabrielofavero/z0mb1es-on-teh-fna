using Microsoft.Xna.Framework;

namespace ZombiesWP7;

internal class ScaleTools
{
	public const float F_DIV = 2f;

	public const int DIV = 2;

	public const float MULT = 2f;

	public static Rectangle ScaledRect(int x, int y, int width, int height)
	{
		return new Rectangle(x / 2, y / 2, width / 2, height / 2);
	}

	public static Rectangle ScaledRect(Rectangle r)
	{
		r.X /= 2;
		r.Y /= 2;
		r.Width /= 2;
		r.Height /= 2;
		return r;
	}
}
