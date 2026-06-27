using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace ZombiesWP7;

/// <summary>
/// Multi-source movement aggregator.
/// Reads accelerometer (mobile), gamepad left stick, and keyboard (WASD/arrows).
/// Priority: accelerometer > gamepad > keyboard.
/// </summary>
public class AccelMgr
{
	/// <summary>Raw accelerometer state (only updated on platforms with sensors).</summary>
	public Vector3 state;

#if MOBILE_SENSORS
	private Microsoft.Devices.Sensors.Accelerometer a;
#endif

	private const float Deadzone = 0.15f;

	public AccelMgr()
	{
#if MOBILE_SENSORS
		a = new Microsoft.Devices.Sensors.Accelerometer();
		a.ReadingChanged += Default_ReadingChanged;
		a.Start();
#endif
	}

#if MOBILE_SENSORS
	private void Default_ReadingChanged(object sender, Microsoft.Devices.Sensors.AccelerometerReadingEventArgs e)
	{
		state.X = (float)e.X;
		state.Y = (float)e.Y;
		state.Z = (float)e.Z;
	}
#endif

	/// <summary>
	/// Returns the best available movement vector, scaled by layer.
	/// The output formula matches the original accelerometer-based feel
	/// and is applied uniformly across all input sources.
	/// </summary>
	public Vector2 GetAdjustedVec(float layer)
	{
		Vector2 scale = layer * 8f * new Vector2(1f, 1.5f);

		// 1. Accelerometer (mobile/tablet tilt)
#if MOBILE_SENSORS
		Vector2 accelVec = new Vector2(0f - state.Y, state.Z * 0.5f + 0.25f);
		if (accelVec.Length() > Deadzone)
			return accelVec * scale;
#endif

		// 2. Gamepad left stick
		GamePadState gs = GamePad.GetState(PlayerIndex.One);
		Vector2 stick = gs.ThumbSticks.Left;
		// ThumbSticks.Left uses +Y = up, so invert Y to match screen coords
		stick.Y = -stick.Y;
		if (stick.Length() > Deadzone)
			return stick * scale;

		// 3. Keyboard (WASD / arrow keys)
		KeyboardState ks = Keyboard.GetState();
		Vector2 keyDir = Vector2.Zero;
		if (ks.IsKeyDown(Keys.A) || ks.IsKeyDown(Keys.Left))  keyDir.X -= 1f;
		if (ks.IsKeyDown(Keys.D) || ks.IsKeyDown(Keys.Right)) keyDir.X += 1f;
		if (ks.IsKeyDown(Keys.W) || ks.IsKeyDown(Keys.Up))    keyDir.Y -= 1f;
		if (ks.IsKeyDown(Keys.S) || ks.IsKeyDown(Keys.Down))  keyDir.Y += 1f;
		if (keyDir != Vector2.Zero)
		{
			keyDir.Normalize();
			return keyDir * scale;
		}

		return Vector2.Zero;
	}
}
