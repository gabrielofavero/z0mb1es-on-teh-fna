using System.Text;
using Microsoft.Xna.Framework;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class NotSignedInToLive : MenuLevel
{
	private const string OK = "ok!!1";

	public NotSignedInToLive()
	{
		title = "i am err0r";
		infos = new StringBuilder[4]
		{
			new StringBuilder(""),
			new StringBuilder("You must be signed"),
			new StringBuilder("in to Xbox LIVE to"),
			new StringBuilder("view leaderboards!")
		};
		item = new MenuItem[1]
		{
			new MenuItem("ok!!1", new Vector2(240f, 260f), 1.1f)
		};
	}

	public override void Click(string idx)
	{
		string text;
		if ((text = idx) != null && text == "ok!!1")
		{
			active = false;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		base.Cancel();
	}
}
