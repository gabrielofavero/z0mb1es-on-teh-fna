using IMAK3Z0MB1EGAEM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace Viking_x86.vikinggame.hud;

public class VikingHUD
{
	public string TheMoon = "the MOON";

	public void Draw(SpriteBatch sprite)
	{
		if (Game1.vgame.charMgr.moon.active && Game1.vgame.charMgr.moon.GetDif() > 300f)
		{
			DrawMoonHealthBox(sprite);
		}
	}

	private void DrawMoonHealthBox(SpriteBatch sprite)
	{
		int num = 400;
		int num2 = 15;
		float num3 = Game1.vgame.charMgr.moon.hp / 500f;
		if (num3 < 0f)
		{
			num3 = 0f;
		}
		if (num3 > 1f)
		{
			num3 = 1f;
		}
		Color color = new Color(1f, 1f, 1f, 0.5f);
		sprite.Draw(Game1.nullTex, new Rectangle((int)VScroll.screenSize.X / 2 - num / 2, (int)VScroll.screenSize.Y - 80, num, 1), color);
		sprite.Draw(Game1.nullTex, new Rectangle((int)VScroll.screenSize.X / 2 - num / 2, (int)VScroll.screenSize.Y - 80 + num2 - 1, num, 1), color);
		sprite.Draw(Game1.nullTex, new Rectangle((int)VScroll.screenSize.X / 2 - num / 2 - 1, (int)VScroll.screenSize.Y - 80, 1, num2), color);
		sprite.Draw(Game1.nullTex, new Rectangle((int)VScroll.screenSize.X / 2 + num / 2, (int)VScroll.screenSize.Y - 80, 1, num2), color);
		int width = (int)((float)(num - 2) * num3);
		sprite.Draw(Game1.nullTex, new Rectangle((int)VScroll.screenSize.X / 2 - num / 2 + 1, (int)VScroll.screenSize.Y - 80 + 2, width, num2 - 4), Color.Red);
		Text.DrawString(sprite, TheMoon, new Vector2(VScroll.screenSize.X / 2f, VScroll.screenSize.Y - 80f + (float)num2 / 2f), 1.5f, Color.White, Text.Justify.Center);
	}

	internal void Update()
	{
	}
}
