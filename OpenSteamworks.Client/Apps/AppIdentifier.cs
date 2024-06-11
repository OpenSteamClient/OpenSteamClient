using OpenSteamworks.Structs;

namespace OpenSteamworks.Client.Apps;

public class AppIdentifier {
    /// <summary>
    /// For storing AppId_t for regular Steam apps
    /// </summary>
    public const string TYPE_STEAM_APP = "steamapp";

    /// <summary>
    /// For storing CGameIDs of mods
    /// </summary>
    public const string TYPE_STEAM_SOURCEMOD = "steamsourcemod";

    /// <summary>
    /// For storing CGameIDs of shortcuts
    /// </summary>
    public const string TYPE_STEAM_SHORTCUT = "steamshortcut";

    public string Provider { get; }
    public string Value { get; }

    private AppIdentifier(string provider, string value) {
        this.Provider = provider;
        this.Value = value;
    }

    public override string ToString()
    {
        return $"{Provider}:{Value}";
    }

    public static AppIdentifier CreateFromSteamApp(AppId_t appid) {
        return new(TYPE_STEAM_APP, appid.ToString());
    }

    public static AppIdentifier CreateFromSteamShortcut(CGameID gameid) {
        return new(TYPE_STEAM_SHORTCUT, ((ulong)gameid).ToString());
    }

    public static AppIdentifier CreateFromSteamSourceMod(CGameID gameid) {
        return new(TYPE_STEAM_SOURCEMOD, ((ulong)gameid).ToString());
    }

    public static AppIdentifier CreateFromString(string str) {
        int colonIdx = str.IndexOf(':');
        if (colonIdx == -1) {
            throw new ArgumentException("Not in AppIdentifier format", nameof(str));
        }

        return new(str[0..colonIdx], str[colonIdx..]);
    }

    public AppId_t GetSteamAppID() {
        if (Provider != TYPE_STEAM_APP) {
            throw new InvalidOperationException($"AppIdentifier {Provider} not valid, expected {TYPE_STEAM_APP}");
        }

        return uint.Parse(Value);
    }

    public CGameID GetSteamShortcutID() {
        if (Provider != TYPE_STEAM_SHORTCUT) {
            throw new InvalidOperationException($"AppIdentifier {Provider} not valid, expected {TYPE_STEAM_SHORTCUT}");
        }

        return new(ulong.Parse(Value));
    }

    public CGameID GetSteamSourceModID() {
        if (Provider != TYPE_STEAM_SOURCEMOD) {
            throw new InvalidOperationException($"AppIdentifier {Provider} not valid, expected {TYPE_STEAM_SOURCEMOD}");
        }

        return new(ulong.Parse(Value));
    }

    public override int GetHashCode()
    {
        return ToString().GetHashCode();
    }
}