using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ZombiesWP7;
using ZombiesWP7.custom;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class CustomMapper : MenuLevel
{
	private const string LEFT = "<";

	private const string RIGHT = ">";

	private const string USE = "use";

	private const int LAST_DEF = 0;

	private const int LAST_POINT = 1;

	private const int LAST_SIZE = 2;

	private const int LAST_USE = 3;

	public int curPic;

	private Vector2 orig = new Vector2(150f, 175f);

	private Vector2 rOrig = new Vector2(330f, 175f);

	public float procUpdate;

	private int lastEdit;

	public bool havePics;

	public CustomMapper()
	{
		title = "cust0m1z3r";
		isCustomMapper = true;
		lastEdit = 0;
	}

	public override void Draw(SpriteBatch sprite)
	{
		base.Draw(sprite);
		Texture2D srcTexture = Game1.custom.GetSrcTexture(curPic);
		if (srcTexture == null)
		{
			return;
		}
		sprite.End();
		sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
		float num = 1f;
		if (!Game1.custom.GetSrcEnabled(curPic))
		{
			num = 0.5f;
		}
		sprite.Draw(srcTexture, orig * Game1.MENUSCALE, new Rectangle(0, 0, srcTexture.Width, srcTexture.Height), new Color(num, num, num, alpha), 0f, new Vector2((float)srcTexture.Width / 2f, (float)srcTexture.Height / 2f), 1.5f * Game1.MENUSCALE, SpriteEffects.None, 1f);
		CustomImg customImageFromSrcIdx = Game1.custom.GetCustomImageFromSrcIdx(curPic);
		if (customImageFromSrcIdx != null)
		{
			srcTexture = customImageFromSrcIdx.texture;
			if (srcTexture != null)
			{
				float num2 = customImageFromSrcIdx.GetScale() * 0.9f;
				sprite.Draw(srcTexture, (rOrig + new Vector2(0f, -20f)) * Game1.MENUSCALE, new Rectangle(0, 0, srcTexture.Width, srcTexture.Height), new Color(1f, 1f, 1f, alpha), 0f, customImageFromSrcIdx.point, 1.3f * num2 * Game1.MENUSCALE, SpriteEffects.None, 1f);
			}
			Vector2 vector = rOrig + new Vector2(0f, 60f);
			sprite.Draw(Game1.nullTex, vector * Game1.MENUSCALE, new Rectangle(0, 0, 1, 1), new Color(0.7f, 0.7f, 0.7f, alpha), 0f, new Vector2(0.5f, 0.5f), new Vector2(100f, 2f) * Game1.MENUSCALE, SpriteEffects.None, 1f);
			sprite.Draw(Game1.nullTex, (vector + new Vector2(-50f + customImageFromSrcIdx.size * 200f, 0f)) * Game1.MENUSCALE, new Rectangle(0, 0, 1, 1), new Color(1f, 1f, 1f, alpha), 0f, new Vector2(0.5f, 0.5f), new Vector2(5f, 10f) * Game1.MENUSCALE, SpriteEffects.None, 1f);
			sprite.End();
			sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
		}
	}

	public override void Update()
	{
		if (active && procUpdate > 0f)
		{
			procUpdate -= Game1.frameTime;
		}
		base.Update();
	}

	public override void Click(string idx)
	{
		switch (idx)
		{
		case "<":
			curPic--;
			if (curPic < 0)
			{
				curPic = 0;
			}
			infos = null;
			item[2].ischecked = Game1.custom.GetSrcEnabled(curPic);
			break;
		case ">":
		{
			curPic++;
			int srcImageCount = Game1.custom.GetSrcImageCount();
			if (curPic > srcImageCount - 1)
			{
				curPic = srcImageCount - 1;
			}
			infos = null;
			item[2].ischecked = Game1.custom.GetSrcEnabled(curPic);
			break;
		}
		case "use":
			item[2].ischecked = !item[2].ischecked;
			lastEdit = 3;
			if (item[2].ischecked)
			{
				Game1.custom.ProcessCustom(curPic);
				if (Game1.custom.GetSrcEnabled(curPic))
				{
					infos = new StringBuilder[1]
					{
						new StringBuilder("We'll use this image randomly.")
					};
				}
				else
				{
					infos = new StringBuilder[1]
					{
						new StringBuilder("Oops: custom image limit reached!")
					};
					item[2].ischecked = false;
				}
			}
			else
			{
				Game1.custom.FreeCustom(curPic);
				infos = new StringBuilder[1]
				{
					new StringBuilder("We won't use this image.")
				};
			}
			break;
		}
		base.Click(idx);
	}

	public override void Cancel()
	{
		Game1.storageMgr.Write();
		active = false;
		Menu.menuLevel[10].active = true;
		base.Cancel();
	}

	internal void CustomClick(Vector2 c)
	{
		if (!havePics || !(procUpdate <= 0f))
		{
			return;
		}
		Vector2 vector = c - orig * Game1.MENUSCALE;
		Vector2 vector2 = (rOrig + new Vector2(0f, 60f)) * Game1.MENUSCALE;
		Vector2 vector3 = c - vector2;
		if (!Game1.custom.GetSrcEnabled(curPic))
		{
			return;
		}
		CustomImg customImageFromSrcIdx = Game1.custom.GetCustomImageFromSrcIdx(curPic);
		if (customImageFromSrcIdx == null)
		{
			return;
		}
		if (vector3.X > -50f && vector3.X < 50f && vector3.Y > -20f && vector3.Y < 20f)
		{
			if (lastEdit != 2)
			{
				lastEdit = 2;
				infos = new StringBuilder[1]
				{
					new StringBuilder("Set the image mask size.")
				};
			}
			float num = vector3.X + 50f;
			num /= 200f;
			customImageFromSrcIdx.size = num;
			Game1.custom.ProcessCustom(curPic);
			procUpdate = 0.1f;
		}
		else if (vector.X < 90f && vector.X > -90f && vector.Y < 90f && vector.Y > -90f)
		{
			if (lastEdit != 1)
			{
				lastEdit = 1;
				infos = new StringBuilder[1]
				{
					new StringBuilder("Set the image mask origin.")
				};
			}
			customImageFromSrcIdx.point = new Vector2(50f, 50f) + vector / 1.5f;
			Game1.custom.ProcessCustom(curPic);
			procUpdate = 0.1f;
		}
	}

	internal void Init()
	{
		if (Game1.custom.GetSrcImageCount() > 0)
		{
			infos = new StringBuilder[1]
			{
				new StringBuilder("Create custom monster sprites!")
			};
			havePics = true;
			item = new MenuItem[3]
			{
				new MenuItem("<", new Vector2(30f, 180f), 1f),
				new MenuItem(">", new Vector2(450f, 180f), 1f),
				new MenuItem("use", new Vector2(240f, 280f), 0.8f, 1)
			};
		}
		else
		{
			infos = new StringBuilder[4]
			{
				new StringBuilder("You don't have any pictures!"),
				new StringBuilder(""),
				new StringBuilder("You need to have some saved pictures"),
				new StringBuilder("to create custom monster sprites.")
			};
			havePics = false;
			item = null;
		}
	}
}
