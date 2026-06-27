using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;
using ZombiesWP7.audio;

namespace Viking_x86.vikinggame.character;

public class Moon
{
	public const float HP_MAX = 500f;

	private const float DIST_SQR = 4356578f / (float)Math.PI;

	public const float VALID_HEIGHT = 300f;

	public Vector2 loc;

	public float hp;

	private float dyingFrame;

	private float hitFrame;

	public bool active;

	private double hitStartTime;

	public bool CheckHit(Vector2 v)
	{
		if (hp < 0f)
		{
			return false;
		}
		double num = 857.0;
		bool flag = (v - loc).LengthSquared() < 1331741.8f;
		if (flag)
		{
			if (hitFrame <= 0f)
			{
				hitFrame = 0.1f;
			}
			if (hitStartTime <= 0.0)
			{
				hitStartTime = TimeMgr.CurTMgr().time;
			}
			double num2 = (TimeMgr.CurTMgr().time - hitStartTime) / (num - hitStartTime);
			num2 = 1.0 - num2;
			if (hp > (float)num2 * 500f)
			{
				hp -= 1f;
			}
			Game1.vgame.charMgr.character[0].AddScore(50);
			Game1.vgame.pMgr.AddParticle(19, v, default(Vector2), 0f, 50, 0);
			if (hp < 0f)
			{
				Vibration.SetVibration(5);
				for (int i = 20; i < 40; i++)
				{
					VikingQuake.SetQuake(1f);
					Game1.vgame.pMgr.AddParticle(2, loc + Rand.GetRandomVec2(-40f, 40f, -20f, 20f) + new Vector2(0f, (float)i * 30f), default(Vector2), 2f, 0, 0);
				}
				Game1.vgame.charMgr.character[0].SetShield(0);
				Game1.achievementMgr.AwardAchievement(9);
				if (!Game1.vgame.diedOnce)
				{
					Game1.achievementMgr.AwardAchievement(11);
				}
			}
		}
		return flag;
	}

	public float GetDif()
	{
		float num = 0f - GetMin();
		float height = Game1.vgame.world.height;
		return height - num;
	}

	public void Init()
	{
		dyingFrame = 0f;
		hp = 500f;
		loc = Game1.vgame.world.risingBaseVec + new Vector2(0f, -2200f);
		active = true;
		hitStartTime = 0.0;
	}

	public float GetMin()
	{
		return loc.Y + 1177.6f + 300f;
	}

	public void Update()
	{
		if (hp < 0f)
		{
			float num = dyingFrame;
			dyingFrame += Game1.frameTime;
			if ((float)(int)(num * 30f) != (float)(int)dyingFrame * 30f)
			{
				Game1.vgame.pMgr.AddParticle(2, loc + Rand.GetRandomVec2(-40f, 40f, -20f, 20f) + new Vector2(Rand.GetRandomFloat(-500f, 500f), Rand.GetRandomFloat(30f, 38f) * 30f), default(Vector2), Rand.GetRandomFloat(0.1f, 1f), 0, 0);
				Game1.vgame.pMgr.AddParticle(13, loc + Rand.GetRandomVec2(-40f, 40f, -20f, 20f) + new Vector2(0f, Rand.GetRandomFloat(30f, 38f) * 30f) + Rand.GetRandomVec2(-1f, 1.4f, 0f, 0f) * dyingFrame * 10f, Rand.GetRandomVec2(-30f, 30f, 100f, 120f), Rand.GetRandomFloat(0.1f, 1f), 0, 0);
				VikingQuake.SetQuake(0.5f);
			}
			if (dyingFrame > 30f)
			{
				active = false;
			}
		}
		if (hitFrame > 0f)
		{
			hitFrame -= Game1.frameTime;
		}
	}

	public void Draw(SpriteBatch sprite)
	{
		Vector2 screenLoc = VScroll.GetScreenLoc(loc, 1f);
		if (hp >= 0f)
		{
			sprite.Draw(Game1.vgame.moonTex[(!(hp > 250f)) ? 1u : 0u], screenLoc, new Rectangle(0, 0, 512, 192), (hitFrame > 0.05f) ? Color.Red : Color.White, VScroll.angle + 3.14f, new Vector2(512f, 1024f) / 2f, VScroll.zoom * 1.15f * 2f, SpriteEffects.None, 1f);
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			sprite.Draw(Game1.vgame.moonTex[2], screenLoc, new Rectangle(i * 256, 0, 256, 192), ((int)(dyingFrame * 30f) % 2 == 0) ? Color.Red : new Color(1f, 1f, 1f, 0.5f), VScroll.angle + 3.14f + dyingFrame * 0.02f * ((i == 0) ? (-1f) : 1f), new Vector2((i == 0) ? 448f : 64f, 1024f) / 2f, VScroll.zoom * 1.15f * 2f, SpriteEffects.None, 1f);
		}
	}
}
