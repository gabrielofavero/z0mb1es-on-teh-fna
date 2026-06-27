using System.Text;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class EndGameYouSure : MenuLevel
{
	private const string CANCEL = "Canc3l";

	private const string END_GAME = "End Gam3";

	/// <summary>No back arrow on the end-game confirmation.</summary>
	public override bool ShowBackButton => false;

	public EndGameYouSure()
	{
		title = "end game!?";
		item = new MenuItem[2]
		{
			new MenuItem("Canc3l", new Vector2(110f, 210f), 1.3f),
			new MenuItem("End Gam3", new Vector2(310f, 160f), 0.9f)
		};
		infos = new StringBuilder[2]
		{
			new StringBuilder("You will lose"),
			new StringBuilder("all progress.")
		};
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "End Gam3":
		{
			switch (GameState.state)
			{
			case 3:
				GameState.state = 2;
				break;
			case 7:
				GameState.state = 6;
				break;
			case 5:
				GameState.state = 4;
				break;
			}
			for (int i = 0; i < 4; i++)
			{
				Menu.playerState[i] = Menu.PlayerState.Out;
			}
			Menu.timeGo = 0f;
			Menu.grace = 3;
			active = false;
			break;
		}
		case "Canc3l":
			active = false;
			Menu.menuLevel[2].active = true;
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		Menu.menuLevel[2].active = true;
		base.Cancel();
	}
}
