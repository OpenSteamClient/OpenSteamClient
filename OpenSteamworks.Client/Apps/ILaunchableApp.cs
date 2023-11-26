namespace OpenSteamworks.Client.Apps;

public interface ILaunchableApp<T, TErr> {
    public IEnumerable<T> LaunchOptions { get; }
    public bool RequiresLaunchOption { get; }
    public Task<TErr> Launch(string userLaunchOptions, T? launchOption = default);
}