using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks;

namespace OpenSteamworks.ClientInterfaces;

public abstract class ClientInterface {

    private SteamClient client;
    public ClientInterface(SteamClient client) {
        this.client = client;
        this.client.CallbackManager.RegisterHandlersFor(this);
    }
    internal virtual async void RunShutdownTasks() {
        await System.Threading.Tasks.Task.CompletedTask;
    }
}