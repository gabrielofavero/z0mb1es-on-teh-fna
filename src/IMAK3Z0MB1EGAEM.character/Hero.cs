using System;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.map;
using IMAK3Z0MB1EGAEM.menu;
using IMAK3Z0MB1EGAEM.particles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using Viking_x86.director;
using Yuki_Win;
using ZombiesWP7;
using ZombiesWP7.audio;
using ZombiesWP7.debug;

namespace IMAK3Z0MB1EGAEM.character;

public class Hero
{
	public const int WEAPON_RIFLE = 0;

	public const int WEAPON_MACHINEGUN = 1;

	public const int WEAPON_ROCKET = 2;

	public const int WEAPON_FLAMETHROWER = 3;

	public const int WEAPON_SHOTTY = 4;

	public const int WEAPON_LASER = 5;

	public const int WEAPON_NEUTRON = 6;

	public Vector2 loc;

	public Vector2 shoot;

	public Vector2 traj;

	public int idx;

	public float angle;

	public bool exists;

	public bool keyStart;

	public bool keyUp;

	public bool keyDown;

	public bool keyAccept;

	public bool keyCancel;

	private Legs legs;

	private float shootFrame;

	public long score;

	public float respawnFrame;

	public char[] name = new char[3];

	public int nameIn;

	public int lives;

	public float standTime;

	public int weapon;

	public int specialAmmo;

	public float spawnFrame;

	public float speedFrame;

	private int shootSnd;

	private bool spaceShots;

	private bool spaceShotsCheck;

	public bool diedOnce;

	public void Kill()
	{
		lives--;
		diedOnce = true;
		weapon = 0;
		speedFrame = 0f;
		HitManager.MakeBloodSplode(loc, 10, Rand.GetRandomFloat(0.5f, 1f), 300f);
		ParticleMan.AddParticle(16, loc, default(Vector2), idx, 0f, 0);
		if (lives <= 0)
		{
			nameIn = 4;
			name[0] = ' ';
			name[1] = ' ';
			name[2] = ' ';
			respawnFrame = 6f;
		}
		else
		{
			respawnFrame = 3f;
		}
		Vibration.SetVibration(1);
	}

	public void SetWeapon(int weapon, int ammo)
	{
		if (weapon == this.weapon)
		{
			specialAmmo += ammo;
			return;
		}
		this.weapon = weapon;
		specialAmmo = ammo;
	}

	public Hero(int idx)
	{
		this.idx = idx;
		legs = new Legs();
		exists = false;
	}

	public void Init(Vector2 loc)
	{
		standTime = 0f;
		lives = 5;
		nameIn = 0;
		this.loc = loc;
		exists = true;
		weapon = 0;
		score = 0L;
		spaceShots = false;
		spaceShotsCheck = false;
		diedOnce = false;
	}

	public void AddPoints(Vector2 mloc, long points, bool kill)
	{
		points *= 2;
		long num = score;
		if (ParticleMan.count < 50 && mloc.X > 0f && mloc.Y > 0f)
		{
			ParticleMan.AddParticle(3, mloc, default(Vector2), 0, 0f, (int)points);
		}
		score += points;
		if (kill)
		{
			Game1.profile.AddKill();
		}
		if (num < 100000 && score >= 100000)
		{
			switch (GameState.state)
			{
			case 3:
				Game1.achievementMgr.AwardAchievement(0);
				break;
			case 7:
				Game1.achievementMgr.AwardAchievement(12);
				break;
			}
		}
		if (num < 1000000 && score >= 1000000)
		{
			switch (GameState.state)
			{
			case 3:
				Game1.achievementMgr.AwardAchievement(1);
				break;
			case 7:
				Game1.achievementMgr.AwardAchievement(13);
				break;
			}
		}
	}

	public void Update()
	{
		bool flag = GameState.state == 7;
		if (nameIn > 0)
		{
			UpdateKeys();
			if (nameIn < 4)
			{
				if (keyUp)
				{
					name[nameIn - 1] += '\u0001';
					if (name[nameIn - 1] > 'Z')
					{
						name[nameIn - 1] = 'A';
					}
					Console.WriteLine(((byte)name[nameIn - 1]).ToString());
				}
				if (keyDown)
				{
					name[nameIn - 1] -= '\u0001';
					if (name[nameIn - 1] < 'A')
					{
						name[nameIn - 1] = 'Z';
					}
					Console.WriteLine(((byte)name[nameIn - 1]).ToString());
				}
				if (nameIn == 1)
				{
					name[1] = '-';
					name[2] = '-';
				}
				else if (nameIn == 2)
				{
					name[2] = '-';
				}
				if (keyAccept)
				{
					nameIn++;
					if (nameIn == 4)
					{
						string text = name[0].ToString() + name[1] + name[2];
						HighScores.AddScore(text, score);
					}
					else
					{
						name[nameIn - 1] = 'A';
					}
				}
				if (keyCancel && nameIn > 1)
				{
					nameIn--;
				}
			}
			else
			{
				respawnFrame -= FMan.frameTime;
				if (respawnFrame <= 0f)
				{
					exists = false;
				}
			}
			return;
		}
		if (respawnFrame > 0f)
		{
			respawnFrame -= FMan.frameTime;
			standTime = 0f;
			if (!(respawnFrame <= 0f))
			{
				return;
			}
			spawnFrame = 5f;
			loc = Rand.GetRandomVec2(-200f, 200f, -200f, 200f) + MapMan.mapSize / 2f;
			if (GameState.state == 7)
			{
				loc = Rand.GetRandomVec2(-200f, 200f, -200f, 200f) + (ZombieGame.GetEndlessRoomTL() + ZombieGame.GetEndlessRoomBR()) / 2f;
			}
		}
		if (spawnFrame > 0f)
		{
			spawnFrame -= FMan.frameTime;
		}
		if (!exists)
		{
			return;
		}
		bool flag2 = false;
		if (GameState.state == 7 && CamMan.endlessTransFrame > 0f)
		{
			flag2 = true;
		}
		if (GameState.state == 3 && !spaceShotsCheck && TimeMgr.ZombieTMgr().phase == 3)
		{
			if (!spaceShots)
			{
				Game1.achievementMgr.AwardAchievement(4);
			}
			spaceShotsCheck = true;
		}
		if (!flag2)
		{
			UpdateKeys();
		}
		legs.traj = traj;
		legs.Update();
		if (!exists)
		{
			return;
		}
		float num = 140f;
		if (speedFrame > 0f)
		{
			speedFrame -= FMan.frameTime;
			num = 240f;
		}
		Vector2 vector = loc;
		if (flag)
		{
			loc.X += traj.X * FMan.frameTime * num;
			if (MapMan.CheckHeroCol(loc))
			{
				loc.X = vector.X;
			}
			loc.Y += traj.Y * FMan.frameTime * num;
			if (MapMan.CheckHeroCol(loc))
			{
				loc.Y = vector.Y;
			}
		}
		else
		{
			loc += traj * FMan.frameTime * num;
		}
		float num2 = angle;
		if (shoot.LengthSquared() > 0.01f)
		{
			num2 = Trig.GetAngle(default(Vector2), shoot);
			if (shootFrame <= 0f && shoot.LengthSquared() > 0.01f)
			{
				angle = num2;
				Vector2 vector2 = shoot;
				vector2.Normalize();
				shootSnd = (shootSnd + 1) % 2;
				switch (weapon)
				{
				case 0:
				{
					Sound.Play("auto");
					shootFrame = 0.1f;
					ParticleMan.AddParticle(1, loc, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -2000f, idx, 0f, 0);
					for (int l = 0; l < 10; l++)
					{
						ParticleMan.AddParticle(2, loc + (float)(l + 20) * -2f * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 5f, idx, Rand.GetRandomFloat(0f, 0.5f), 0);
					}
					break;
				}
				case 1:
				{
					if (shootSnd == 0)
					{
						Sound.Play("shotgun");
					}
					else
					{
						Sound.Play("shotgun2");
					}
					shootFrame = 0.04f;
					specialAmmo--;
					float num5 = angle;
					num5 += Rand.GetRandomFloat(-0.12f, 0.12f);
					ParticleMan.AddParticle(1, loc, new Vector2((float)Math.Cos(num5), (float)Math.Sin(num5)) * -2000f, idx, 0f, 0);
					for (int m = 0; m < 10; m++)
					{
						ParticleMan.AddParticle(2, loc + (float)(m + 20) * -2f * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 5f, idx, Rand.GetRandomFloat(0f, 0.5f), 0);
					}
					break;
				}
				case 4:
				{
					Sound.Play("shotty");
					shootFrame = 0.4f;
					specialAmmo--;
					for (int j = 0; j < 10; j++)
					{
						float num4 = angle;
						num4 += Rand.GetRandomFloat(-0.2f, 0.2f);
						ParticleMan.AddParticle(1, loc, new Vector2((float)Math.Cos(num4), (float)Math.Sin(num4)) * -2000f, idx, 0f, 0);
					}
					for (int k = 0; k < 10; k++)
					{
						ParticleMan.AddParticle(2, loc + (float)(k + 20) * -2f * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)), new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 5f, idx, Rand.GetRandomFloat(0f, 0.5f), 0);
					}
					break;
				}
				case 3:
					if (shootSnd == 0)
					{
						Sound.Play("flame");
					}
					else
					{
						Sound.Play("flame2");
					}
					shootFrame = 0.03f;
					specialAmmo--;
					ParticleMan.AddParticle(9, loc + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -26f, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -500f, idx, 0f, 0);
					break;
				case 2:
					Sound.Play("launch");
					shootFrame = 0.3f;
					specialAmmo--;
					ParticleMan.AddParticle(11, loc + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -26f, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -1100f, idx, 0f, 0);
					break;
				case 6:
				{
					Sound.Play("plas2");
					shootFrame = 0.2f;
					specialAmmo--;
					for (int i = -1; i < 2; i++)
					{
						float num3 = angle + (float)i * 0.3f;
						ParticleMan.AddParticle(12, loc + new Vector2((float)Math.Cos(num3), (float)Math.Sin(num3)) * -26f, new Vector2((float)Math.Cos(num3), (float)Math.Sin(num3)) * -1300f, idx, 0f, 0);
					}
					break;
				}
				case 5:
					Sound.Play("shrink");
					shootFrame = 0.2f;
					specialAmmo--;
					ParticleMan.AddParticle(14, loc + new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -26f, new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * -2000f, idx, 0f, 0);
					break;
				}
				if (GameState.state == 3 && TimeMgr.ZombieTMgr().phase == 2)
				{
					spaceShots = true;
				}
				if (DebugMgr.laser)
				{
					weapon = 5;
					specialAmmo = 5;
				}
				if (weapon != 0 && specialAmmo <= 0)
				{
					weapon = 0;
				}
			}
		}
		else if (traj.Length() > 0f)
		{
			num2 = Trig.GetAngle(default(Vector2), traj);
			num2 += (float)Math.Sin(legs.frame) * 0.1f;
		}
		if (traj.LengthSquared() > 0f)
		{
			standTime = 0f;
		}
		else if (GameState.state == 3)
		{
			float num6 = standTime;
			standTime += FMan.frameTime;
			if (standTime > 60f && num6 <= 60f)
			{
				Game1.achievementMgr.AwardAchievement(5);
			}
		}
		if (shootFrame > 0f)
		{
			shootFrame -= FMan.frameTime;
		}
		float num7;
		for (num7 = num2 - angle; num7 < -3.14f; num7 += 6.28f)
		{
		}
		while (num7 > 3.14f)
		{
			num7 -= 6.28f;
		}
		angle += num7 * FMan.frameTime * 20f;
		float num8 = 100f;
		if (!flag)
		{
			if (loc.X < num8)
			{
				loc.X = num8;
			}
			if (loc.Y < num8)
			{
				loc.Y = num8;
			}
			if (loc.X > MapMan.mapSize.X - num8)
			{
				loc.X = MapMan.mapSize.X - num8;
			}
			if (loc.Y > MapMan.mapSize.Y - num8)
			{
				loc.Y = MapMan.mapSize.Y - num8;
			}
		}
	}

	public void Draw(SpriteBatch sprite)
	{
		if (nameIn <= 0 && !(respawnFrame > 0f) && exists)
		{
			legs.Draw(loc, sprite, 1f);
			sprite.Draw(ZombieGame.spritesTex, ScrollMan.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(256 * idx, 0, 256, 128), Color.White, angle, new Vector2(138f, 64f) / 2f, 0.3f * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
		}
	}

	private void UpdateKeys()
	{
		traj = InputMgr.lVec;
		shoot = InputMgr.rVec;
		if (DebugMgr.autoGun && lives < 5)
		{
			lives = 5;
		}
		keyUp = false;
		keyDown = false;
		keyAccept = false;
		keyCancel = false;
	}
}
