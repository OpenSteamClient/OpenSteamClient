using System;
using System.Collections.Generic;
using System.Linq;
using OpenSteamworks.Callbacks.Structs;

namespace OpenSteamworks.Callbacks;

internal static partial class CallbackConstants {
    public readonly static Dictionary<Type, int> TypeToID = new()
    {
        {typeof(AppEventStateChange_t), 1280006},
        {typeof(AppLastPlayedTimeChanged_t), 1020070},
        {typeof(AppLaunchResult_t), 1280027},
        {typeof(AppLicensesChanged_t), 1020094},
        {typeof(AppLifetimeNotice_t), 1020030},
        {typeof(AppMinutesPlayedDataNotice_t), 1020046},
        {typeof(AppUpdateStateChanged_t), 1280010},
        {typeof(CompatManagerToolRegistered_t), 1200002},
        {typeof(DownloadScheduleChanged_t), 1280009},
        {typeof(FriendRichPresenceUpdate_t), 336},
        {typeof(PersonaStateChange_t), 304},
        {typeof(PostLogonState_t), 1020087},
        {typeof(SharedConnectionMessageReady_t), 1170001},
        {typeof(SteamAPICallCompleted_t), 703},
        {typeof(SteamConfigStoreChanged_t), 1040011},
        {typeof(SteamServerConnectFailure_t), 102},
        {typeof(SteamServersConnected_t), 101},
        {typeof(SteamServersDisconnected_t), 103},
        {typeof(VerifyPasswordResponse_t), 1020040},
        {typeof(HTML_BrowserReady_t), 4501},
        {typeof(HTML_NeedsPaint_t), 4502},
        {typeof(HTML_StartRequest_t), 4503},
        {typeof(HTML_CloseBrowser_t), 4504},
        {typeof(HTML_URLChanged_t), 4505},
        {typeof(HTML_FinishedRequest_t), 4506},
        {typeof(HTML_OpenLinkInNewTab_t), 4507},
        {typeof(HTML_ChangedTitle_t), 4508},
        {typeof(HTML_SearchResults_t), 4509},
        {typeof(HTML_CanGoBackAndForward_t), 4510},
        {typeof(HTML_HorizontalScroll_t), 4511},
        {typeof(HTML_VerticalScroll_t), 4512},
        {typeof(HTML_LinkAtPosition_t), 4513},
        {typeof(HTML_JSAlert_t), 4514},
        {typeof(HTML_JSConfirm_t), 4515},
        {typeof(HTML_FileOpenDialog_t), 4516},
        {typeof(HTML_NewWindow_t), 4521},
        {typeof(HTML_SetCursor_t), 4522},
        {typeof(HTML_StatusText_t), 4523},
        {typeof(HTML_ShowToolTip_t), 4524},
        {typeof(HTML_UpdateToolTip_t), 4525},
        {typeof(HTML_HideToolTip_t), 4526},
        {typeof(HTML_BrowserRestarted_t), 4527},
        {typeof(StopPlayingBorrowedApp_t), 1080003},
        {typeof(ShortcutChanged_t), 1130001},
        {typeof(ShortcutRemoved_t), 1130002},
        {typeof(UnknownCallback4_1090001), 1090001},
        {typeof(UnknownCallback8_1040026), 1040026},
        {typeof(WindowFocusChanged_t), 1040015},
        {typeof(FocusedAndFocusableWindowsUpdate_t), 1040044},
        {typeof(UnknownCallback4288_1040033), 1040033},
        {typeof(StartVRDashboard_t), 1040018},
        {typeof(UIModeChanged_t), 1040029},
        {typeof(StreamClientRaiseWindow_t), 1300001},
        {typeof(OverlayGameWindowFocusChange_t), 1020105},
        {typeof(WebAuthRequestCallback_t), 1020042}

    };
    public readonly static Dictionary<int, Type> IDToType = TypeToID.ToDictionary(x => x.Value, x => x.Key);
}
