using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Viking_x86.director;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.audio;

internal class Music
{
	public const int ZOMBIE_SONG = 0;

	public const int VIKING_SONG = 1;

	public const int ENDLESS_SONG = 2;

	public const int MENU_SONG = 3;

	private static Song[] bank = new Song[3];

	public static int song;

	public static bool mediafail = false;

	public static void Init()
	{
		MediaPlayer.MediaStateChanged += MediaPlayer_MediaStateChanged;
	}

	public static void LoadSong(int song, ContentManager Content, string path)
	{
		if (bank[song] == null || bank[song].IsDisposed)
		{
			bank[song] = Content.Load<Song>(path);
		}
		FrameworkDispatcher.Update();
	}

	public static void Update(int _song)
	{
		song = _song;
		TimeMgr.time = _song;
		if (!Game1.player.settings.bgm)
		{
			mediafail = true;
		}
		if (MediaPlayer.State != MediaState.Playing)
		{
			if (TimeMgr.CurTMgr().playNum > 0)
			{
				if (song == 2)
				{
					TimeMgr.CurTMgr().playNum = 0;
				}
				return;
			}
			if (!mediafail)
			{
				try
				{
					MediaPlayer.Play(bank[song]);
				}
				catch
				{
					mediafail = true;
				}
			}
			TimeMgr.CurTMgr().Start();
		}
		else
		{
			_ = mediafail;
		}
	}

	private static void MediaPlayer_MediaStateChanged(object sender, EventArgs e)
	{
	}

	public static void Stop()
	{
		if (!MediaPlayer.GameHasControl)
		{
			mediafail = true;
		}
		if (!Game1.player.settings.bgm)
		{
			mediafail = true;
		}
		if (!mediafail && (MediaPlayer.State == MediaState.Playing || MediaPlayer.State == MediaState.Paused))
		{
			MediaPlayer.Stop();
		}
	}

	public static void Start()
	{
		mediafail = !MediaPlayer.GameHasControl;
		if (!Game1.player.settings.bgm)
		{
			mediafail = true;
		}
		if (mediafail)
		{
			TimeMgr.CurTMgr().Start();
			return;
		}
		if (!MediaPlayer.GameHasControl)
		{
			mediafail = true;
		}
		else
		{
			if (MediaPlayer.State == MediaState.Playing)
			{
				MediaPlayer.Stop();
			}
			try
			{
				if (MediaPlayer.State != MediaState.Playing)
				{
					MediaPlayer.Play(bank[song]);
				}
			}
			catch
			{
				mediafail = true;
			}
		}
		TimeMgr.CurTMgr().Start();
	}

	public static void Pause()
	{
		if (!Game1.player.settings.bgm)
		{
			mediafail = true;
		}
		if (!mediafail && MediaPlayer.State != MediaState.Paused)
		{
			MediaPlayer.Pause();
		}
	}

	public static void Resume()
	{
		if (!Game1.player.settings.bgm)
		{
			mediafail = true;
		}
		if (!mediafail && MediaPlayer.State == MediaState.Paused)
		{
			MediaPlayer.Resume();
		}
	}
}
