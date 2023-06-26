//==========================  Open Steamworks  ================================
//
// This file is part of the Open Steamworks project. All individuals associated
// with this project do not claim ownership of the contents
// 
// The code,  comments,  and all related files,  projects,  resources, 
// redistributables included with this project are Copyright Valve Corporation.
// Additionally,  Valve,  the Valve logo,  Half-Life,  the Half-Life logo,  the
// Lambda logo,  Steam,  the Steam logo,  Team Fortress,  the Team Fortress logo, 
// Opposing Force,  Day of Defeat,  the Day of Defeat logo,  Counter-Strike,  the
// Counter-Strike logo,  Source,  the Source logo,  and Counter-Strike Condition
// Zero are trademarks and or registered trademarks of Valve Corporation.
// All other trademarks are property of their respective owners.
//
//=============================================================================

#ifndef ICLIENTCONFIGSTORE_H
#define ICLIENTCONFIGSTORE_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "UtilsCommon.h"


#define CLIENTCONFIGSTORE_INTERFACE_VERSION "CLIENTCONFIGSTORE_INTERFACE_VERSION001"


abstract_class IClientConfigStore
{
public:
    virtual bool IsSet( EConfigStore eConfigStore, const char *pszKeyNameIn ) = 0; //argc: 2, index 1
    
    virtual bool GetBool( EConfigStore eConfigStore, const char *pszKeyNameIn, bool defaultValue ) = 0; //argc: 3, index 2
    virtual int32 GetInt( EConfigStore eConfigStore, const char *pszKeyName, int32 defaultValue ) = 0; //argc: 3, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual uint64 GetUint64( EConfigStore eConfigStore, const char *pszKeyName, uint64 defaultValue ) = 0; //argc: 4, index 4
    virtual float GetFloat( EConfigStore eConfigStore, const char *pszKeyName, float defaultValue ) = 0; //argc: 3, index 5
    virtual const char* GetString( EConfigStore eConfigStore, const char *pszKeyName, const char *defaultValue ) = 0; //argc: 3, index 6
    virtual uint32 GetBinary( EConfigStore eConfigStore, const char *pszKeyName, uint8 *pubBuf, uint32 cubBuf ) = 0; //argc: 4, index 7
    virtual uint32 GetBinary( EConfigStore eConfigStore, const char *pszKeyName, CUtlBuffer *pUtlBuf) = 0; //argc: 3, index 8
    virtual uint32 GetBinaryWatermarked( EConfigStore eConfigStore, const char *pszKeyName, uint8 *pubBuf, uint32 cubBuf ) = 0; //argc: 4, index 9
    
    virtual bool SetBool( EConfigStore eConfigStore, const char *pszKeyNameIn, bool value ) = 0; //argc: 3, index 10
    virtual bool SetInt( EConfigStore eConfigStore, const char *pszKeyNameIn, int32 nValue ) = 0; //argc: 3, index 11
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SetUint64( EConfigStore eConfigStore, const char *pszKeyNameIn, uint64 unValue ) = 0; //argc: 4, index 12
    virtual bool SetFloat( EConfigStore eConfigStore, const char *pszKeyNameIn, float flValue ) = 0; //argc: 3, index 13
    virtual bool SetString( EConfigStore eConfigStore, const char *pszKeyNameIn, const char *pszValue ) = 0; //argc: 3, index 14
    virtual bool SetBinary( EConfigStore eConfigStore, const char *pszKeyName, const uint8 *pubData, uint32 cubData ) = 0; //argc: 4, index 15
    virtual bool SetBinaryWatermarked( EConfigStore eConfigStore, const char *pszKeyName, const uint8 *pubData, uint32 cubData ) = 0; //argc: 4, index 16
    
    virtual bool RemoveKey( EConfigStore eConfigStore, const char *pszKeyName ) = 0; //argc: 2, index 17
    virtual int32 GetKeySerialized( EConfigStore eConfigStore, const char *pszKeyNameIn, uint8 *pchBuffer, int32 cbBufferMax ) = 0; //argc: 4, index 18
    
    virtual bool FlushToDisk( bool bIsShuttingDown ) = 0; //argc: 1, index 19
};

#endif // ICLIENTCONFIGSTORE_H