using System;

namespace OpenSteamworks.Enums;

public enum ELaunchSource : UInt32
{
	None = 0,
	_2ftLibraryDetails = 100,
	_2ftLibraryListView = 101,
	_2ftLibraryGrid = 103,
	InstallSubComplete = 104,
	DownloadsPage = 105,
	RemoteClientStartStreaming = 106,
	_2ftMiniModeList = 107,
	_10ft = 200,
	DashAppLaunchCmdLine = 300,
	DashGameIdLaunchCmdLine = 301,
	RunByGameDir = 302,
	SubCmdRunDashGame = 303,
	SteamURL_Launch = 400,
	SteamURL_Run = 401,
	SteamURL_JoinLobby = 402,
	SteamURL_RunGame = 403,
	SteamURL_RunGameIdOrJumplist = 404,
	SteamURL_RunSafe = 405,
	TrayIcon = 500,
	LibraryLeftColumnContextMenu = 600,
	LibraryLeftColumnDoubleClick = 601,
	Dota2Launcher = 700,
	IRunGameEngine = 800,
	DRMFailureResponse = 801,
	DRMDataRequest = 802,
	CloudFilePanel = 803,
	DiscoveredAlreadyRunning = 804,
	GameActionJoinParty = 900,
	AppPortraitContextMenu = 1000,
}