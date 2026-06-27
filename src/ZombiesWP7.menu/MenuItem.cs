using IMAK3Z0MB1EGAEM;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ZombiesWP7.menu;

public class MenuItem
{
	public const int FLAG_NONE = 0;

	public const int FLAG_CHECKBOX = 1;

	public const int FLAG_ACHIEVEMENT = 2;

	public const int FLAG_LEADER_ENTRY = 3;

	public Vector2 loc;

	public Vector2 tiltLoc;

	public string text;

	private string[] checkText;

	public float clickFrame;

	public float layer;

	public int flag;

	public bool ischecked;

	private string aIDx;

	private string desc;

	private bool earned;

	public MenuItem(string str, Vector2 loc, float layer)
	{
		Init(str, loc, layer, 0, null, null, earned: false);
	}

	public MenuItem(string str, Vector2 loc, float layer, int flag)
	{
		Init(str, loc, layer, flag, null, null, earned: false);
	}

	public MenuItem(string str, Vector2 loc, float layer, int flag, string aIDx, string desc, bool earned)
	{
		Init(str, loc, layer, flag, aIDx, desc, earned);
	}

	internal void Init(string str, Vector2 loc, float layer, int flag, string aIDx, string desc, bool earned)
	{
		this.loc = loc;
		tiltLoc = loc;
		this.flag = flag;
		this.layer = layer * 5f;
		this.aIDx = aIDx;
		this.desc = desc;
		this.earned = earned;
		clickFrame = 0f;
		switch (flag)
		{
		case 1:
			text = str;
			checkText = new string[2]
			{
				"¼ " + str,
				"½ " + str
			};
			break;
		case 0:
			text = str;
			break;
		case 2:
			text = str;
			break;
		case 3:
			text = str;
			break;
		}
	}

	internal bool TryClick(Vector2 click, MenuLevel parent)
	{
		float x = 0f;
		switch (flag)
		{
		case 0:
			x = Text.GetStringWidth(text);
			break;
		case 1:
			x = Text.GetStringWidth(checkText[0]);
			break;
		}
		Vector2 vector = tiltLoc - new Vector2(x, 5f) * layer;
		Vector2 vector2 = tiltLoc + new Vector2(x, 5f) * layer;
		vector *= Game1.MENUSCALE;
		vector2 *= Game1.MENUSCALE;
		if (click.X > vector.X && click.Y > vector.Y && click.X < vector2.X && click.Y < vector2.Y)
		{
			clickFrame = 0.1f;
			Sound.PlayHit();
			parent.Click(text);
			return true;
		}
		return false;
	}

	/// <summary>
	/// Tests whether a screen-space point hits this menu item.
	/// </summary>
	internal bool HitTest(Vector2 point)
	{
		float x = 0f;
		switch (flag)
		{
		case 0:
			x = Text.GetStringWidth(text);
			break;
		case 1:
			x = Text.GetStringWidth(checkText[0]);
			break;
		}
		Vector2 vector = tiltLoc - new Vector2(x, 5f) * layer;
		Vector2 vector2 = tiltLoc + new Vector2(x, 5f) * layer;
		vector *= Game1.MENUSCALE;
		vector2 *= Game1.MENUSCALE;
		return point.X > vector.X && point.Y > vector.Y && point.X < vector2.X && point.Y < vector2.Y;
	}

	internal void Draw(SpriteBatch sprite, float alpha, Vector2 sVec, Color? highlightColor = null)
	{
		float num = layer;
		if (clickFrame > 0f)
		{
			num += clickFrame * 10f;
		}
		Color textColor = highlightColor ?? new Color(1f, 1f, 1f, alpha);
		switch (flag)
		{
		case 0:
			Text.DrawString(sprite, text, loc + (tiltLoc - loc) * 0.4f * Game1.MENUSCALE, layer * 5f, new Color(0.1f, 0.1f, 0.1f, alpha), Text.Justify.Center);
			Text.DrawString(sprite, text, tiltLoc * Game1.MENUSCALE, num, textColor, Text.Justify.Center);
			break;
		case 1:
			Text.DrawString(sprite, checkText[ischecked ? 1 : 0], loc + (tiltLoc - loc) * 0.4f * Game1.MENUSCALE, layer * 5f, new Color(0.1f, 0.1f, 0.1f, alpha), Text.Justify.Center);
			Text.DrawString(sprite, checkText[ischecked ? 1 : 0], tiltLoc * Game1.MENUSCALE, num, textColor, Text.Justify.Center);
			break;
		case 2:
			num = 5f;
			sprite.Draw(Game1.achieveTextures[aIDx], loc + sVec, new Rectangle(0, 0, 64, 64), new Color(1f, 1f, 1f, earned ? alpha : (alpha * 0.4f)), 0f, default(Vector2), num * 0.2f, SpriteEffects.None, 1f);
			sprite.DrawString(Game1.arial, text, loc + new Vector2(14.25f, -0.6f) * num + sVec, new Color(1f, 1f, 1f, earned ? alpha : (alpha * 0.4f)), 0f, default(Vector2), num * 0.29f, SpriteEffects.None, 1f);
			sprite.DrawString(Game1.arial, desc, loc + new Vector2(14.25f, 4f) * num + sVec, new Color(0.7f, 0.7f, 0.7f, earned ? alpha : (alpha * 0.7f)), 0f, default(Vector2), num * 0.19f, SpriteEffects.None, 1f);
			break;
		case 3:
			num = 1f;
			sprite.DrawString(Game1.arial, aIDx, loc + new Vector2(0f, 0f) + sVec, new Color(0.7f, 0.7f, 0.7f, alpha), 0f, default(Vector2), 1f, SpriteEffects.None, 1f);
			sprite.DrawString(Game1.arial, text, loc + new Vector2(45f, 0f) + sVec, new Color(1f, 1f, 1f, alpha), 0f, default(Vector2), 1f, SpriteEffects.None, 1f);
			sprite.DrawString(Game1.arial, desc, loc + new Vector2(400f, 0f) + sVec + new Vector2(0f - Game1.arial.MeasureString(desc).X, 0f), new Color(0.7f, 0.7f, 0.7f, alpha), 0f, default(Vector2), 1f, SpriteEffects.None, 1f);
			break;
		}
	}

	internal void Update()
	{
		// Only apply accelerometer/gamepad/keyboard movement to item parallax
		// when touch is the active input source. Keyboard/controller inputs should
		// be reserved for menu navigation (D-pad style), not visual movement.
		if (!InputMgr.IsNonTouchActive)
		{
			Vector2 vector = loc + Game1.accelMgr.GetAdjustedVec(layer);
			tiltLoc += (vector - tiltLoc) * Game1.frameTime * 10f;
		}
		if (tiltLoc.Y > 275f)
		{
			tiltLoc.Y = 275f;
		}
		if (clickFrame > 0f)
		{
			clickFrame -= Game1.frameTime;
			if (clickFrame < 0f)
			{
				clickFrame = 0f;
			}
		}
	}
}
