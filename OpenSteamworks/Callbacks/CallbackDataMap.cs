using System;
using System.Collections.Generic;
using System.Linq;
using OpenSteamworks.Callbacks.Structs;

namespace OpenSteamworks.Callbacks;

internal static partial class CallbackConstants {
    public readonly static Dictionary<Type, int> TypeToID = new()
    {
        {typeof(AppEventStateChange_t), 1280006},
        {typeof(CompatManagerToolRegistered_t), 1200002},
        {typeof(DownloadScheduleChanged_t), 1280009},
        {typeof(FriendRichPresenceUpdate_t), 336},
        {typeof(PersonaStateChange_t), 304},
        {typeof(SharedConnectionMessageReady_t), 1170001},
        {typeof(SteamAPICallCompleted_t), 703},
        {typeof(SteamConfigStoreChanged_t), 1040011},
        {typeof(UpdateJobFlagsChanged_t), 1280010},
    };
    public readonly static Dictionary<int, Type> IDToType = TypeToID.ToDictionary(x => x.Value, x => x.Key);
}
