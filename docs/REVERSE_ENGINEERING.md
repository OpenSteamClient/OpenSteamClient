# Reverse Engineering
A shallow guide on figuring out how Steam works.
Note that this in most cases is not necessary, unless you are doing something that isn't well documented. 
This guide assumes you use linux. Some things may not be accurate on Windows.

## Warning
Reversing/modifying/testing APIs with the Steam client may lead to:
- Account termination
- Non-functional client install
- Corruption
- VAC bans

Do not play anticheat games if you are running a debugger or other tool on Steam. 
Also, don't do stuff with the Steam Client Service while playing a VAC protected game, as VAC resides in the Steam Client Service and your account will most likely get flagged

## Tools

### Ghidra
No guide for this by us, just a couple things to know:

- `steamui.so` is extremely helpful. Just make sure the version matches ours, or else vtable calls will be incorrect. SteamUI has things like:
	- Web-to-native mappings (`User.SetLoginInformation` etc)
	- All the interfaces you can get from `IClientEngine` (Search for any interface's name or `ClientAPI_Init`, there should be a long function that initializes all the interfaces) 

- You should use the `ubuntu12_32/steamclient.so` in Ghidra, and allow importing any libraries it needs
  - You should analyze libtier0_s first, then vstdlib, then steamclient in order for imports to show up the best

- Debug symbols
  - Some executables are built with debug symbols
  - No libraries seem to be built with debug symbols

- Whenever you come across a vtable call with an offset like `+ 0x20`, you should turn it into Decimal and then divide it by 8 (or 4 if using a 32-bit executable). Now you know the index of the function in a given vtable, just look it up from `OpenSteamworks/Generated/` if you know the interface name.

  

### VProf
See [vprof.md](https://github.com/OpenSteamClient/OpenSteamClient/blob/c%23-remake/docs/VPROF.md).

### CEF devtools
Since large parts of Steam are made with React+CEF, it's quite easy to debug and find out how some things work. 
Launch Steam with `-opendevtools` to open the CEF dev tools. 
