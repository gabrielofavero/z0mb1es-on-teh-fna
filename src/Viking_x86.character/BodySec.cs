using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using xCharEdit.Character;
using ZombiesWP7;

namespace Viking_x86.character;

public class BodySec
{
	public const string RUN = "run";

	public const string IDLE = "idle";

	public const string ZAP = "zap";

	public const string FLY = "fly";

	public const string ATTACK = "attack";

	public const string LAND = "land";

	public const string UZAP = "uzap";

	public const string ULZAP = "ulzap";

	public const string WARPIN = "warpin";

	public const string WARP = "warp";

	public const string DIE = "die";

	private int key;

	private float curFrame;

	private int anim;

	public string animName;

	public Vector2 legAnchorVec;

	public Vector2 torsoAnchorVec;

	internal void Update(int sec, Character me)
	{
		Animation animation = CharDefMgr.charDef[me.defID].GetAnimation(anim);
		KeyFrame keyFrame = animation.GetKeyFrame(key);
		curFrame += Game1.frameTime * 30f;
		int num = key;
		if (curFrame > (float)keyFrame.duration)
		{
			curFrame -= keyFrame.duration;
			key++;
			keyFrame = animation.GetKeyFrame(key);
			if (key >= animation.getKeyFrameArray().Length)
			{
				key = 0;
				if (me.split && sec == 1)
				{
					me.split = false;
				}
			}
		}
		if (keyFrame.frameRef >= 0)
		{
			return;
		}
		key = 0;
		if (me.split && sec == 1)
		{
			me.split = false;
		}
		if (animName == "land" || animName == "attack")
		{
			me.SetAnimation("idle", 0, overRide: true);
		}
		else if (animName == "warpin")
		{
			me.SetAnimation("idle", 0, overRide: true);
		}
		else if (animName == "warp")
		{
			key = num;
		}
		else if (animName == "die")
		{
			if (me.ID == 0)
			{
				Game1.vgame.diedOnce = true;
			}
			me.lives--;
			int lives = me.lives;
			if (me.lives >= 0)
			{
				me.Init(Game1.vgame.world.GetBase(), 0, Rand.GetRandomInt(0, 2), 0);
				me.loc.Y = Game1.vgame.world.GetMinY(me.loc.X);
				me.SetAnimation("warpin", 0, overRide: true);
				me.lives = lives;
				me.SetShield(0);
				me.SetShot(0);
			}
			else
			{
				me.KillChar();
			}
		}
	}

	internal void Draw(SpriteBatch spriteBatch, Vector2 loc, float size, int face, Character me, int sec)
	{
		CharDef charDef = CharDefMgr.charDef[me.defID];
		int num = 0;
		if (charDef.GetAnimation(anim).GetKeyFrame(key).lerp)
		{
			num = charDef.GetAnimation(anim).GetKeyFrame(key).frameRef;
			if (num < 0)
			{
				num = 0;
			}
			int idx = key + 1;
			if (charDef.GetAnimation(anim).GetKeyFrame(idx).duration <= 0)
			{
				idx = 0;
			}
			Draw(spriteBatch, loc, size, face, num, charDef.GetAnimation(anim).GetKeyFrame(idx).frameRef, me, sec);
		}
		else
		{
			num = charDef.GetAnimation(anim).GetKeyFrame(key).frameRef;
			if (num < 0)
			{
				num = 0;
			}
			Draw(spriteBatch, loc, size, face, num, -1, me, sec);
		}
	}

	internal Vector2 GetAnchorVec(int sec, Vector2 loc, float size, int face, int frameIdx, int next, Character me)
	{
		CharDef charDef = CharDefMgr.charDef[me.defID];
		Frame frame = CharDefMgr.charDef[me.defID].GetFrame(frameIdx);
		float angle = VScroll.angle;
		face = 1 - face;
		for (int i = 0; i < frame.GetPartArray().Length; i++)
		{
			Part part = frame.GetPart(i);
			if (part.idx <= -1 || VikingGame.textures[charDef.texName].GetSpriteFlags(part.idx) != sec)
			{
				continue;
			}
			_ = part.rotation;
			if (float.IsNaN(part.location.X))
			{
				part.location.X = 0f;
			}
			if (float.IsNaN(part.location.Y))
			{
				part.location.Y = 0f;
			}
			Vector2 result = VScroll.GetRotatedVec2(part.location) * size + loc;
			_ = part.scaling * size;
			if (face == 0)
			{
				_ = part.rotation;
				Vector2 location = part.location;
				location.X -= part.location.X * 2f;
				result = VScroll.GetRotatedVec2(location) * size + loc;
			}
			if (next > -1)
			{
				Frame frame2 = charDef.GetFrame(next);
				if (Frame.CanLerp(frame, frame2, i))
				{
					Part part2 = frame2.GetPart(i);
					Animation animation = charDef.GetAnimation(anim);
					KeyFrame keyFrame = animation.GetKeyFrame(key);
					float progress = curFrame / (float)keyFrame.duration;
					Vector2 location2 = part.location;
					Vector2 location3 = part2.location;
					float num = part.rotation;
					float num2 = part2.rotation;
					if (face == 0)
					{
						num = 0f - num;
						num2 = 0f - num2;
						location2.X -= part.location.X * 2f;
						location3.X -= part2.location.X * 2f;
					}
					result = VScroll.GetRotatedVec2(Frame.LerpLoc(location2, location3, progress)) * size + loc;
					Frame.LerpRotation(num, num2, progress);
					_ = Frame.LerpScale(part.scaling, part2.scaling, progress) * size;
				}
			}
			return result;
		}
		return loc;
	}

	internal void Draw(SpriteBatch spriteBatch, Vector2 loc, float size, int face, int frameIdx, int next, Character me, int sec)
	{
		Draw(spriteBatch, loc, size, face, frameIdx, next, me, sec, anchorOnly: false);
	}

	internal void Draw(SpriteBatch spriteBatch, Vector2 loc, float size, int face, int frameIdx, int next, Character me, int sec, bool anchorOnly)
	{
		Rectangle value = default(Rectangle);
		CharDef charDef = CharDefMgr.charDef[me.defID];
		Frame frame = CharDefMgr.charDef[me.defID].GetFrame(frameIdx);
		float angle = VScroll.angle;
		Vector2 origin = default(Vector2);
		Vector2 vector = default(Vector2);
		float foreBright = Game1.vgame.world.GetForeBright();
		if (!anchorOnly && sec == 1)
		{
			Vector2 vector2 = me.bodySec[0].legAnchorVec;
			Vector2 anchorVec = GetAnchorVec(3, loc, size, face, frameIdx, next, me);
			vector = vector2 - anchorVec;
		}
		face = 1 - face;
		for (int i = 0; i < frame.GetPartArray().Length; i++)
		{
			Part part = frame.GetPart(i);
			if (part.idx <= -1)
			{
				continue;
			}
			bool flag = true;
			switch (sec)
			{
			case 1:
				if (VikingGame.textures[charDef.texName].GetSpriteFlags(part.idx) != 2 && VikingGame.textures[charDef.texName].GetSpriteFlags(part.idx) != 3)
				{
					flag = false;
				}
				break;
			case 0:
				if (VikingGame.textures[charDef.texName].GetSpriteFlags(part.idx) != 1)
				{
					flag = false;
				}
				break;
			}
			if (!flag)
			{
				continue;
			}
			float rotation = part.rotation + angle;
			if (float.IsNaN(part.location.X))
			{
				part.location.X = 0f;
			}
			if (float.IsNaN(part.location.Y))
			{
				part.location.Y = 0f;
			}
			Vector2 position = VScroll.GetRotatedVec2(part.location) * size + loc;
			Vector2 vector3 = part.scaling * size;
			bool flag2 = false;
			if ((face == 1 && part.flip == 0) || (face == 0 && part.flip == 1))
			{
				flag2 = true;
			}
			if (face == 0)
			{
				rotation = 0f - part.rotation + angle;
				Vector2 location = part.location;
				location.X -= part.location.X * 2f;
				position = VScroll.GetRotatedVec2(location) * size + loc;
			}
			if (next > -1)
			{
				Frame frame2 = charDef.GetFrame(next);
				if (Frame.CanLerp(frame, frame2, i))
				{
					Part part2 = frame2.GetPart(i);
					Animation animation = charDef.GetAnimation(anim);
					KeyFrame keyFrame = animation.GetKeyFrame(key);
					float progress = curFrame / (float)keyFrame.duration;
					Vector2 location2 = part.location;
					Vector2 location3 = part2.location;
					float num = part.rotation;
					float num2 = part2.rotation;
					if (face == 0)
					{
						num = 0f - num;
						num2 = 0f - num2;
						location2.X -= part.location.X * 2f;
						location3.X -= part2.location.X * 2f;
					}
					position = VScroll.GetRotatedVec2(Frame.LerpLoc(location2, location3, progress)) * size + loc;
					rotation = Frame.LerpRotation(num, num2, progress) + angle;
					vector3 = Frame.LerpScale(part.scaling, part2.scaling, progress) * size;
				}
			}
			Color color = new Color(new Vector4(foreBright, foreBright, foreBright, 1f));
			if (part.idx >= 2000)
			{
				spriteBatch.End();
				int num3 = face;
				if (num3 == 1)
				{
					if (part.flip == 1)
					{
						num3 = 1 - num3;
					}
				}
				else if (part.flip == 1)
				{
					num3 = 1 - num3;
				}
				spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
			}
			else
			{
				if (part.idx >= 1000)
				{
					continue;
				}
				Texture2D texture2D;
				if (part.idx < 64)
				{
					if (VikingGame.textures.ContainsKey(charDef.texName))
					{
						texture2D = VikingGame.textures[charDef.texName].texture;
						value = VikingGame.textures[charDef.texName].GetSpriteRect(part.idx);
						origin = VikingGame.textures[charDef.texName].GetSpriteOrigin(part.idx);
						origin.X -= value.X;
						origin.Y -= value.Y;
						if (!flag2)
						{
							origin.X = (float)value.Width - origin.X;
						}
					}
					else
					{
						texture2D = null;
					}
				}
				else
				{
					texture2D = null;
				}
				if (texture2D != null)
				{
					if (!anchorOnly)
					{
						position += vector;
						spriteBatch.Draw(texture2D, position, value, color, rotation, origin, vector3 * 2f, (!flag2) ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 1f);
					}
					switch (VikingGame.textures[charDef.texName].GetSpriteFlags(part.idx))
					{
					case 1:
						legAnchorVec = position;
						break;
					case 3:
						torsoAnchorVec = position;
						break;
					}
				}
			}
		}
	}

	internal void SetAnim(int anim, string animName, bool overRide)
	{
		this.animName = animName;
		if (this.anim != anim || overRide)
		{
			this.anim = anim;
			key = 0;
			curFrame = 0f;
		}
	}
}
