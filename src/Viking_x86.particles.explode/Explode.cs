using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace Viking_x86.particles.explode;

public class Explode : BaseParticle
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, float size, int flags, int owner)
	{
		p.loc = loc;
		p.traj = traj;
		p.size = size;
		p.alpha = true;
		p.frame = 0.2f;
		base.Init(p, loc, traj, size, flags, owner);
	}

	public override void Update(Particle p)
	{
		base.Update(p);
	}

	public override void Draw(SpriteBatch sprite, Particle p, Vector2 pLoc)
	{
		if ((int)(p.frame * 30f) % 2 == 0)
		{
			sprite.Draw(Game1.vgame.spritesTex, pLoc, ScaleTools.ScaledRect(0, 0, 128, 128), new Color(1f, 1f, 1f, 1f), 0f, new Vector2(64f, 64f) / 2f, p.size * VScroll.zoom * 2f, SpriteEffects.None, 1f);
		}
	}
}
