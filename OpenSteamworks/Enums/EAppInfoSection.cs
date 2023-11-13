namespace OpenSteamworks.Enums;

public enum EAppInfoSection : int
{
	Unknown = 0,
	All,
	Common,
	Extended,
	Config,
	Stats,
	Install,
	Depots,
	/// <summary>
    /// Valve Anti Cheat data
    /// Seemingly unused
    /// </summary>
	Vac,
	Drm,
	/// <summary>
    /// Steam Cloud (User File System?)
    /// </summary>
	Ufs,
	/// <summary>
    /// Seemingly unused
    /// </summary>
	Ogg,
	Items,
	Policies,
	/// <summary>
    /// Legacy way for Steam to check system requirements.
    /// Unused in modern games, as they check requirements themselves.
    /// Used mostly for MacOS stuff.
    /// </summary>
	Sysreqs,
	Community,
	Store,
	/// <summary>
    /// Localization data. This section tends to be huge.
    /// </summary>
    Localization,
	/// <summary>
    /// Unknown
    /// </summary>
    Broadcastgamedata,
	/// <summary>
    /// Also unknown
    /// </summary>
	Computed,
	/// <summary>
    /// Info for soundtracks.
    /// Tells you about tracks, artist, composer, label, other credits.
    /// Also contains the hash of the album cover.
    /// </summary>
	Albummetadata,
};