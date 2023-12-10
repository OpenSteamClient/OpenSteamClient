using System;
using System.Runtime.InteropServices;

namespace OpenSteamworks.Enums;

public enum ELogonState
{
    LoggedOff = 0,
    Connecting = 1,
    Connected = 2,
    LoggingOn = 3,
    LoggedOn = 4,
    LoggingOff = 5,
};