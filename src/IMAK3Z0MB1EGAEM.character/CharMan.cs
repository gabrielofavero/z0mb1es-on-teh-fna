using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.character;

public class CharMan
{
	public static Hero[] hero;

	public static Monster[] monster;

	public static int monsterCount;

	public static bool areAlphas;

	public static void Init()
	{
		hero = new Hero[4];
		for (int i = 0; i < hero.Length; i++)
		{
			hero[i] = new Hero(i);
		}
		monster = new Monster[96];
		for (int j = 0; j < monster.Length; j++)
		{
			monster[j] = new Monster(j);
		}
	}

	public static void Update()
	{
		monsterCount = 0;
		for (int i = 0; i < hero.Length; i++)
		{
			hero[i].Update();
		}
		for (int j = 0; j < monster.Length; j++)
		{
			monster[j].Update();
			if (monster[j].exists)
			{
				monsterCount++;
			}
		}
	}

	public static void MakeMonster(Vector2 loc, int type)
	{
		MakeMonster(loc, type, midSpawn: false);
	}

	public static void MakeMonster(Vector2 loc, int type, bool midSpawn)
	{
		int num = 0;
		float num2 = 0f;
		for (int i = 0; i < monster.Length; i++)
		{
			if (!monster[i].exists)
			{
				monster[i].Init(loc, type, midSpawn);
				return;
			}
			if (monster[i].age > num2)
			{
				num = i;
				num2 = monster[i].age;
			}
		}
		monster[num].Init(loc, type, midSpawn);
	}

	public static void Draw(SpriteBatch sprite)
	{
		areAlphas = false;
		for (int i = 0; i < hero.Length; i++)
		{
			if (hero[i].exists && hero[i].respawnFrame <= 0f)
			{
				DrawUnderglow(hero[i].loc, sprite, i, 0.2f);
			}
		}
		for (int j = 0; j < hero.Length; j++)
		{
			hero[j].Draw(sprite);
		}
		for (int k = 0; k < monster.Length; k++)
		{
			if (monster[k].type != 10)
			{
				monster[k].Draw(sprite);
			}
			else
			{
				areAlphas = true;
			}
		}
	}

	public static void DrawAlphas(SpriteBatch sprite)
	{
		for (int i = 0; i < monster.Length; i++)
		{
			if (monster[i].type == 10 && monster[i].exists)
			{
				monster[i].Draw(sprite);
			}
		}
	}

	private static void DrawShadow(Vector2 loc, SpriteBatch sprite, float scale)
	{
		sprite.Draw(ZombieGame.spritesTex, ScrollMan.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(576, 128, 192, 192), new Color(0f, 0f, 0f, 0.5f), 0f, new Vector2(96f, 96f) / 2f, 0.4f * ScrollMan.zoom * scale * 2f, SpriteEffects.None, 1f);
	}

	public static void DrawUnderglow(Vector2 loc, SpriteBatch sprite, int idx, float scale)
	{
		Color color = Color.White;
		switch (idx)
		{
		case 0:
			color = new Color(0.2f, 0.2f, 1f, 0.4f);
			break;
		case 1:
			color = new Color(1f, 0.2f, 0.2f, 0.4f);
			break;
		case 2:
			color = new Color(1f, 1f, 0.2f, 0.4f);
			break;
		case 3:
			color = new Color(0.2f, 1f, 0.2f, 0.4f);
			break;
		}
		for (int i = 0; i < 3; i++)
		{
			sprite.Draw(ZombieGame.spritesTex, ScrollMan.GetScreenLoc(loc, 1f), ScaleTools.ScaledRect(0, 768, 256, 256), color, 0f, new Vector2(128f, 128f) / 2f, (scale + (float)i * 0.04f) * ScrollMan.zoom * 2f, SpriteEffects.None, 1f);
		}
	}
}
