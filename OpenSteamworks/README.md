# OpenSteamworks
OpenSteamworks is a C# library to interact with Steam's IClient interfaces. You'll need to provide your own binaries, versioned exactly the same as `Generated/VersionInfo.cs`
Note that this library cannot check the version, instead various issues will manifest at runtime like segfaults, wrong functions being called, issues connecting to existing clients, etc.