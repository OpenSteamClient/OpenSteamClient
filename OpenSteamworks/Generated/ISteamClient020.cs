//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Enums;

namespace OpenSteamworks.Generated;

public unsafe interface ISteamClient020 {
    /// <summary>
    /// <para> Creates a communication pipe to the Steam client.</para>
    /// <para> NOT THREADSAFE - ensure that no other threads are accessing Steamworks API when calling</para>
    /// </summary>
    public HSteamPipe CreateSteamPipe();

    /// <summary>
    /// <para> Releases a previously created communications pipe</para>
    /// <para> NOT THREADSAFE - ensure that no other threads are accessing Steamworks API when calling</para>
    /// </summary>
    public bool BReleaseSteamPipe(HSteamPipe hSteamPipe);

    /// <summary>
    /// <para> connects to an existing global user, failing if none exists</para>
    /// <para> used by the game to coordinate with the steamUI</para>
    /// <para> NOT THREADSAFE - ensure that no other threads are accessing Steamworks API when calling</para>
    /// </summary>
    public HSteamUser ConnectToGlobalUser(HSteamPipe hSteamPipe);

    /// <summary>
    /// <para> used by game servers, create a steam user that won't be shared with anyone else</para>
    /// <para> NOT THREADSAFE - ensure that no other threads are accessing Steamworks API when calling</para>
    /// </summary>
    public HSteamUser CreateLocalUser(out HSteamPipe phSteamPipe, EAccountType eAccountType);

    /// <summary>
    /// <para> removes an allocated user</para>
    /// <para> NOT THREADSAFE - ensure that no other threads are accessing Steamworks API when calling</para>
    /// </summary>
    public void ReleaseUser(HSteamPipe hSteamPipe, HSteamUser hUser);

    /// <summary>
    /// <para> retrieves the ISteamUser interface associated with the handle</para>
    /// </summary>
    public IntPtr GetISteamUser(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> retrieves the ISteamGameServer interface associated with the handle</para>
    /// </summary>
    public IntPtr GetISteamGameServer(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> set the local IP and Port to bind to</para>
    /// <para> this must be set before CreateLocalUser()</para>
    /// </summary>
    public void SetLocalIPBinding(UInt32 unIP, ushort usPort);

    /// <summary>
    /// <para> returns the ISteamFriends interface</para>
    /// </summary>
    public IntPtr GetISteamFriends(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the ISteamUtils interface</para>
    /// </summary>
    public IntPtr GetISteamUtils(HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the ISteamMatchmaking interface</para>
    /// </summary>
    public IntPtr GetISteamMatchmaking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the ISteamMatchmakingServers interface</para>
    /// </summary>
    public IntPtr GetISteamMatchmakingServers(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the a generic interface</para>
    /// </summary>
    public IntPtr GetISteamGenericInterface(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the ISteamUserStats interface</para>
    /// </summary>
    public IntPtr GetISteamUserStats(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the ISteamGameServerStats interface</para>
    /// </summary>
    public IntPtr GetISteamGameServerStats(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns apps interface</para>
    /// </summary>
    public IntPtr GetISteamApps(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> networking</para>
    /// </summary>
    public IntPtr GetISteamNetworking(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> remote storage</para>
    /// </summary>
    public IntPtr GetISteamRemoteStorage(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> user screenshots</para>
    /// </summary>
    public IntPtr GetISteamScreenshots(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> game search</para>
    /// </summary>
    public IntPtr GetISteamGameSearch(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns the number of IPC calls made since the last time this function was called</para>
    /// <para> Used for perf debugging so you can understand how many IPC calls your game makes per frame</para>
    /// <para> Every IPC call is at minimum a thread context switch if not a process one so you want to rate</para>
    /// <para> control how often you do them.</para>
    /// </summary>
    public uint GetIPCCallCount();

    /// <summary>
    /// <para> API warning handling</para>
    /// <para> 'int' is the severity; 0 for msg, 1 for warning</para>
    /// <para> 'const char *' is the text of the message</para>
    /// <para> callbacks will occur directly after the API function is called that generated the warning or message.</para>
    /// </summary>
    public void SetWarningMessageHook(void* pFunction);

    /// <summary>
    /// <para> Trigger global shutdown for the DLL</para>
    /// </summary>
    public bool BShutdownIfAllPipesClosed();

    /// <summary>
    /// <para> Expose HTTP interface</para>
    /// </summary>
    public IntPtr GetISteamHTTP(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Exposes the ISteamController interface - deprecated in favor of Steam Input</para>
    /// </summary>
    public IntPtr GetISteamController(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Exposes the ISteamUGC interface</para>
    /// </summary>
    public IntPtr GetISteamUGC(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> returns app list interface, only available on specially registered apps</para>
    /// </summary>
    public IntPtr GetISteamAppList(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Music Player</para>
    /// </summary>
    public IntPtr GetISteamMusicFake(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Music Player Remote</para>
    /// </summary>
    public IntPtr GetISteamMusicRemote(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Music Player</para>
    /// </summary>
    public IntPtr GetISteamMusic(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> inventory</para>
    /// </summary>
    public IntPtr GetISteamInventory(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> html page display</para>
    /// </summary>
    public ISteamHTMLSurface005 GetISteamHTMLSurface(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Video</para>
    /// </summary>
    public IntPtr GetISteamVideo(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Parental controls</para>
    /// </summary>
    public IntPtr GetISteamParentalSettings(HSteamUser hSteamuser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Exposes the Steam Input interface for controller support</para>
    /// </summary>
    public IntPtr GetISteamInput(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Steam Parties interface</para>
    /// </summary>
    public IntPtr GetISteamParties(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);

    /// <summary>
    /// <para> Steam Remote Play interface</para>
    /// </summary>
    public IntPtr GetISteamRemotePlay(HSteamUser hSteamUser, HSteamPipe hSteamPipe, string pchVersion);
}