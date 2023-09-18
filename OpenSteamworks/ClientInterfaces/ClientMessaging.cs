using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.Structs;

namespace OpenSteamworks.ClientInterfaces;

public class ClientMessaging : ClientInterface
{
    private SteamClient client;
    private IClientSharedConnection iSharedConnection;
    private IClientUnifiedMessages iUnifiedMessages;
    private IClientUser iClientUser;
    private List<Connection> connections = new();

    public Connection AllocateConnection() {
        var conn = new Connection(iSharedConnection, iClientUser);
        connections.Add(conn);
        return conn;
    }

    public ClientMessaging(SteamClient client) : base(client)
    {
        this.client = client;
        this.iSharedConnection = client.NativeClient.IClientSharedConnection;
        this.iUnifiedMessages = client.NativeClient.IClientUnifiedMessages;
        this.iClientUser = client.NativeClient.IClientUser;
    }
    
    internal override void RunShutdownTasks()
    {
        foreach (var item in connections)
        {
            item.Dispose();
        }
        base.RunShutdownTasks();
    }

}