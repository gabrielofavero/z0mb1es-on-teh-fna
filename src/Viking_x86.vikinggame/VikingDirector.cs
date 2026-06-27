using IMAK3Z0MB1EGAEM.audio;
using IMAK3Z0MB1EGAEM.director;
using Viking_x86.director;
using ZombiesWP7;

namespace Viking_x86.vikinggame;

public class VikingDirector
{
	public const int PHASE_PRE = 0;

	public const int PHASE_WARPING = 1;

	public const int PHASE_MAIN = 2;

	public int phase;

	public float frame;

	public VikingDirector()
	{
		Init();
	}

	public void Init()
	{
		phase = 0;
		frame = 0f;
	}

	public void Update()
	{
		switch (phase)
		{
		case 0:
			if (Game1.vgame.charMgr.character[0].loc.X > Game1.vgame.world.towerX + 100f)
			{
				phase = 1;
				frame = 0f;
				Game1.vgame.charMgr.character[0].SetAnimation("warp", 0, overRide: true);
			}
			break;
		case 1:
			frame += Game1.frameTime;
			if (frame >= 1f)
			{
				phase = 2;
				Game1.vgame.charMgr.character[0].SetAnimation("warpin", 0, overRide: true);
				VScroll.zoom = 1.5f;
				if (Music.mediafail)
				{
					TimeMgr.VikingTMgr().Start();
				}
			}
			break;
		case 2:
			Music.Update(1);
			TimeMgr.VikingTMgr().Update();
			if (Music.mediafail && TimeMgr.VikingTMgr().playMode != BaseTimeMgr.PlayMode.Playing)
			{
				TimeMgr.VikingTMgr().playMode = BaseTimeMgr.PlayMode.Playing;
			}
			break;
		}
	}
}
