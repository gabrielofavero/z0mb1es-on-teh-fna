using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.particles.particles;

public class MuzzleFlash : BaseParticleDef
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, int owner, float size, int flags)
	{
		p.frame = 0.1f;
		p.loc = loc;
		p.traj = traj;
		p.owner = -1;
		p.size = size;
		p.flags = Rand.GetRandomInt(0, 2);
		p.alpha = true;
		p.g = Rand.GetRandomFloat(-10f, 10f);
		base.Init(p, loc, traj, owner, size, flags);
	}

	public override void Update(Particle p, float frameTime)
	{
		p.angle += p.g * FMan.frameTime;
		base.Update(p, frameTime);
	}

	public override void Draw(Particle p, Vector2 loc, SpriteBatch sprite)
	{
		sprite.Draw(ZombieGame.spritesTex, loc, ScaleTools.ScaledRect(p.flags * 128, 320, 128, 128), new Color(1f, p.frame * 40f, p.frame * 20f, p.frame * 10f), p.angle, new Vector2(64f, 64f) / 2f, p.size * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
	}
}
