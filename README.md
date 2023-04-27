# OpenSteamClient
A partially open-source Steam client for Linux. 

# Features
- 64-bit (but needs 32-bit only steamservice for some functionality)
- Lightweight (around 70MB ram use)
- No steamwebhelper requirement
- All games supported (technically)
- VAC supported (you can play, but you _might_ get banned. We're not responsible if you get banned.)
- Customize as you wish (Uses your desktop's color scheme)
- Wayland supported without need for XWayland
- Uses Qt (6)

# Todo
- Everything marked `//TODO` in the code
- A better logo (I'm not a graphic designer)
- Login is a bit janky
- User switcher
- Global settings
- Update steamclient binaries

# System requirements
## Distro requirements
- Arch Linux 

- Ubuntu 23.04 (Lunar Lobster) or newer

- Any other distro with Qt6 6.4.2 or greater

# Install (Ready-to-go binaries)
Warning: These releases (currently) are only meant for developers. There is an uncountable amount of bugs I've yet to fix. 

## Debian and debian based distros
1. Download the deb from the Releases
2. Double click or install through the command line
3. You're set! Now launch OpenSteamClient through your application launcher, or use the terminal: `opensteam`

After installation, read [After Install](#after-install-important)

## Arch Linux
We provide an AUR package:
`opensteam-git`

Install it through makepkg or your AUR helper. Use your application launcher to start it, or run `opensteam` in a terminal.

Then read [After Install](#after-install-important)

# After install (Important)

Note: you shouldn't start the official steam client now as it'll override our files and break both installs. 
Read [Restoring ValveSteam](#restoring-valvesteam) below if you want to use the official client again.


# Install and Launching (Building from source)
0. Make sure you have the build dependencies installed. 

1. Clone this repo recursively `git clone https://github.com/20PercentRendered/opensteamclient --recursive`

2. `mkdir build && cd build`

3. `cmake ..`

4. `make`

5. Make sure steam is not running.

6. `./opensteamclient`

7. OpenSteamClient will be installed and launched.


# Restoring ValveSteam
If you wish to revert back to the official client, first:
1. Close OpenSteamClient
2. Launch `opensteamclient` with the `--bootstrapper-restore-valvesteam` option.

# Q&A
## pls windows suprort
Although all the technologies we use are cross platform, windows support is not planned.

## Partially open source?
This is a GUI for Valve's own Steam Client binaries like `steamclient.so` and `steamservice.so`. 
Those binaries are not open source and Valve doesn't officially support 3rd-party usage of these. 
This also means we inherit design choices and potential bugs from these files.
Due to this, we cannot fix everything, such as the client not conforming to the XDG paths specification.

Additionally, Valve does not provide any headers or code to go along with these binaries, so this project essentially works because of guesswork by lots of community members and projects (Thank you!) (such as [open-steamworks](https://github.com/SteamRE/open-steamworks), [open-steamworks fork by m4dEngi](https://github.com/m4dEngi/open-steamworks), [SteamTracking](https://github.com/SteamDatabase/SteamTracking), [protobufs dumped from the steam client](https://github.com/SteamDatabase/Protobufs) and [MiniUTL](https://github.com/FWGS/MiniUTL)). 

## What version of Steam's binaries do you use?
The repo includes a copy of `steam_client_ubuntu12.vdf`, which shows the version and has download paths for the binaries.
The urls point to regular zip files, just use `unzip`.

## The client crashes a lot or doesn't start
Delete ~/.local/share/opensteam and try again. Also check that you have a PC that meets the requirements.
Also, run OpenSteamClient from the terminal and post the logs in a Github issue clearly describing your situation.

## Why such high distro requirements?
The client was made on Arch Linux, for Arch Linux (primarily), so I used whatever features were available. 
