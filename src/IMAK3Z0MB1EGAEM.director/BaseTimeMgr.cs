using System;
using System.Collections.Generic;
using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.hud;
using IMAK3Z0MB1EGAEM.menu;
using Microsoft.Xna.Framework.Media;
using ZombiesWP7;

namespace IMAK3Z0MB1EGAEM.director;

public class BaseTimeMgr
{
	public enum PlayMode
	{
		Stopped,
		Playing,
		Paused
	}

	public List<TimeSlice> timeSlice;

	public double time;

	public long startTime;

	public PlayMode playMode;

	public long pauseStartTime;

	public int phase;

	public double pulse;

	public int beat;

	public int quadbeat;

	public int octobeat;

	public int hexadecobeat;

	public double trackTime;

	public double trackLeft;

	public int playNum;

	public void Pause(int idx)
	{
		switch (GameState.state)
		{
		case 4:
		case 5:
			Game1.vgame.paused = true;
			break;
		}
		HUD.pauseOwner = idx;
		Menu.menuLevel[2].active = true;
		pauseStartTime = DateTime.UtcNow.Ticks;
		playMode = PlayMode.Paused;
		Music.Pause();
	}

	public void UnPause()
	{
		switch (GameState.state)
		{
		case 4:
		case 5:
			Game1.vgame.paused = false;
			break;
		}
		long num = DateTime.UtcNow.Ticks - pauseStartTime;
		startTime += num;
		playMode = PlayMode.Playing;
		Music.Resume();
	}

	public void Start()
	{
		beat = -1;
		phase = 0;
		startTime = DateTime.UtcNow.Ticks;
		playMode = PlayMode.Playing;
		playNum++;
	}

	public void Update()
	{
		int num = beat;
		double num2 = time;
		if (playMode != PlayMode.Playing)
		{
			return;
		}
		long ticks = DateTime.UtcNow.Ticks;
		long num3 = ticks - startTime;
		if (Music.mediafail)
		{
			time = (double)num3 / 10000000.0;
		}
		else
		{
			if (MediaPlayer.State == MediaState.Stopped)
			{
				Music.mediafail = true;
			}
			if (MediaPlayer.State == MediaState.Paused)
			{
				try
				{
					MediaPlayer.Resume();
				}
				catch
				{
					Music.mediafail = true;
				}
			}
			time = (double)MediaPlayer.PlayPosition.Ticks / 10000000.0;
		}
		for (int i = 0; i < timeSlice.Count; i++)
		{
			if (timeSlice[i].start < time)
			{
				phase = i;
			}
		}
		double num4 = time - timeSlice[phase].start;
		double num5 = 60.0 / timeSlice[phase].bpm;
		pulse = num4;
		beat = 0;
		while (pulse > num5)
		{
			pulse -= num5;
			beat++;
		}
		pulse /= num5;
		_ = pulse / num5;
		quadbeat = beat * 4 + (int)(pulse * 4.0);
		octobeat = beat * 8 + (int)(pulse * 8.0);
		hexadecobeat = beat * 16 + (int)(pulse * 16.0);
		trackTime = time - timeSlice[phase].start;
		if (phase < timeSlice.Count - 1)
		{
			trackLeft = timeSlice[phase + 1].start - time;
		}
		else
		{
			trackLeft = 0.0;
		}
		if (trackTime >= 0.0)
		{
			if (beat != num)
			{
				SpawnMgr.DoClick(phase, beat);
			}
			else if (num2 < timeSlice[0].start && time >= timeSlice[0].start)
			{
				SpawnMgr.DoClick(0, 0);
			}
		}
	}
}
