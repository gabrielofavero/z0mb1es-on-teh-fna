using System.Text;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class HelpAndOptions : MenuLevel
{
	private const string HELP = "H3lp";

	private const string OPTIONS = "s3tt1ngs";

	private const string CREDITS = "cr3d1ts";

	private const string CUSTOM = "cust0m";

	public HelpAndOptions()
	{
		title = "h3lp & 0pti0ns";
		item = new MenuItem[3]
		{
			new MenuItem("H3lp", new Vector2(110f, 150f), 1.4f),
			new MenuItem("s3tt1ngs", new Vector2(210f, 240f), 1.2f),
			new MenuItem("cr3d1ts", new Vector2(320f, 190f), 1f)
		};
		infos = new StringBuilder[2]
		{
			new StringBuilder("Version 1.0"),
			new StringBuilder("Contact us: support@ska-studios.com")
		};
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "s3tt1ngs":
		{
			active = false;
			Settings settings = (Settings)Menu.menuLevel[5];
			settings.infos = null;
			settings.active = true;
			settings.SetFromPlayer();
			break;
		}
		case "cr3d1ts":
			active = false;
			Menu.menuLevel[13].active = true;
			break;
		case "H3lp":
			active = false;
			Menu.menuLevel[14].active = true;
			break;
		case "cust0m":
			active = false;
			Menu.menuLevel[9].active = true;
			break;
		}
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
