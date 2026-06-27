using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.particles;

public class ParticleMan
{
	public static Particle[] particle;

	private static ParticleCatalog catalog;

	private static int addIdx;

	public static int count;

	public static void Init()
	{
		particle = new Particle[2048];
		catalog = new ParticleCatalog();
		for (int i = 0; i < particle.Length; i++)
		{
			particle[i] = new Particle();
		}
	}

	public static void AddParticle(int type, Vector2 loc, Vector2 traj, int owner, float size, int flags)
	{
		for (int i = 0; i < particle.Length; i++)
		{
			if (!particle[i].exists)
			{
				particle[i].alpha = false;
				catalog.catalog[type].Init(particle[i], loc, traj, owner, size, flags);
				particle[i].type = type;
				addIdx = i + 1;
				break;
			}
		}
	}

	public static void Update()
	{
		addIdx = 0;
		count = 0;
		for (int i = 0; i < ParticleMan.particle.Length; i++)
		{
			Particle particle = ParticleMan.particle[i];
			if (!particle.exists)
			{
				continue;
			}
			float frameTime = Game1.frameTime;
			switch (particle.type)
			{
			case 1:
			case 11:
			case 12:
			case 14:
				catalog.catalog[particle.type].Update(particle, frameTime / 2f);
				if (particle.exists)
				{
					catalog.catalog[particle.type].Update(particle, frameTime / 2f);
				}
				break;
			default:
				catalog.catalog[particle.type].Update(particle, frameTime);
				break;
			}
			count++;
			if (!float.IsNaN(particle.loc.X) && !float.IsNaN(particle.loc.Y) && !float.IsNaN(particle.frame) && !float.IsNaN(particle.traj.X) && !float.IsNaN(particle.traj.Y))
			{
				float.IsNaN(particle.angle);
			}
		}
	}

	public static void Draw(SpriteBatch sprite, bool alpha)
	{
		for (int i = 0; i < ParticleMan.particle.Length; i++)
		{
			Particle particle = ParticleMan.particle[i];
			if (particle.exists)
			{
				Vector2 screenLoc = ScrollMan.GetScreenLoc(particle.loc, 1f);
				if ((particle.type == 14 || (screenLoc.X > 0f && screenLoc.Y > 0f && screenLoc.X < ScrollMan.screenSize.X && screenLoc.Y < ScrollMan.screenSize.Y)) && particle.alpha == alpha)
				{
					catalog.catalog[particle.type].Draw(particle, screenLoc, sprite);
				}
			}
		}
	}
}
