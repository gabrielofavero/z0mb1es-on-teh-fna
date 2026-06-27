using System.IO;

namespace ZombiesWP7.player;

public class Settings
{
	public const byte TRUE = 1;

	public const byte FALSE = 0;

	public bool relativeControls;

	public bool vibration;

	public bool showTouchPads = true;

	public bool enableCustom;

	public bool bgm = true;

	public bool sfx = true;

	internal void Write(BinaryWriter w)
	{
		w.Write((byte)(relativeControls ? 1 : 0));
		w.Write((byte)(vibration ? 1 : 0));
		w.Write((byte)(showTouchPads ? 1 : 0));
		w.Write((byte)(enableCustom ? 1 : 0));
		w.Write((byte)(bgm ? 1 : 0));
		w.Write((byte)(sfx ? 1 : 0));
	}

	internal void Read(BinaryReader r)
	{
		relativeControls = r.ReadByte() == 1;
		vibration = r.ReadByte() == 1;
		showTouchPads = r.ReadByte() == 1;
		enableCustom = r.ReadByte() == 1;
		bgm = r.ReadByte() == 1;
		sfx = r.ReadByte() == 1;
	}
}
