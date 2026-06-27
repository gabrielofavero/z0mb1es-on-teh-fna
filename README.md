# z0mb1es (on teh phone) - FNA Recompilation

This is a recompilation of the WP7 game `z0mb1es (on teh phone)`. We had some great IMAGWZI recomps in the past (Godot, FNA on Steam), but they all targeted the base Xbox 360 Indie game. The WP7 was by far the most feature-complete version, since it not only had the base game, but also Time Viking, an endless mode and some perks.

The idea of this project is porting it to FNA so that it can be preserved. Until now, this game was basically dead media, so we will finally get to play the exclusive modes again :)

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

### FNA and fnalibs

Both FNA and the native libs (fnalibs) are included in the repo, so you don't need to grab them separately. Just clone and you're good.

### Game assets

The original game assets come from a Windows Phone `.xap` package which you'll need to obtain separately (e.g., from a WP7 device backup or an app archive). An extraction script is provided.

1. Place the version 1.0 `.xap` file in `asset-extractor/`
2. Install ffmpeg for your OS (required for WMA→OGG conversion):
   - **Windows:** `winget install ffmpeg`
   - **macOS:** `brew install ffmpeg`
   - **Linux:** `sudo apt install ffmpeg`
3. Run the extractor:

```sh
cd asset-extractor
python asset-extractor.py
```

4. The extracted `asset-extractor/Content/` folder is already in the right structure the game requires. Just move it to src/

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
- **[Andrew McRae](https://github.com/fesh0r/xnb_parse)** — for building a python asset extractor that actually works with WP7 and 360 assets
- **My mom** — Love you ma
