using System;
using Microsoft.Xna.Framework;
using Yuki_Win;
using ZombiesWP7;

namespace Viking_x86.character;

public class CharKeys
{
	public Vector2 runVec;

	public Vector2 shootVec;

	public void Update(int IDX)
	{
		runVec = InputMgr.lVec;
		shootVec = InputMgr.rVec;
		runVec.Y = 0f - runVec.Y;
		float angle = Trig.GetAngle(runVec, default(Vector2));
		angle += VScroll.angle;
		runVec = new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
	}

	internal void Clear()
	{
	}
}
