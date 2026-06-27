namespace ZombiesWP7.xdktools;

/// <summary>
/// Leaderboard uploads are disabled on desktop (no Xbox LIVE).
/// </summary>
public class LeaderboardMgr
{
	public const int IM_SCORE = 0;
	public const int TV_SCORE = 1;
	public const int EZ_SCORE = 2;

	public void Write(int lIdx, long rating)
	{
		// No-op: no Xbox LIVE leaderboards on desktop
	}
}
