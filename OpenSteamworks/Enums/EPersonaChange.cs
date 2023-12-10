using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum EPersonaChange
{
    Name = 1 << 0,
    Status = 1 << 1,
    ComeOnline = 1 << 2,
    GoneOffline = 1 << 3,
    GamePlayed = 1 << 4,
    GameServer = 1 << 5,
    Avatar = 1 << 6,
    JoinedSource = 1 << 7,
    LeftSource = 1 << 8,
    RelationshipChanged = 1 << 9,
    NameFirstSet = 1 << 10,
    FacebookInfo = 1 << 11,
    Nickname = 1 << 12,
    SteamLevel = 1 << 13,
    RichPresence = 1 << 14
};