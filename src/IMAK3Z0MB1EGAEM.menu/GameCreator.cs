using System.Text;
using Microsoft.Xna.Framework;
using ZombiesWP7;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class GameCreator : MenuLevel
{
	private const string EDIT = "Edit Monsters";

	private const string ENABLE = "Enable";

	public GameCreator()
	{
		title = "GAME CREATOR";
		item = new MenuItem[2]
		{
			new MenuItem("Edit Monsters", new Vector2(175f, 190f), 1.1f),
			new MenuItem("Enable", new Vector2(240f, 230f), 0.9f, 1)
		};
	}

	public void SetFromPlayer()
	{
		item[1].ischecked = Game1.player.settings.enableCustom;
		infos = new StringBuilder[4]
		{
			new StringBuilder("Use Mega Game Creator 2000 to"),
			new StringBuilder("create custom monster sprites from"),
			new StringBuilder("your photo thumbnails."),
			new StringBuilder("It's exactly that mega!")
		};
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "Edit Monsters":
		{
			active = false;
			Menu.menuLevel[9].active = true;
			CustomMapper customMapper = (CustomMapper)Menu.menuLevel[9];
			customMapper.Init();
			break;
		}
		case "Enable":
			item[1].ischecked = !item[1].ischecked;
			Game1.player.settings.enableCustom = item[1].ischecked;
			if (item[1].ischecked)
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("We'll use your custom monster sprites.")
				};
			}
			else
			{
				infos = new StringBuilder[1]
				{
					new StringBuilder("We'll stick with classic game mode.")
				};
			}
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		active = false;
		if (Game1.player.settings.enableCustom && Game1.custom.GetAnyCustoms())
		{
			Game1.achievementMgr.AwardAchievement(19);
		}
		Game1.storageMgr.Write();
		base.Cancel();
	}
}
