namespace OpenSteamworks.Client.Apps.Library;

public class JSONFilterSpec
{
    public int nFormatVersion { get; set; } = 2;
    public string strSearchText { get; set; } = "";
    /// <summary>
    /// So, as it turns out the order is REALLY important.
    /// There's basically no data here, but if you look closelier, you can see that the array indices make the filtergroups make sense.
    /// Currently there's seven groups: <br/>
    /// 0 = <br/>
    /// 1 = State (Installed, Playable, Unplayed, Played). options = ???<br/>
    /// 2 = Features, hardware support, deck verified, player count. options = (not yet reversed enum)<br/>
    /// 3 = <br/>
    /// 4 = Store tags, genres. options = ???<br/>
    /// 5 = <br/>
    /// 6 = Friends that own common games. options = friend's accountid <br/>
    /// </summary>
    public JSONFilterGroup[] filterGroups { get; set; } = new JSONFilterGroup[6] {
        new JSONFilterGroup(),
        new JSONFilterGroup(),
        new JSONFilterGroup(),
        new JSONFilterGroup(),
        new JSONFilterGroup(),
        new JSONFilterGroup(),
    };

    /// <summary>
    /// This is unused. 
    /// </summary>
    public List<object>? setSuggestions { get; set; } = new();
}