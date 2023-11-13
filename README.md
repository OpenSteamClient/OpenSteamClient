<img src="Assets/opensteam-logo.svg" alt="OpenSteamClient logo" title="OpenSteamClient" align="left" height="65" />

# OpenSteamClient (C# version, still in heavy development)
A partially open-source Steam client for Windows and Linux

# Current development status
- [x] Bootstrapper
- [ ] Backend stuff:
  - [ ] JITEngine classgen with fields (would be nice for concommand support)
  - [x] Custom value types
  - [x] Callback system
  - [ ] Misc code cleanups
  - [ ] Fix TODO:s and BLOCKER:s
  - [x] Callresult system for non-callback results (needed for steamwebhelper/chromehtml/storepages)
- [ ] Account system:
  - [x] Login
  - [x] Logout
  - [x] Forget account
  - [ ] Profile pictures
  - [x] 2FA 
- [ ] Settings UI
- [ ] Library UI
  - [x] Collections backend
  - [ ] Collection editing GUI
  - [ ] Games list
  - [ ] Focused game view
  - [ ] Mini mode
  - [ ] Game settings page
  - [ ] Downloads page
  - [ ] Disabling updates for certain apps
- [ ] ConCommand support
- [ ] Steamwebhelper support
  - [ ] Store, community, profile pages
  - [ ] Non-janky typing implementation 
- [ ] Windows support
- [ ] Custom SDL lib
- [ ] Future:
  - [ ] UI Animations
  - [ ] VAC support on Windows
  - [ ] ProtonDB integration
  - [ ] Disabling workshop mods without unsubbing
  - [ ] Automatic game tweaking
  - [ ] External modding sources (like Nexus for Fallout games)
  - [ ] Cloud file manager GUI
  - [ ] UI sounds
  - [ ] Shut off PC when game finishes installing


# Features
- 64-bit (but needs some 32-bit libraries for some functionality)
- No steamwebhelper requirement (but can be used ingame and for browsing the store)
- All games supported (technically), Steam2 games unknown
- VAC supported on Linux (you can play, but you _might_ get banned. We're not responsible if you do.)
- Supports Windows and Linux, and even theoretically MacOS (PRs for support welcome)


# Blockers
Current blockers are marked as BLOCKER. Just search for it in all the files.

# Developing
See DEVELOPING.md in the root of this repo.
This will eventually replace the old opensteamclient.

# Contributing
Nothing for now.
Compile and run by going into ClientUI and running `dotnet run -v:m`.
Do not omit the verbosity flag, as important output from CMake and your compiler will be missing in the case of build errors. 

# Screenshots
Nothing for now.

# Usage
This is only meant for developers. 
Once this is good enough I will write new install instructions. For now end users can use the old opensteamclient by switching to the master branch.
If you're a dev, be cautious about adding things, as I am probably working on it already.


# System requirements
## Windows
- Windows 10 tested
### For development
- MSVC
## Linux
### For development
- Ubuntu 23.04 or newer (maybe optional if not using MingW)
- MingW 10.0.0/GCC12 (optional if Windows cross compile not wanted)
- OSXCross if you want a macos cross compile
  - You might get cryptic errors without the newest mingw, such as `std::this_thread` missing 
- GCC, G++, CMake

## Credits
Decompiling and datamining the steam client: 
- [open-steamworks](https://github.com/SteamRE/open-steamworks)
- [open-steamworks fork by m4dEngi](https://github.com/m4dEngi/open-steamworks)
- [SteamTracking](https://github.com/SteamDatabase/SteamTracking)
- [protobufs dumped from the steam client](https://github.com/SteamDatabase/Protobufs)
- [MiniUTL](https://github.com/FWGS/MiniUTL)
- [Logo by nPHYN1T3](https://github.com/nPHYN1T3)
- [Sound assets by nPHYN1T3](https://github.com/nPHYN1T3)

# Q&A

## Partially open source?
This is a GUI for Valve's own Steam Client binaries like `steamclient.so`, `steamservice.so` and `chromehtml.so`. 
Those binaries are not open source and Valve doesn't officially support 3rd-party usage of these. 
This also means we inherit design choices and potential bugs from these files.
Due to this, we cannot fix everything, such as the client not conforming to the XDG paths specification (although we've limited the pollution to a .steam symlink in your home folder only).

Additionally, 

Also, thank you Valve for improving Linux gaming, and making a native Steam Client in the first place.

## What version of Steam's binaries do you use?
The repo includes a copy of all the manifests at `Manifests/steam_client_PLATFORM.vdf`, which shows the version and has download paths for the binaries.
The `file` names point to regular zip files at the various client download addresses like `https://client-update.akamai.steamstatic.com/`, just use `unzip`. GUI tools don't seem to work.
`zipvz` seems to be some sort of proprietary zip format, which we don't use.

## The client crashes a lot or doesn't start
Delete ~/.local/share/OpenSteam and try again. Also check that you have a PC that meets the requirements for Steam officially.
Also, run OpenSteamClient from the terminal and post the logs in a Github issue clearly describing your situation. 
