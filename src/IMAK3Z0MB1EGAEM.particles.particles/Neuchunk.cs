using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using Yuki_Win;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.particles.particles;

public class Neuchunk : BaseParticleDef
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, int owner, float size, int flags)
	{
		p.frame = 0.5f;
		p.loc = loc;
		p.traj = traj;
		p.owner = owner;
		p.size = Rand.GetRandomFloat(0f, 0.5f);
		p.angle = Trig.GetAngle(default(Vector2), traj);
		p.flags = 3;
		p.alpha = true;
		base.Init(p, loc, traj, owner, size, flags);
	}

	public override void Draw(Particle p, Vector2 loc, SpriteBatch sprite)
	{
		for (int i = 0; i < 2; i++)
		{
			sprite.Draw(ZombieGame.spritesTex, loc, ScaleTools.ScaledRect(Rand.GetRandomInt(0, 2) * 128, 320, 128, 128), new Color(0.7f, 1f, 0.7f, p.frame), Rand.GetRandomFloat(0f, 6.28f), new Vector2(64f, 64f) / 2f, new Vector2(0.5f, 0.1f) * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
		}
	}
}
