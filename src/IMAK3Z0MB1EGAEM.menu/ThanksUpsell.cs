using System.Text;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using Viking_x86.director;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class ThanksUpsell : MenuLevel
{
	private const string WOOT = "w00t!!1";

	public ThanksUpsell()
	{
		title = "thanks 0k!!1";
		infos = new StringBuilder[4]
		{
			new StringBuilder(""),
			new StringBuilder("Your support helps bring"),
			new StringBuilder("us closer to our goal"),
			new StringBuilder("of maximum awesome!")
		};
		item = new MenuItem[1]
		{
			new MenuItem("w00t!!1", new Vector2(240f, 260f), 1.1f)
		};
	}

	public override void Click(string idx)
	{
		string text;
		if ((text = idx) != null && text == "w00t!!1")
		{
			active = false;
		}
		ResumeGame();
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		ResumeGame();
		base.Cancel();
	}

	private void ResumeGame()
	{
		if (!active && GameState.state == 3)
		{
			TimeMgr.CurTMgr().UnPause();
		}
	}
}
