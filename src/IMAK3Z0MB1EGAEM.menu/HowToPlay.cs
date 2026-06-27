using System.Text;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class HowToPlay : MenuLevel
{
	public HowToPlay()
	{
		title = "h3lp!!?";
		infos = new StringBuilder[8]
		{
			new StringBuilder("It's a twin stick shooter!"),
			new StringBuilder(""),
			new StringBuilder("Kill everything and don't die!"),
			new StringBuilder(""),
			new StringBuilder("Move with your left thumb."),
			new StringBuilder("Kill with your right thumb!"),
			new StringBuilder(""),
			new StringBuilder("Powerups are great!")
		};
		item = new MenuItem[0];
	}

	public override void Cancel()
	{
		active = false;
		Menu.menuLevel[3].active = true;
		base.Cancel();
	}
}
