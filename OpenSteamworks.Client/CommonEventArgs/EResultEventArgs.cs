using OpenSteamworks.Enums;

namespace OpenSteamworks.Client.CommonEventArgs;
public class EResultEventArgs : EventArgs
{
    public EResultEventArgs(EResult eResult) { EResult = eResult; }
    public EResult EResult { get; }
}