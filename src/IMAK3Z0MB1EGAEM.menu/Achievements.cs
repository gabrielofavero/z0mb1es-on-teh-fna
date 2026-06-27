using System;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using Viking_x86;
using ZombiesWP7;
using ZombiesWP7.menu;
using ZombiesWP7.xdktools;

namespace IMAK3Z0MB1EGAEM.menu;

public class Achievements : MenuLevel
{
	public void Init()
	{
		// Build menu items from local achievement definitions
		AchievementDef[] defs = AchievementMgr.Definitions;
		item = new MenuItem[defs.Length];
		for (int i = 0; i < defs.Length; i++)
		{
			bool earned = Game1.achievementMgr.IsAchieved(i);
			string desc = earned ? defs[i].Description : defs[i].HowToEarn;
			item[i] = new MenuItem(
				defs[i].Name + "(" + defs[i].GamerScore + "G)",
				new Vector2(20f, 90f + (float)i * 72f),
				Rand.GetRandomFloat(0.9f, 1.1f),
				2,
				defs[i].Key,
				desc,
				earned);
		}
	}

	public Achievements()
	{
		title = "ach1evements";
		scrolling = true;
		scrollMax = 1240f;
		item = new MenuItem[0];
	}

	public override void Update()
	{
		base.Update();
	}

	public override void Click(string idx)
	{
		base.Click(idx);
	}

	public override void Cancel()
	{
		int state = GameState.state;
		if (state == 1)
		{
			active = false;
		}
		else
		{
			active = false;
			Menu.menuLevel[2].active = true;
		}
		base.Cancel();
	}
}
