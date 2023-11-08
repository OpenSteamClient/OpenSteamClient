using System;

namespace OpenSteamworks.Native.Platform;

public class WindowsPlatform : IPlatform
{
    public string DefaultSpewOutputFuncSig => throw new NotImplementedException();

    public string DefaultSpewOutputFuncSigMask => throw new NotImplementedException();
}