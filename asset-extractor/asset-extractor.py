#!/usr/bin/env python3
"""
Asset Extractor for Z0MB1ES (on teh ph0ne)

Extracts the .xap (Windows Phone package), converts all .xnb assets
(except Arial.xnb) using xnb_parse, converts .wma audio to .ogg,
and assembles the final output matching src/Content structure.

Usage:
    python asset-extractor.py           # full extraction, tmp deleted
    python asset-extractor.py --keep-tmp # keep temp files for inspection
    python asset-extractor.py --no-move # keep output in asset-extractor/Content/
"""

import os
import sys
import shutil
import zipfile
import subprocess
import fnmatch
import argparse
from pathlib import Path

# ---------------------------------------------------------------------------
# Paths
# ---------------------------------------------------------------------------
SCRIPT_DIR = Path(__file__).parent.absolute()
XNB_PARSE_DIR = SCRIPT_DIR / 'xnb_parse'
TMP_DIR = SCRIPT_DIR / 'tmp'
EXTRACTED_DIR = TMP_DIR / 'extracted'
CONVERTED_DIR = TMP_DIR / 'converted'
OUTPUT_DIR = SCRIPT_DIR / 'Content'
SRC_CONTENT_DIR = SCRIPT_DIR.parent / 'src' / 'Content'

# Add xnb_parse to Python path so we can import its modules
sys.path.insert(0, str(XNB_PARSE_DIR))

# ---------------------------------------------------------------------------
# Helpers
# ---------------------------------------------------------------------------

def find_single_xap(directory: Path) -> Path:
    """Find exactly one .xap file in the given directory."""
    xap_files = list(directory.glob('*.xap'))
    if len(xap_files) == 0:
        sys.exit('ERROR: No .xap file found in ' + str(directory))
    if len(xap_files) > 1:
        sys.exit('ERROR: Multiple .xap files found. Please keep only one.\n' +
                 '\n'.join('  ' + str(f.name) for f in xap_files))
    return xap_files[0]


def clean_dir(path: Path):
    """Remove directory if it exists, then recreate it empty."""
    if path.exists():
        shutil.rmtree(path)
    path.mkdir(parents=True, exist_ok=True)


def copy_dir(src: Path, dst: Path):
    """Copy entire directory tree from src to dst."""
    if src.is_dir():
        if dst.exists():
            shutil.rmtree(dst)
        shutil.copytree(src, dst)
    elif src.is_file():
        dst.parent.mkdir(parents=True, exist_ok=True)
        shutil.copy2(src, dst)


# ---------------------------------------------------------------------------
# Step 1: Find and extract .xap
# ---------------------------------------------------------------------------

def step_extract_xap(xap_path: Path):
    """Copy .xap to tmp as .zip, then extract to tmp/extracted."""
    print('=' * 60)
    print('Step 1: Extracting .xap')
    print('=' * 60)

    clean_dir(TMP_DIR)
    extracted_dir = EXTRACTED_DIR
    clean_dir(extracted_dir)

    # Copy .xap → tmp/archive.zip
    zip_path = TMP_DIR / 'archive.zip'
    print(f'Copying: {xap_path.name} -> {zip_path.name}')
    shutil.copy2(xap_path, zip_path)

    # Extract
    print(f'Extracting to: {extracted_dir}')
    with zipfile.ZipFile(zip_path, 'r') as zf:
        zf.extractall(extracted_dir)

    # Clean up zip
    zip_path.unlink()

    # Show what we got
    top_items = sorted([p.name for p in extracted_dir.iterdir()])
    print(f'Extracted {len(top_items)} top-level items:')
    for item in top_items:
        full = extracted_dir / item
        if full.is_dir():
            print(f'  {item}/')
        else:
            print(f'  {item}')

    return extracted_dir


# ---------------------------------------------------------------------------
# Step 2: Convert XNB files (except Arial)
# ---------------------------------------------------------------------------

def step_convert_xnb(extracted_dir: Path):
    """Run xnb_parse on every .xnb in extracted/Content (except Arial)."""
    print()
    print('=' * 60)
    print('Step 2: Converting XNB files')
    print('=' * 60)

    content_dir = extracted_dir / 'Content'
    if not content_dir.is_dir():
        sys.exit('ERROR: Content/ directory not found in extracted files')

    # Remove Arial.xnb temporarily so it's not converted
    arial_xnb = content_dir / 'Arial.xnb'
    arial_backup = None
    if arial_xnb.exists():
        arial_backup = TMP_DIR / 'Arial.xnb.bak'
        print('Skipping Arial.xnb (will use the one from src/Content)')
        shutil.move(str(arial_xnb), str(arial_backup))

    # Prepare converted output directory
    clean_dir(CONVERTED_DIR)

    # Use xnb_parse's read_xnb_dir to convert all XNBs
    # It scans Content/ recursively, parses each .xnb, and exports to export_dir
    print(f'Converting XNBs from: {content_dir}')
    print(f'Exporting to:        {CONVERTED_DIR}')

    from xnb_parse.read_xnb_dir import read_xnb_dir
    read_xnb_dir(str(content_dir), str(CONVERTED_DIR))

    # Restore Arial.xnb if it was backed up
    if arial_backup and arial_backup.exists():
        shutil.move(str(arial_backup), str(arial_xnb))

    # Count converted files
    converted_count = sum(1 for _ in CONVERTED_DIR.rglob('*') if _.is_file())
    print(f'Converted {converted_count} files.')

    return CONVERTED_DIR


# ---------------------------------------------------------------------------
# Step 3: Convert WMA to OGG
# ---------------------------------------------------------------------------

def find_ffmpeg() -> str | None:
    """Try to locate ffmpeg on the system. Works cross-platform (Windows, Linux, macOS)."""
    # shutil.which searches PATH on all platforms
    return shutil.which('ffmpeg')


def step_convert_wma(extracted_dir: Path):
    """Convert all .wma files to .ogg using ffmpeg."""
    print()
    print('=' * 60)
    print('Step 3: Converting WMA -> OGG')
    print('=' * 60)

    # Find .wma files first
    content_dir = extracted_dir / 'Content'
    wma_files = list(content_dir.rglob('*.wma'))

    if not wma_files:
        print('No .wma files found.')
        return

    print(f'Found {len(wma_files)} .wma file(s)')

    # Locate ffmpeg
    encoder = find_ffmpeg()
    if not encoder:
        print()
        print('ERROR: ffmpeg not found in PATH.')
        print('  WMA audio files cannot be converted automatically.')
        print()
        print('  Install ffmpeg for your platform:')
        print('    Windows:  winget install ffmpeg')
        print('              (or https://ffmpeg.org/download.html)')
        print('    macOS:    brew install ffmpeg')
        print('    Linux:    sudo apt install ffmpeg  (or your package manager)')
        print()
        sys.exit(1)

    print(f'Using ffmpeg: {encoder}')

    converted = 0
    for wma_path in wma_files:
        rel_path = wma_path.relative_to(content_dir)
        ogg_rel = rel_path.with_suffix('.ogg')
        ogg_path = CONVERTED_DIR / ogg_rel

        ogg_path.parent.mkdir(parents=True, exist_ok=True)

        print(f'  {rel_path} -> {ogg_rel}')

        try:
            subprocess.run(
                [encoder, '-y', '-i', str(wma_path),
                 '-c:a', 'libvorbis', '-q:a', '6', str(ogg_path)],
                check=True, capture_output=True, text=True
            )
            converted += 1
        except subprocess.CalledProcessError as e:
            print(f'    FAILED: {e.stderr.strip() if e.stderr else e}')

    print(f'Converted {converted}/{len(wma_files)} WMA files.')


# ---------------------------------------------------------------------------
# Step 4: Copy non-XNB companion files from extracted
# ---------------------------------------------------------------------------

def step_copy_companion_files(extracted_dir: Path):
    """Copy non-XNB files from extracted that need to be alongside converted assets.

    These are files like .zsx, .zax, .zcx that are NOT in Content/ but at
    the zip root, and should end up next to the converted XNB outputs.
    """
    print()
    print('=' * 60)
    print('Step 4: Copying companion files')
    print('=' * 60)

    content_dir = extracted_dir / 'Content'

    # First, copy any non-XNB, non-WMA files from extracted/Content
    # (e.g., if there are any sidecar files inside Content/)
    if content_dir.is_dir():
        for f in content_dir.rglob('*'):
            if f.is_file() and f.suffix.lower() not in ('.xnb', '.wma'):
                rel = f.relative_to(content_dir)
                dest = CONVERTED_DIR / rel
                if not dest.exists():
                    dest.parent.mkdir(parents=True, exist_ok=True)
                    shutil.copy2(f, dest)
                    print(f'  Content/{rel}')

    # Copy root-level folders that have companion files outside Content/
    companion_folders = ['chardef', 'gfx', 'gfx_2', 'maps', 'scene']
    for folder_name in companion_folders:
        src_folder = extracted_dir / folder_name
        if not src_folder.is_dir():
            continue

        for f in src_folder.rglob('*'):
            if f.is_file():
                rel = f.relative_to(extracted_dir)
                dest = CONVERTED_DIR / rel
                if not dest.exists():
                    dest.parent.mkdir(parents=True, exist_ok=True)
                    shutil.copy2(f, dest)
                    print(f'  {rel}')

    print('Done copying companion files.')


# ---------------------------------------------------------------------------
# Step 5: Assemble final output
# ---------------------------------------------------------------------------

def step_assemble_output():
    """Create tmp/output matching src/Content structure.

    Combines:
    - Converted files from tmp/converted
    - Arial.xnb from src/Content
    """
    print()
    print('=' * 60)
    print('Step 5: Assembling final output')
    print('=' * 60)

    clean_dir(OUTPUT_DIR)

    # Copy everything from converted to output
    print(f'Copying converted files to {OUTPUT_DIR}...')
    for item in CONVERTED_DIR.iterdir():
        dest = OUTPUT_DIR / item.name
        if item.is_dir():
            shutil.copytree(item, dest)
        else:
            shutil.copy2(item, dest)

    # Copy Arial.xnb from src/Content
    arial_src = SRC_CONTENT_DIR / 'Arial.xnb'
    arial_dst = OUTPUT_DIR / 'Arial.xnb'
    if arial_src.exists():
        print(f'Copying Arial.xnb from src/Content')
        shutil.copy2(arial_src, arial_dst)
    else:
        print('WARNING: Arial.xnb not found in src/Content')

    # Copy png_manifest.json from src/Content (metadata for texture atlases)
    manifest_src = SRC_CONTENT_DIR / 'png_manifest.json'
    manifest_dst = OUTPUT_DIR / 'png_manifest.json'
    if manifest_src.exists():
        print(f'Copying png_manifest.json from src/Content')
        shutil.copy2(manifest_src, manifest_dst)
    else:
        print('WARNING: png_manifest.json not found in src/Content')

    # Count final output
    file_count = sum(1 for _ in OUTPUT_DIR.rglob('*') if _.is_file())
    print(f'Final output: {file_count} files in {OUTPUT_DIR}')

    # Show output structure
    print()
    print('Output structure:')
    for item in sorted(OUTPUT_DIR.rglob('*')):
        if item.is_file():
            rel = item.relative_to(OUTPUT_DIR)
            print(f'  {rel}')


# ---------------------------------------------------------------------------
# Main
# ---------------------------------------------------------------------------

def main():
    parser = argparse.ArgumentParser(description='Extract Z0MB1ES assets from .xap')
    parser.add_argument('--keep-tmp', action='store_true',
                        help='keep temporary files after extraction')
    parser.add_argument('--no-move', action='store_true',
                        help='always keep output in asset-extractor/Content/ ' +
                             '(do not auto-move to src/Content)')
    args = parser.parse_args()
    main.keep_tmp = args.keep_tmp
    main.no_move = args.no_move

    print('Z0MB1ES Asset Extractor')
    print('=' * 60)
    print()

    # Ensure we're running from asset-extractor directory
    os.chdir(SCRIPT_DIR)

    try:
        # Find the .xap file
        xap_path = find_single_xap(SCRIPT_DIR)
        print(f'Found: {xap_path.name}')
        print()

        # Step 1: Extract .xap
        extracted_dir = step_extract_xap(xap_path)

        # Step 2: Convert XNB files
        step_convert_xnb(extracted_dir)

        # Step 3: Convert WMA to OGG
        step_convert_wma(extracted_dir)

        # Step 4: Copy companion files (.zsx, .zax, .zcx, etc.)
        step_copy_companion_files(extracted_dir)

        # Step 5: Assemble final output
        step_assemble_output()

        # Move to src/Content if it doesn't exist (unless --no-move)
        final_output_dir = OUTPUT_DIR
        if not getattr(main, 'no_move', False) and not SRC_CONTENT_DIR.exists():
            print()
            print(f'Moving {OUTPUT_DIR.name}/ -> {SRC_CONTENT_DIR.parent.name}/{SRC_CONTENT_DIR.name}/')
            shutil.move(str(OUTPUT_DIR), str(SRC_CONTENT_DIR))
            final_output_dir = SRC_CONTENT_DIR
        elif SRC_CONTENT_DIR.exists() and not getattr(main, 'no_move', False):
            print()
            print(f'NOTE: {SRC_CONTENT_DIR} already exists.')
            print(f'  Output left in {OUTPUT_DIR} for manual merging.')

        print()
        print('=' * 60)
        print('DONE!')
        print(f'Output is in: {final_output_dir}')
        print('=' * 60)
    finally:
        # Clean up tmp directory unless --keep-tmp was passed
        if TMP_DIR.exists() and not getattr(main, 'keep_tmp', False):
            print()
            print('Cleaning up temp files...')
            shutil.rmtree(TMP_DIR)
            print('Done.')


if __name__ == '__main__':
    main()
