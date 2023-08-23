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
        {typeof(VerifyPasswordResponse_t), 1020040}
    };
    public readonly static Dictionary<int, Type> IDToType = TypeToID.ToDictionary(x => x.Value, x => x.Key);
}
