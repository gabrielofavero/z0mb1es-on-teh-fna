"""
Wrapper script for the XNA asset extraction pipeline.

This script:
1. Verifies a .xap file is present in the asset-extractor directory.
2. Runs the real extractor (xna-360-and-wp7-asset-extractor/asset-extractor.py)
   with config.json.
3. Moves the resulting Content folder out of output/z0mb1es and cleans up.
4. Places Content into ../src/ (or warns if Content already exists there).
"""

import glob
import os
import shutil
import subprocess
import sys

SCRIPT_DIR = os.path.dirname(os.path.abspath(__file__))

EXTRACTOR_DIR = os.path.join(SCRIPT_DIR, "xna-360-and-wp7-asset-extractor")
EXTRACTOR_SCRIPT = os.path.join(EXTRACTOR_DIR, "asset-extractor.py")
CONFIG_FILE = os.path.join(SCRIPT_DIR, "config.json")
OUTPUT_DIR = os.path.join(SCRIPT_DIR, "output")
CONTENT_SOURCE = os.path.join(OUTPUT_DIR, "z0mb1es", "Content")
CONTENT_DEST_TEMP = os.path.join(SCRIPT_DIR, "Content")
SRC_DIR = os.path.join(SCRIPT_DIR, "..", "src")
CONTENT_FINAL = os.path.join(SRC_DIR, "Content")


def die(msg, code=1):
    """Print an error message and exit."""
    print("ERROR:", msg, file=sys.stderr)
    sys.exit(code)


def find_xap():
    """Return the path to the first .xap file found in the asset-extractor dir."""
    xaps = glob.glob(os.path.join(SCRIPT_DIR, "*.xap"))
    if not xaps:
        return None
    return xaps[0]


def main():
    # --- Step 1: Check for .xap file ---
    xap_path = find_xap()
    if xap_path is None:
        die("No .xap file found in the asset-extractor directory. "
            "Please place the .xap file here and try again.")

    print(f"Found XAP: {os.path.basename(xap_path)}")

    # --- Step 2: Run the real extractor ---
    print("Running asset extractor...")
    result = subprocess.run(
        [sys.executable, EXTRACTOR_SCRIPT, "config", CONFIG_FILE],
        cwd=SCRIPT_DIR,
    )

    if result.returncode != 0:
        die(f"Asset extraction failed with exit code {result.returncode}.")

    print("Asset extraction completed successfully.")

    # --- Step 3: Move Content out of output/z0mb1es ---
    if not os.path.isdir(CONTENT_SOURCE):
        die(f"Expected Content folder not found at: {CONTENT_SOURCE}")

    shutil.move(CONTENT_SOURCE, CONTENT_DEST_TEMP)
    print("Moved Content folder to asset-extractor directory.")

    # --- Step 4: Delete the output directory ---
    shutil.rmtree(OUTPUT_DIR)
    print("Deleted output directory.")

    # --- Step 5: Move Content into src ---
    if os.path.isdir(CONTENT_FINAL) or os.path.isfile(CONTENT_FINAL):
        print(
            "\nWARNING: A Content folder already exists in src/.\n"
            "The extracted Content is at asset-extractor/Content.\n"
            "Please review and merge it manually into src/Content."
        )
    else:
        shutil.move(CONTENT_DEST_TEMP, CONTENT_FINAL)
        print("Moved Content folder into src/. Done!")


if __name__ == "__main__":
    main()
