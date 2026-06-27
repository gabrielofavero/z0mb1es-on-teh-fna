using Microsoft.Xna.Framework;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class QuitYouSure : MenuLevel
{
	private const string QUIT_YES = "i shall quit!1";

	private const string QUIT_NO = "i shall not quit!!1";

	public QuitYouSure()
	{
		title = "quit: y0u sure!??";
		item = new MenuItem[2]
		{
			new MenuItem("i shall quit!1", new Vector2(100f, 100f), 1f),
			new MenuItem("i shall not quit!!1", new Vector2(300f, 200f), 1f)
		};
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "i shall quit!1":
			Menu.needsQuit = true;
			break;
		case "i shall not quit!!1":
			Menu.quitYouSure = -1;
			Menu.grace = 3;
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		Menu.quitYouSure = -1;
		Menu.grace = 3;
		base.Cancel();
	}
}
