<img src="Assets/opensteam-logo.svg" alt="OpenSteamClient logo" title="OpenSteamClient" align="left" height="65" />

# OpenSteamClient (C# version, still in heavy development)
A partially open-source Steam client for Windows and Linux

# Current development status
Everything below is blockers. Lots of stuff that's only documented in my head is also blockers. Lots of code cleanups are due. 
Stuff marked later can wait for after we make an MVP.
Stuff marked future can be done eventually or just completely ignored

- [x] Bootstrapper
- [ ] Backend stuff:
  - [ ] (optional) JITEngine classgen with fields (would be nice for concommand support)
  - [x] Custom value types
  - [x] Callback system
    - [ ] Make more performant
      - Chokes pretty badly right now with the HTML rendering
  - [ ] Misc code cleanups
  - [ ] Fix TODO:s and BLOCKER:s
  - [x] Callresult system for non-callback results (needed for steamwebhelper/chromehtml/storepages)
    - [ ] Make not spaghetti
  - [ ] Non-terrible, cross-store extensible, interface-based and pluginable appsystem
- [ ] Account system:
  - [x] Login
  - [x] Logout
  - [x] Forget account
  - [ ] Profile pictures
  - [ ] QR code in the loginwindow doesn't have bottom margin
  - [ ] 2FA 
    - [ ] 2FA window improvements
- [ ] Client settings UI
- [ ] Library UI
  - [ ] Game news and patch notes
  - [ ] Search bar
  - [x] Collections backend
    - [ ] Fix
      - Sometimes it's missing games. Why?
    - [ ] Needs to have edit functionality
    - [ ] Fix 2
      - Doesn't sync to the cloud, it broke at some point and I don't know how to fix it
  - [ ] Collection editing GUI (later)
  - [x] Games list
  - [ ] Focused game view (library art, friends who play, etc)
    - [x] Library hero art 
      - TODO: depends on the appsystem rewrite (later)
    - [x] Play button 
    - [ ] Friends who play section (later)
    - [ ] Store, Community, Workshop, etc buttons (later)
  - [ ] Game settings page
    - [ ] Overlay settings (later)
      - Needs to wait until we actually get an overlay
    - [ ] Cloud settings
      - [ ] How much space is used
      - [ ] Cloud file viewer UI (later)
    - [ ] Launch settings
      - [ ] Set command line
      - [ ] Set default launch option (later)
        - [ ] Visualize the full launch option in the command line box (later)
      - [ ] Add custom launch options (later)
    - [ ] Lang settings
    - [ ] Compat settings
      - The API seemingly has a way to set compat strings like forcelgadd, explore adding this functionality (later)
    - [ ] Workshop/Mod settings
      - [ ] See installed workshop size
      - [ ] See installed items
      - [ ] Unsubscribe installed items
      - [ ] Disabling workshop mods without unsubbing (later)
      - [ ] Load order (later)
      - [ ] Support 3rd party mod platforms (future)
  - [ ] Downloads page
    - [ ] Support showing 3rd party launcher's download statuses (future)
- [x] Steamwebhelper support (later)
  - [x] Store, community, profile pages
  - [ ] Fix blurriness on non-100% DPIs (later) (wtf how did this even become a problem)
  - [ ] Make reliable??? (later)
    - For some reason, sometimes the init fails for a reason or another. In that case, the web elements can't be used until the user restarts OSC completely. And the client will throw a timeout erorr.
  - [ ] ~~Non-janky typing implementation~~ (probably never, unless we make our own CEF thingy)
- [x] Windows support
- [ ] Close OpenSteamClient when pressing X on the progress dialog
- [ ] Split project into multi-repo OpenSteamworks, OpenSteamClient
- [ ] Make managed versions of all interfaces
  - In the future, this would allow pure IPC as well
  - This requires us to know 100% of the interfaces (and would be a pretty big rewrite)
- [ ] Future (optional):
  - [ ] Remove JITEngine in favour of code generation (probably no, but could be good for 3rd parties that want NativeAOT)
  - [ ] Rewrite InterfaceDebugger to follow MVVM
  - [ ] Plugin system
    - This'd be useful for making sure our code is high quality and not a mess
    - Could also add support for 3rd party apps and whatnot
    - Possible plugin types:
      - [ ] Library art provider
      - [ ] App provider
      - [ ] Social provider
      - [ ] Auth provider
      - [ ] Misc UI data provider (protondb, widescreen, etc patches)
      - [ ] Overlay provider
      - [ ] ConCommand provider
      - [ ] Store/Community provider
      - [ ] Mod provider
  - [ ] Disabling updates for certain apps
  - [ ] Custom SDL lib
    - [ ] Some valve specific code for handling steering wheels etc
  - [ ] ConCommand support
  - [ ] Download additional depots (example: CS2 workshop tools with proton needs Windows CS2 binaries)
    - Should be doable, just need to improve our hooking systems
  - [ ] UI Animations
    - Does avalonia support this?
  - [ ] VAC support on Windows
    - How the hell? We'd basically have to bypass VAC and that's a big no-no
  - [ ] ProtonDB integration
  - [ ] Automatic game tweaking
  - [ ] External modding sources (like Nexus for Fallout games, r2modman for Lethal Company, etc)
  - [ ] Cloud file manager GUI
  - [ ] UI sounds
  - [ ] Shut off PC when game finishes installing
  - [ ] Big picture mode
  - [ ] Small mode
  - [ ] Terminal UI
  - [ ] IMGui reimplementation?
    - It'd be a lot snappier
    - Not as styleable
    - Apparently doesn't have good accessibility features
    - Does have premade C# bindings though
  - [ ] VGUI reimplementation?
    - Probably unnecessary, leaving here for community interest
    - Could probably very simply just load the OG ui from the cached/ folder, but VGUI_s is 32-bit and we're 64-bit
    - Would need to reimplement the entire UI framework from scratch though
  - [ ] Reimplement steamclient.dll/so
  - [ ] MacOS


# Features
- 64-bit (but needs some 32-bit libraries for some functionality)
- No steamwebhelper requirement (but can be used ingame and for browsing the store and community pages)
- All games supported (technically), Steam2 games unknown
- VAC supported on Linux (you can play, but you _might_ get banned. We're not responsible if you do.)
- Supports Windows and Linux, and even theoretically MacOS (PRs for support welcome)

# Contributing
Nothing for now.
Clone by running `git clone -b c#-remake --single-branch https://github.com/OpenSteamClient/opensteamclient.git --recursive`
Compile and run by going into OpenSteamClient and running `dotnet run -v:m`.
Occasionally updates break existing repos, just delete the whole repo and reclone if that happens.

# Screenshots
Nothing for now.

# Usage
This is only meant for developers. 
Once this is good enough I will write new install instructions. For now end users can use the old opensteamclient (TERRIBLE) by switching to the master branch (Linux only).
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

Also, thank you Valve for improving Linux gaming, and making a native Steam Client in the first place.

## What version of Steam's binaries do you use?
The repo includes a copy of all the manifests at `Manifests/steam_client_PLATFORM.vdf`, which shows the version and has download paths for the binaries.
The `file` names point to regular zip files at the various client download addresses like `https://client-update.akamai.steamstatic.com/`, just use `unzip`. GUI tools don't seem to work.
`zipvz` seems to be some sort of proprietary zip format, which we don't use.

## The client crashes a lot or doesn't start
Delete `~/.local/share/OpenSteam` and try again. Also check that you have a PC that meets the requirements for Steam officially.
Also, run OpenSteamClient from the terminal and post the logs in a Github issue clearly describing your situation. 
