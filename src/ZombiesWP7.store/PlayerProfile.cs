using System;
using System.IO;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.menu;
using Viking_x86.director;
using ZombiesWP7.xdktools;

namespace ZombiesWP7.store;

public class PlayerProfile
{
	public int kills;

	public bool[] achievables = new bool[20];

	public void AddKill()
	{
		kills++;
		if (kills == 1000)
		{
			Game1.achievementMgr.AwardAchievement(17);
		}
		if (kills == 10000)
		{
			Game1.achievementMgr.AwardAchievement(18);
		}
	}

	public void AddAchievable(int idx)
	{
		if (TrialMgr.IsTrial() && !achievables[idx])
		{
			switch (GameState.state)
			{
			case 1:
				Menu.SetLevel(11);
				break;
			case 3:
				if (TimeMgr.CurTMgr().playMode != BaseTimeMgr.PlayMode.Paused)
				{
					TimeMgr.CurTMgr().Pause(0);
					Menu.SetLevel(16);
				}
				break;
			}
		}
		achievables[idx] = true;
	}

	internal void Write(BinaryWriter w)
	{
		byte[] bytes = BitConverter.GetBytes(kills);
		w.Write((byte)bytes.Length);
		for (int i = 0; i < bytes.Length; i++)
		{
			w.Write(bytes[i]);
		}
		for (int j = 0; j < achievables.Length; j++)
		{
			w.Write((byte)(achievables[j] ? 1 : 0));
		}
	}

	internal void Read(BinaryReader r)
	{
		int num = r.ReadByte();
		byte[] array = new byte[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = r.ReadByte();
		}
		kills = BitConverter.ToInt32(array, 0);
		for (int j = 0; j < achievables.Length; j++)
		{
			achievables[j] = r.ReadByte() == 1;
		}
		AwardStoredAchievables();
	}

	internal void AwardStoredAchievables()
	{
		for (int i = 0; i < achievables.Length; i++)
		{
			if (achievables[i])
			{
				Game1.achievementMgr.AwardAchievement(i);
			}
		}
	}
}
