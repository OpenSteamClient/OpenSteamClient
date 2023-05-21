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

#ifndef ICLIENTDEVICEAUTH_H
#define ICLIENTDEVICEAUTH_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"

#define CLIENTDEVICEAUTH_INTERFACE_VERSION "CLIENTDEVICEAUTH_INTERFACE_VERSION001"

abstract_class IClientDeviceAuth
{
public:
	virtual uint64 AuthorizeLocalDevice( const char * pubUnk, uint32 cubUnk ) = 0;
	virtual uint64 DeauthorizeLocalDevice( uint32 uUnk) = 0;

	virtual uint64 DeauthorizeRemoteDevice( uint64 uUnk ) = 0;
	virtual uint64 RequestAuthorizationInfos() = 0;
	virtual uint32 GetDeviceAuthorizations( uint64 *, uint32, bool) = 0;
	virtual bool GetDeviceAuthorizationInfo( uint64, uint32 *, uint32 *, bool *, char *, int32, char *, int32, char *, int32, uint32 *) = 0;

	virtual uint32 GetAuthorizedBorrowsers( uint32 *, uint32 ) = 0;
	virtual uint32 GetLocalUsers( uint32 *, uint32 ) = 0;
	virtual bool GetBorrowserInfo( uint32, char *, int32, bool * ) = 0;
	virtual uint64 UpateAuthorizedBorrowers( const uint32 *, uint32, bool ) = 0;

	virtual uint32 GetSharedLibraryLockedBy( uint32 uUnk ) = 0;
	virtual uint32 GetSharedLibraryOwners( uint32 *, uint32 ) = 0;
};

#endif // ICLIENTDEVICEAUTH_H
