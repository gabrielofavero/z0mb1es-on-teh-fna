using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class OrbitTools
{
	private Vector2[] star;

	private Vector3[] cloud;

	public OrbitTools()
	{
		star = new Vector2[128];
		for (int i = 0; i < star.Length; i++)
		{
			ref Vector2 reference = ref star[i];
			reference = Rand.GetRandomVec2(0f, VScroll.screenSize.X, 0f, VScroll.screenSize.Y);
		}
		cloud = new Vector3[16];
		for (int j = 0; j < cloud.Length; j++)
		{
			ref Vector3 reference2 = ref cloud[j];
			reference2 = Rand.GetRandomVec3(-400f, 400f, -200f, 100f, 0.25f, 1f);
		}
	}

	public void Draw(SpriteBatch sprite)
	{
		float num = 1f;
		if (TimeMgr.CurTMgr().trackLeft < 43.0)
		{
			num = (float)TimeMgr.CurTMgr().trackLeft / 43f;
		}
		sprite.Draw(Game1.vgame.grayTex, VScroll.screenSize / 2f, ScaleTools.ScaledRect(0, 0, 480, 480), new Color(0.1f * num, 0.1f * num, 0.1f * num, 1f), VScroll.angle, new Vector2(240f, 240f) / 2f, 4f, SpriteEffects.None, 1f);
		for (int i = 0; i < star.Length; i++)
		{
			sprite.Draw(Game1.vgame.spritesTex, star[i], ScaleTools.ScaledRect(128, 64, 128, 128), new Color(1f, 1f, 1f, num), 0f, new Vector2(64f, 64f) / 2f, (float)(i % 5) * 0.01f + 0.1f, SpriteEffects.None, 1f);
		}
		sprite.Draw(Game1.vgame.atmosTex, VScroll.GetScreenLoc(Game1.vgame.world.risingTrackBase, 0.2f), ScaleTools.ScaledRect(0, 0, 480, 480), new Color(1f, 1f, 1f, num), VScroll.angle, new Vector2(240f, 240f) / 2f, new Vector2(3f, 3f) * 2f, SpriteEffects.None, 1f);
		for (int j = 0; j < cloud.Length; j++)
		{
			sprite.Draw(Game1.vgame.spritesTex, VScroll.GetScreenLoc(new Vector2(cloud[j].X + Game1.vgame.world.risingTrackBase.X, cloud[j].Y + Game1.vgame.world.risingTrackBase.Y), cloud[j].Z), ScaleTools.ScaledRect(128, 64, 128, 128), new Color(0.2f * num, 0.2f * num, 0.2f * num, 0.2f * num), VScroll.angle, new Vector2(64f, 64f) / 2f, new Vector2(3f, 0.5f) * VScroll.zoom * 2f, SpriteEffects.None, 1f);
		}
		if (TimeMgr.VikingTMgr().trackTime < 1.0)
		{
			sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Rectangle(0, 0, 1, 1), new Color(1f - (float)TimeMgr.VikingTMgr().trackTime, 1f - (float)TimeMgr.VikingTMgr().trackTime, 1f - (float)TimeMgr.VikingTMgr().trackTime, 1f - (float)TimeMgr.VikingTMgr().trackTime));
		}
	}
}
