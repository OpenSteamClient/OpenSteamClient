using System;

namespace OpenSteamworks.Enums;

[Flags]
public enum EDurationControlOnlineState
{
    /// <summary>
    /// Offline play
    /// </summary>
    Offline = 1,
    /// <summary>
    /// Online play
    /// </summary>
    Online,
    /// <summary>
    /// Online play - game requests that steam not force exit the game
    /// </summary>
    OnlineHighPri
};