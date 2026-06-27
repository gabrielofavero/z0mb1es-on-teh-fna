using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace IMAK3Z0MB1EGAEM.menu;

public class HighScores
{
	public static HighScore[] highScore = new HighScore[10];

	public static void Init()
	{
		for (int i = 0; i < highScore.Length; i++)
		{
			highScore[i] = new HighScore("", 0L);
		}
	}

	public static void Read(BinaryReader reader)
	{
		for (int i = 0; i < 10; i++)
		{
			highScore[i] = new HighScore(reader.ReadString(), reader.ReadInt64());
		}
	}

	public static void Write(BinaryWriter writer)
	{
		for (int i = 0; i < 10; i++)
		{
			writer.Write(highScore[i].name);
			writer.Write(highScore[i].score);
		}
	}

	public static void AddScore(string name, long score)
	{
	}

	public static void DrawScores(SpriteBatch sprite, Vector2 loc)
	{
	}
}
