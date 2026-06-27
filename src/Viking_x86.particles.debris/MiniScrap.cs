using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace Viking_x86.particles.debris;

public class MiniScrap : BaseParticle
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, float size, int flags, int owner)
	{
		p.loc = loc;
		p.traj = traj;
		p.size = 0.15f;
		p.flags = Rand.GetRandomInt(8, 13);
		p.owner = owner;
		p.frame = 1f;
		p.rotationSpeed = Rand.GetRandomFloat(-35f, 35f);
		base.Init(p, loc, traj, size, flags, owner);
	}

	public override void Update(Particle p)
	{
		p.traj.Y += 20f;
		p.angle += p.rotationSpeed * Game1.frameTime;
		if (p.ID > 100)
		{
			p.exists = false;
		}
		if (Game1.vgame.world.TestCollision(p.loc))
		{
			Game1.vgame.world.AddDebris(p.loc, Rand.GetRandomInt(0, 4));
			Game1.vgame.pMgr.AddParticle(3, p.loc, Rand.GetRandomVec2(-10f, 10f, -40f, 0f), Rand.GetRandomFloat(0.2f, 0.5f), 0, 0);
			p.exists = false;
		}
		base.Update(p);
	}

	public override void Draw(SpriteBatch sprite, Particle p, Vector2 pLoc)
	{
		float foreBright = Game1.vgame.world.GetForeBright();
		sprite.Draw(VikingGame.textures["scrap"].texture, pLoc, VikingGame.textures["scrap"].GetSpriteRect(p.flags), new Color(foreBright, foreBright, foreBright, 1f), p.angle, VikingGame.textures["scrap"].GetRelativeSpriteOrigin(p.flags), p.size * VScroll.zoom * 2f, SpriteEffects.None, 1f);
	}
}
