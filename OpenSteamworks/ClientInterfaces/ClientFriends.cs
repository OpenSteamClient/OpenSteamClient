using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Structs;

namespace OpenSteamworks.ClientInterfaces;

public class ClientFriends : ClientInterface
{
    private SteamClient client;

    [CallbackListener<FriendRichPresenceUpdate_t>]
    private void OnFriendRichPresenceUpdate(FriendRichPresenceUpdate_t friendRichPresenceUpdate) {
        client.NativeClient.IClientFriends.GetFriendRichPresence();
    }

    [CallbackListener<PersonaStateChange_t>]
    private void OnPersonaStateChange(PersonaStateChange_t personaStateChange) {
        
    }

    public ClientFriends(SteamClient client) : base(client)
    {
        this.client = client;
    }
    
    internal override void RunShutdownTasks()
    {
        base.RunShutdownTasks();
        
    }

}