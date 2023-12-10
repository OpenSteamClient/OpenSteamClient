namespace OpenSteamworks.Enums;

public enum ESteamAPICallFailure : int
{
    /// <summary>
    /// no failure
    /// </summary>
	None = -1,
    /// <summary>
    /// the local Steam process has gone away
    /// </summary>
	SteamGone = 0,
    /// <summary>
    /// the network connection to Steam has been broken, or was already broken
    /// </summary>
	NetworkFailure = 1,
    /// <summary>
    /// the SteamAPICall_t handle passed in no longer exists
    /// </summary>
	InvalidHandle = 2,
    /// <summary>
    /// GetAPICallResult() was called with the wrong callback type for this API call
    /// </summary>
	MismatchedCallback = 3,
};