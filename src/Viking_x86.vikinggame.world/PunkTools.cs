using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class PunkTools
{
	public void Draw(SpriteBatch sprite)
	{
		Rectangle r = new Rectangle
		{
			X = TimeMgr.CurTMgr().beat % 3 * 224,
			Y = TimeMgr.CurTMgr().beat % 12 / 3 * 160,
			Width = 224,
			Height = 160
		};
		for (float num = (float)TimeMgr.CurTMgr().pulse * 2f; num > 1f; num -= 1f)
		{
		}
		sprite.Draw(Game1.vgame.punkTex[(!((float)TimeMgr.CurTMgr().pulse > 0.1f)) ? 1u : 0u], VScroll.screenSize / 2f, ScaleTools.ScaledRect(r), new Color(0.65f, 0.65f, 0.65f, 1f), VScroll.angle, new Vector2(112f, 80f) / 2f, 8f + (float)TimeMgr.CurTMgr().pulse * 0.35f * 2f, SpriteEffects.None, 1f);
	}
}
