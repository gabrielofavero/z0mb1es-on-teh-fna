using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;

namespace IMAK3Z0MB1EGAEM.particles.particles;

public class Score : BaseParticleDef
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, int owner, float size, int flags)
	{
		p.frame = 0.5f;
		p.loc = loc;
		p.traj = new Vector2(0f, -350f);
		p.owner = -1;
		p.size = size;
		p.flags = flags;
		p.g = Rand.GetRandomFloat(-10f, 10f);
		base.Init(p, loc, traj, owner, size, flags);
	}

	public override void Update(Particle p, float frameTime)
	{
		if (p.frame < 0.4f)
		{
			p.traj.Y = 0f;
		}
		base.Update(p, frameTime);
	}

	public override void Draw(Particle p, Vector2 loc, SpriteBatch sprite)
	{
		Text.DrawScore(sprite, p.flags, loc, 2f * ScrollMan.zoom, new Color(Rand.GetRandomFloat(0.75f, 1f), Rand.GetRandomFloat(0.75f, 1f), Rand.GetRandomFloat(0.75f, 1f), 0.8f), Text.Justify.Center);
	}
}
