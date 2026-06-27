using xCharEdit.Character;
using ZombiesWP7;

namespace Viking_x86.character;

public class CharDefMgr
{
	public const int DEF_VIKING = 0;

	public const int DEF_ROBOT = 1;

	public const int DEF_INVADER = 2;

	public const int DEF_BOMB = 3;

	public const int DEF_NEKO = 4;

	public const int DEF_GALAGA = 5;

	public const int DEF_BLARTARD = 6;

	public static CharDef[] charDef;

	public static void Initialize()
	{
		charDef = new CharDef[2];
		for (int i = 0; i < charDef.Length; i++)
		{
			charDef[i] = new CharDef();
		}
		ReadCharDef(0, "viking");
		Game1.loader.SetText("MAED U A V1K1NG!");
		ReadCharDef(1, "robot");
		Game1.loader.SetText("MAED U A R0B0Tz!11");
	}

	private static void ReadCharDef(int ID, string path)
	{
		charDef[ID].path = "chardef/" + path + ".zsx";
		charDef[ID].ReadShort(abs: true);
	}
}
