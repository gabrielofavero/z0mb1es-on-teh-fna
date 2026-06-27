using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Z0MB1ES;

namespace IMAK3Z0MB1EGAEM.math;

/// <summary>
/// Maps source rectangles and positions from reference (logical) coordinates
/// to actual texture pixel coordinates, driven by the PngManifest.
/// For existing assets (Reference = Actual), all operations are identity (no-op).
/// For HD replacement assets, scale factors are automatically applied.
/// </summary>
public static class AssetScaler
{
    private static Dictionary<string, (float ScaleX, float ScaleY)> _scaleFactors;

    /// <summary>
    /// Initialize scale factors from the loaded PngManifest.
    /// Call once after PngManifest.Load().
    /// </summary>
    public static void Initialize()
    {
        _scaleFactors = new Dictionary<string, (float, float)>(StringComparer.OrdinalIgnoreCase);

        var entries = PngManifest.Entries;
        if (entries == null)
            return;

        foreach (var entry in entries)
        {
            float refW = entry.ReferenceWidth > 0 ? entry.ReferenceWidth : entry.Width;
            float refH = entry.ReferenceHeight > 0 ? entry.ReferenceHeight : entry.Height;
            _scaleFactors[entry.FileName] = (entry.Width / refW, entry.Height / refH);
        }
    }

    /// <summary>
    /// Get the scale factor for a texture (actual / reference).
    /// Returns (1, 1) for unknown textures (no scaling).
    /// </summary>
    public static (float X, float Y) GetScaleFactor(string textureName)
    {
        if (_scaleFactors != null && _scaleFactors.TryGetValue(textureName, out var sf))
            return sf;
        return (1f, 1f);
    }

    /// <summary>
    /// Map a source rectangle from reference coordinates to actual texture pixel coordinates.
    /// For existing assets (Reference = Actual), this is a no-op.
    /// For HD replacements, this scales the rect proportionally.
    /// </summary>
    public static Rectangle MapSource(string textureName, Rectangle referenceRect)
    {
        var (sx, sy) = GetScaleFactor(textureName);
        return new Rectangle(
            (int)(referenceRect.X * sx),
            (int)(referenceRect.Y * sy),
            (int)(referenceRect.Width * sx),
            (int)(referenceRect.Height * sy)
        );
    }

    /// <summary>Map a single coordinate value (X axis).</summary>
    public static float MapX(string textureName, float x)
    {
        return x * GetScaleFactor(textureName).X;
    }

    /// <summary>Map a single coordinate value (Y axis).</summary>
    public static float MapY(string textureName, float y)
    {
        return y * GetScaleFactor(textureName).Y;
    }

    /// <summary>Map a Vector2 position/size.</summary>
    public static Vector2 MapVec(string textureName, Vector2 v)
    {
        var (sx, sy) = GetScaleFactor(textureName);
        return new Vector2(v.X * sx, v.Y * sy);
    }

#if DEBUG
    /// <summary>
    /// Debug-only validation: checks a loaded texture's actual dimensions against the manifest.
    /// Warns if they don't match or if the texture isn't in the manifest.
    /// </summary>
    public static void ValidateTexture(string name, Texture2D tex)
    {
        var entry = PngManifest.ByFileName(name + ".png");
        if (entry == null)
        {
            System.Diagnostics.Debug.WriteLine($"[AssetScaler] WARNING: '{name}.png' not found in manifest");
        }
        else if (entry.Width != tex.Width || entry.Height != tex.Height)
        {
            System.Diagnostics.Debug.WriteLine(
                $"[AssetScaler] WARNING: '{name}' manifest={entry.Width}x{entry.Height} actual={tex.Width}x{tex.Height}");
        }
    }
#endif
}
