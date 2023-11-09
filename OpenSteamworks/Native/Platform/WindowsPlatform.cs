using System;

namespace OpenSteamworks.Native.Platform;

public class WindowsPlatform : IPlatform
{
    public string DefaultSpewOutputFuncSig => "\x40\x00\x48\x83\x00\x00\x8B\xD9\x48\x8D\x00\x00\x00\x00\x00\xE8";

    public string DefaultSpewOutputFuncSigMask => "x?xx??xxxx?????x";
}