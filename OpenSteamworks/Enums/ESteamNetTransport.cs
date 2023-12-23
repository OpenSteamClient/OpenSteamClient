namespace OpenSteamworks.Enums;

public enum ESteamNetTransport
{
    Unknown = 0,
    LoopbackBuffers = 1,
    LocalHost = 2,
    UDP = 3,
    UDPProbablyLocal = 4,
    TURN = 5,
    SDRP2P = 6,
    SDRHostedServer = 7,
}