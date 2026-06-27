using System;
using IMAK3Z0MB1EGAEM.audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using Viking_x86.vikinggame;
using xCharEdit.Character;
using Yuki_Win;
using ZombiesWP7;
using ZombiesWP7.audio;
using ZombiesWP7.debug;

namespace Viking_x86.character;

public class Character
{
	public const int STATE_AIR = 0;

	public const int STATE_GROUND = 1;

	public const int BODY_ALL = 0;

	public const int BODY_TOP = 1;

	public const int FACE_LEFT = 0;

	public const int FACE_RIGHT = 1;

	public const int TEAM_GOOD = 0;

	public const int TEAM_EVIL = 1;

	public const int SHOT_NORMAL = 0;

	public const int SHOT_RAPID = 1;

	public const int SHOT_BOMB = 2;

	public const int SHOT_SPREAD = 3;

	public const int SHIELD_RESPAWN = 0;

	public const int SHIELD_PICKUP = 1;

	public const int SHIELD_MOON = 2;

	public Vector2 loc;

	public Vector2 traj;

	public int state;

	public bool exists;

	public int face;

	public BodySec[] bodySec;

	public float delta;

	public float gamma;

	private float shootFrame;

	private int shootFace;

	public float respawnFrame;

	public bool split;

	public int defID;

	public int ID;

	public int team;

	public CharKeys charKeys;

	public int hp;

	private float angle;

	private int shotType;

	private int ammo;

	private float shieldFrame;

	public int lives;

	public long score;

	public void SetShield(int type)
	{
		switch (type)
		{
		case -1:
			shieldFrame = 0f;
			break;
		case 0:
			shieldFrame = 5f;
			break;
		case 1:
			shieldFrame = 20f;
			break;
		case 2:
			shieldFrame = (float)TimeMgr.CurTMgr().trackLeft;
			break;
		}
	}

	public void SetShot(int type)
	{
		bool flag = type == shotType;
		shotType = type;
		int num = 0;
		switch (type)
		{
		case 1:
			num = 300;
			break;
		case 2:
			num = 40;
			break;
		case 3:
			num = 80;
			break;
		}
		if (flag)
		{
			ammo += num;
		}
		else
		{
			ammo = num;
		}
	}

	public Character(int ID)
	{
		this.ID = ID;
		bodySec = new BodySec[2];
		for (int i = 0; i < bodySec.Length; i++)
		{
			bodySec[i] = new BodySec();
		}
		state = 1;
		SetAnimation("idle", 0, overRide: true);
		charKeys = new CharKeys();
	}

	internal void Update()
	{
		if (ID == 0)
		{
			if (respawnFrame > 0f)
			{
				respawnFrame -= Game1.frameTime;
				if (respawnFrame <= 0f)
				{
					exists = false;
				}
			}
			charKeys.Update(0);
			if (shieldFrame > 0f)
			{
				shieldFrame -= Game1.frameTime;
				for (int i = 0; i < Game1.vgame.charMgr.character.Length; i++)
				{
					Character character = Game1.vgame.charMgr.character[i];
					if (character.exists && character.team != team)
					{
						float num = (character.loc - loc).LengthSquared();
						if (num < 3600f)
						{
							character.hp = 0;
							character.Hit(ID);
						}
					}
				}
			}
			if (DebugMgr.autoGun && lives < 2)
			{
				lives = 2;
			}
		}
		switch (defID)
		{
		case 0:
		case 1:
		{
			float num4 = 200f;
			if (ID > 0)
			{
				charKeys.Clear();
				if (state == 1)
				{
					if (loc.X >= Game1.vgame.charMgr.character[0].loc.X + 10f)
					{
						charKeys.runVec.X = -1f;
					}
					else if (loc.X <= Game1.vgame.charMgr.character[0].loc.X - 10f)
					{
						charKeys.runVec.X = 1f;
					}
					else
					{
						Sound.PlaySword();
						SetAnimation("attack", 0, overRide: false);
						Game1.vgame.charMgr.character[0].Hit(ID);
					}
				}
			}
			float num5 = 10f;
			bool flag = loc.X >= Game1.vgame.world.towerX + num5 && loc.X <= Game1.vgame.world.towerX + 320f - num5;
			switch (state)
			{
			case 1:
			{
				loc.X += traj.X * Game1.frameTime;
				if (traj.X > 0f)
				{
					traj.X -= Game1.frameTime * 800f;
					if (traj.X < 0f)
					{
						traj.X = 0f;
					}
				}
				if (traj.X < 0f)
				{
					traj.X += Game1.frameTime * 800f;
					if (traj.X > 0f)
					{
						traj.X = 0f;
					}
				}
				switch (bodySec[0].animName)
				{
				case "idle":
				case "run":
				{
					float num6 = 0f;
					if (charKeys.runVec.X < -0.1f)
					{
						SetAnimation("run", 0, overRide: false);
						face = 0;
						num6 = charKeys.runVec.X * num4;
					}
					else if (charKeys.runVec.X > 0.1f)
					{
						SetAnimation("run", 0, overRide: false);
						face = 1;
						num6 = charKeys.runVec.X * num4;
					}
					else
					{
						SetAnimation("idle", 0, overRide: false);
					}
					if (traj.X > num6)
					{
						traj.X -= Game1.frameTime * 2000f;
						if (traj.X < num6)
						{
							traj.X = num6;
						}
					}
					if (traj.X < num6)
					{
						traj.X += Game1.frameTime * 2000f;
						if (traj.X > num6)
						{
							traj.X = num6;
						}
					}
					if (shootFrame > 0f)
					{
						shootFrame -= Game1.frameTime;
					}
					if (charKeys.shootVec.LengthSquared() > 0.005f && shootFrame <= 0f)
					{
						switch (shotType)
						{
						case 0:
							shootFrame = 0.15f;
							break;
						case 1:
							shootFrame = 0.05f;
							break;
						case 3:
							shootFrame = 0.2f;
							break;
						case 2:
							shootFrame = 0.4f;
							break;
						}
						Vector2 shootVec = charKeys.shootVec;
						shootVec.Normalize();
						float num7 = Trig.GetAngle(default(Vector2), shootVec) + 3.14f;
						num7 -= VScroll.angle;
						if (Math.Cos(num7) > 0.0)
						{
							shootFace = 1;
						}
						else
						{
							shootFace = 0;
						}
						double num8 = 0.0 - Math.Sin(num7);
						Vector2 vector3 = default(Vector2);
						if (num8 > 0.8600000143051147)
						{
							SetAnimation("uzap", 1, overRide: true);
							vector3 = new Vector2(0f, -50f);
						}
						else if (num8 > 0.5)
						{
							SetAnimation("ulzap", 1, overRide: true);
							vector3 = new Vector2(10f, -46f);
						}
						else
						{
							SetAnimation("zap", 1, overRide: true);
							vector3 = new Vector2(20f, -40f);
						}
						Vector2 vector4 = loc + new Vector2(vector3.X * ((shootFace == 1) ? 1f : (-1f)), vector3.Y);
						switch (shotType)
						{
						case 0:
						{
							Sound.Play("znormal");
							for (int l = -1; l < 2; l++)
							{
								Game1.vgame.pMgr.AddParticle(4, vector4, new Vector2((float)Math.Cos((float)l * 0.1f + num7), (float)Math.Sin((float)l * 0.1f + num7)) * 800f, 0f, 0, ID);
							}
							break;
						}
						case 3:
						{
							Sound.Play("zspread");
							for (int j = -4; j < 5; j++)
							{
								Game1.vgame.pMgr.AddParticle(4, vector4, new Vector2((float)Math.Cos((float)j * 0.1f + num7), (float)Math.Sin((float)j * 0.1f + num7)) * 800f, 0f, 0, ID);
							}
							ammo--;
							if (ammo <= 0)
							{
								shotType = 0;
							}
							break;
						}
						case 1:
						{
							Sound.Play("zrapid");
							for (int k = 0; k < 2; k++)
							{
								float num9 = num7 + Rand.GetRandomFloat(-0.1f, 0.1f);
								Game1.vgame.pMgr.AddParticle(4, vector4, new Vector2((float)Math.Cos(num9), (float)Math.Sin(num9)) * 800f, 0f, 0, ID);
							}
							ammo--;
							if (ammo <= 0)
							{
								shotType = 0;
							}
							break;
						}
						case 2:
							Sound.Play("zbomb");
							Game1.vgame.pMgr.AddParticle(17, vector4, new Vector2((float)Math.Cos(num7), (float)Math.Sin(num7)) * 800f, 0f, 0, ID);
							ammo--;
							if (ammo <= 0)
							{
								shotType = 0;
							}
							break;
						}
						Game1.vgame.pMgr.AddParticle(5, vector4, default(Vector2), 0.4f, 0, ID);
					}
					if (shootFrame > 0f)
					{
						face = shootFace;
					}
					break;
				}
				}
				float minY = Game1.vgame.world.GetMinY(loc.X);
				if (loc.Y < minY)
				{
					loc.Y += Game1.frameTime * 100f;
					if (loc.Y > minY)
					{
						loc.Y = minY;
					}
				}
				if (loc.Y > minY)
				{
					loc.Y -= Game1.frameTime * 100f;
					if (loc.Y < minY)
					{
						loc.Y = minY;
					}
				}
				break;
			}
			case 0:
				loc += traj * Game1.frameTime;
				traj.Y += 6f;
				if (Game1.vgame.world.TestCollision(loc) || loc.Y > Game1.vgame.world.height + 640f)
				{
					loc.Y = Game1.vgame.world.GetMinY(loc.X);
					state = 1;
					SetAnimation("land", 0, overRide: true);
				}
				break;
			}
			if (flag)
			{
				if (loc.X < Game1.vgame.world.towerX + num5)
				{
					loc.X = Game1.vgame.world.towerX + num5;
				}
				if (loc.X > Game1.vgame.world.towerX + 320f - num5)
				{
					loc.X = Game1.vgame.world.towerX + 320f - num5;
				}
			}
			if (loc.X < 0f)
			{
				loc.X = 0f;
			}
			bodySec[0].Update(0, this);
			if (split)
			{
				bodySec[1].Update(1, this);
			}
			if (team == 1 && loc.Y > Game1.vgame.charMgr.character[0].loc.Y + 500f)
			{
				exists = false;
			}
			break;
		}
		case 2:
			loc += traj * Game1.frameTime;
			if (traj.X < 0f && loc.X < Game1.vgame.world.towerX - 300f)
			{
				exists = false;
			}
			if (traj.X > 0f && loc.X > Game1.vgame.world.towerX + 320f + 300f)
			{
				exists = false;
			}
			delta += Game1.frameTime;
			if (delta > 1f)
			{
				delta -= 1f;
				if (Rand.CoinToss(0.5f))
				{
					Game1.vgame.pMgr.AddParticle(9, loc, new Vector2(0f, 100f), 0f, 0, ID);
				}
			}
			break;
		case 6:
			loc += traj * Game1.frameTime;
			if (traj.X < 0f && loc.X < Game1.vgame.world.towerX - 300f)
			{
				exists = false;
			}
			if (traj.X > 0f && loc.X > Game1.vgame.world.towerX + 320f + 300f)
			{
				exists = false;
			}
			delta += Game1.frameTime;
			if (gamma > 0f)
			{
				gamma -= Game1.frameTime;
				if (loc.X > Game1.vgame.charMgr.character[0].loc.X - 16f && loc.X < Game1.vgame.charMgr.character[0].loc.X + 16f)
				{
					gamma -= 1f;
				}
				if (loc.X > Game1.vgame.world.towerX + 30f && loc.X < Game1.vgame.world.towerX + 320f - 30f && gamma <= 0f)
				{
					traj.Y = ((traj.X < 0f) ? (0f - traj.X) : traj.X);
					traj.X = 0f;
					gamma -= 1f;
				}
			}
			if (Game1.vgame.world.TestCollision(loc + new Vector2(0f, -32f)))
			{
				Kill(ID);
			}
			else if (Game1.vgame.charMgr.character[0].CheckHit(loc + new Vector2(0f, -40f)))
			{
				Game1.vgame.charMgr.character[0].Hit(ID);
				Kill(ID);
			}
			break;
		case 3:
			traj.Y += Game1.frameTime * 7f;
			loc += traj * Game1.frameTime;
			if (Game1.vgame.world.TestCollision(loc) || loc.Y > Game1.vgame.world.height + 640f)
			{
				Kill(ID);
			}
			break;
		case 4:
		{
			if (hp <= 0)
			{
				traj.Y += Game1.frameTime * 300f;
				loc += traj * Game1.frameTime;
				delta += Game1.frameTime;
				if (loc.Y > VScroll.scroll.Y + 500f)
				{
					exists = false;
				}
				break;
			}
			Vector2 vector = new Vector2
			{
				Y = VScroll.scroll.Y
			};
			float num3 = Game1.vgame.world.towerX + 160f;
			if (loc.X > num3)
			{
				vector.X = num3 + 250f;
			}
			else
			{
				vector.X = num3 - 250f;
			}
			delta += Game1.frameTime;
			vector += new Vector2((float)Math.Cos(delta), (float)Math.Sin(delta)) * new Vector2(5f, 90f);
			loc += (vector - loc) * Game1.frameTime * 0.5f;
			if (delta > 6.28f && Math.Sin(delta) > 0.0 && Math.Cos(delta) > 0.0)
			{
				Vector2 vector2 = loc + new Vector2(20f * ((face == 1) ? 1f : (-1f)), -69f);
				Game1.vgame.pMgr.AddParticle(12, vector2, new Vector2(500f * ((face == 1) ? 1f : (-1f)), 300f) * 0.1f, 0.2f, 0, ID);
				Game1.vgame.pMgr.AddParticle(10, vector2, new Vector2(500f * ((face == 1) ? 1f : (-1f)), 300f), 0f, 0, ID);
				VikingQuake.SetQuake(0.25f);
			}
			break;
		}
		case 5:
		{
			delta += Game1.frameTime;
			float num2 = ((face == 0) ? 1f : (-1f));
			if (delta < 4f)
			{
				angle += Game1.frameTime * 0.35f * num2;
			}
			else if (delta < 5f)
			{
				angle += Game1.frameTime * 5f * num2;
			}
			else
			{
				angle += Game1.frameTime * num2;
			}
			traj = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle)) * 140f;
			loc += traj * Game1.frameTime;
			if (Game1.vgame.world.TestCollision(loc))
			{
				Kill(ID);
			}
			else if (Game1.vgame.charMgr.character[0].CheckHit(loc))
			{
				Game1.vgame.charMgr.character[0].Hit(ID);
				Kill(ID);
			}
			if (delta > 8f)
			{
				exists = false;
			}
			break;
		}
		}
	}

	public void SetAnimation(string newAnim, int sec, bool overRide)
	{
		int animFromName = GetAnimFromName(newAnim);
		if (sec == 0)
		{
			bodySec[0].SetAnim(animFromName, newAnim, overRide);
			return;
		}
		bodySec[1].SetAnim(animFromName, newAnim, overRide);
		split = true;
	}

	private int GetAnimFromName(string newAnim)
	{
		CharDef charDef = CharDefMgr.charDef[defID];
		for (int i = 0; i < charDef.GetAnimationArray().Length; i++)
		{
			Animation animation = charDef.GetAnimation(i);
			if (animation != null && animation.name == newAnim)
			{
				return i;
			}
		}
		return 0;
	}

	internal void Draw(SpriteBatch sprite)
	{
		if (respawnFrame > 0f)
		{
			return;
		}
		float foreBright = Game1.vgame.world.GetForeBright();
		if (shieldFrame > 0f)
		{
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			for (int i = 0; i < 4; i++)
			{
				float num;
				for (num = (float)i * 0.25f + shieldFrame; num > 1f; num -= 1f)
				{
				}
				num = 1f - num;
				float num2 = 1f;
				if (num > 0.8f)
				{
					num2 = (1f - num) * 5f;
				}
				if (num < 0.5f)
				{
					num2 = num * 2f;
				}
				Color color = new Color(1f - num, 1f - num, 1f, num2);
				if (shieldFrame < 3f)
				{
					color = new Color(1f, 1f - num, 1f - num, ((int)(shieldFrame * 30f) % 2 == 0) ? num2 : 0f);
				}
				sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(loc + new Vector2(0f, -25f), 1f), ScaleTools.ScaledRect(0, 832, 128, 128), color, 0f, new Vector2(64f, 64f) / 2f, num * VScroll.zoom * 2f, SpriteEffects.None, 1f);
			}
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		}
		switch (defID)
		{
		case 0:
		case 1:
		{
			Vector2 screenLoc = VScroll.GetScreenLoc(loc, 1f);
			float size = 0.15f * VScroll.zoom;
			if (split)
			{
				bodySec[0].Draw(sprite, screenLoc, size, face, this, 0);
				bodySec[1].Draw(sprite, screenLoc, size, face, this, 1);
			}
			else
			{
				bodySec[0].Draw(sprite, screenLoc, size, face, this, -1);
			}
			break;
		}
		case 2:
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(256, 64 + 128 * ((int)(delta * 4f) % 2), 192, 128), new Color(foreBright, foreBright, foreBright * 0.5f, 1f), VScroll.angle, new Vector2(96f, 168f) / 2f, 0.4f * VScroll.zoom * 2f, SpriteEffects.None, 1f);
			break;
		case 3:
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(598, 65, 426, 190), new Color(foreBright, foreBright, foreBright, 1f), VScroll.angle - 1.57f, new Vector2(96f, 96f) / 2f, 0.2f * VScroll.zoom * 2f, SpriteEffects.None, 1f);
			break;
		case 4:
		{
			Color color2 = new Color(1f, 1f, 1f, 1f);
			if (hp <= 0)
			{
				color2 = (((int)(delta * 30f) % 2 != 0) ? new Color(foreBright, foreBright, foreBright, 0.5f) : new Color(1f, 0f, 0f, 0.5f));
			}
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(0, 192, 192, 384), color2, VScroll.angle, new Vector2(96f, 192f) / 2f, 0.6f * VScroll.zoom * 2f, (face != 1) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
			break;
		}
		case 5:
		{
			while (angle < 0f)
			{
				angle += 6.28f;
			}
			while (angle > 6.28f)
			{
				angle -= 6.28f;
			}
			float num3;
			for (num3 = angle; num3 > 1.57f; num3 -= 1.57f)
			{
			}
			num3 /= 1.57f;
			num3 *= 6f;
			int num4 = (int)(angle / 1.57f);
			float num5 = delta * 2f;
			if (num5 > 1f)
			{
				num5 = 1f;
			}
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect((int)num3 * 128, 576, 128, 128), new Color(foreBright, foreBright, foreBright, num5), (float)num4 * 1.57f + VScroll.angle - 1.57f, new Vector2(64f, 64f) / 2f, 0.6f * VScroll.zoom * 2f, SpriteEffects.None, 1f);
			break;
		}
		case 6:
			sprite.Draw(Game1.vgame.grassTex, VScroll.GetScreenLoc(loc, 1f), new Rectangle(448, 128 + 64 * ((int)(delta * 16f) % 4), 64, 64), new Color(foreBright, foreBright, foreBright, 1f), VScroll.angle, new Vector2(32f, 64f), 0.7f * VScroll.zoom, SpriteEffects.None, 1f);
			break;
		}
	}

	internal void Init(Vector2 loc, int defID, int face, int team)
	{
		this.loc = loc;
		this.defID = defID;
		this.face = face;
		this.team = team;
		exists = true;
		delta = 0f;
		hp = 1;
		switch (defID)
		{
		case 3:
			hp = 10;
			break;
		case 4:
			hp = 20;
			break;
		case 1:
			hp = 2;
			break;
		case 5:
		{
			angle = 1.57f;
			float num = 0.5f;
			if (face == 0)
			{
				angle += num;
			}
			else
			{
				angle -= num;
			}
			break;
		}
		case 6:
			gamma = Rand.GetRandomFloat(2f, 3f);
			break;
		case 0:
			lives = 3;
			break;
		case 2:
			break;
		}
	}

	internal bool CheckHit(Vector2 v)
	{
		return CheckHit(v, 0f);
	}

	internal bool CheckHit(Vector2 v, float buf)
	{
		float num = 20f + buf;
		float num2 = 80f + buf;
		switch (defID)
		{
		case 0:
			if (Game1.vgame.charMgr.moon.active && Game1.vgame.charMgr.moon.hp < 0f)
			{
				return false;
			}
			if (shieldFrame > 0f)
			{
				return false;
			}
			num2 = 60f;
			if (v.X > loc.X - num && v.X < loc.X + num && v.Y < loc.Y && v.Y > loc.Y - num2)
			{
				return true;
			}
			break;
		case 5:
			num = 30f + buf;
			num2 = 30f + buf;
			if (v.X > loc.X - num && v.X < loc.X + num && v.Y < loc.Y + num2 && v.Y > loc.Y - num2)
			{
				return true;
			}
			break;
		case 4:
			if (hp <= 0)
			{
				return false;
			}
			num2 = 100f + buf;
			if (v.X > loc.X - num && v.X < loc.X + num && v.Y < loc.Y + num2 && v.Y > loc.Y - num2)
			{
				return true;
			}
			break;
		default:
			if (v.X > loc.X - num && v.X < loc.X + num && v.Y < loc.Y && v.Y > loc.Y - num2)
			{
				return true;
			}
			break;
		}
		return false;
	}

	internal void Kill(int killer)
	{
		if (ID == 0)
		{
			Vibration.SetVibration(1);
			if (bodySec[0].animName != "die")
			{
				hp = 0;
				SetAnimation("die", 0, overRide: true);
				split = false;
			}
			return;
		}
		exists = false;
		float num = 25f;
		int num2 = 50;
		switch (defID)
		{
		case 1:
			Game1.vgame.pMgr.MakeScrapBomb(loc + new Vector2(0f, -50f));
			break;
		case 2:
			Game1.vgame.pMgr.MakePixelBomb(loc + new Vector2(0f, -50f));
			num2 = 200;
			break;
		case 6:
			Game1.vgame.pMgr.MakePixelBomb(loc + new Vector2(0f, -50f));
			num2 = 200;
			break;
		case 5:
			Game1.vgame.pMgr.MakeGalagaBomb(loc + new Vector2(0f, 0f));
			num = 0f;
			break;
		case 3:
		{
			Sound.PlayExplode();
			Game1.vgame.pMgr.MakeGiantBomb(loc + new Vector2(0f, -50f));
			num2 = 150;
			float num3 = (Game1.vgame.charMgr.character[0].loc - loc).LengthSquared();
			if (num3 < 6000f)
			{
				Game1.vgame.charMgr.character[0].Hit(ID);
			}
			break;
		}
		case 4:
			exists = true;
			num2 = 500;
			break;
		}
		num2 *= 2;
		if (killer == 0)
		{
			Game1.vgame.charMgr.character[killer].AddScore(num2);
			Game1.profile.AddKill();
			Game1.vgame.pMgr.AddParticle(19, loc + new Vector2(0f, 0f - num), default(Vector2), 0f, num2, 0);
		}
		if (Game1.vgame.world.pickupFrame <= 0f && loc.X > Game1.vgame.world.towerX && loc.X < Game1.vgame.world.towerX + 320f)
		{
			Game1.vgame.pMgr.AddParticle(15, loc + new Vector2(0f, 0f - num), new Vector2(0f, -50f), 0f, Rand.GetRandomInt(0, 5), 0);
			Game1.vgame.world.pickupFrame = Rand.GetRandomFloat(10f, 15f);
		}
	}

	internal void Hit(int hitter)
	{
		if (!(shieldFrame > 0f))
		{
			hp--;
			switch (defID)
			{
			case 1:
				loc.Y -= 5f;
				break;
			case 3:
				loc.Y -= 5f;
				break;
			}
			if (hp <= 0)
			{
				Kill(hitter);
			}
		}
	}

	internal int GetShot()
	{
		return shotType;
	}

	internal long GetAmmo()
	{
		return ammo;
	}

	internal void KillChar()
	{
		if (respawnFrame <= 0f)
		{
			respawnFrame = 5f;
		}
	}

	internal void AddScore(int p)
	{
		long num = score;
		score += p;
		if (num < 100000 && score >= 100000)
		{
			Game1.achievementMgr.AwardAchievement(7);
		}
		if (num < 1000000 && score >= 1000000)
		{
			Game1.achievementMgr.AwardAchievement(8);
		}
	}
}
