using System.Text.Json;

namespace OpenSteamworks.Client.Managers;

/// <summary>
/// These are useless on their own. They need to be in an array in proper order to make sense of them. WTF...
/// </summary>
public class JSONFilterGroup {
    public List<JsonElement> rgOptions { get; set; } = new();
    public bool bAcceptUnion { get; set; } = false;
}