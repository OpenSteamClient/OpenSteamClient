//========= Copyright � 1996-2008, Valve LLC, All rights reserved. ============
//
// Declare common types used by the Steamworks SDK.
//
//=============================================================================

#ifndef STEAMCLIENTPUBLIC_H
#define STEAMCLIENTPUBLIC_H

#include "SteamTypes.h"



// Steam account types
enum EAccountType
{
	k_EAccountTypeInvalid = 0,			
	k_EAccountTypeIndividual = 1,		// single user account
	k_EAccountTypeMultiseat = 2,		// multiseat (e.g. cybercafe) account
	k_EAccountTypeGameServer = 3,		// game server account
	k_EAccountTypeAnonGameServer = 4,	// anonymous game server account
	k_EAccountTypePending = 5,			// pending
	k_EAccountTypeContentServer = 6,	// content server
	k_EAccountTypeClan = 7,
	k_EAccountTypeChat = 8,
	k_EAccountTypeConsoleUser = 9,		// Fake SteamID for local PSN account on PS3 or Live account on 360, etc.
	k_EAccountTypeAnonUser = 10,

	// Max of 16 items in this field
	k_EAccountTypeMax
};



//-----------------------------------------------------------------------------
// Purpose: Broadcast upload result details
//-----------------------------------------------------------------------------
enum EBroadcastUploadResult
{
	k_EBroadcastUploadResultNone = 0,	// broadcast state unknown
	k_EBroadcastUploadResultOK = 1,		// broadcast was good, no problems
	k_EBroadcastUploadResultInitFailed = 2,	// broadcast init failed
	k_EBroadcastUploadResultFrameFailed = 3,	// broadcast frame upload failed
	k_EBroadcastUploadResultTimeout = 4,	// broadcast upload timed out
	k_EBroadcastUploadResultBandwidthExceeded = 5,	// broadcast send too much data
	k_EBroadcastUploadResultLowFPS = 6,	// broadcast FPS too low
	k_EBroadcastUploadResultMissingKeyFrames = 7,	// broadcast sending not enough key frames
	k_EBroadcastUploadResultNoConnection = 8,	// broadcast client failed to connect to relay
	k_EBroadcastUploadResultRelayFailed = 9,	// relay dropped the upload
	k_EBroadcastUploadResultSettingsChanged = 10,	// the client changed broadcast settings 
	k_EBroadcastUploadResultMissingAudio = 11,	// client failed to send audio data
	k_EBroadcastUploadResultTooFarBehind = 12,	// clients was too slow uploading
	k_EBroadcastUploadResultTranscodeBehind = 13,	// server failed to keep up with transcode
	k_EBroadcastUploadResultNotAllowedToPlay = 14, // Broadcast does not have permissions to play game
	k_EBroadcastUploadResultBusy = 15, // RTMP host to busy to take new broadcast stream, choose another
	k_EBroadcastUploadResultBanned = 16, // Account banned from community broadcast
	k_EBroadcastUploadResultAlreadyActive = 17, // We already already have an stream running.
	k_EBroadcastUploadResultForcedOff = 18, // We explicitly shutting down a broadcast
	k_EBroadcastUploadResultAudioBehind = 19, // Audio stream was too far behind video 
	k_EBroadcastUploadResultShutdown = 20,	// Broadcast Server was shut down
	k_EBroadcastUploadResultDisconnect = 21,	// broadcast uploader TCP disconnected 
	k_EBroadcastUploadResultVideoInitFailed = 22,	// invalid video settings 
	k_EBroadcastUploadResultAudioInitFailed = 23,	// invalid audio settings 
};


//-----------------------------------------------------------------------------
// Purpose: Reasons a user may not use the Community Market.
//          Used in MarketEligibilityResponse_t.
//-----------------------------------------------------------------------------
enum EMarketNotAllowedReasonFlags
{
	k_EMarketNotAllowedReason_None = 0,

	// A back-end call failed or something that might work again on retry
	k_EMarketNotAllowedReason_TemporaryFailure = (1 << 0),

	// Disabled account
	k_EMarketNotAllowedReason_AccountDisabled = (1 << 1),

	// Locked account
	k_EMarketNotAllowedReason_AccountLockedDown = (1 << 2),

	// Limited account (no purchases)
	k_EMarketNotAllowedReason_AccountLimited = (1 << 3),

	// The account is banned from trading items
	k_EMarketNotAllowedReason_TradeBanned = (1 << 4),

	// Wallet funds aren't tradable because the user has had no purchase
	// activity in the last year or has had no purchases prior to last month
	k_EMarketNotAllowedReason_AccountNotTrusted = (1 << 5),

	// The user doesn't have Steam Guard enabled
	k_EMarketNotAllowedReason_SteamGuardNotEnabled = (1 << 6),

	// The user has Steam Guard, but it hasn't been enabled for the required
	// number of days
	k_EMarketNotAllowedReason_SteamGuardOnlyRecentlyEnabled = (1 << 7),

	// The user has recently forgotten their password and reset it
	k_EMarketNotAllowedReason_RecentPasswordReset = (1 << 8),

	// The user has recently funded his or her wallet with a new payment method
	k_EMarketNotAllowedReason_NewPaymentMethod = (1 << 9),

	// An invalid cookie was sent by the user
	k_EMarketNotAllowedReason_InvalidCookie = (1 << 10),

	// The user has Steam Guard, but is using a new computer or web browser
	k_EMarketNotAllowedReason_UsingNewDevice = (1 << 11),

	// The user has recently refunded a store purchase by his or herself
	k_EMarketNotAllowedReason_RecentSelfRefund = (1 << 12),

	// The user has recently funded his or her wallet with a new payment method that cannot be verified
	k_EMarketNotAllowedReason_NewPaymentMethodCannotBeVerified = (1 << 13),

	// Not only is the account not trusted, but they have no recent purchases at all
	k_EMarketNotAllowedReason_NoRecentPurchases = (1 << 14),

	// User accepted a wallet gift that was recently purchased
	k_EMarketNotAllowedReason_AcceptedWalletGift = (1 << 15),
};


//
// describes XP / progress restrictions to apply for games with duration control /
// anti-indulgence enabled for minor Steam China users.
//
// WARNING: DO NOT RENUMBER
enum EDurationControlProgress
{
	k_EDurationControlProgress_Full = 0,	// Full progress
	k_EDurationControlProgress_Half = 1,	// deprecated - XP or persistent rewards should be halved
	k_EDurationControlProgress_None = 2,	// deprecated - XP or persistent rewards should be stopped

	k_EDurationControl_ExitSoon_3h = 3,		// allowed 3h time since 5h gap/break has elapsed, game should exit - steam will terminate the game soon
	k_EDurationControl_ExitSoon_5h = 4,		// allowed 5h time in calendar day has elapsed, game should exit - steam will terminate the game soon
	k_EDurationControl_ExitSoon_Night = 5,	// game running after day period, game should exit - steam will terminate the game soon
};


//
// describes which notification timer has expired, for steam china duration control feature
//
// WARNING: DO NOT RENUMBER
enum EDurationControlNotification
{
	k_EDurationControlNotification_None = 0,		// just informing you about progress, no notification to show
	k_EDurationControlNotification_1Hour = 1,		// "you've been playing for N hours"
	
	k_EDurationControlNotification_3Hours = 2,		// deprecated - "you've been playing for 3 hours; take a break"
	k_EDurationControlNotification_HalfProgress = 3,// deprecated - "your XP / progress is half normal"
	k_EDurationControlNotification_NoProgress = 4,	// deprecated - "your XP / progress is zero"
	
	k_EDurationControlNotification_ExitSoon_3h = 5,	// allowed 3h time since 5h gap/break has elapsed, game should exit - steam will terminate the game soon
	k_EDurationControlNotification_ExitSoon_5h = 6,	// allowed 5h time in calendar day has elapsed, game should exit - steam will terminate the game soon
	k_EDurationControlNotification_ExitSoon_Night = 7,// game running after day period, game should exit - steam will terminate the game soon
};


//
// Specifies a game's online state in relation to duration control
//
enum EDurationControlOnlineState
{
	k_EDurationControlOnlineState_Invalid = 0,				// nil value
	k_EDurationControlOnlineState_Offline = 1,				// currently in offline play - single-player, offline co-op, etc.
	k_EDurationControlOnlineState_Online = 2,				// currently in online play
	k_EDurationControlOnlineState_OnlineHighPri = 3,		// currently in online play and requests not to be interrupted
};


#pragma pack( push, 1 )

#define CSTEAMID_DEFINED

// Steam ID structure (64 bits total)
class CSteamID
{
public:

	//-----------------------------------------------------------------------------
	// Purpose: Constructor
	//-----------------------------------------------------------------------------
	CSteamID()
	{
		m_steamid.m_comp.m_unAccountID = 0;
		m_steamid.m_comp.m_EAccountType = k_EAccountTypeInvalid;
		m_steamid.m_comp.m_EUniverse = k_EUniverseInvalid;
		m_steamid.m_comp.m_unAccountInstance = 0;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Constructor
	// Input  : unAccountID -	32-bit account ID
	//			eUniverse -		Universe this account belongs to
	//			eAccountType -	Type of account
	//-----------------------------------------------------------------------------
	CSteamID( uint32 unAccountID, EUniverse eUniverse, EAccountType eAccountType )
	{
		Set( unAccountID, eUniverse, eAccountType );
	}


	//-----------------------------------------------------------------------------
	// Purpose: Constructor
	// Input  : unAccountID -	32-bit account ID
	//			unAccountInstance - instance 
	//			eUniverse -		Universe this account belongs to
	//			eAccountType -	Type of account
	//-----------------------------------------------------------------------------
	CSteamID( uint32 unAccountID, unsigned int unAccountInstance, EUniverse eUniverse, EAccountType eAccountType )
	{
#if defined(_SERVER) && defined(Assert)
		Assert( ( k_EAccountTypeIndividual != eAccountType ) || ( unAccountInstance == k_unSteamUserDefaultInstance ) );	// enforce that for individual accounts, instance is always 1
#endif // _SERVER
		InstancedSet( unAccountID, unAccountInstance, eUniverse, eAccountType );
	}


	//-----------------------------------------------------------------------------
	// Purpose: Constructor
	// Input  : ulSteamID -		64-bit representation of a Steam ID
	// Note:	Will not accept a uint32 or int32 as input, as that is a probable mistake.
	//			See the stubbed out overloads in the private: section for more info.
	//-----------------------------------------------------------------------------
	CSteamID( uint64 ulSteamID )
	{
		SetFromUint64( ulSteamID );
	}
#ifdef INT64_DIFFERENT_FROM_INT64_T
	CSteamID( uint64_t ulSteamID )
	{
		SetFromUint64( (uint64)ulSteamID );
	}
#endif


	//-----------------------------------------------------------------------------
	// Purpose: Sets parameters for steam ID
	// Input  : unAccountID -	32-bit account ID
	//			eUniverse -		Universe this account belongs to
	//			eAccountType -	Type of account
	//-----------------------------------------------------------------------------
	void Set( uint32 unAccountID, EUniverse eUniverse, EAccountType eAccountType )
	{
		m_steamid.m_comp.m_unAccountID = unAccountID;
		m_steamid.m_comp.m_EUniverse = eUniverse;
		m_steamid.m_comp.m_EAccountType = eAccountType;

		if ( eAccountType == k_EAccountTypeClan || eAccountType == k_EAccountTypeGameServer )
		{
			m_steamid.m_comp.m_unAccountInstance = 0;
		}
		else
		{
			m_steamid.m_comp.m_unAccountInstance = k_unSteamUserDefaultInstance;
		}
	}


	//-----------------------------------------------------------------------------
	// Purpose: Sets parameters for steam ID
	// Input  : unAccountID -	32-bit account ID
	//			eUniverse -		Universe this account belongs to
	//			eAccountType -	Type of account
	//-----------------------------------------------------------------------------
	void InstancedSet( uint32 unAccountID, uint32 unInstance, EUniverse eUniverse, EAccountType eAccountType )
	{
		m_steamid.m_comp.m_unAccountID = unAccountID;
		m_steamid.m_comp.m_EUniverse = eUniverse;
		m_steamid.m_comp.m_EAccountType = eAccountType;
		m_steamid.m_comp.m_unAccountInstance = unInstance;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Initializes a steam ID from its 52 bit parts and universe/type
	// Input  : ulIdentifier - 52 bits of goodness
	//-----------------------------------------------------------------------------
	void FullSet( uint64 ulIdentifier, EUniverse eUniverse, EAccountType eAccountType )
	{
		m_steamid.m_comp.m_unAccountID = ( ulIdentifier & k_unSteamAccountIDMask );						// account ID is low 32 bits
		m_steamid.m_comp.m_unAccountInstance = ( ( ulIdentifier >> 32 ) & k_unSteamAccountInstanceMask );			// account instance is next 20 bits
		m_steamid.m_comp.m_EUniverse = eUniverse;
		m_steamid.m_comp.m_EAccountType = eAccountType;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Initializes a steam ID from its 64-bit representation
	// Input  : ulSteamID -		64-bit representation of a Steam ID
	//-----------------------------------------------------------------------------
	void SetFromUint64( uint64 ulSteamID )
	{
		m_steamid.m_unAll64Bits = ulSteamID;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Clear all fields, leaving an invalid ID.
	//-----------------------------------------------------------------------------
    void Clear()
	{
		m_steamid.m_comp.m_unAccountID = 0;
		m_steamid.m_comp.m_EAccountType = k_EAccountTypeInvalid;
		m_steamid.m_comp.m_EUniverse = k_EUniverseInvalid;
		m_steamid.m_comp.m_unAccountInstance = 0;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Converts steam ID to its 64-bit representation
	// Output : 64-bit representation of a Steam ID
	//-----------------------------------------------------------------------------
	uint64 ConvertToUint64() const
	{
		return m_steamid.m_unAll64Bits;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Converts the static parts of a steam ID to a 64-bit representation.
	//			For multiseat accounts, all instances of that account will have the
	//			same static account key, so they can be grouped together by the static
	//			account key.
	// Output : 64-bit static account key
	//-----------------------------------------------------------------------------
	uint64 GetStaticAccountKey() const
	{
		// note we do NOT include the account instance (which is a dynamic property) in the static account key
		return (uint64) ( ( ( (uint64) m_steamid.m_comp.m_EUniverse ) << 56 ) + ((uint64) m_steamid.m_comp.m_EAccountType << 52 ) + m_steamid.m_comp.m_unAccountID );
	}


	//-----------------------------------------------------------------------------
	// Purpose: create an anonymous game server login to be filled in by the AM
	//-----------------------------------------------------------------------------
	void CreateBlankAnonLogon( EUniverse eUniverse )
	{
		m_steamid.m_comp.m_unAccountID = 0;
		m_steamid.m_comp.m_EAccountType = k_EAccountTypeAnonGameServer;
		m_steamid.m_comp.m_EUniverse = eUniverse;
		m_steamid.m_comp.m_unAccountInstance = 0;
	}


	//-----------------------------------------------------------------------------
	// Purpose: create an anonymous game server login to be filled in by the AM
	//-----------------------------------------------------------------------------
	void CreateBlankAnonUserLogon( EUniverse eUniverse )
	{
		m_steamid.m_comp.m_unAccountID = 0;
		m_steamid.m_comp.m_EAccountType = k_EAccountTypeAnonUser;
		m_steamid.m_comp.m_EUniverse = eUniverse;
		m_steamid.m_comp.m_unAccountInstance = 0;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this an anonymous game server login that will be filled in?
	//-----------------------------------------------------------------------------
	bool BBlankAnonAccount() const
	{
		return m_steamid.m_comp.m_unAccountID == 0 && BAnonAccount() && m_steamid.m_comp.m_unAccountInstance == 0;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this a game server account id?  (Either persistent or anonymous)
	//-----------------------------------------------------------------------------
	bool BGameServerAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeGameServer || m_steamid.m_comp.m_EAccountType == k_EAccountTypeAnonGameServer;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this a persistent (not anonymous) game server account id?
	//-----------------------------------------------------------------------------
	bool BPersistentGameServerAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeGameServer;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this an anonymous game server account id?
	//-----------------------------------------------------------------------------
	bool BAnonGameServerAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeAnonGameServer;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this a content server account id?
	//-----------------------------------------------------------------------------
	bool BContentServerAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeContentServer;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Is this a clan account id?
	//-----------------------------------------------------------------------------
	bool BClanAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeClan;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Is this a chat account id?
	//-----------------------------------------------------------------------------
	bool BChatAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeChat;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this a chat account id?
	//-----------------------------------------------------------------------------
	bool IsLobby() const
	{
		return ( m_steamid.m_comp.m_EAccountType == k_EAccountTypeChat )
			&& ( m_steamid.m_comp.m_unAccountInstance & k_EChatInstanceFlagLobby );
	}


	//-----------------------------------------------------------------------------
	// Purpose: Is this an individual user account id?
	//-----------------------------------------------------------------------------
	bool BIndividualAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeIndividual || m_steamid.m_comp.m_EAccountType == k_EAccountTypeConsoleUser;
	}


	//-----------------------------------------------------------------------------
	// Purpose: Is this an anonymous account?
	//-----------------------------------------------------------------------------
	bool BAnonAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeAnonUser || m_steamid.m_comp.m_EAccountType == k_EAccountTypeAnonGameServer;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this an anonymous user account? ( used to create an account or reset a password )
	//-----------------------------------------------------------------------------
	bool BAnonUserAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeAnonUser;
	}

	//-----------------------------------------------------------------------------
	// Purpose: Is this a faked up Steam ID for a PSN friend account?
	//-----------------------------------------------------------------------------
	bool BConsoleUserAccount() const
	{
		return m_steamid.m_comp.m_EAccountType == k_EAccountTypeConsoleUser;
	}
	
	const char *SteamRender() const // renders this steam ID to string using the new rendering style
	{
		const int k_cBufLen = 37;
		const int k_cBufs = 4;
		char* pchBuf;

		static char rgchBuf[k_cBufs][k_cBufLen];
		static int nBuf = 0;

		pchBuf = rgchBuf[nBuf++];
		nBuf %= k_cBufs;

		switch (m_steamid.m_comp.m_EAccountType)
		{
		case k_EAccountTypeAnonGameServer:
			sprintf(pchBuf, "[A:%u:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID, m_steamid.m_comp.m_unAccountInstance);
			break;
		case k_EAccountTypeGameServer:
			sprintf(pchBuf, "[G:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		case k_EAccountTypeMultiseat:
			sprintf(pchBuf, "[M:%u:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID, m_steamid.m_comp.m_unAccountInstance);
			break;
		case k_EAccountTypePending:
			sprintf(pchBuf, "[P:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		case k_EAccountTypeContentServer:
			sprintf(pchBuf, "[C:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		case k_EAccountTypeClan:
			sprintf(pchBuf, "[g:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		case k_EAccountTypeChat:
			switch (m_steamid.m_comp.m_unAccountInstance & ~k_EChatAccountInstanceMask)
			{
			case k_EChatInstanceFlagClan:
				sprintf(pchBuf, "[c:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
				break;
			case k_EChatInstanceFlagLobby:
				sprintf(pchBuf, "[L:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
				break;
			default:
				sprintf(pchBuf, "[T:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
				break;
			}
			break;
		case k_EAccountTypeInvalid:
			sprintf(pchBuf, "[I:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		case k_EAccountTypeIndividual:
			sprintf(pchBuf, "[U:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		default:
			sprintf(pchBuf, "[i:%u:%u]", m_steamid.m_comp.m_EUniverse, m_steamid.m_comp.m_unAccountID);
			break;
		}

		return pchBuf;
	}


	// simple accessors
	void SetAccountID( uint32 unAccountID )		{ m_steamid.m_comp.m_unAccountID = unAccountID; }
	void SetAccountInstance( uint32 unInstance ){ m_steamid.m_comp.m_unAccountInstance = unInstance; }
		
	AccountID_t GetAccountID() const			{ return m_steamid.m_comp.m_unAccountID; }
	uint32 GetUnAccountInstance() const			{ return m_steamid.m_comp.m_unAccountInstance; }
	EAccountType GetEAccountType() const		{ return ( EAccountType ) m_steamid.m_comp.m_EAccountType; }
	EUniverse GetEUniverse() const				{ return m_steamid.m_comp.m_EUniverse; }
	void SetEUniverse( EUniverse eUniverse )	{ m_steamid.m_comp.m_EUniverse = eUniverse; }
	bool IsValid() const;

	// this set of functions is hidden, will be moved out of class
	explicit CSteamID( const char *pchSteamID, EUniverse eDefaultUniverse = k_EUniverseInvalid );

	void SetFromString( const char *pchSteamID, EUniverse eDefaultUniverse );
    // SetFromString allows many partially-correct strings, constraining how
    // we might be able to change things in the future.
    // SetFromStringStrict requires the exact string forms that we support
    // and is preferred when the caller knows it's safe to be strict.
    // Returns whether the string parsed correctly.
	bool SetFromStringStrict( const char *pchSteamID, EUniverse eDefaultUniverse );

	inline bool operator==( const CSteamID &val ) const { return m_steamid.m_unAll64Bits == val.m_steamid.m_unAll64Bits; } 
	inline bool operator!=( const CSteamID &val ) const { return !operator==( val ); }
	inline bool operator<( const CSteamID &val ) const { return m_steamid.m_unAll64Bits < val.m_steamid.m_unAll64Bits; }
	inline bool operator>( const CSteamID &val ) const { return m_steamid.m_unAll64Bits > val.m_steamid.m_unAll64Bits; }

	// DEBUG function
	bool BValidExternalSteamID() const;

private:
	// These are defined here to prevent accidental implicit conversion of a u32AccountID to a CSteamID.
	// If you get a compiler error about an ambiguous constructor/function then it may be because you're
	// passing a 32-bit int to a function that takes a CSteamID. You should explicitly create the SteamID
	// using the correct Universe and account Type/Instance values.
	CSteamID( uint32 );
	CSteamID( int32 );

	// 64 bits total
	union SteamID_t
	{
		struct SteamIDComponent_t
		{
#ifdef VALVE_BIG_ENDIAN
			EUniverse			m_EUniverse : 8;	// universe this account belongs to
			unsigned int		m_EAccountType : 4;			// type of account - can't show as EAccountType, due to signed / unsigned difference
			unsigned int		m_unAccountInstance : 20;	// dynamic instance ID
			uint32				m_unAccountID : 32;			// unique account identifier
#else
			uint32				m_unAccountID : 32;			// unique account identifier
			unsigned int		m_unAccountInstance : 20;	// dynamic instance ID
			unsigned int		m_EAccountType : 4;			// type of account - can't show as EAccountType, due to signed / unsigned difference
			EUniverse			m_EUniverse : 8;	// universe this account belongs to
#endif
		} m_comp;

		uint64 m_unAll64Bits;
	} m_steamid;
};

inline bool CSteamID::IsValid() const
{
	if ( m_steamid.m_comp.m_EAccountType <= k_EAccountTypeInvalid || m_steamid.m_comp.m_EAccountType >= k_EAccountTypeMax )
		return false;
	
	if ( m_steamid.m_comp.m_EUniverse <= k_EUniverseInvalid || m_steamid.m_comp.m_EUniverse >= k_EUniverseMax )
		return false;

	if ( m_steamid.m_comp.m_EAccountType == k_EAccountTypeIndividual )
	{
		if ( m_steamid.m_comp.m_unAccountID == 0 || m_steamid.m_comp.m_unAccountInstance != k_unSteamUserDefaultInstance )
			return false;
	}

	if ( m_steamid.m_comp.m_EAccountType == k_EAccountTypeClan )
	{
		if ( m_steamid.m_comp.m_unAccountID == 0 || m_steamid.m_comp.m_unAccountInstance != 0 )
			return false;
	}

	if ( m_steamid.m_comp.m_EAccountType == k_EAccountTypeGameServer )
	{
		if ( m_steamid.m_comp.m_unAccountID == 0 )
			return false;
		// Any limit on instances?  We use them for local users and bots
	}
	return true;
}

#if defined( INCLUDED_STEAM2_USERID_STRUCTS ) 

//-----------------------------------------------------------------------------
// Purpose: Initializes a steam ID from a Steam2 ID structure
// Input:	pTSteamGlobalUserID -	Steam2 ID to convert
//			eUniverse -				universe this ID belongs to
//-----------------------------------------------------------------------------
inline CSteamID SteamIDFromSteam2UserID( TSteamGlobalUserID *pTSteamGlobalUserID, EUniverse eUniverse )
{
	uint32 unAccountID = pTSteamGlobalUserID->m_SteamLocalUserID.Split.Low32bits * 2 + 
		pTSteamGlobalUserID->m_SteamLocalUserID.Split.High32bits;

	return CSteamID( unAccountID, k_unSteamUserDefaultInstance, eUniverse, k_EAccountTypeIndividual );
}

bool SteamIDFromSteam2String( const char *pchSteam2ID, EUniverse eUniverse, CSteamID *pSteamIDOut );

//-----------------------------------------------------------------------------
// Purpose: Fills out a Steam2 ID structure
// Input:	pTSteamGlobalUserID -	Steam2 ID to write to
//-----------------------------------------------------------------------------
inline TSteamGlobalUserID SteamIDToSteam2UserID( CSteamID steamID )
{
	TSteamGlobalUserID steamGlobalUserID;

	steamGlobalUserID.m_SteamInstanceID = 0;
	steamGlobalUserID.m_SteamLocalUserID.Split.High32bits = steamID.GetAccountID() % 2;
	steamGlobalUserID.m_SteamLocalUserID.Split.Low32bits = steamID.GetAccountID() / 2;

	return steamGlobalUserID;
}


#endif

// generic invalid CSteamID
#define k_steamIDNil CSteamID()

// This steamID comes from a user game connection to an out of date GS that hasnt implemented the protocol
// to provide its steamID
#define k_steamIDOutofDateGS CSteamID( 0, 0, k_EUniverseInvalid, k_EAccountTypeInvalid )
// This steamID comes from a user game connection to an sv_lan GS
#define k_steamIDLanModeGS CSteamID( 0, 0, k_EUniversePublic, k_EAccountTypeInvalid )
// This steamID can come from a user game connection to a GS that has just booted but hasnt yet even initialized
// its steam3 component and started logging on.
#define k_steamIDNotInitYetGS CSteamID( 1, 0, k_EUniverseInvalid, k_EAccountTypeInvalid )
// This steamID can come from a user game connection to a GS that isn't using the steam authentication system but still
// wants to support the "Join Game" option in the friends list
#define k_steamIDNonSteamGS CSteamID( 2, 0, k_EUniverseInvalid, k_EAccountTypeInvalid )


#ifdef STEAM
// Returns the matching chat steamID, with the default instance of 0
// If the steamID passed in is already of type k_EAccountTypeChat it will be returned with the same instance
CSteamID ChatIDFromSteamID( const CSteamID &steamID );
// Returns the matching clan steamID, with the default instance of 0
// If the steamID passed in is already of type k_EAccountTypeClan it will be returned with the same instance
CSteamID ClanIDFromSteamID( const CSteamID &steamID );
// Asserts steamID type before conversion
CSteamID ChatIDFromClanID( const CSteamID &steamIDClan );
// Asserts steamID type before conversion
CSteamID ClanIDFromChatID( const CSteamID &steamIDChat );

#endif // _STEAM


//-----------------------------------------------------------------------------
// Purpose: encapsulates an appID/modID pair
//-----------------------------------------------------------------------------
class CGameID
{
public:

	CGameID()
	{
		m_gameID.m_nType = k_EGameIDTypeApp;
		m_gameID.m_nAppID = k_uAppIdInvalid;
		m_gameID.m_nModID = 0;
	}

	explicit CGameID( uint64 ulGameID )
	{
		m_ulGameID = ulGameID;
	}
#ifdef INT64_DIFFERENT_FROM_INT64_T
	CGameID( uint64_t ulGameID )
	{
		m_ulGameID = (uint64)ulGameID;
	}
#endif

	explicit CGameID( int32 nAppID )
	{
		m_ulGameID = 0;
		m_gameID.m_nAppID = nAppID;
	}

	explicit CGameID( uint32 nAppID )
	{
		m_ulGameID = 0;
		m_gameID.m_nAppID = nAppID;
	}

	CGameID( uint32 nAppID, uint32 nModID )
	{
		m_ulGameID = 0;
		m_gameID.m_nAppID = nAppID;
		m_gameID.m_nModID = nModID;
		m_gameID.m_nType = k_EGameIDTypeGameMod;
	}

	CGameID( const CGameID &that )
	{
		m_ulGameID = that.m_ulGameID;
	}

	CGameID& operator=( const CGameID & that )
	{
		m_ulGameID = that.m_ulGameID;
		return *this;
	}

	// Hidden functions used only by Steam
	explicit CGameID( const char *pchGameID );
	const char *Render() const;					// render this Game ID to string
	static const char *Render( uint64 ulGameID );		// static method to render a uint64 representation of a Game ID to a string

	uint64 ToUint64() const
	{
		return m_ulGameID;
	}

	uint64 *GetUint64Ptr()
	{
		return &m_ulGameID;
	}

	void Set( uint64 ulGameID )
	{
		m_ulGameID = ulGameID;
	}

	bool IsMod() const
	{
		return ( m_gameID.m_nType == k_EGameIDTypeGameMod );
	}

	bool IsShortcut() const
	{
		return ( m_gameID.m_nType == k_EGameIDTypeShortcut );
	}

	bool IsP2PFile() const
	{
		return ( m_gameID.m_nType == k_EGameIDTypeP2P );
	}

	bool IsSteamApp() const
	{
		return ( m_gameID.m_nType == k_EGameIDTypeApp );
	}
		
	uint32 ModID() const
	{
		return m_gameID.m_nModID;
	}

	uint32 AppID() const
	{
		return m_gameID.m_nAppID;
	}

	bool operator == ( const CGameID &rhs ) const
	{
		return m_ulGameID == rhs.m_ulGameID;
	}

	bool operator != ( const CGameID &rhs ) const
	{
		return !(*this == rhs);
	}

	bool operator < ( const CGameID &rhs ) const
	{
		return ( m_ulGameID < rhs.m_ulGameID );
	}

	bool IsValid() const
	{
		// each type has it's own invalid fixed point:
		switch( m_gameID.m_nType )
		{
		case k_EGameIDTypeApp:
			return m_gameID.m_nAppID != k_uAppIdInvalid;

		case k_EGameIDTypeGameMod:
			return m_gameID.m_nAppID != k_uAppIdInvalid && m_gameID.m_nModID & 0x80000000;

		case k_EGameIDTypeShortcut:
			return (m_gameID.m_nModID & 0x80000000) != 0;

		case k_EGameIDTypeP2P:
			return m_gameID.m_nAppID == k_uAppIdInvalid && m_gameID.m_nModID & 0x80000000;

		default:
			return false;
		}

	}

	void Reset() 
	{
		m_ulGameID = 0;
	}

//
// Internal stuff.  Use the accessors above if possible
//

	enum EGameIDType
	{
		k_EGameIDTypeApp		= 0,
		k_EGameIDTypeGameMod	= 1,
		k_EGameIDTypeShortcut	= 2,
		k_EGameIDTypeP2P		= 3,
	};

	struct GameID_t
	{
#ifdef VALVE_BIG_ENDIAN
		unsigned int m_nModID : 32;
		unsigned int m_nType : 8;
		unsigned int m_nAppID : 24;
#else
		unsigned int m_nAppID : 24;
		unsigned int m_nType : 8;
		unsigned int m_nModID : 32;
#endif
	};

	union
	{
		uint64 m_ulGameID;
		GameID_t m_gameID;
	};
};

#pragma pack( pop )

//-----------------------------------------------------------------------------
// Purpose: Passed as argument to SteamAPI_UseBreakpadCrashHandler to enable optional callback
//  just before minidump file is captured after a crash has occurred.  (Allows app to append additional comment data to the dump, etc.)
//-----------------------------------------------------------------------------
typedef void (*PFNPreMinidumpCallback)(void *context);

enum EGameSearchErrorCode_t
{
	k_EGameSearchErrorCode_OK = 1,
	k_EGameSearchErrorCode_Failed_Search_Already_In_Progress = 2,
	k_EGameSearchErrorCode_Failed_No_Search_In_Progress = 3,
	k_EGameSearchErrorCode_Failed_Not_Lobby_Leader = 4, // if not the lobby leader can not call SearchForGameWithLobby
	k_EGameSearchErrorCode_Failed_No_Host_Available = 5, // no host is available that matches those search params
	k_EGameSearchErrorCode_Failed_Search_Params_Invalid = 6, // search params are invalid
	k_EGameSearchErrorCode_Failed_Offline = 7, // offline, could not communicate with server
	k_EGameSearchErrorCode_Failed_NotAuthorized = 8, // either the user or the application does not have priveledges to do this
	k_EGameSearchErrorCode_Failed_Unknown_Error = 9, // unknown error
};

enum EPlayerResult_t
{
	k_EPlayerResultFailedToConnect = 1, // failed to connect after confirming
	k_EPlayerResultAbandoned = 2,		// quit game without completing it
	k_EPlayerResultKicked = 3,			// kicked by other players/moderator/server rules
	k_EPlayerResultIncomplete = 4,		// player stayed to end but game did not conclude successfully ( nofault to player )
	k_EPlayerResultCompleted = 5,		// player completed game
};


enum ESteamIPv6ConnectivityProtocol
{
	k_ESteamIPv6ConnectivityProtocol_Invalid = 0,
	k_ESteamIPv6ConnectivityProtocol_HTTP = 1,		// because a proxy may make this different than other protocols
	k_ESteamIPv6ConnectivityProtocol_UDP = 2,		// test UDP connectivity. Uses a port that is commonly needed for other Steam stuff. If UDP works, TCP probably works. 
};

// For the above transport protocol, what do we think the local machine's connectivity to the internet over ipv6 is like
enum ESteamIPv6ConnectivityState
{
	k_ESteamIPv6ConnectivityState_Unknown = 0,	// We haven't run a test yet
	k_ESteamIPv6ConnectivityState_Good = 1,		// We have recently been able to make a request on ipv6 for the given protocol
	k_ESteamIPv6ConnectivityState_Bad = 2,		// We failed to make a request, either because this machine has no ipv6 address assigned, or it has no upstream connectivity
};


// Define compile time assert macros to let us validate the structure sizes.
#define VALVE_COMPILE_TIME_ASSERT( pred ) typedef char compile_time_assert_type[(pred) ? 1 : -1];

#if defined(__linux__) || defined(__APPLE__) 
// The 32-bit version of gcc has the alignment requirement for uint64 and double set to
// 4 meaning that even with #pragma pack(8) these types will only be four-byte aligned.
// The 64-bit version of gcc has the alignment requirement for these types set to
// 8 meaning that unless we use #pragma pack(4) our structures will get bigger.
// The 64-bit structure packing has to match the 32-bit structure packing for each platform.
#define VALVE_CALLBACK_PACK_SMALL
#else
#define VALVE_CALLBACK_PACK_LARGE
#endif

#if defined( VALVE_CALLBACK_PACK_SMALL )
#pragma pack( push, 4 )
#elif defined( VALVE_CALLBACK_PACK_LARGE )
#pragma pack( push, 8 )
#else
#error ???
#endif 

typedef struct ValvePackingSentinel_t
{
    uint32 m_u32;
    uint64 m_u64;
    uint16 m_u16;
    double m_d;
} ValvePackingSentinel_t;

#pragma pack( pop )


#if defined(VALVE_CALLBACK_PACK_SMALL)
VALVE_COMPILE_TIME_ASSERT( sizeof(ValvePackingSentinel_t) == 24 )
#elif defined(VALVE_CALLBACK_PACK_LARGE)
VALVE_COMPILE_TIME_ASSERT( sizeof(ValvePackingSentinel_t) == 32 )
#else
#error ???
#endif

#endif // STEAMCLIENTPUBLIC_H
