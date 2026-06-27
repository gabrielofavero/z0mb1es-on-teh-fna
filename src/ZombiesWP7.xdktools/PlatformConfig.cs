namespace ZombiesWP7.xdktools;

/// <summary>
/// Desktop platform configuration. No trial mode, no Xbox LIVE.
/// </summary>
public static class PlatformConfig
{
	/// <summary>Always false — trial mode only exists on Xbox/WP7.</summary>
	public const bool IsTrial = false;
}
