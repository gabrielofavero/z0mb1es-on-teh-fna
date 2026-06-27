using System.Text;
using IMAK3Z0MB1EGAEM.audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Viking_x86;
using ZombiesWP7;
using ZombiesWP7.menu;

namespace IMAK3Z0MB1EGAEM.menu;

public class MenuLevel
{
public StringBuilder[] infos;

public MenuItem[] item;

public int sel;

public string title;

public int grace;

public float alpha;

public bool active;

private Vector2 titleOff;

public float scroll;

public float scrollMax;

public bool scrolling;

public bool isCustomMapper;

public bool isLeader;

// --- Keyboard/Controller navigation ---
private int _navIndex = -1;          // currently highlighted item index (-1 = none)
private float _navRepeatTimer = 0f;  // repeat delay timer
private const float NAV_INITIAL_DELAY = 0.35f;
private const float NAV_REPEAT_DELAY = 0.12f;
private bool _navUpPrev = false;
private bool _navDownPrev = false;

// --- Back arrow (left side, animated like main menu arrows) ---
private const float BACK_ARROW_X = 0.05f;  // fraction of screen width
private const float BACK_ARROW_Y = 0.9f;   // fraction of screen height
private const float BACK_ARROW_SIZE = 2f;
private float _arrowFrame = 0f;
private bool _backArrowHovered = false;

/// <summary>Whether to show the back arrow on this menu level. Override to false for Pause.</summary>
public virtual bool ShowBackButton => true;

/// <summary>Gets the currently highlighted item index for keyboard/controller navigation.</summary>
public int NavIndex => _navIndex;

public virtual void Update()
{
if (alpha > 0f)
{
if (item != null)
{
for (int i = 0; i < item.Length; i++)
{
item[i].Update();
}
}
		// Only apply accelerometer/gamepad/keyboard movement to title parallax
		// when touch is the active input source. Keyboard/controller inputs should
		// be reserved for menu navigation (D-pad style), not visual movement.
		if (!InputMgr.IsNonTouchActive)
		{
			Vector2 adjustedVec = Game1.accelMgr.GetAdjustedVec(0.5f);
			titleOff += (adjustedVec - titleOff) * Game1.frameTime * 15f;
		}
}
if (active)
{
if (alpha < 1f)
{
alpha += Game1.frameTime * 5f;
if (alpha > 1f)
{
alpha = 1f;
}
}

// Animate back arrow
_arrowFrame += Game1.frameTime * 0.6f;
if (_arrowFrame > 0.5f)
{
_arrowFrame -= 0.5f;
}

// --- Keyboard/Controller navigation ---
if (alpha > 0.9f && item != null && item.Length > 0)
{
UpdateNavigation();
}

if (alpha > 0.9f)
{
bool backClicked = false;

// --- Back arrow click (left side, "<") — check BEFORE menu items ---
if (ShowBackButton && InputMgr.IsClick())
{
Vector2 arrowPos = ScrollMan.screenSize * new Vector2(BACK_ARROW_X, BACK_ARROW_Y);
float arrowHitSize = 40f;
float arrowLeft = arrowPos.X - arrowHitSize * 0.5f;
float arrowRight = arrowPos.X + arrowHitSize * 0.5f;
float arrowTop = arrowPos.Y - arrowHitSize * 0.5f;
float arrowBottom = arrowPos.Y + arrowHitSize * 0.5f;

Vector2 cv = InputMgr.GetClickVec();
if (cv.X > arrowLeft && cv.X < arrowRight &&
cv.Y > arrowTop && cv.Y < arrowBottom)
{
Cancel();
backClicked = true;
}
}

if (!backClicked && InputMgr.IsClick() && item != null)
{
// Keyboard/Controller confirm: directly invoke the highlighted item, skip hit-test
if (InputMgr.IsNonTouchConfirm && _navIndex >= 0 && _navIndex < item.Length)
{
item[_navIndex].clickFrame = 0.1f;
Sound.PlayHit();
Click(item[_navIndex].text);
}
else
{
// Touch/Mouse: hit-test based click
for (int j = 0; j < item.Length && !item[j].TryClick(InputMgr.GetClickVec(), this); j++)
{
}
}
}
if (InputMgr.swiping && isCustomMapper)
{
CustomMapper customMapper = (CustomMapper)this;
customMapper.CustomClick(InputMgr.dragVec);
}

// Mouse hover tracking for back arrow (visual only)
if (ShowBackButton)
{
Vector2 arrowPos = ScrollMan.screenSize * new Vector2(BACK_ARROW_X, BACK_ARROW_Y);
float arrowHitSize = 40f;
float arrowLeft = arrowPos.X - arrowHitSize * 0.5f;
float arrowRight = arrowPos.X + arrowHitSize * 0.5f;
float arrowTop = arrowPos.Y - arrowHitSize * 0.5f;
float arrowBottom = arrowPos.Y + arrowHitSize * 0.5f;

Vector2 mousePos = InputMgr.MousePosition;
_backArrowHovered = mousePos.X > arrowLeft && mousePos.X < arrowRight &&
mousePos.Y > arrowTop && mousePos.Y < arrowBottom;
}

if (InputMgr.isBack)
{
Cancel();
}
}
if (scrolling)
{
// Auto-scroll to keep keyboard/controller highlighted item visible
if (_navIndex >= 0 && _navIndex < item.Length && item[_navIndex] != null)
{
float itemY = item[_navIndex].loc.Y - scroll;
float topMargin = 60f;
float bottomMargin = Game1.VIEWPORT.Y - 60f;
if (itemY < topMargin)
scroll -= (topMargin - itemY) * Game1.frameTime * 10f;
else if (itemY > bottomMargin)
scroll += (itemY - bottomMargin) * Game1.frameTime * 10f;
}
scroll -= InputMgr.swipe.Y;
if (scroll < 0f)
{
scroll = 0f;
}
if (scroll > scrollMax)
{
scroll = scrollMax;
}
}
}
else if (alpha > 0f)
{
alpha -= Game1.frameTime * 5f;
if (alpha < 0f)
{
alpha = 0f;
_navIndex = -1;
}
}
}

/// <summary>
/// Handles keyboard (Up/Down/Enter/Escape) and gamepad (D-pad/LeftStick/A/B) navigation.
/// Uses InputMgr.NavUpPressed / NavDownPressed for discrete step movement.
/// </summary>
private void UpdateNavigation()
{
	// Edge-triggered: move one item per press
	if (InputMgr.NavUpPressed || InputMgr.NavDownPressed)
	{
		if (_navIndex < 0 || _navIndex >= item.Length)
		{
			_navIndex = InputMgr.NavDownPressed ? 0 : (item.Length - 1);
		}
		else if (InputMgr.NavUpPressed)
		{
			_navIndex--;
			if (_navIndex < 0) _navIndex = item.Length - 1;
		}
		else if (InputMgr.NavDownPressed)
		{
			_navIndex++;
			if (_navIndex >= item.Length) _navIndex = 0;
		}
	}
	// Repeat while held (with delay)
	else if (InputMgr.NavUpHeld || InputMgr.NavDownHeld)
	{
		_navRepeatTimer += Game1.frameTime;
		float requiredDelay = (_navUpPrev || _navDownPrev) ? NAV_REPEAT_DELAY : NAV_INITIAL_DELAY;

		if (_navRepeatTimer >= requiredDelay)
		{
			_navRepeatTimer = 0f;

			if (_navIndex < 0 || _navIndex >= item.Length)
			{
				_navIndex = InputMgr.NavDownHeld ? 0 : (item.Length - 1);
			}
			else if (InputMgr.NavUpHeld)
			{
				_navIndex--;
				if (_navIndex < 0) _navIndex = item.Length - 1;
			}
			else if (InputMgr.NavDownHeld)
			{
				_navIndex++;
				if (_navIndex >= item.Length) _navIndex = 0;
			}
		}
	}
	else
	{
		_navRepeatTimer = 0f;
	}

	// Mouse hover overrides nav index
	if (InputMgr.ActiveSource == InputMgr.ActiveInputSource.Mouse)
	{
		Vector2 mousePos = InputMgr.MousePosition;
		for (int i = 0; i < item.Length; i++)
		{
			if (item[i] != null && item[i].HitTest(mousePos))
			{
				_navIndex = i;
				break;
			}
		}
	}

	_navUpPrev = InputMgr.NavUpHeld;
	_navDownPrev = InputMgr.NavDownHeld;
}

public virtual void Draw(SpriteBatch sprite)
{
Vector2 vector = Game1.VIEWPORT / 2f;
vector.Y = 50f;
Vector2 vector2 = new Vector2(0f, 0f - scroll);
sprite.End();
sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
sprite.Draw(Game1.nullTex, ScrollMan.screenSize / 2f, new Rectangle(0, 0, 1, 1), new Color(0f, 0f, 0f, alpha * 0.85f), 0f, new Vector2(0.5f, 0.5f), ScrollMan.screenSize, SpriteEffects.None, 1f);
sprite.End();
sprite.Begin(SpriteSortMode.Deferred, BlendState.Additive);
float num = 1f;
float num2 = Text.GetStringWidth(title) * 11f;
float num3 = Game1.VIEWPORT.X - 20f;
if (num2 > num3)
{
num = num3 / num2;
}
if (infos != null)
{
for (int i = 0; i < infos.Length; i++)
{
sprite.DrawString(Game1.arial, infos[i], vector + titleOff * 5f + new Vector2(0f, 45f + 18f * (float)i) + vector2, new Color(1f, 1f, 1f, alpha), 0f, Game1.arial.MeasureString(infos[i]) / 2f, 1f, SpriteEffects.None, 1f);
}
}
Text.DrawString(sprite, title, vector + titleOff * 10f + vector2, num * 11f, new Color(0.5f, 0.5f, 0.5f, alpha), Text.Justify.Center);
if (item != null)
{
for (int j = 0; j < item.Length; j++)
{
// Flashy text color for the hovered/selected item (like the title)
Color? hlColor = (j == _navIndex && InputMgr.IsNonTouchActive)
? GetHighlightTextColor(alpha)
: null;
item[j].Draw(sprite, alpha, vector2, hlColor);
}
}

// --- Draw back arrow (left side, "<") ---
if (ShowBackButton)
{
DrawBackArrow(sprite, alpha);
}

if (isLeader)
{
Leaderboards leaderboards = (Leaderboards)this;
if (leaderboards.reading)
{
sprite.End();
Game1.loader.Draw(sprite, (scroll < 40f) ? (40f - scroll) : 0f);
sprite.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
}
}
}

/// <summary>
/// Returns a flashy rainbow color for the highlighted menu item text,
/// matching the title flicker effect.
/// </summary>
private static Color GetHighlightTextColor(float alpha)
{
return new Color(
Rand.GetRandomFloat(0.4f, 1f),
Rand.GetRandomFloat(0.4f, 1f),
Rand.GetRandomFloat(0.4f, 1f),
alpha);
}

/// <summary>
/// Draws an animated "\u003c" back arrow on the left side, matching the main menu arrow style.
/// </summary>
private void DrawBackArrow(SpriteBatch sprite, float alpha)
{
// Animate the arrow like DrawArrows in MainMenu
float num3;
for (num3 = _arrowFrame; num3 > 0.5f; num3 -= 0.5f)
{
}
num3 -= 0.25f;
float num4 = 0.1f;
float num5 = num3;
if (num5 < 0f)
{
num5 = 0f - num5;
}
num5 = (0.25f - num5) * 4f;

Vector2 arrowPos = ScrollMan.screenSize * new Vector2(BACK_ARROW_X - num3 * num4, BACK_ARROW_Y);
Color arrowColor = new Color(num5, num5, num5, alpha);
Text.DrawString(sprite, "<", arrowPos, BACK_ARROW_SIZE, arrowColor, Text.Justify.Center);
}

public virtual void Click(string idx)
{
}

public virtual void Cancel()
{
}
}
