using Microsoft.Xna.Framework;
using Viking_x86.director;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class Pause : MenuLevel
{
	private const string RESUME = "Resum3";

	private const string END_GAME = "End Gam3";

	private const string HELP_AND_OPTIONS = "h3lp & 0pti0ns";

	/// <summary>No back arrow on the pause menu.</summary>
	public override bool ShowBackButton => false;

	public Pause()
	{
		title = "Paus3d!!";
		item = new MenuItem[3]
		{
			new MenuItem("Resum3", new Vector2(110f, 150f), 1.3f),
			new MenuItem("End Gam3", new Vector2(210f, 230f), 1f),
			new MenuItem("h3lp & 0pti0ns", new Vector2(310f, 190f), 0.9f)
		};
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "End Gam3":
			active = false;
			Menu.menuLevel[4].active = true;
			break;
		case "Resum3":
			active = false;
			TimeMgr.CurTMgr().UnPause();
			break;
		case "h3lp & 0pti0ns":
			active = false;
			Menu.menuLevel[3].active = true;
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		TimeMgr.CurTMgr().UnPause();
		base.Cancel();
	}
}
