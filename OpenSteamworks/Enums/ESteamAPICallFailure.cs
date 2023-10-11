namespace OpenSteamworks.Enums;

public enum ESteamAPICallFailure : int
{
    /// <summary>
    /// no failure
    /// </summary>
	k_ESteamAPICallFailureNone = -1,
    /// <summary>
    /// the local Steam process has gone away
    /// </summary>
	k_ESteamAPICallFailureSteamGone = 0,
    /// <summary>
    /// the network connection to Steam has been broken, or was already broken
    /// </summary>
	k_ESteamAPICallFailureNetworkFailure = 1,
    /// <summary>
    /// the SteamAPICall_t handle passed in no longer exists
    /// </summary>
	k_ESteamAPICallFailureInvalidHandle = 2,
    /// <summary>
    /// GetAPICallResult() was called with the wrong callback type for this API call
    /// </summary>
	k_ESteamAPICallFailureMismatchedCallback = 3,
};