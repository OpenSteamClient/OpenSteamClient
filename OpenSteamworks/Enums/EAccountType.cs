namespace OpenSteamworks.Enums;

public enum EAccountType {
    Invalid = 0,
    /// <summary>
    /// single user account
    /// </summary>
    Individual = 1,	
    /// <summary>
    /// multiseat (e.g. cybercafe) account
    /// </summary>
    Multiseat = 2,	
    /// <summary>
    /// game server account
    /// </summary>
    GameServer = 3,
    /// <summary>
    ///  anonymous game server account
    /// </summary>
    AnonGameServer = 4,	
    Pending = 5,		
    /// <summary>
    /// content server
    /// </summary>
    ContentServer = 6,	
    Clan = 7,
    Chat = 8,
    /// <summary>
    /// Fake SteamID for local PSN account on PS3 or Live account on 360, etc.
    /// </summary>
    ConsoleUser = 9,	
    AnonUser = 10,

    // Max of 16 items in this field
    Max
}