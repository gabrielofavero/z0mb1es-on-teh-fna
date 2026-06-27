using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class HighScoreMenu : MenuLevel
{
	public HighScoreMenu()
	{
		title = "HIGH SC0RES!1";
		item = new MenuItem[1]
		{
			new MenuItem("", default(Vector2), 0f)
		};
	}

	public override void Click(string idx)
	{
		if (sel == 0)
		{
			Menu.scoreMode = -1;
			Menu.grace = 3;
		}
		base.Click(idx);
	}

	public override void Draw(SpriteBatch sprite)
	{
		HighScores.DrawScores(sprite, new Vector2(490f, 210f));
		base.Draw(sprite);
	}

	public override void Cancel()
	{
		Menu.scoreMode = -1;
		Menu.grace = 3;
		base.Cancel();
	}
}
