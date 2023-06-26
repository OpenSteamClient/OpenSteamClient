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

#ifndef ICLIENTSHAREDCONNECTION_H
#define ICLIENTSHAREDCONNECTION_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

typedef uint32 HSharedConnection;

struct SharedConnectionMessageReady_t
{
	enum { k_iCallback = k_iClientSharedConnectionCallbacks + 1 };

	uint32 m_hResult;
};

abstract_class IClientSharedConnection
{
public:
    virtual HSharedConnection AllocateSharedConnection() = 0; //argc: 0, index 1
    virtual void ReleaseSharedConnection(HSharedConnection hConn) = 0; //argc: 1, index 1
    virtual int SendMessage(HSharedConnection hConn, void* pBuf, size_t szBuf) = 0; //argc: 3, index 2
    virtual int SendMessageAndAwaitResponse(HSharedConnection hConn, void* pBuf, size_t szBuf) = 0; //argc: 3, index 3
    virtual void RegisterEMsgHandler(HSharedConnection hConn, uint32 eMsg) = 0; //argc: 2, index 4
    virtual void RegisterServiceMethodHandler(HSharedConnection hConn, const char* msgHandler) = 0; //argc: 2, index 5
    virtual bool BPopReceivedMessage(HSharedConnection hConn, CUtlBuffer *bufOut, uint32 *hCall) = 0; //argc: 3, index 6
    virtual unknown_ret InitiateConnection(HSharedConnection hConn) = 0; //argc: 1, index 7
};

#endif // ICLIENTSHAREDCONNECTION_H