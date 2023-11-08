namespace OpenSteamworks.Native.Platform;

public class LinuxPlatform : IPlatform
{
    public string DefaultSpewOutputFuncSig => "\x53\x48\x89\x00\x89\xFB";

    public string DefaultSpewOutputFuncSigMask => "xxx?xx";
}