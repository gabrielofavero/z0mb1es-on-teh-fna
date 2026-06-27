using System;
using System.Text;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using ZombiesWP7;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class Leaderboards : MenuLevel
{
	private StringBuilder[] leaderName = new StringBuilder[3]
	{
		new StringBuilder("I MAED A GAM3 W1TH Z0MB1ES 1N IT!!!1"),
		new StringBuilder("Time Viking"),
		new StringBuilder("Endless Zombies")
	};

	public int curLeader;

	public bool reading;

	public void Init()
	{
		item = new MenuItem[0];
		reading = false;
		// Leaderboards are offline on desktop
		infos = new StringBuilder[6]
		{
			leaderName[curLeader],
			new StringBuilder(),
			new StringBuilder(),
			new StringBuilder(),
			new StringBuilder("Leaderboards are not available"),
			new StringBuilder("in this version."),
		};
		if (curLeader >= 0 && curLeader < leaderName.Length)
		{
			infos[0] = leaderName[curLeader];
		}
		Game1.loader.Reset();
	}

	public Leaderboards()
	{
		title = "l3aderb0ards";
		scrolling = false;
		scrollMax = 0f;
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
