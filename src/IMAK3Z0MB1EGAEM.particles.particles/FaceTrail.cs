using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.particles.particles;

public class FaceTrail : BaseParticleDef
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, int owner, float size, int flags)
	{
		p.frame = 1f;
		p.loc = loc;
		p.traj = traj;
		p.owner = owner;
		base.Init(p, loc, traj, owner, size, flags);
	}

	public override void Draw(Particle p, Vector2 loc, SpriteBatch sprite)
	{
		sprite.Draw(ZombieGame.spritesTex, loc, ScaleTools.ScaledRect(128, 1024, 128, 128), new Color(1f, 1f, 1f, p.frame * 5f), 0f, new Vector2(64f, 64f) / 2f, 0.25f * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
	}
}
