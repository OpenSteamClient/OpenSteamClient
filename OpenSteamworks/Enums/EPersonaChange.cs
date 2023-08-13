using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

[Flags]
public enum EPersonaChange
{
    k_EPersonaChangeName = 1 << 0,
    k_EPersonaChangeStatus = 1 << 1,
    k_EPersonaChangeComeOnline = 1 << 2,
    k_EPersonaChangeGoneOffline = 1 << 3,
    k_EPersonaChangeGamePlayed = 1 << 4,
    k_EPersonaChangeGameServer = 1 << 5,
    k_EPersonaChangeAvatar = 1 << 6,
    k_EPersonaChangeJoinedSource = 1 << 7,
    k_EPersonaChangeLeftSource = 1 << 8,
    k_EPersonaChangeRelationshipChanged = 1 << 9,
    k_EPersonaChangeNameFirstSet = 1 << 10,
    k_EPersonaChangeFacebookInfo = 1 << 11,
    k_EPersonaChangeNickname = 1 << 12,
    k_EPersonaChangeSteamLevel = 1 << 13,
    k_EPersonaChangeRichPresence = 1 << 14
};