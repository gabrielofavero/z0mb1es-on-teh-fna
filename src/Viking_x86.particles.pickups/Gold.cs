using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace Viking_x86.particles.pickups;

public class Gold : BaseParticle
{
	public override void Init(Particle p, Vector2 loc, Vector2 traj, float size, int flags, int owner)
	{
		p.loc = loc;
		p.traj = traj;
		p.size = size;
		p.owner = owner;
		p.frame = 5f;
		base.Init(p, loc, traj, size, flags, owner);
	}

	public override void Update(Particle p)
	{
		if (p.loc.X > Game1.vgame.charMgr.character[0].loc.X - 20f && p.loc.X < Game1.vgame.charMgr.character[0].loc.X + 20f && p.loc.Y > Game1.vgame.charMgr.character[0].loc.Y - 60f && p.loc.Y < Game1.vgame.charMgr.character[0].loc.Y)
		{
			Game1.vgame.charMgr.character[0].AddScore(1000);
			Game1.vgame.pMgr.AddParticle(19, p.loc, default(Vector2), 0f, 1000, 0);
			for (int i = 0; i < 8; i++)
			{
				Game1.vgame.pMgr.AddParticle(14, p.loc, Rand.GetRandomVec2(-50f, 50f, -50f, 50f), Rand.GetRandomFloat(0.5f, 15f), 0, 0);
			}
			p.exists = false;
		}
		base.Update(p);
	}

	public override void Draw(SpriteBatch sprite, Particle p, Vector2 pLoc)
	{
		Vector2 vector = new Vector2((float)Math.Cos(p.frame * 10f), 1f);
		Color color = new Color(1f, 1f, 0f, 1f);
		if (vector.X < 0f)
		{
			vector.X = 0f - vector.X;
			color = new Color(1f, 0.65f, 0f, 1f);
		}
		vector.X = vector.X * 0.8f + 0.2f;
		sprite.Draw(Game1.vgame.spritesTex, pLoc, ScaleTools.ScaledRect(0, 0, 128, 128), color, VScroll.angle, new Vector2(64f, 64f) / 2f, VScroll.zoom * vector * 0.2f * 2f, SpriteEffects.None, 1f);
		vector.X -= 0.5f;
		if (vector.X > 0f)
		{
			float num;
			for (num = p.frame * 10f; num > 3.14f; num -= 3.14f)
			{
			}
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(p.loc + new Vector2(num - 1.57f, num - 1.57f), 1f), ScaleTools.ScaledRect(128, 64, 128, 128), new Color(1f, 1f, 1f, vector.X), VScroll.angle, new Vector2(64f, 64f) / 2f, VScroll.zoom * vector.X * 0.5f * 2f, SpriteEffects.None, 1f);
		}
	}
}
