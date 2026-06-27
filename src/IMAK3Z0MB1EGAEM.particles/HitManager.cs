using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.character;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using Viking_x86;
using ZombiesWP7.debug;

namespace IMAK3Z0MB1EGAEM.particles;

public class HitManager
{
	public static void HitMonster(Monster m, Vector2 traj, Vector2 loc, int owner)
	{
		int num = 10;
		if (ParticleMan.count > 10)
		{
			num = 8;
		}
		if (ParticleMan.count > 20)
		{
			num = 6;
		}
		if (ParticleMan.count > 30)
		{
			num = 4;
		}
		if (ParticleMan.count > 40)
		{
			num = 2;
		}
		if (ParticleMan.count > 50)
		{
			num = 1;
		}
		if (ParticleMan.count > 60)
		{
			num = 0;
		}
		switch (m.type)
		{
		case 0:
			MakeBloodChunks(m.loc, traj, num);
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 100L, kill: true);
			}
			m.exists = false;
			if (Rand.CoinToss(0.01f))
			{
				SpawnMgr.MakeGoodie(m.loc);
			}
			break;
		case 10:
		{
			for (int num2 = 0; num2 < num; num2++)
			{
				ParticleMan.AddParticle(15, m.loc, Rand.GetRandomVec2(-200f, 200f, -200f, 200f), 0, Rand.GetRandomFloat(0.1f, 0.3f), 0);
			}
			m.exists = false;
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 120L, kill: true);
			}
			if (Rand.CoinToss(0.02f))
			{
				SpawnMgr.MakeGoodie(m.loc);
			}
			break;
		}
		case 9:
		{
			for (int num6 = 0; num6 < num; num6++)
			{
				ParticleMan.AddParticle(7, m.loc, Rand.GetRandomVec2(-200f, 200f, -200f, 200f), 0, Rand.GetRandomFloat(0.3f, 0.5f), 0);
			}
			m.exists = false;
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 150L, kill: true);
			}
			if (Rand.CoinToss(0.01f))
			{
				SpawnMgr.MakeGoodie(m.loc);
			}
			break;
		}
		case 8:
			m.hp--;
			if (m.hp <= 0)
			{
				for (int num5 = 0; num5 < num; num5++)
				{
					ParticleMan.AddParticle(10, m.loc, Rand.GetRandomVec2(-200f, 200f, -200f, 200f), 0, Rand.GetRandomFloat(1.3f, 2f), 0);
				}
				Sound.PlayExplode();
				if (owner == 0)
				{
					CharMan.hero[owner].AddPoints(m.loc, 500L, kill: true);
				}
				m.exists = false;
				if (Rand.CoinToss(0.1f))
				{
					SpawnMgr.MakeGoodie(m.loc);
				}
			}
			else
			{
				MakeBloodSplode(loc, num / 3, 0.2f, 150f);
				m.loc += traj * 0.002f;
			}
			break;
		case 1:
			m.hp--;
			if (m.hp <= 0)
			{
				MakeBloodChunks(m.loc, traj, num);
				MakeBloodSplode(loc, num, Rand.GetRandomFloat(0.5f, 1f), 300f);
				if (owner == 0)
				{
					CharMan.hero[owner].AddPoints(m.loc, 500L, kill: true);
				}
				m.exists = false;
				if (Rand.CoinToss(0.4f))
				{
					SpawnMgr.MakeGoodie(m.loc);
				}
			}
			else
			{
				MakeBloodSplode(loc, num / 4, 0.2f, 150f);
				m.loc += traj * 0.002f;
			}
			break;
		case 2:
		{
			m.exists = false;
			for (int num4 = 0; num4 < 4; num4++)
			{
				CharMan.MakeMonster(m.loc, 3);
			}
			MakePixelSplode(loc, num, Rand.GetRandomFloat(0.4f, 1f), 500f);
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 200L, kill: true);
			}
			if (Rand.CoinToss(0.1f))
			{
				SpawnMgr.MakeGoodie(m.loc);
			}
			break;
		}
		case 3:
		{
			m.exists = false;
			for (int num3 = 0; num3 < 3; num3++)
			{
				CharMan.MakeMonster(m.loc, 4);
			}
			MakePixelSplode(loc, num / 2, Rand.GetRandomFloat(0.3f, 0.7f), 300f);
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 150L, kill: true);
			}
			if (Rand.CoinToss(0.02f))
			{
				SpawnMgr.MakeGoodie(m.loc);
			}
			break;
		}
		case 4:
			m.exists = false;
			MakePixelSplode(loc, num / 2, Rand.GetRandomFloat(0.3f, 0.4f), 200f);
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 50L, kill: true);
			}
			if (Rand.CoinToss(0.02f))
			{
				SpawnMgr.MakeGoodie(m.loc);
			}
			break;
		case 5:
			m.hp--;
			if (m.hp <= 0)
			{
				MakeGoo(loc, num, Rand.GetRandomFloat(0.5f, 1f), 300f);
				if (owner == 0)
				{
					CharMan.hero[owner].AddPoints(m.loc, 800L, kill: true);
				}
				m.exists = false;
				if (Rand.CoinToss(0.4f))
				{
					SpawnMgr.MakeGoodie(m.loc);
				}
			}
			else
			{
				MakeGoo(loc, num / 3, 0.2f, 50f);
				m.loc += traj * 0.002f;
				for (int n = 0; n < 3; n++)
				{
					CharMan.MakeMonster(m.loc + Rand.GetRandomVec2(-20f, 20f, -20f, 20f), 6, midSpawn: true);
				}
			}
			break;
		case 6:
			m.exists = false;
			MakeGoo(loc, num / 2, Rand.GetRandomFloat(0.3f, 0.4f), 200f);
			if (owner == 0)
			{
				CharMan.hero[owner].AddPoints(m.loc, 50L, kill: true);
			}
			break;
		case 7:
			m.hp--;
			if (m.hp <= 0)
			{
				m.exists = false;
				for (int i = 0; i < num; i++)
				{
					ParticleMan.AddParticle(7, m.loc, Rand.GetRandomVec2(-500f, 500f, -500f, 500f), 0, 0f, 0);
				}
				for (int j = 0; j < ParticleMan.particle.Length; j++)
				{
					if (ParticleMan.particle[j].exists && ParticleMan.particle[j].type == 6 && ParticleMan.particle[j].owner == m.idx)
					{
						for (int k = 0; k < num / 3; k++)
						{
							ParticleMan.AddParticle(7, ParticleMan.particle[j].loc, Rand.GetRandomVec2(-200f, 200f, -200f, 200f), 0, 0f, 0);
						}
						ParticleMan.particle[j].exists = false;
					}
				}
				if (Rand.CoinToss(0.4f))
				{
					SpawnMgr.MakeGoodie(m.loc);
				}
				if (owner == 0)
				{
					CharMan.hero[owner].AddPoints(m.loc, 600L, kill: true);
				}
			}
			else
			{
				for (int l = 0; l < num / 2; l++)
				{
					ParticleMan.AddParticle(7, m.loc, Rand.GetRandomVec2(-500f, 500f, -500f, 500f), 0, 0f, 0);
				}
			}
			break;
		}
	}

	public static void CheckHeroSmash(Monster m)
	{
		Vector2 loc = m.loc;
		for (int i = 0; i < CharMan.hero.Length; i++)
		{
			if (!CharMan.hero[i].exists || !(CharMan.hero[i].respawnFrame <= 0f))
			{
				continue;
			}
			Hero hero = CharMan.hero[i];
			float num = 20f;
			if (CharMan.hero[i].spawnFrame > 0f)
			{
				num = 50f;
			}
			if (hero.loc.X > loc.X - num && hero.loc.X < loc.X + num && hero.loc.Y > loc.Y - num && hero.loc.Y < loc.Y + num)
			{
				if (CharMan.hero[i].spawnFrame > 0f)
				{
					HitMonster(m, m.loc - CharMan.hero[i].loc, m.loc, i);
				}
				else if (!DebugMgr.godmode)
				{
					hero.Kill();
				}
			}
		}
	}

	private static void MakeBloodChunks(Vector2 loc, Vector2 traj, int total)
	{
		for (int i = 0; i < total; i++)
		{
			ParticleMan.AddParticle(0, loc, Rand.GetRandomVec2(-150f, 150f, -150f, 150f) + traj * Rand.GetRandomFloat(0f, 0.3f), 0, Rand.GetRandomFloat(0.2f, 0.5f), 0);
			ParticleMan.AddParticle(0, loc, Rand.GetRandomVec2(-150f, 150f, -150f, 150f), 0, Rand.GetRandomFloat(0.2f, 0.5f), 0);
		}
	}

	public static void MakeBloodSplode(Vector2 loc, int reps, float size, float traj)
	{
		for (int i = 0; i < reps; i++)
		{
			ParticleMan.AddParticle(0, loc, Rand.GetRandomVec2(0f - traj, traj, 0f - traj, traj), 0, size, 0);
		}
	}

	private static void MakePixelSplode(Vector2 loc, int reps, float size, float traj)
	{
		for (int i = 0; i < reps; i++)
		{
			ParticleMan.AddParticle(4, loc, Rand.GetRandomVec2(0f - traj, traj, 0f - traj, traj), 0, size, 0);
		}
	}

	private static void MakeGoo(Vector2 loc, int reps, float size, float traj)
	{
		for (int i = 0; i < reps; i++)
		{
			ParticleMan.AddParticle(5, loc, Rand.GetRandomVec2(0f - traj, traj, 0f - traj, traj), 0, size, 0);
		}
	}
}
