using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace Viking_x86.main;

public class MenuBlip
{
	public Vector2 loc;

	public Vector2 traj;

	public float frame;

	public float size;

	public float r;

	public float g;

	public float b;

	public bool exists;

	internal void Init(Vector2 loc, Vector2 traj, float size, float a)
	{
		this.loc = loc;
		this.traj = traj;
		this.size = size;
		r = Rand.GetRandomFloat(0f, 1f) * a;
		g = Rand.GetRandomFloat(0f, 1f) * a;
		b = Rand.GetRandomFloat(0f, 1f) * a;
		frame = 0.5f;
		exists = true;
	}

	public void Update()
	{
		loc += traj * Game1.frameTime;
		frame -= Game1.frameTime;
		if (frame < 0f)
		{
			exists = false;
		}
	}

	public void Draw(SpriteBatch sprite)
	{
		float a = 1f;
		if (frame > 0.25f)
		{
			a = (0.5f - frame) * 4f;
		}
		if (frame < 0.25f)
		{
			a = frame * 4f;
		}
		sprite.Draw(Game1.nullTex, loc, new Rectangle(0, 0, 1, 1), new Color(r, g, b, a), 0f, new Vector2(0.5f, 0.5f), size, SpriteEffects.None, 1f);
	}
}
