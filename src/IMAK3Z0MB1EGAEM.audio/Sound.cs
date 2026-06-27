using System.Collections.Generic;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.audio;

public class Sound
{
	private const string EXPLODE = "explode";

	private const string SWORD = "sword1";

	private static Dictionary<string, SoundEffect> effects;

	private static float explodeFrame = 0f;

	private static float swordFrame = 0f;

	public static void Init(ContentManager Content)
	{
		effects = new Dictionary<string, SoundEffect>();
		LoadSound(Content, "auto");
		LoadSound(Content, "explode");
		LoadSound(Content, "flame");
		LoadSound(Content, "flame2");
		LoadSound(Content, "launch");
		LoadSound(Content, "plas2");
		LoadSound(Content, "shotgun");
		LoadSound(Content, "shotgun2");
		LoadSound(Content, "shotty");
		LoadSound(Content, "shrink");
		LoadSound(Content, "suit");
		LoadSound(Content, "swing");
		LoadSound(Content, "sword1");
		LoadSound(Content, "znormal");
		LoadSound(Content, "zrapid");
		LoadSound(Content, "zspread");
		LoadSound(Content, "zbomb");
		LoadSound(Content, "hit");
		LoadSound(Content, "back");
		SoundEffect.MasterVolume = 1f;
	}

	private static void LoadSound(ContentManager Content, string s)
	{
		effects.Add(s, Content.Load<SoundEffect>("sfx/fx/" + s));
	}

	public static void PlayExplode()
	{
		if (Game1.player.settings.sfx && explodeFrame <= 0f)
		{
			effects["explode"].Play();
			explodeFrame = 0.1f;
		}
	}

	public static void PlaySword()
	{
		if (Game1.player.settings.sfx && swordFrame <= 0f)
		{
			effects["sword1"].Play();
			swordFrame = 0.5f;
		}
	}

	public static void Update()
	{
		if (explodeFrame > 0f)
		{
			explodeFrame -= Game1.frameTime;
		}
		if (swordFrame > 0f)
		{
			swordFrame -= Game1.frameTime;
		}
	}

	public static void Play(string s)
	{
		if (Game1.player.settings.sfx && effects.ContainsKey(s))
		{
			effects[s].Play();
		}
	}

	internal static void PlayHit()
	{
		if (Game1.player.settings.sfx)
		{
			effects["hit"].Play();
		}
	}
}
