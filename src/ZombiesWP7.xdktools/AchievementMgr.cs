using System;
using System.IO;

namespace ZombiesWP7.xdktools;

/// <summary>
/// Achievement definition — name, gamerScore, description (earned), how-to-earn hint.
/// </summary>
public struct AchievementDef
{
	public string Key;       // matches texture key in Game1.achieveTextures
	public string Name;
	public int GamerScore;
	public string Description;
	public string HowToEarn;

	public AchievementDef(string key, string name, int score, string desc, string howTo)
	{
		Key = key;
		Name = name;
		GamerScore = score;
		Description = desc;
		HowToEarn = howTo;
	}
}

public class AchievementMgr
{
	public const int IM_100K = 0;
	public const int IM_MILLION = 1;
	public const int IM_WIN = 2;
	public const int IM_INVINCIBLE = 3;
	public const int IM_ASTEROIDS = 4;
	public const int IM_NOMOVE = 5;
	public const int IM_LASER = 6;
	public const int TV_100K = 7;
	public const int TV_MILLION = 8;
	public const int TV_WIN = 9;
	public const int TV_TOWER = 10;
	public const int TV_INVINCIBLE = 11;
	public const int EZ_100K = 12;
	public const int EZ_MILLION = 13;
	public const int EZ_ONEROOM = 14;
	public const int EZ_ONEROUND = 15;
	public const int EZ_TWOROUNDS = 16;
	public const int KILL_1000 = 17;
	public const int KILL_10000 = 18;
	public const int MEGA = 19;

	public const int COUNT = 20;

	/// <summary>Local achievement definitions — replaces Xbox LIVE data.</summary>
	public static readonly AchievementDef[] Definitions = new AchievementDef[COUNT]
	{
		new AchievementDef("im_100k",      "Score 100,000",      10, "Solid shooting!",            "Score 100k in I MAED A GAM3"),
		new AchievementDef("im_million",   "Score 1,000,000",    20, "A million points of pain!",  "Score 1 million in I MAED"),
		new AchievementDef("im_win",       "Beat the Game",      15, "You saved the day!",         "Beat I MAED A GAM3 mode"),
		new AchievementDef("im_invincible","Invincible",         25, "Not a scratch on you!",      "Beat I MAED without dying"),
		new AchievementDef("im_asteroids", "Asteroids",          10, "Rock blaster!",              "Destroy all asteroids in I MAED"),
		new AchievementDef("im_nomove",    "No Movement",        15, "Stand your ground!",         "Beat I MAED without moving"),
		new AchievementDef("im_laser",     "Laser Master",       10, "Pew pew!",                   "Use only laser in I MAED"),
		new AchievementDef("tv_100k",      "Viking 100k",        10, "A viking fortune!",          "Score 100k in Time Viking"),
		new AchievementDef("tv_million",   "Viking Million",     20, "Richer than Odin!",          "Score 1 million in Time Viking"),
		new AchievementDef("tv_win",       "Viking Victory",     15, "Valhalla awaits!",           "Beat Time Viking"),
		new AchievementDef("tv_tower",     "Tower Climber",      15, "To the top!",                 "Reach the tower in Time Viking"),
		new AchievementDef("tv_invincible","Viking Invincible",  25, "Blessed by the gods!",       "Beat Time Viking without dying"),
		new AchievementDef("ez_100k",      "Endless 100k",       10, "Endless riches!",            "Score 100k in Endless"),
		new AchievementDef("ez_million",   "Endless Million",    20, "Endless fortune!",           "Score 1 million in Endless"),
		new AchievementDef("ez_oneroom",   "One Room",           10, "Claustrophobic!",            "Survive one room in Endless"),
		new AchievementDef("ez_oneround",  "One Round",          15, "Round and round!",           "Complete one round in Endless"),
		new AchievementDef("ez_tworounds", "Two Rounds",         20, "Double trouble!",            "Complete two rounds in Endless"),
		new AchievementDef("kill1000",     "Kill 1,000",         10, "Zombie slayer!",             "Kill 1,000 zombies"),
		new AchievementDef("kill10000",    "Kill 10,000",        25, "Zombie exterminator!",       "Kill 10,000 zombies"),
		new AchievementDef("mega",         "Mega Achievement",   50, "You did EVERYTHING!",        "Unlock every achievement"),
	};

	/// <summary>Which achievements have been awarded locally.</summary>
	public bool[] achieved;

	/// <summary>Index of the most recently awarded achievement (for toast), or -1.</summary>
	public int newestAwarded = -1;

	/// <summary>Timer for toast display.</summary>
	private float toastTimer;

	public AchievementMgr()
	{
		achieved = new bool[COUNT];
	}

	/// <summary>Award an achievement locally. No Xbox LIVE required.</summary>
	public void AwardAchievement(int idx)
	{
		if (idx < 0 || idx >= COUNT)
			return;

		Game1.profile.AddAchievable(idx);

		// Don't double-award
		if (achieved[idx])
			return;

		achieved[idx] = true;
		newestAwarded = idx;
		toastTimer = 4f; // show toast for ~4 seconds

		// Check if all 20 are unlocked for the "mega" achievement
		if (idx != MEGA)
		{
			bool all = true;
			for (int i = 0; i < COUNT - 1; i++)
			{
				if (!achieved[i])
				{
					all = false;
					break;
				}
			}
			if (all && !achieved[MEGA])
			{
				AwardAchievement(MEGA);
			}
		}
	}

	/// <summary>Check if an achievement has been unlocked.</summary>
	public bool IsAchieved(int idx)
	{
		if (idx < 0 || idx >= COUNT)
			return false;
		return achieved[idx];
	}

	public void Update()
	{
		if (toastTimer > 0f)
		{
			toastTimer -= Game1.frameTime;
			if (toastTimer <= 0f)
			{
				newestAwarded = -1;
			}
		}
	}

	/// <summary>Persist unlocked achievements.</summary>
	public void Write(BinaryWriter w)
	{
		for (int i = 0; i < COUNT; i++)
		{
			w.Write((byte)(achieved[i] ? 1 : 0));
		}
	}

	/// <summary>Restore unlocked achievements from save.</summary>
	public void Read(BinaryReader r)
	{
		for (int i = 0; i < COUNT; i++)
		{
			achieved[i] = r.ReadByte() == 1;
		}
	}
}
