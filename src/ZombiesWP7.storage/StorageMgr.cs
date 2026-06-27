using System.IO;
using ZombiesWP7.xdktools;

namespace ZombiesWP7.storage;

public class StorageMgr
{
	public const byte VERSION = 102;

	private const string SAVE_FILE = "savegame.dat";

	internal void Write()
	{
		using FileStream fs = File.Create(SAVE_FILE);
		using BinaryWriter w = new BinaryWriter(fs);
		w.Write(VERSION);
		Game1.profile.Write(w);
		Game1.player.Write(w);
		Game1.custom.Write(w);
		Game1.achievementMgr.Write(w);
	}

	internal void Read()
	{
		try
		{
			if (!File.Exists(SAVE_FILE))
			{
				return;
			}
			using FileStream fs = File.OpenRead(SAVE_FILE);
			using BinaryReader r = new BinaryReader(fs);
			byte b = r.ReadByte();
			if (b >= 101)
			{
				Game1.profile.Read(r);
				Game1.player.Read(r);
				Game1.custom.Read(r);
				if (b >= 102)
				{
					Game1.achievementMgr.Read(r);
				}
			}
			else
			{
				ResetData();
			}
		}
		catch
		{
		}
	}

	private void ResetData()
	{
	}
}
