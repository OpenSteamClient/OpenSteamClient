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
    private ISteamClient client;
    private IClientSharedConnection iSharedConnection;
    private IClientUnifiedMessages iUnifiedMessages;
    private IClientUser clientUser;
    private List<Connection> connections = new();

    public Connection AllocateConnection() {
        var conn = new Connection(iSharedConnection, clientUser);
        connections.Add(conn);
        return conn;
    }

    public ClientMessaging(ISteamClient client)
    {
        this.client = client;
        this.iSharedConnection = client.IClientSharedConnection;
        this.iUnifiedMessages = client.IClientUnifiedMessages;
        this.clientUser = client.IClientUser;
    }
    
    internal void Shutdown(IProgress<string> progress)
    {
        progress.Report("Clearing shared connections");

        foreach (var item in connections)
        {
            item.Dispose();
        }
    }
}