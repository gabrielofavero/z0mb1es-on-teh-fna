using System.Text;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using ZombiesWP7;
using ZombiesWP7.menu;
using ZombiesWP7.xdktools;

namespace IMAK3Z0MB1EGAEM.menu;

public class Upsell : MenuLevel
{
	public bool quitUponExit;

	public Upsell()
	{
		title = "Buy full gam3!!1";
		infos = new StringBuilder[6]
		{
			new StringBuilder(""),
			new StringBuilder("- Experience Epic Indie Artness in Time Viking!"),
			new StringBuilder("- Tire of twin stick mayhem in Endless Zombies!"),
			new StringBuilder("- Post scores on the Xbox LIVE leaderboards!"),
			new StringBuilder("- Earn the achievements!"),
			new StringBuilder("- Unleash your creativity in Mega Game Creator 2000!")
		};
		item = new MenuItem[1]
		{
			new MenuItem("Buy full gam3", new Vector2(240f, 260f), 1.1f)
		};
	}

	public override void Update()
	{
		// Trial is always false on desktop — this screen should never be active
		if (active && !TrialMgr.IsTrial())
		{
			active = false;
			Menu.menuLevel[12].active = true;
			Game1.profile.AwardStoredAchievables();
		}
		base.Update();
	}

	public override void Click(string idx)
	{
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		if (GameState.state == 3)
		{
			Menu.needsQuit = true;
		}
		if (quitUponExit)
		{
			Game1.needsQuit = true;
		}
		base.Cancel();
	}
}
