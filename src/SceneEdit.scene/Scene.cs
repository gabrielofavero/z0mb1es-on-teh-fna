using System.Collections.Generic;
using System.IO;

namespace SceneEdit.scene;

public class Scene
{
	public List<Layer> layer;

	public string name = "";

	public float duration;

	public float r;

	public float g;

	public float b;

	public Scene()
	{
		layer = new List<Layer>();
	}

	internal void Read(BinaryReader reader)
	{
		name = reader.ReadString();
		duration = reader.ReadSingle();
		this.layer = new List<Layer>();
		int num = reader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			Layer layer = new Layer();
			layer.Read(reader);
			this.layer.Add(layer);
		}
	}

	internal void Write(BinaryWriter writer)
	{
		writer.Write(name);
		writer.Write(duration);
		writer.Write(layer.Count);
		for (int i = 0; i < layer.Count; i++)
		{
			layer[i].Write(writer);
		}
	}
}
