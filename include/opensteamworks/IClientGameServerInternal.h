//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code, comments, and all related files, projects, resources, 
// redistributables included with this project are Copyright Valve Corporation.
// Additionally, Valve, the Valve logo, Half-Life, the Half-Life logo, the
// Lambda logo, Steam, the Steam logo, Team Fortress, the Team Fortress logo, 
// Opposing Force, Day of Defeat, the Day of Defeat logo, Counter-Strike, the
// Counter-Strike logo, Source, the Source logo, and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef ICLIENTGAMESERVERINTERNAL_H
#define ICLIENTGAMESERVERINTERNAL_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

abstract_class IClientGameServerInternal
{
public:
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_0_DONTUSE() = 0; //argc: -1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_1_DONTUSE() = 0; //argc: -1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetSDRLogin() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_3_DONTUSE() = 0; //argc: -1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InitGameServerSerialized() = 0; //argc: 8, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetProduct() = 0; //argc: 1, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetGameDescription() = 0; //argc: 1, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetModDir() = 0; //argc: 1, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetDedicatedServer() = 0; //argc: 1, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret LogOn() = 0; //argc: 1, index 10
    virtual unknown_ret LogOnAnonymous() = 0; //argc: 0, index 11
    virtual unknown_ret LogOff() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetSteamID() = 0; //argc: 1, index 0
    virtual unknown_ret BLoggedOn() = 0; //argc: 0, index 1
    virtual unknown_ret BSecure() = 0; //argc: 0, index 0
    virtual unknown_ret WasRestartRequested() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetMaxPlayerCount() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetBotPlayerCount() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetServerName() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetMapName() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetPasswordProtected() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetSpectatorPort() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetSpectatorServerName() = 0; //argc: 1, index 6
    virtual unknown_ret ClearAllKeyValues() = 0; //argc: 0, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetKeyValue() = 0; //argc: 2, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetGameTags() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetGameData() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetRegion() = 0; //argc: 1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SendUserConnectAndAuthenticate() = 0; //argc: 4, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CreateUnauthenticatedUserConnection() = 0; //argc: 1, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SendUserDisconnect() = 0; //argc: 2, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BUpdateUserData() = 0; //argc: 4, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAuthSessionTicket() = 0; //argc: 3, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetAuthSessionTicketV2() = 0; //argc: 4, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret BeginAuthSession() = 0; //argc: 4, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EndAuthSession() = 0; //argc: 2, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret CancelAuthTicket() = 0; //argc: 1, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret IsUserSubscribedAppInTicket() = 0; //argc: 3, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RequestUserGroupStatus() = 0; //argc: 4, index 14
    virtual unknown_ret GetGameplayStats() = 0; //argc: 0, index 15
    virtual unknown_ret GetServerReputation() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetPublicIP() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EnableHeartbeats() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetHeartbeatInterval() = 0; //argc: 1, index 2
    virtual unknown_ret ForceHeartbeat() = 0; //argc: 0, index 3
    virtual unknown_ret GetLogonState() = 0; //argc: 0, index 0
    virtual unknown_ret BConnected() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RaiseConnectionPriority() = 0; //argc: 1, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ResetConnectionPriority() = 0; //argc: 1, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetCellID() = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TrackSteamUsageEvent() = 0; //argc: 3, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetCountOfSimultaneousGuestUsersPerSteamAccount() = 0; //argc: 1, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret EnumerateConnectedUsers() = 0; //argc: 2, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret AssociateWithClan() = 0; //argc: 2, index 6
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ComputeNewPlayerCompatibility() = 0; //argc: 2, index 7
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _BGetUserAchievementStatus() = 0; //argc: 3, index 8
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSSetSpawnCount() = 0; //argc: 1, index 9
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSGetSteam2GetEncryptionKeyToSendToNewClient() = 0; //argc: 3, index 10
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSSendSteam2UserConnect() = 0; //argc: 7, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSSendSteam3UserConnect() = 0; //argc: 5, index 12
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSSendUserConnect() = 0; //argc: 5, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSRemoveUserConnect() = 0; //argc: 1, index 14
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSUpdateStatus() = 0; //argc: 6, index 15
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSCreateUnauthenticatedUser() = 0; //argc: 1, index 16
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSSetServerType() = 0; //argc: 9, index 17
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _SetBasicServerData() = 0; //argc: 7, index 18
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret _GSSendUserDisconnect() = 0; //argc: 3, index 19
};

#endif // ICLIENTGAMESERVERINTERNAL_H