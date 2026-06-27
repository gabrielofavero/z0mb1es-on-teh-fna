using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Viking_x86.main;

public class MenuButton
{
	public const int BUTTON_Z0MB1ES = 0;

	public const int BUTTON_VIKING = 1;

	public const int BUTTON_ENDL3SS = 2;

	public Vector2 loc;

	public int ID;

	public MenuButton(Vector2 loc, int ID)
	{
		this.ID = ID;
		this.loc = loc;
	}

	public void Update()
	{
	}

	public void Draw(SpriteBatch sprite)
	{
		switch (ID)
		{
		}
	}
}
