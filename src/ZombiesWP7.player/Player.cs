using System.IO;

namespace ZombiesWP7.player;

public class Player
{
	public Settings settings;

	public Player()
	{
		settings = new Settings();
	}

	internal void Write(BinaryWriter w)
	{
		settings.Write(w);
	}

	internal void Read(BinaryReader r)
	{
		settings.Read(r);
	}
}
