using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Z0MB1ES
{
    /// <summary>
    /// Represents a single PNG file entry in the manifest.
    /// </summary>
    public sealed class PngEntry
    {
        public string FileName { get; set; }
        public string RelativePath { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        /// <summary>The resolution the game's source rectangles were authored for.
        /// Defaults to Width if not specified (1:1 mapping for existing assets).</summary>
        public int ReferenceWidth { get; set; }

        /// <summary>The resolution the game's source rectangles were authored for.
        /// Defaults to Height if not specified (1:1 mapping for existing assets).</summary>
        public int ReferenceHeight { get; set; }
    }

    /// <summary>
    /// Loads and provides access to the PNG asset manifest (png_manifest.json).
    /// The manifest tracks every .png in the project: file name, relative path, and resolution.
    /// </summary>
    public static class PngManifest
    {
        private static List<PngEntry> _entries;
        private static Dictionary<string, PngEntry> _byFileName;
        private static Dictionary<string, PngEntry> _byRelativePath;

        /// <summary>All PNG entries in the project.</summary>
        public static IReadOnlyList<PngEntry> Entries => _entries?.AsReadOnly();

        /// <summary>
        /// Load the manifest from a JSON file. Call once at startup.
        /// </summary>
        /// <param name="jsonPath">
        /// Absolute or relative path to png_manifest.json.
        /// If null, defaults to "png_manifest.json" next to the executable.
        /// </param>
        public static void Load(string jsonPath = null)
        {
            jsonPath ??= Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "png_manifest.json");

            string json = File.ReadAllText(jsonPath);
            _entries = JsonSerializer.Deserialize<List<PngEntry>>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            _byFileName = new Dictionary<string, PngEntry>(StringComparer.OrdinalIgnoreCase);
            _byRelativePath = new Dictionary<string, PngEntry>(StringComparer.OrdinalIgnoreCase);

            foreach (var entry in _entries)
            {
                // Default reference resolution to actual resolution when not specified
                if (entry.ReferenceWidth <= 0)
                    entry.ReferenceWidth = entry.Width;
                if (entry.ReferenceHeight <= 0)
                    entry.ReferenceHeight = entry.Height;

                // FileName may not be unique; last write wins
                _byFileName[entry.FileName] = entry;
                _byRelativePath[entry.RelativePath] = entry;
            }
        }

        /// <summary>Look up a PNG entry by exact file name (case-insensitive).</summary>
        public static PngEntry ByFileName(string fileName)
        {
            _byFileName.TryGetValue(fileName, out var entry);
            return entry;
        }

        /// <summary>Look up a PNG entry by relative path (forward-slash separated, case-insensitive).</summary>
        public static PngEntry ByRelativePath(string relativePath)
        {
            _byRelativePath.TryGetValue(relativePath, out var entry);
            return entry;
        }
    }
}
