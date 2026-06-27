using System.Text;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using Viking_x86.director;
using ZombiesWP7;
using ZombiesWP7.menu;
using ZombiesWP7.xdktools;

namespace IMAK3Z0MB1EGAEM.menu;

public class AchieveUpsell : MenuLevel
{
	public bool quitUponExit;

	public AchieveUpsell()
	{
		title = Strings.AchieveUpsell;
		infos = new StringBuilder[7]
		{
			new StringBuilder(Strings.AchieveUpsellText1),
			new StringBuilder(""),
			new StringBuilder(Strings.AchieveUpsellText2),
			new StringBuilder(Strings.AchieveUpsellText3),
			new StringBuilder(Strings.AchieveUpsellText4),
			new StringBuilder(Strings.AchieveUpsellText5),
			new StringBuilder(Strings.AchieveUpsellText6)
		};
		item = new MenuItem[1]
		{
			new MenuItem("buy full gam3", new Vector2(240f, 260f), 1.1f)
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
			TimeMgr.CurTMgr().UnPause();
		}
		if (quitUponExit)
		{
			Game1.needsQuit = true;
		}
		base.Cancel();
	}
}
