using System;

namespace ZombiesWP7.audio;

public class Vibration
{
	public const int DEATH_TIME = 1;

	public const int MOON_TIME = 5;

	public static void SetVibration(int seconds)
	{
#if MOBILE_SENSORS
		if (Game1.player.settings.vibration)
		{
			try
			{
				Microsoft.Devices.VibrateController.Default.Start(TimeSpan.FromSeconds(seconds));
			}
			catch
			{
			}
		}
#endif
	}
}
