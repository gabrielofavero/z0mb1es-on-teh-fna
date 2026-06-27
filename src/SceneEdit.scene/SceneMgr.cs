using System;
using System.Collections.Generic;
using System.IO;
using IMAK3Z0MB1EGAEM;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;
using ZombiesWP7.xdktools;

namespace SceneEdit.scene;

public class SceneMgr
{
	public Dictionary<string, Texture2D> texture;

	public Video video;

	public int curScene;

	public int selLayer;

	public int selBubble;

	public int selKeyframe;

	public string path;

	public bool smoothcam;

	public SceneMgr()
	{
		video = new Video();
		texture = new Dictionary<string, Texture2D>();
	}

	private void DrawLayers(bool mask, SpriteBatch sprite, Scene scene)
	{
		Vector3 location = SceneCam.location;
		foreach (Layer item in scene.layer)
		{
			if (item.keyframe.Count <= 0)
			{
				continue;
			}
			switch (item.name)
			{
			case "cam":
			case "master":
				continue;
			}
			if ((!mask || !(item.name == "mask")) && (mask || !(item.name != "mask")))
			{
				continue;
			}
			int num = 0;
			float num2 = 0f;
			for (int i = 0; i < item.keyframe.Count; i++)
			{
				Keyframe keyframe = item.keyframe[i];
				if (keyframe.time <= video.time && keyframe.time > num2)
				{
					num = i;
					num2 = keyframe.time;
				}
			}
			Keyframe keyframe2 = item.keyframe[num];
			if (keyframe2.texture == null)
			{
				continue;
			}
			Vector3 loc = keyframe2.loc;
			Vector2 vector = keyframe2.scale;
			float num3 = keyframe2.r;
			float num4 = keyframe2.g;
			float num5 = keyframe2.b;
			float num6 = keyframe2.a;
			float num7 = keyframe2.angle;
			if (keyframe2.tween && num < item.keyframe.Count - 1)
			{
				Keyframe keyframe3 = item.keyframe[num + 1];
				Vector3 loc2 = keyframe3.loc;
				if (mask)
				{
					loc.Z = 0.1f;
				}
				float num8 = (video.time - keyframe2.time) / (keyframe3.time - keyframe2.time);
				loc += (loc2 - loc) * num8;
				vector = keyframe2.scale + (keyframe3.scale - keyframe2.scale) * num8;
				num3 = keyframe2.r + (keyframe3.r - keyframe2.r) * num8;
				num4 = keyframe2.g + (keyframe3.g - keyframe2.g) * num8;
				num5 = keyframe2.b + (keyframe3.b - keyframe2.b) * num8;
				num6 = keyframe2.a + (keyframe3.a - keyframe2.a) * num8;
				num7 = keyframe2.angle + (keyframe3.angle - keyframe2.angle) * num8;
			}
			num3 *= scene.r;
			num4 *= scene.g;
			num5 *= scene.b;
			if (item.name.Length > 4)
			{
				try
				{
					if (item.name.Substring(0, 4) == "rot-")
					{
						loc.X += (float)Math.Cos(video.time * 3.14f + keyframe2.loc.X + keyframe2.loc.Y) * 20f;
						loc.Y += (float)Math.Sin(video.time * 3.14f + keyframe2.loc.X + keyframe2.loc.Y) * 20f;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.StackTrace);
				}
			}
			Vector2 screenLoc = SceneCam.GetScreenLoc(loc);
			Vector2 vector2 = screenLoc - new Vector2(640f, 360f);
			num7 += SceneCam.rotation;
			screenLoc = new Vector2(640f, 360f) + new Vector2((float)Math.Cos(SceneCam.rotation) * vector2.X, (float)Math.Sin(SceneCam.rotation) * vector2.X) + new Vector2((float)Math.Cos(SceneCam.rotation + 1.57f) * vector2.Y, (float)Math.Sin(SceneCam.rotation + 1.57f) * vector2.Y);
			vector *= loc.Z;
			try
			{
				vector *= SceneCam.location.Z;
				if (!video.playing && scene.name != video.scenes[curScene].name)
				{
					num6 /= 10f;
				}
				screenLoc += loc.Z * SceneCam.tiltOffset * 5f;
				screenLoc = (screenLoc - Game1.VIEWPORT / 2f) * 0.8f + Game1.VIEWPORT / 2f;
				vector *= 0.8f;
				bool flag = true;
				switch (keyframe2.texture)
				{
				case "t_PURCHASE":
				case "t_BUY":
				case "t_FULL":
				case "t_GAME!!1":
					if (!TrialMgr.IsTrial())
					{
						flag = false;
					}
					break;
				}
				if (flag)
				{
					if (keyframe2.texture.Substring(0, 2) == "t_")
					{
						Text.DrawString(sprite, keyframe2.texture.Substring(2), screenLoc * Game1.VIEWSCALE, 10f * (vector.X + vector.Y) * Game1.VIEWSCALE, new Color(num3, num4, num5, num6), Text.Justify.Center, glow: true);
					}
					else
					{
						sprite.Draw(texture[keyframe2.texture], screenLoc * Game1.VIEWSCALE, new Rectangle(0, 0, texture[keyframe2.texture].Width, texture[keyframe2.texture].Height), new Color(num3 * num6, num4 * num6, num5 * num6, num6), num7, new Vector2((float)texture[keyframe2.texture].Width / 2f, (float)texture[keyframe2.texture].Height / 2f), ((vector.X < 0f) ? new Vector2(0f - vector.X, vector.Y) : vector) * Game1.VIEWSCALE, (vector.X < 0f) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
					}
				}
			}
			catch (Exception ex2)
			{
				Console.WriteLine(ex2.ToString());
			}
		}
		smoothcam = true;
		SceneCam.location = location;
	}

	public void Draw(SpriteBatch sprite)
	{
		if (video.scenes.Count > 0)
		{
			Scene scene = video.scenes[curScene];
			sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			DrawLayers(mask: false, sprite, scene);
			sprite.End();
		}
	}

	internal void Read(string path, ContentManager Content)
	{
		this.path = path;
		BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
		video.Read(binaryReader);
		binaryReader.Close();
		ReadTextures(Content);
	}

	private void ReadTextures(ContentManager Content)
	{
		foreach (Scene scene in video.scenes)
		{
			foreach (Layer item in scene.layer)
			{
				if (item == null)
				{
					continue;
				}
				foreach (Keyframe item2 in item.keyframe)
				{
					if (item2 != null && !(item2.texture == "cam") && !(item2.texture == "master") && !texture.ContainsKey(item2.texture))
					{
						texture.Add(item2.texture, Content.Load<Texture2D>("gfx_2/scenes/" + item2.texture));
					}
				}
			}
		}
	}

	internal void Append(string path)
	{
		this.path = path;
		BinaryReader binaryReader = new BinaryReader(File.Open(path, FileMode.Open, FileAccess.Read));
		video.Append(binaryReader);
		binaryReader.Close();
	}

	internal void Write(string path)
	{
		this.path = path;
		Write();
	}

	internal void Write()
	{
		BinaryWriter binaryWriter = new BinaryWriter(File.Open(path, FileMode.OpenOrCreate, FileAccess.Write));
		video.Write(binaryWriter);
		binaryWriter.Close();
	}

	internal void Read(string path)
	{
		this.path = path;
		BinaryReader binaryReader = new BinaryReader(TitleContainer.OpenStream(path));
		video.Read(binaryReader);
		binaryReader.Close();
	}
}
