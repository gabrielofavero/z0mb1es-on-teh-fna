using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86.vikinggame.character;

namespace Viking_x86.character;

public class CharacterMgr
{
	public Character[] character;

	public Moon moon;

	internal void Reset()
	{
		for (int i = 0; i < character.Length; i++)
		{
			character[i].exists = false;
		}
		moon.active = false;
		character[0].Init(new Vector2(90f, 640f), 0, 1, 0);
		character[0].SetAnimation("idle", 0, overRide: true);
		character[0].SetShot(0);
		character[0].SetShield(-1);
		character[0].score = 0L;
	}

	public CharacterMgr()
	{
		moon = new Moon();
		character = new Character[64];
		for (int i = 0; i < character.Length; i++)
		{
			character[i] = new Character(i);
		}
	}

	public void Init(int def, Vector2 loc, Vector2 traj, int state, int face, int team)
	{
		for (int i = 0; i < character.Length; i++)
		{
			if (character[i].exists)
			{
				continue;
			}
			character[i].Init(loc, def, face, team);
			character[i].state = state;
			character[i].traj = traj;
			switch (def)
			{
			default:
				switch (state)
				{
				case 1:
					character[i].SetAnimation("idle", 0, overRide: true);
					break;
				case 0:
					character[i].SetAnimation("fly", 0, overRide: true);
					break;
				}
				break;
			case 2:
			case 3:
			case 4:
			case 5:
			case 6:
				break;
			}
			break;
		}
	}

	public void Update()
	{
		for (int i = 0; i < character.Length; i++)
		{
			if (character[i].exists)
			{
				character[i].Update();
			}
		}
		if (moon.active)
		{
			moon.Update();
		}
	}

	public void Draw(SpriteBatch sprite)
	{
		if (moon.active)
		{
			moon.Draw(sprite);
		}
		for (int i = 0; i < character.Length; i++)
		{
			if (character[i].exists)
			{
				character[i].Draw(sprite);
			}
		}
	}

	internal void KillMonsters()
	{
		for (int i = 1; i < character.Length; i++)
		{
			character[i].exists = false;
		}
	}
}
