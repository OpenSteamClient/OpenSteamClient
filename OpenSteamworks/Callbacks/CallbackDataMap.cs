using System;
using System.Collections.Generic;
using System.Linq;
using OpenSteamworks.Callbacks.Structs;

namespace OpenSteamworks.Callbacks;

internal static partial class CallbackConstants {
    public readonly static Dictionary<Type, int> TypeToID = new Dictionary<Type, int>
    {
        {typeof(SteamAPICallCompleted_t), 703},
        {typeof(SharedConnectionMessageReady_t), 1170001}
    };
    public readonly static Dictionary<int, Type> IDToType = TypeToID.ToDictionary(x => x.Value, x => x.Key);
}
