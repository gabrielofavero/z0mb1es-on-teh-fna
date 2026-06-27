using System.Text;
using Microsoft.Xna.Framework;
using ZombiesWP7.debug;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class Debug : MenuLevel
{
	private const string AUTOPLAY = "AUT0PLAY";

	private const string SCREENSHOTMODE = "screenshotmode";

	private const string GODMODE = "godmode";

	public Debug()
	{
		title = "d3bug";
		item = new MenuItem[3]
		{
			new MenuItem("AUT0PLAY", new Vector2(175f, 150f), 1.1f, 1),
			new MenuItem("screenshotmode", new Vector2(230f, 210f), 0.9f, 1),
			new MenuItem("godmode", new Vector2(320f, 170f), 0.9f, 1)
		};
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "AUT0PLAY":
			item[0].ischecked = !item[0].ischecked;
			DebugMgr.autoGun = item[0].ischecked;
			if (item[0].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Autogun is enabled.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Autogun is disabled.")
				};
			}
			break;
		case "screenshotmode":
			item[1].ischecked = !item[1].ischecked;
			DebugMgr.screenshotMode = item[1].ischecked;
			if (item[0].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Screenshotmode is enabled.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Screenshotmode is disabled.")
				};
			}
			break;
		case "godmode":
			item[2].ischecked = !item[1].ischecked;
			DebugMgr.godmode = item[2].ischecked;
			if (item[2].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Godmode is enabled.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Godmode is disabled.")
				};
			}
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		Menu.menuLevel[3].active = true;
		base.Cancel();
	}
}
