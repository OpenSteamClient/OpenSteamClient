<img src="Assets/opensteam-logo.svg" alt="OpenSteamClient logo" title="OpenSteamClient" align="left" height="65" />

# OpenSteamClient (C# version, still in heavy development)
A partially open-source Steam frontend for Windows and Linux

# Current development status
Everything below is blockers. Lots of stuff that's only documented in my head is also blockers. Lots of code cleanups are due. 
Stuff marked later can wait for after we make an MVP.
Stuff marked future can be done eventually or just completely ignored

- [ ] Startup wizard
  - [ ] initial settings
  - [ ] steamapps linking 
- [ ] Backend stuff:
  - [x] (optional) JITEngine classgen with fields
  - [x] Callback system
    - [ ] Make more performant
      - Chokes pretty badly right now with the HTML rendering
  - [ ] Misc code cleanups
  - [ ] Fix TODO:s and BLOCKER:s
  - [x] Callresult system for non-callback results (needed for steamwebhelper/chromehtml/storepages)
    - [ ] Make not spaghetti
- [x] Account system:
  - [ ] Profile pictures
  - [ ] QR code in the loginwindow doesn't have bottom margin
  - [x] 2FA 
    - [ ] 2FA window improvements (the layout is VERY crude)
- [ ] Client settings UI
  - [ ] Library folder management
  - [ ] Compat settings
  - [ ] Persona name change
  - [ ] Download speed cap
- [x] Fix CPU fan speeding due to IPCClient
- [x] Friends list
  - [x] localizations
  - [x] Auto-updating "offline since" timer
  - [ ] Online, InGame, Offline sorting
  - [ ] Chats
  - [x] Join friend's game
  - [ ] Different colours for different statuses
  - [ ] Rich presence
  - [ ] Animated avatars
  - [ ] Avatar frames
  - [ ] Miniprofiles
- [ ] Library UI
  - [ ] Game news and patch notes
  - [x] Search bar
  - [x] Collections backend
    - [ ] Needs to have edit functionality
    - [x] Sync to the cloud
  - [ ] Collection editing GUI (later)
  - [x] Games list
    - [ ] Stop using TreeView
      - Perf. problems
      - Stupid "Name" hack.
    - [ ] Context menu for launching, settings, etc
  - [ ] Focused game view (library art, friends who play, etc)
    - [ ] Friends who play section (later)
    - [ ] Store, Community, Workshop, etc buttons (later)
    - [ ] Settings button
    - [ ] Edit collection button
    - [ ] Custom library art
  - [ ] Game settings page
    - [ ] Overlay settings (later)
      - Needs to wait until we actually get an overlay
    - [ ] Cloud settings
      - [ ] How much space is used
      - [ ] Cloud file viewer UI (later)
    - [ ] Launch settings
      - [x] Launch settings dialog
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
- [ ] Steamwebhelper support (later)
  - Seems to break with every other update. We need another way to display store pages.
  - [ ] Store, community, profile pages
  - [ ] Fix blurriness on non-100% DPIs (later) (avalonia bug with x11 scaling)
  - [ ] Make reliable??? (later)
    - For some reason, sometimes the init fails for a reason or another. In that case, the web elements can't be used until the user restarts OSC completely. And the client will throw a timeout erorr.
  - [ ] ~~Non-janky typing implementation~~ (probably never, unless we make our own CEF wrapper and use it instead of SteamWebHelper)
- [x] Windows support
  - [ ] Installer
  - [ ] Uninstaller
- [ ] Close OpenSteamClient when pressing X on the progress dialog
- [ ] Split project into multi-repo OpenSteamworks, OpenSteamClient

## TODO: Future/Never
- [ ] Employ observability at the OSW.Client layer
- [ ] Make managed versions of all interfaces
- In the future, this would allow pure C# instead of needing a dependency on the native client and JITEngine
  - IClientEngine and some bits of init would still be unavoidable
- This requires us to know 100% of the interfaces (and would be a pretty big rewrite)
- [ ] Remove JITEngine in favour of code generation (probably no, but could be good for API consumers that want NativeAOT)
- [ ] Rewrite InterfaceDebugger to follow MVVM
- [ ] Plugin system
  - This'd be useful for making sure our code is high quality and not a mess
  - Could also add support for extra game stores and whatnot
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
- [ ] VAC support
  - Basically impossible unless we get a .valvesig from Valve, which is unlikely given the nature of our main exe being the regular dotnet runtime
- [ ] ProtonDB integration
- [ ] Automatic game tweaking
- [ ] External modding sources (like Nexus for Fallout games, r2modman for Lethal Company, etc)
- [ ] Cloud file manager GUI
- [ ] UI sounds
- [ ] Shut off PC when game finishes installing
- [ ] Big picture mode
- [ ] Small mode
- Alternative GUIs
  - [ ] Terminal UI
  - [ ] IMGui reimplementation?
    - Leaving here for community interest.
    - It'd be a lot snappier
    - Not as styleable
    - Apparently doesn't have good accessibility features
    - Does have premade C# bindings though
  - [ ] Rust rewrite
    - "egui" seems pretty cool
      - Can also build for the web
  - [ ] VGUI reimplementation?
    - Could probably very simply just load the OG ui from the cached/ folder, but VGUI_s is 32-bit and we're 64-bit
    - Would need to reimplement the entire UI framework from scratch though
- [ ] Reimplement steamclient.dll/so
- [ ] MacOS
- [ ] Reverse ZipVZ (ValveZip?) format

# Support
We support:
- Arch Linux
- Windows 10
Anything else is not officially supported, and your issue may get closed.

# Features
NOTE: The features mentioned here are the criteria for replacing the old Qt/C++ based OSC. These may not be implemented yet! And these also aren't final
- The basics you'd expect:
  - Achievements
  - Steam Cloud
  - Invites and friends network
    - There's no overlay yet though, so you'll need to ALT+TAB
  - Workshop
    - Load order, enable/disable
  - New family sharing
  - Old family sharing
- No steamservice requirement (still recommended however, requires 32-bit binaries)
  - Required on Linux for compat tools proper functionality.
  - Required on Windows for desktop shortcut creation, game registry entries
- No web technology (also known as CEF, SteamWebHelper)
- Most games supported
  - Steam2 games untested
  - Some multiplayer games might not work
  - VAC games unsupported (nothing we can do about this, sorry!)
- Depot browser
  - Download extra depots
  - Download individual files
- Build history browser
  - Lock specific build
- Steam cloud filebrowser
- Misc QOL features, such as:
  - Download all updates button
- Linux users will also enjoy:
  - 64-bit main executable  
    - One app less that requires multilib/32-bit libraries
  - ProtonDB Integration
  - Compat tool improvements:
    - Run .exe in prefix
    - Run winecfg/winetricks/protontricks for game
    - Adjust compat preferences like forcelgaddr easily
    - Compat tool downloader
  

# Contributing
See [CONTRIBUTING.md](https://github.com/OpenSteamClient/OpenSteamClient/blob/c%23-remake/CONTRIBUTING.md) for guidelines.
Clone by running `git clone -b c#-remake --single-branch https://github.com/OpenSteamClient/opensteamclient.git --recursive`
Compile and run by going into OpenSteamClient and running `dotnet run -v:m`.
Occasionally updates break existing repos, just delete the whole repo and reclone if that happens.

# Screenshots
Nothing for now.

# Usage
This is only meant for developers. 
Once this is in a good enough state I will write new install instructions.

# System requirements
## Windows
- Windows 10 tested
### For development
- MSVC
## Linux
### For development
- Ubuntu 23.04 or newer (maybe optional if not using MingW)
- MingW 10.0.0/GCC12 (optional if Windows cross compile not wanted)
- OSXCross (if you want a macos cross compile)
  - You might get cryptic errors without the newest mingw, such as `std::this_thread` missing 
- GCC, G++, CMake

## Credits
Decompiling and datamining the steam client: 
- [open-steamworks](https://github.com/SteamRE/open-steamworks)
- [open-steamworks fork by m4dEngi](https://github.com/m4dEngi/open-steamworks)
- [SteamTracking](https://github.com/SteamDatabase/SteamTracking)
- [protobufs dumped from the steam client](https://github.com/SteamDatabase/Protobufs)
Other credits:
- [MiniUTL](https://github.com/FWGS/MiniUTL)
- [Logo by nPHYN1T3](https://github.com/nPHYN1T3)
- [Sound assets by nPHYN1T3](https://github.com/nPHYN1T3)

# Q&A

## Gah! Why is making this taking such a long time???
There's a single developer (Rosentti) working on every aspect of the rewrite. This alone makes things slow, but also the tremendous workload can cause burnout very easily. 
To combat burnout related to OSC, I sometimes work on other projects and different parts of OSC. 

## Partially open source?
This is a GUI for Valve's own Steam Client binaries like `steamclient.so`, `steamservice.so` and `chromehtml.so`. 
Those binaries are not open source and Valve doesn't officially support 3rd-party usage of these. 
This also means we inherit design choices and potential bugs from these files.
Due to this, we cannot fix everything, such as the client not conforming to the XDG paths specification (although we've limited the pollution to a .steam symlink in your home folder only).

Also, thank you Valve for improving Linux gaming, and making a native Steam Client in the first place.

## What version of Steam's binaries do you use?
The repo includes a copy of all the manifests at `Manifests/steam_client_PLATFORM.vdf`, which shows the version and has download paths for the binaries.
The `file` names point to regular zip files at the various client download addresses like `https://client-update.akamai.steamstatic.com/`, just use `unzip`. GUI tools don't seem to work.
`zipvz` seems to be some sort of proprietary zip format, which we haven't reversed.

## The client crashes a lot or doesn't start
Delete `~/.local/share/OpenSteam` and try again. Also check that you have a PC that meets the requirements for Steam officially.
Also, run OpenSteamClient from the terminal and post the logs in a Github issue clearly describing your situation. 
