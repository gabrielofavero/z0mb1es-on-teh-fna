using IMAK3Z0MB1EGAEM.character;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using Yuki_Win;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.particles.particles;

public class Laser : BaseParticleDef
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, int owner, float size, int flags)
	{
		p.frame = 0.5f;
		p.loc = loc;
		p.traj = traj;
		p.owner = owner;
		p.size = Rand.GetRandomFloat(0f, 0.5f);
		p.angle = Trig.GetAngle(default(Vector2), traj);
		p.flags = 0;
		p.alpha = true;
		base.Init(p, loc, traj, owner, size, flags);
	}

	public override void Update(Particle p, float frameTime)
	{
		for (int i = 0; i < 3; i++)
		{
			Monster[] monster = CharMan.monster;
			foreach (Monster monster2 in monster)
			{
				if (!monster2.exists || !(monster2.spawnFrame <= 1f) || !(monster2.loc.X > p.loc.X - 32f) || !(monster2.loc.Y > p.loc.Y - 32f) || !(monster2.loc.X < p.loc.X + 32f) || !(monster2.loc.Y < p.loc.Y + 32f) || !((monster2.loc - p.loc).Length() < 32f))
				{
					continue;
				}
				HitManager.HitMonster(monster2, p.traj, p.loc, p.owner);
				if (!monster2.exists)
				{
					p.flags++;
					if (p.flags == 20)
					{
						Game1.achievementMgr.AwardAchievement(6);
					}
				}
			}
			base.Update(p, frameTime);
		}
	}

	public override void Draw(Particle p, Vector2 loc, SpriteBatch sprite)
	{
		sprite.Draw(ZombieGame.spritesTex, ScrollMan.GetScreenLoc(p.loc, 1f), ScaleTools.ScaledRect(0, 704, 128, 64), new Color(0.7f, 0.7f, 1f, p.frame * 10f), p.angle, new Vector2(0f, 32f) / 2f, new Vector2(15f * (0.5f - p.frame), 0.5f) * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
	}
}
