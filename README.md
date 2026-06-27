# z0mb1es (on teh phone) - FNA Recompilation

## About

This is a recompilation of the WP7 game `z0mb1es (on teh phone)`. We had some IMAGWZI recomps in the past (Godot, FNA on Steam), but they all targeted the base Xbox 360 Indie game. The WP7 was by far the most feature-complete version, since it not only had the base game, but also time viking, an endless mode and some perks. 

The idea of this project is porting it to FNA so that it can be preserved.

## What remains and what has changed

The goal here is to have as similar to the original experience as possible. However, since it was a fully published Xbox-structured game, some adaptations had to be made.

| Removed / Stubbed                 | Replacement                                       |
| --------------------------------- | ------------------------------------------------- |
| Xbox LIVE sign-in & GamerServices | Local profile — no sign-in needed                |
| Xbox LIVE achievements            | Offline achievement system with local persistence |
| Xbox LIVE leaderboards            | Disabled (offline only)                           |
| Trial mode                        | Always full game                                  |
| IsolatedStorage (WP7)             | `System.IO.FileStream` save files                 |
| WP7 back button / Guide dialogs   | Escape key / in-game confirmations                |
| `Microsoft.Phone.Tasks`           | Removed                                           |

I've also included minimal keyboard + mouse and controller support, since the original only had touch. And if you wish to use higher assets (from the 360 titles or even upscaled), I've also implemented a little asset scaler feature. Everything else is as similar as it can be.

I'm working on a side project to expand this version and make it not only more desktop optimized, but also with some extra stuff. More on that later once I create something worth sharing.

## Preparing

### FNA and fnalibs

Both FNA and the native libs (fnalibs) are included in the repo, so you don't need to grab them separately. Just clone and you're good.

### Game assets

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

This will output everything to a `publish/` folder. You can zip that up and ship it.