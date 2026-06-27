using IMAK3Z0MB1EGAEM.character;
using IMAK3Z0MB1EGAEM.director;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.character;
using ZombiesWP7;
using ZombiesWP7.debug;
using ZombiesWP7.xdktools;

namespace IMAK3Z0MB1EGAEM.hud;

public class HUD
{
	private const string PAUSED = "PAUSED!!1";

	private const string UNLOCKED = "UNLOCKED!!!1";

	/// <summary>Timer for the achievement toast popup.</summary>
	private static float achievementToastTimer;

	public static string[] playerName = new string[4] { "", "", "", "" };

	private static float playerNameWidth;

	public static bool playersInited = false;

	public static int pauseOwner = -1;

	private static string[] livesStr = new string[50]
	{
		"x0", "x1", "x2", "x3", "x4", "x5", "x6", "x7", "x8", "x9",
		"x10", "x11", "x12", "x13", "x14", "x15", "x16", "x17", "x18", "x19",
		"x20", "x21", "x22", "x23", "x24", "x25", "x26", "x27", "x28", "x29",
		"x30", "x31", "x32", "x33", "x34", "x35", "x36", "x37", "x38", "x39",
		"x40", "x41", "x42", "x43", "x44", "x45", "x46", "x47", "x48", "x49"
	};

	public static void InitPlayers()
	{
		playerName = new string[4];
		for (int i = 0; i < CharMan.hero.Length; i++)
		{
			playerName[i] = "IMRADLOL" + (i + 1);
		}
		if (Game1.gamerName != null)
		{
			playerName[0] = Game1.gamerName.ToString();
			playerNameWidth = Text.GetStringWidth(playerName[0]);
		}
	}

	public static void Update()
	{
	}

	public static void Draw(SpriteBatch sprite)
	{
		if (Menu.GetActive())
		{
			Menu.Draw(sprite);
		}
		else
		{
			if (DebugMgr.screenshotMode)
			{
				return;
			}
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			InputMgr.DrawThumbpads(sprite, 1f);

			// Draw mouse cursor when non-touch input is active
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);
			InputMgr.DrawMouseCursor(sprite);
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);

			// --- Achievement toast ---
			int newest = Game1.achievementMgr.newestAwarded;
			if (newest >= 0 && newest < AchievementMgr.COUNT)
			{
				AchievementDef def = AchievementMgr.Definitions[newest];
				float toastAlpha = 1f;
				achievementToastTimer += Game1.frameTime;
				if (achievementToastTimer > 3f)
					toastAlpha = 1f - (achievementToastTimer - 3f);
				if (toastAlpha > 0f)
				{
					Vector2 toastPos = new Vector2(ScrollMan.screenSize.X / 2f, ScrollMan.screenSize.Y - 60f);
					// Background
					sprite.Draw(Game1.nullTex, toastPos, new Rectangle(0, 0, 1, 1),
						new Color(0f, 0f, 0f, 0.75f * toastAlpha), 0f,
						new Vector2(0.5f, 0.5f), new Vector2(280f, 50f), SpriteEffects.None, 1f);
					// Icon
					if (Game1.achieveTextures != null && Game1.achieveTextures.TryGetValue(def.Key, out Texture2D icon))
					{
						sprite.Draw(icon, toastPos + new Vector2(-120f, 0f),
							new Rectangle(0, 0, 64, 64), new Color(1f, 1f, 1f, toastAlpha),
							0f, new Vector2(32f, 32f), 0.5f, SpriteEffects.None, 1f);
					}
					// Title
					Text.DrawString(sprite, def.Name, toastPos + new Vector2(-50f, -10f),
						2.5f, new Color(1f, 1f, 0.2f, toastAlpha), Text.Justify.Left);
					// "UNLOCKED!!!" label
					Text.DrawString(sprite, UNLOCKED, toastPos + new Vector2(-50f, 10f),
						2f, new Color(1f, 1f, 1f, toastAlpha), Text.Justify.Left);
				}
				if (achievementToastTimer > 4f)
					achievementToastTimer = 0f;
			}
			else
			{
				achievementToastTimer = 0f;
			}
			// --- End achievement toast ---

			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
			float num = 0f;
			float num2 = 0f;
			int num3 = 0;
			float num4 = playerNameWidth * 3f - 109f + 32f;
			if (num4 < 105f)
			{
				num4 = 105f;
			}
			switch (GameState.state)
			{
			case 4:
			case 5:
			{
				for (int i = 0; i < 2; i++)
				{
					Character character = Game1.vgame.charMgr.character[i];
					if (character.team == 0 && character.exists)
					{
						num = 120f;
						num2 = -25f;
						Text.DrawString(sprite, playerName[i], new Vector2(num - 109f, 50f + num2), 3f, idToColor(i), Text.Justify.Left);
						num3 = character.lives;
						if (num3 < 0)
						{
							num3 = 0;
						}
						if (num3 > livesStr.Length - 1)
						{
							num3 = livesStr.Length - 1;
						}
						Text.DrawString(sprite, livesStr[num3], new Vector2(num + num4, 50f + num2), 3f, idToColor(i), Text.Justify.Right);
						num2 -= 5f;
						Text.DrawScore(sprite, character.score, new Vector2(num + 105f, 80f + num2), 2f, Color.White, Text.Justify.Right);
						if (character.GetShot() != 0)
						{
							sprite.Draw(Game1.vgame.spritesTex, new Vector2(num - 82f, 77f + num2), ScaleTools.ScaledRect((character.GetShot() - 1) * 64 + 256, 704, 64, 64), Color.White, 0f, new Vector2(64f, 0f) / 2f, 0.536f, SpriteEffects.None, 1f);
							Text.DrawScore(sprite, character.GetAmmo(), new Vector2(num - 80f, 80f + num2), 2f, Color.White, Text.Justify.Left);
						}
						if (CharMan.hero[i].nameIn > 0)
						{
							Text.DrawString(sprite, CharMan.hero[i].name, new Vector2(num, 150f + num2), 8f, Color.White, Text.Justify.Center, CharMan.hero[i].nameIn - 1);
						}
					}
				}
				return;
			}
			}
			for (int j = 0; j < CharMan.hero.Length; j++)
			{
				if (CharMan.hero[j].exists)
				{
					num = 120f;
					num2 = -25f;
					Text.DrawString(sprite, playerName[j], new Vector2(num - 109f, 50f + num2), 3f, idToColor(j), Text.Justify.Left);
					num3 = CharMan.hero[j].lives;
					if (num3 < 0)
					{
						num3 = 0;
					}
					if (num3 > livesStr.Length - 1)
					{
						num3 = livesStr.Length - 1;
					}
					Text.DrawString(sprite, livesStr[num3], new Vector2(num + num4, 50f + num2), 3f, idToColor(j), Text.Justify.Right);
					num2 -= 5f;
					Text.DrawScore(sprite, CharMan.hero[j].score, new Vector2(num + 105f, 80f + num2), 2f, Color.White, Text.Justify.Right);
					if (CharMan.hero[j].weapon != 0)
					{
						sprite.Draw(ZombieGame.spritesTex, new Vector2(num - 82f, 77f + num2), ScaleTools.ScaledRect(new Rectangle((CharMan.hero[j].weapon - 1) * 128, 1152, 128, 128)), Color.White, 0f, new Vector2(128f, 0f), 0.268f, SpriteEffects.None, 1f);
						Text.DrawScore(sprite, CharMan.hero[j].specialAmmo, new Vector2(num - 80f, 80f + num2), 2f, Color.White, Text.Justify.Left);
					}
					if (CharMan.hero[j].nameIn > 0)
					{
						Text.DrawString(sprite, CharMan.hero[j].name, new Vector2(num, 150f + num2), 8f, Color.White, Text.Justify.Center, CharMan.hero[j].nameIn - 1);
					}
				}
			}
		}
	}

	public static Color idToColor(int id)
	{
		return idToColor(id, 1f);
	}

	public static Color idToColor(int id, float a)
	{
		return id switch
		{
			0 => new Color(0.5f, 0.5f, 1f, a), 
			1 => new Color(1f, 0.5f, 0.5f, a), 
			2 => new Color(1f, 1f, 0.5f, a), 
			3 => new Color(0.5f, 1f, 0.5f, a), 
			_ => Color.White, 
		};
	}
}
