# OpenSteamClient (Alpha)
![Build Status](https://github.com/20PercentRendered/opensteamclient/actions/workflows/cmake.yaml/badge.svg)
A partially open-source Steam client for Linux. 
# Current status: Being rewritten in C#
## Issues, feature requests, etc
Do not expect any responses to issues with the Qt/C++ version, or even the C# version. One is very much abandoned, and the other is still WIP.
Feel free to still open issues for the Qt based client, since it's possible someone else may want to rewrite this in the future, in which case we'll transfer the applicable issues there. Oh and you can also put in feature requests, but I'll only consider them for the C# remake

# Features
- 64-bit (but needs 32-bit only steamservice for some functionality)
- Lightweight (around 70MB ram use, however this is very leaky currently)
- No steamwebhelper requirement
- All games supported (technically)
- VAC supported (you can play, but you _might_ get banned. We're not responsible if you get banned.)
- Customize as you wish (Uses your desktop's color scheme)
- Wayland supported without need for XWayland
- Uses Qt (6)

# Todo

## Important
- Everything marked `//TODO` in the code
- Update steamclient binaries
- UI is pretty crude
- Lots of debug and placeholder things in UI
- Download queue (most likely next feature)
- Friends network support
- Steam cloud support
- Shader management support
- Shutting down shouldn't be fast (we should wait for downloads to stop)

## Nice-to-haves
- Optional background music while downloading apps and/or browsing the store
- Styled UI
- Artwork


# Screenshots
![image](https://github.com/20PercentRendered/opensteamclient/assets/32398752/a83319b2-2c94-4cc1-a3ed-52982fddb4ea)
![image](https://github.com/20PercentRendered/opensteamclient/assets/32398752/d55ac400-7522-4202-84d6-8563ba8d268a)
![image](https://github.com/20PercentRendered/opensteamclient/assets/32398752/bac196f8-abce-4187-908e-d41aa410fa50)
![image](https://github.com/20PercentRendered/opensteamclient/assets/32398752/be6d4227-0911-431f-8c52-05fbb4ec112e)
![image](https://github.com/20PercentRendered/opensteamclient/assets/32398752/b2c695ec-6df6-4d2b-817b-b2a8bca0ac36)


# System requirements
## Distro requirements
- Arch Linux 

- Other distros are unsupported (as is this Qt/C++ version), since the build runs on my local arch machine and uses whatever Qt is available (terrible right? luckily not doing that mistake with the C# remake)

# Install (Ready-to-go binaries)
Warning: These releases (currently) are only meant for developers. There is an uncountable amount of bugs I've yet to fix. 

After installation, read [After Install](#after-install-important)

## From master branch or some commit (unsupported) 
1. Go to the Actions tab here on Github.
2. Click the commit you want if the build succeeded (with the title CMake)
3. There should be an "Artifacts" section
4. Download "linux-artifacts", it contains tar.gz files.
5. Extract the zip somewhere
   
After installation, read [After Install](#after-install-important)

## Arch Linux
We provide an AUR package:
`opensteam-git`

Install it through makepkg or your AUR helper. Use your application launcher to start it, or run `opensteam` in a terminal.

Then read [After Install](#after-install-important)

# After install (Important)

Note: you shouldn't start the official steam client now as it'll override our files and break both installs. 
Read [Restoring ValveSteam](#restoring-valvesteam) below if you want to use the official client again. THIS IS NOT A PROBLEM WITH THE C# REMAKE


# Install and Launching (Building from source)
0. Make sure you have the build dependencies installed. 

1. Clone this repo recursively `git clone https://github.com/OpenSteamClient/OpenSteamClient --recursive`

2. `mkdir build && cd build`

3. `cmake ..`

4. `make`

5. Make sure steam is not running.

6. `./opensteam`

7. OpenSteamClient will be installed and launched.


# Restoring ValveSteam
If you wish to revert back to the official client, you have three options:

## From the UI
1. Go to the "Steam" dropdown and pick "Quit and restore ValveSteam"
2. Opensteam will close and the official client will be restored. 

## From the command line
1. Close OpenSteamClient
2. Launch `opensteam` with the `--bootstrapper-restore-valvesteam` option.

## Manual
I won't provide exact instructions, since it's a lot of effort. Instead, I'll give you a quick rundown on how the split install works.
When opensteam is initially installed, it renames .steam to .valvesteam. It also renames .local/share/Steam to .local/share/ValveSteam. 

Opensteam then installs itself into .opensteam and .local/share/OpenSteam.

It then creates .steam as a symlink pointing to .opensteam, and .local/share/Steam pointing to .local/share/OpenSteam.
If you want to revert manually, you need to change the symlinks's targets to point to .valvesteam and .local/share/ValveSteam. 

# Q&A
## Is this abandoned?
Yes! The Qt/C++ version is abandoned, and will NOT receive further updates ever. Please wait some time as I make a C# remake. For progress on that, see [PR #20](https://github.com/OpenSteamClient/OpenSteamClient/pull/20).

## pls windows suprort
Although all the technologies we use are cross platform, windows support is not planned. THE C# REMAKE WILL SUPPORT WINDOWS! (and maybe mac)

## Partially open source?
This is a GUI for Valve's own Steam Client binaries like `steamclient.so` and `steamservice.so`. 
Those binaries are not open source and Valve doesn't officially support 3rd-party usage of these. 
This also means we inherit design choices and potential bugs from these files.
Due to this, we cannot fix everything, such as the client not conforming to the XDG paths specification. EXCEPT WE FIXED THE PATHS WITH THE C# REMAAAAAAAKE

Additionally, Valve does not provide any headers or code to go along with these binaries, so this project essentially works because of guesswork by lots of community members and projects (Thank you!) (such as [open-steamworks](https://github.com/SteamRE/open-steamworks), [open-steamworks fork by m4dEngi](https://github.com/m4dEngi/open-steamworks), [SteamTracking](https://github.com/SteamDatabase/SteamTracking), [protobufs dumped from the steam client](https://github.com/SteamDatabase/Protobufs) and [MiniUTL](https://github.com/FWGS/MiniUTL)). 

Also, thank you Valve for improving Linux gaming, and making a native Steam Client in the first place.

## What version of Steam's binaries do you use?
The repo includes a copy of `steam_client_ubuntu12.vdf`, which shows the version and has download paths for the binaries.
The urls point to regular zip files, just use `unzip`. GUI file managers don't know how to extract them for some reason.

## The client crashes a lot or doesn't start
Delete ~/.local/share/opensteam and try again. Also check that you have a PC that meets the requirements.
Also, run OpenSteamClient from the terminal and post the logs in a Github issue clearly describing your situation.

## Why such high distro requirements?
The client was made on Arch Linux, for Arch Linux (primarily), so I used whatever features were available. 

## I don't like the "Web bloats". Can I get rid of them?
Yes. This app was built to fit as many users needs as possible.

You can disable building the webview component by passing `-DNOWEBVIEW` to `cmake`.
You can also pass `--no-browser` as a command line argument to `opensteam` to disable it.

(Don't worry, the C# version supports this as well)
