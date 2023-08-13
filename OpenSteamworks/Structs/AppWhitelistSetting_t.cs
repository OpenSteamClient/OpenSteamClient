using System;
using System.Runtime.InteropServices;
using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;

namespace OpenSteamworks.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AppWhitelistSetting_t {
    public AppId_t appid;
    public CUtlString comment;
    public ERemoteStoragePlatform platform;
    public CUtlString toolName;
    public CUtlString config;
}