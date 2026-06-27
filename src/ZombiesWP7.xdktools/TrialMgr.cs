namespace ZombiesWP7.xdktools;

/// <summary>
/// Trial mode is always disabled on desktop.
/// </summary>
public class TrialMgr
{
	public static bool IsTrial()
	{
		return false;
	}

	public static void CheckTrial()
	{
		// No-op on desktop — no trial mode
	}
}
