using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;

namespace OpenSteamworks.Client.Utils.DI;
public interface ILogonLifetime {
    public Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e);
    public Task OnLoggingOff(IProgress<string> progress);
}