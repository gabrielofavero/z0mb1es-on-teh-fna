# z0mb1es (on teh phone) - FNA Recompilation

This is a recompilation of the WP7 game `z0mb1es (on teh phone)`. We had some great IMAGWZI recomps in the past (Godot, FNA on Steam), but they all targeted the base Xbox 360 Indie game. The WP7 was by far the most feature-complete version, since it not only had the base game, but also Time Viking, an endless mode and some perks.

The idea of this project is porting it to FNA so that it can be preserved. Until now, this game was basically dead media, so we will finally get to play the exclusive modes again :)

> **Cloning:** This repo uses git submodules. Clone with `git clone --recurse-submodules <url>`. If you already cloned without, run `git submodule update --init --recursive`.

## What remains and what has changed

The goal here is to keep the experience as close to the original as possible. However, since this was a Windows Phone title built on Xbox infrastructure, some adaptations had to be made.

> I'm working on a side project to expand this version further with desktop optimizations and extra features. More on that later once there's something worth sharing.

### Platform & services

| Removed / Stubbed                 | Replacement                                       |
| --------------------------------- | ------------------------------------------------- |
| Xbox LIVE sign-in & GamerServices | Local profile — no sign-in needed                |
| Xbox LIVE achievements            | Offline achievement system with local persistence |
| Xbox LIVE leaderboards            | Disabled (offline only)                           |
| Trial mode                        | Always full game                                  |
| IsolatedStorage (WP7)             | `System.IO.FileStream` save files               |
| `Microsoft.Phone.Tasks`         | Removed                                           |

### Input

The original game only supported touch input. I've added a minimal structure for Keyboard + mouse and Controller. Keep in mind this is a touch focused game, so it won't be as smooth as a native experience.

### Graphics & assets

- **Asset scaler** — if you'd like to use higher-resolution assets (from the Xbox 360 releases or upscaled), you can drop them in and the game will scale them. The game reads textures as PNGs instead of XNBs, so it's much easier to swap in custom assets without needing the XNB toolchain. Keep in mind this is just a little quality of life, optional, feature. You can simply use the original assets and have the vanilla experience.

## Preparing

### FNA

We target [FNA 26.06](https://fna.flibitijibibo.com/archive/FNA-2606.zip), but a more recent version should also work.

1. Download FNA from [fna-xna.github.io/download/](https://fna-xna.github.io/download/)
2. Extract the contents of the archive into `src/FNA/`

### Native libraries (fnalibs)

1. Download the latest build artifacts from [github.com/FNA-XNA/fnalibs-dailies/actions](https://github.com/FNA-XNA/fnalibs-dailies/actions)
2. Extract the contents into `fnalibs/`

> A full guide is available [here](https://fna-xna.github.io/docs/1%3A-Setting-Up-FNA/#step-2-download-native-libraries).

### Game assets

The original game assets come from a Windows Phone `.xap` package which you'll need to obtain separately (e.g., from a WP7 device backup or an app archive). This project uses [xna-360-and-wp7-asset-extractor](https://github.com/gabrielofavero/xna-360-and-wp7-asset-extractor) (included as a submodule) to extract and convert them.

1. Place the version 1.0 `.xap` file in `asset-extractor/`.
2. Install ffmpeg for your OS (required for WMA→OGG conversion):
   - **Windows:** `winget install ffmpeg`
   - **macOS:** `brew install ffmpeg`
   - **Linux:** `sudo apt install ffmpeg`
3. Run the extractor with the included config:

```sh
python asset-extractor/xna-360-and-wp7-asset-extractor/asset-extractor.py config asset-extractor/config.json
```

4. The extractor will:
   - Extract the `.xap` into `extracted/z0mb1es/`
   - Convert all `.xnb` files (except `Arial.xnb`) to standard formats
   - Convert `.wma` audio to `.ogg`
   - Move companion folders (`chardef/`, `gfx/`, `gfx_2/`, `maps/`, `scene/`) into `Content/`
   - Copy `Arial.xnb` and `png_manifest.json` from `src/Content/` into the output
   - Assemble everything into `output/z0mb1es/`

5. Move the result to `src/Content`:

```sh
# If src/Content already exists, back it up first
mv src/Content src/Content.bak
mv output/z0mb1es/Content src/Content
```

> The `asset-extractor/config.json` file controls the extraction pipeline. See the [extractor's README](https://github.com/gabrielofavero/xna-360-and-wp7-asset-extractor#configuration) for all available options.

## Building

```sh
dotnet build src/ZombiesWP7.csproj
```

## Running

```sh
dotnet run --project src/ZombiesWP7.csproj
```

Or build first and run the output:

```sh
dotnet build src/ZombiesWP7.csproj
./src/bin/Debug/net8.0/ZombiesWP7.exe
```

## Publishing

To create a standalone publish (self-contained with all dependencies):

```sh
dotnet publish src/ZombiesWP7.csproj -c Release -o publish
```

This will output everything to a `publish/` folder.

## License

### Original intellectual property

The original games — *I MAED A GAM3 W1TH Z0MB1ES 1N IT!!!1*, *z0mb1es (on teh phone)*, and *TIME VIKING* — are the intellectual property of **[Ska Studios](https://ska-studios.com/)**. All rights reserved.

### Source code

This repository contains a **heavily modified recompilation** of the original XNA 4.0 source code, targeting modern .NET via FNA.

- The **original code** remains © Ska Studios.
- Any modifications made by me are provided as open-source.
- This project is a non profit fan preservation effort, released in good faith for educational and archival purposes. It is not affiliated with or endorsed by Ska Studios.

### Assets

This repository does not contain any game assets (graphics, audio, fonts, maps, etc.). To run the game you must supply your own legally obtained copy of the original files. The project makes no claim over those assets.                     |

### Copyrights Disclaimer

This project is not intended to infringe on any copyrights. If you are a rights holder and have concerns, please reach out.

## Acknowledgments

- **[Ska Studios](https://ska-studios.com/)** — for the original games that made all of this possible
- **[OMNIViOLET](https://x.com/OmnivioletS)** — for the outstanding ports of IMAGWZ1I
- **[Ethan Lee](https://fna-xna.github.io/)** — for FNA, the XNA 4.0 reimplementation that powers this project
- **[Andrew McRae](https://github.com/fesh0r/xnb_parse)** — for building the XNB parser that makes WP7 and 360 asset extraction possible
- **[ryzendew](https://github.com/ryzendew/XBLA-Extract)** — for the Xbox 360 STFS extraction tooling
- **My mom** — Love you ma
