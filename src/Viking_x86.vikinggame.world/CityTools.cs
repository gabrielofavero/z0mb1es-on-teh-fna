using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame.world;

public class CityTools
{
	public void DrawBack(SpriteBatch sprite)
	{
		Vector2 vector = Game1.vgame.world.GetBase();
		vector.Y += 700f;
		float num = 800f;
		WorldTools worldTools = Game1.vgame.world.worldTools;
		Vector3 vector2 = new Vector3(vector.X, vector.Y, 0f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector, 0.25f), ScaleTools.ScaledRect(0, 0, 512, 810), Color.Black, VScroll.angle, new Vector2(256f, 810f) / 2f, VScroll.zoom * 1.2f * 2f, SpriteEffects.FlipHorizontally, 1f);
		worldTools.DrawLight(sprite, new Color(1f, 1f, 1f, 0.29f), vector2 + new Vector3(-100f, 0f, 0.45f), -1.6700001f, 1.3f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector, 0.3f), ScaleTools.ScaledRect(0, 0, 512, 810), Color.White, VScroll.angle, new Vector2(256f, 810f) / 2f, VScroll.zoom * 2f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector + new Vector2(0f - num, 0f), 0.35f), ScaleTools.ScaledRect(512, 0, 192, 640), Color.White, VScroll.angle, new Vector2(96f, 640f) / 2f, VScroll.zoom * 2f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector + new Vector2(num, 0f), 0.35f), ScaleTools.ScaledRect(704, 0, 192, 640), Color.White, VScroll.angle, new Vector2(96f, 640f) / 2f, VScroll.zoom * 2f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector, 0.35f), ScaleTools.ScaledRect(992, 0, 32, 768), Color.White, VScroll.angle, new Vector2(16f, 750f) / 2f, VScroll.zoom * 2f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector + new Vector2(-550f, 0f), 0.4f), ScaleTools.ScaledRect(992, 0, 32, 768), Color.White, VScroll.angle, new Vector2(16f, 750f) / 2f, VScroll.zoom * 1.09f * 2f, SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(vector + new Vector2(550f, 0f), 0.45f), ScaleTools.ScaledRect(992, 0, 32, 768), Color.White, VScroll.angle, new Vector2(16f, 750f) / 2f, VScroll.zoom * 1.11f * 2f, SpriteEffects.None, 1f);
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		for (int i = 0; i < 10; i++)
		{
			worldTools.DrawBlip(sprite, 1f, 0.2f, 0.2f, vector2 + new Vector3(0f, (float)i * -232f, 0.35f));
		}
		for (int j = 0; j < 10; j++)
		{
			worldTools.DrawBlip(sprite, 1f, 0.2f, 0.2f, vector2 + new Vector3(-550f, (float)j * -220f, 0.4f));
		}
		for (int k = 0; k < 10; k++)
		{
			worldTools.DrawBlip(sprite, 1f, 0.2f, 0.2f, vector2 + new Vector3(550f, (float)k * -200f, 0.45f));
		}
		worldTools.DrawLight(sprite, new Color(1f, 1f, 1f, 0.5f), vector2 + new Vector3(600f, 0f, 0.45f), -1.97f, 1f);
		worldTools.DrawLight(sprite, new Color(1f, 1f, 1f, 0.5f), vector2 + new Vector3(-700f, 0f, 0.5f), -1.47f, 1f);
		if (TimeMgr.CurTMgr().trackLeft < 2.0)
		{
			sprite.Draw(Game1.nullTex, new Rectangle(0, 0, (int)VScroll.screenSize.X, (int)VScroll.screenSize.Y), new Color((float)(1.0 - TimeMgr.CurTMgr().trackLeft / 2.0), (float)(1.0 - TimeMgr.CurTMgr().trackLeft / 2.0), (float)(1.0 - TimeMgr.CurTMgr().trackLeft / 2.0), (float)(1.0 - TimeMgr.CurTMgr().trackLeft / 2.0)));
		}
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
	}

	public void DrawFore(SpriteBatch sprite)
	{
		Vector2 vector = Game1.vgame.world.GetBase();
		Vector3 vector2 = new Vector3(vector.X, vector.Y + 100f, 0f);
		WorldTools worldTools = Game1.vgame.world.worldTools;
		worldTools.DrawLight(sprite, new Color(1f, 1f, 1f, 0.15f), vector2 + new Vector3(-300f, 11f, 1.25f), -1.47f, 1f);
		worldTools.DrawLight(sprite, new Color(1f, 1f, 1f, 0.15f), vector2 + new Vector3(300f, 11f, 1.25f), -1.6700001f, 1f);
		sprite.Draw(Game1.nullTex, VScroll.GetScreenLoc(new Vector2(VScroll.scroll.X, vector.Y + 10f), 1f), new Rectangle(0, 0, 1, 1), new Color(0f, 0f, 0f, 1f), VScroll.angle, new Vector2(0.5f, 0f), new Vector2(900f, 500f), SpriteEffects.None, 1f);
		sprite.Draw(Game1.vgame.cityTex, VScroll.GetScreenLoc(new Vector2(vector.X, vector.Y), 1f), ScaleTools.ScaledRect(0, 992, 1024, 32), Color.White, VScroll.angle, new Vector2(512f, 14f) / 2f, new Vector2(2f, 1f) * 2f, SpriteEffects.None, 1f);
	}
}
