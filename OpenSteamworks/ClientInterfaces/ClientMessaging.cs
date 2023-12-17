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

public class ClientMessaging
{
    private SteamClient client;
    private IClientSharedConnection iSharedConnection;
    private IClientUnifiedMessages iUnifiedMessages;
    private IClientUser clientUser;
    private List<Connection> connections = new();

    public Connection AllocateConnection() {
        var conn = new Connection(iSharedConnection, clientUser);
        connections.Add(conn);
        return conn;
    }

    public ClientMessaging(SteamClient client)
    {
        this.client = client;
        this.iSharedConnection = client.NativeClient.IClientSharedConnection;
        this.iUnifiedMessages = client.NativeClient.IClientUnifiedMessages;
        this.clientUser = client.NativeClient.IClientUser;
    }
    
    internal void Shutdown()
    {
        foreach (var item in connections)
        {
            item.Dispose();
        }
    }
}