namespace OpenSteamworks.Client.Managers;

public class JSONCollection
{
    public string id { get; set; } = "";
    public string name { get; set; } = "";

    public List<uint>? added { get; set; } = new();
    /// <summary>
    /// What does this field do?
    /// </summary>
    public List<uint>? removed { get; set; } = new();
    public JSONFilterSpec? filterSpec { get; set; } = null;
}