using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Yuki_Win;
using ZombiesWP7;

namespace Viking_x86.particles.evilzaps;

public class NekoZap : BaseParticle
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, float size, int flags, int owner)
	{
		p.loc = loc;
		p.traj = traj;
		p.angle = Trig.GetAngle(default(Vector2), traj);
		p.alpha = true;
		p.owner = owner;
		p.frame = 3f;
		base.Init(p, loc, traj, size, flags, owner);
	}

	public override void Update(Particle p)
	{
		if (HitManager.CheckHit(p))
		{
			p.exists = false;
		}
		else if (Game1.vgame.world.TestCollision(p.loc))
		{
			p.exists = false;
		}
		if (!p.exists)
		{
			Game1.vgame.pMgr.AddParticle(12, p.loc, p.traj * Rand.GetRandomFloat(-0.5f, 0f), 0.5f, 0, 0);
		}
		base.Update(p);
	}

	public override void Draw(SpriteBatch sprite, Particle p, Vector2 pLoc)
	{
		sprite.Draw(Game1.vgame.spritesTex, pLoc, ScaleTools.ScaledRect(128 + Rand.GetRandomInt(0, 6) * 128, 0, 128, 64), new Color(1f, ((int)(p.frame * 30f) % 2 == 0) ? 1f : 0f, 0f, 1f), p.angle + VScroll.angle + 3.14f, new Vector2(32f, 32f) / 2f, 0.5f * VScroll.zoom * 2f, SpriteEffects.None, 1f);
	}
}
