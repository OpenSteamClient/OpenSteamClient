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

abstract_class UNSAFE_INTERFACE IClientSharedConnection
{
public:
    virtual HSharedConnection AllocateSharedConnection() = 0;
	virtual void ReleaseSharedConnection(HSharedConnection hConn) = 0;
	virtual int SendMessage(HSharedConnection hConn, void* pBuf, size_t szBuf) = 0;
	virtual int SendMessageAndAwaitResponse(HSharedConnection hConn, void* pBuf, size_t szBuf) = 0;
	virtual void RegisterEMsgHandler(HSharedConnection hConn, uint32 eMsg) = 0;
	virtual void RegisterServiceMethodHandler(HSharedConnection hConn, const char* msgHandler) = 0;
	virtual bool BPopReceivedMessage(HSharedConnection hConn, CUtlBuffer *bufOut, uint32 *hCall) = 0;
	virtual unknown_ret InitiateConnection(HSharedConnection hConn) = 0;
    
};

#endif // ICLIENTSHAREDCONNECTION_H
