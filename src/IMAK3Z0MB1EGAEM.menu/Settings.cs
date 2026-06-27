using System.Text;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.director;
using Microsoft.Xna.Framework;
using ZombiesWP7;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class Settings : MenuLevel
{
	private const string SFX = "SFX";

	private const string BGM = "BGM";

	private const string VIBRATION = "V1BRATI0N";

	private const string RELATIVECONTROLS = "RELATIVE CONTR0LS";

	private const string DRAWTOUCHPADS = "SH0W T0UCHPADS";

	public Settings()
	{
		title = "s3tt1ngs";
		item = new MenuItem[5]
		{
			new MenuItem("V1BRATI0N", new Vector2(175f, 160f), 1.1f, 1),
			new MenuItem("RELATIVE CONTR0LS", new Vector2(230f, 200f), 0.9f, 1),
			new MenuItem("SH0W T0UCHPADS", new Vector2(210f, 240f), 0.95f, 1),
			new MenuItem("SFX", new Vector2(125f, 120f), 0.91f, 1),
			new MenuItem("BGM", new Vector2(270f, 120f), 0.89f, 1)
		};
	}

	public void SetFromPlayer()
	{
		item[0].ischecked = Game1.player.settings.vibration;
		item[1].ischecked = Game1.player.settings.relativeControls;
		item[2].ischecked = Game1.player.settings.showTouchPads;
		item[3].ischecked = Game1.player.settings.sfx;
		item[4].ischecked = Game1.player.settings.bgm;
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "SFX":
			item[3].ischecked = !item[3].ischecked;
			Game1.player.settings.sfx = item[3].ischecked;
			if (item[3].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Sound effects are enabled.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Sound effects are disabled.")
				};
			}
			break;
		case "BGM":
			if (GameState.state == 1)
			{
				item[4].ischecked = !item[4].ischecked;
				Game1.player.settings.bgm = item[4].ischecked;
				if (item[4].ischecked)
				{
					infos = new StringBuilder[1]
					{
						new StringBuilder("Music is enabled.")
					};
					Music.mediafail = false;
				}
				else
				{
					infos = new StringBuilder[1]
					{
						new StringBuilder("Music is disabled.")
					};
				}
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Can't toggle BGM during a game.")
				};
			}
			break;
		case "V1BRATI0N":
			item[0].ischecked = !item[0].ischecked;
			Game1.player.settings.vibration = item[0].ischecked;
			if (item[0].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Vibration is enabled.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Vibration is disabled.")
				};
			}
			break;
		case "RELATIVE CONTR0LS":
			item[1].ischecked = !item[1].ischecked;
			Game1.player.settings.relativeControls = item[1].ischecked;
			if (item[1].ischecked)
			{
				infos = new StringBuilder[2]
				{
					new StringBuilder("Virtual thumbsticks are relative to"),
					new StringBuilder("touch locations.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Virtual thumbsticks are fixed.")
				};
			}
			break;
		case "SH0W T0UCHPADS":
			item[2].ischecked = !item[2].ischecked;
			Game1.player.settings.showTouchPads = item[2].ischecked;
			if (item[2].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Show virtual thumbsticks.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("Don't show virtual thumbsticks.")
				};
			}
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		Game1.storageMgr.Write();
		active = false;
		Menu.menuLevel[3].active = true;
		base.Cancel();
	}
}
