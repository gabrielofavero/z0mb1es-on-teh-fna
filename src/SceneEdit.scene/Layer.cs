using System.Collections.Generic;
using System.IO;

namespace SceneEdit.scene;

public class Layer
{
	public List<Keyframe> keyframe;

	public string name = "";

	public Layer()
	{
		keyframe = new List<Keyframe>();
	}

	internal void Write(BinaryWriter writer)
	{
		writer.Write(name);
		writer.Write(keyframe.Count);
		for (int i = 0; i < keyframe.Count; i++)
		{
			keyframe[i].Write(writer);
		}
	}

	internal void Read(BinaryReader reader)
	{
		name = reader.ReadString();
		int num = reader.ReadInt32();
		this.keyframe = new List<Keyframe>();
		for (int i = 0; i < num; i++)
		{
			Keyframe keyframe = new Keyframe();
			keyframe.Read(reader);
			if (keyframe.texture == "t_AND")
			{
				keyframe.texture = "t_&";
			}
			if (keyframe.texture == "t_PURCHASE")
			{
				keyframe.texture = "t_BUY";
			}
			this.keyframe.Add(keyframe);
		}
	}
}
