using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.vikinggame;

namespace Viking_x86.particles;

public class ParticleMgr
{
	public const float GRAVITY = 20f;

	private Particle[] particle;

	public int count;

	public int FLICKR;

	internal void Reset()
	{
		for (int i = 0; i < particle.Length; i++)
		{
			particle[i].exists = false;
		}
	}

	public ParticleMgr()
	{
		particle = new Particle[512];
		for (int i = 0; i < particle.Length; i++)
		{
			particle[i] = new Particle(i);
		}
		ParticleCatalog.Init();
	}

	public void MakeScrapBomb(Vector2 loc)
	{
		for (int i = 0; i < 8; i++)
		{
			AddParticle(0, loc, Rand.GetRandomVec2(-300f, 300f, -300f, 300f), 0f, 0, 0);
		}
		for (int j = 0; j < 12; j++)
		{
			AddParticle(1, loc, Rand.GetRandomVec2(-300f, 300f, -300f, 300f), 0f, 0, 0);
		}
		AddParticle(2, loc, default(Vector2), 0.7f, 0, 0);
	}

	public void MakePixelBomb(Vector2 loc)
	{
		for (int i = 0; i < 8; i++)
		{
			AddParticle(6, loc, Rand.GetRandomVec2(-300f, 300f, -300f, 300f), 0f, 0, 0);
		}
		for (int j = 0; j < 24; j++)
		{
			AddParticle(7, loc, Rand.GetRandomVec2(-500f, 500f, -500f, 500f), Rand.GetRandomFloat(10f, 20f), 0, 0);
		}
		AddParticle(2, loc, default(Vector2), 0.7f, 0, 0);
	}

	public void MakeGalagaBomb(Vector2 loc)
	{
		for (int i = 0; i < 8; i++)
		{
			AddParticle(6, loc, Rand.GetRandomVec2(-300f, 300f, -300f, 300f), 0f, 0, 0);
		}
		for (int j = 0; j < 8; j++)
		{
			AddParticle(7, loc, Rand.GetRandomVec2(-500f, 500f, -500f, 500f), Rand.GetRandomFloat(5f, 10f), 0, 0);
		}
		AddParticle(2, loc, default(Vector2), 0.7f, 0, 0);
	}

	public void MakeGiantBomb(Vector2 loc)
	{
		for (int i = 0; i < 16; i++)
		{
			AddParticle(8, loc, Rand.GetRandomVec2(-300f, 300f, -300f, 300f), 0f, 0, 0);
		}
		for (int j = 0; j < 24; j++)
		{
			AddParticle(7, loc, Rand.GetRandomVec2(-500f, 500f, -500f, 500f), Rand.GetRandomFloat(10f, 20f), 0, 0);
		}
		AddParticle(2, loc, default(Vector2), 2f, 0, 0);
		VikingQuake.SetQuake(0.5f);
	}

	public void AddParticle(int type, Vector2 loc, Vector2 traj, float size, int flags, int owner)
	{
		for (int i = 0; i < particle.Length; i++)
		{
			if (!particle[i].exists)
			{
				particle[i].alpha = false;
				particle[i].type = type;
				ParticleCatalog.particle[type].Init(particle[i], loc, traj, size, flags, owner);
				break;
			}
		}
	}

	internal void Update()
	{
		for (int i = 0; i < particle.Length; i++)
		{
			if (particle[i].exists)
			{
				ParticleCatalog.particle[particle[i].type].Update(particle[i]);
			}
		}
	}

	internal void Draw(SpriteBatch spriteBatch, bool alpha)
	{
		for (int i = 0; i < particle.Length; i++)
		{
			if (particle[i].exists && particle[i].alpha == alpha)
			{
				Vector2 screenLoc = VScroll.GetScreenLoc(particle[i].loc, 1f);
				if (screenLoc.X > 0f && screenLoc.Y > 0f && screenLoc.X < VScroll.screenSize.X && screenLoc.Y < VScroll.screenSize.Y)
				{
					ParticleCatalog.particle[particle[i].type].Draw(spriteBatch, particle[i], screenLoc);
				}
			}
		}
	}
}
