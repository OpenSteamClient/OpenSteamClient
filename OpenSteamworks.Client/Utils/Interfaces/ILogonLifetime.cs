using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;

public interface ILogonLifetime {
    public Task OnLoggedOn(IExtendedProgress<int> progress, LoggedOnEventArgs e);
    public Task OnLoggingOff(IExtendedProgress<int> progress);
}