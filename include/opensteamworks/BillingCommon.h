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

#ifndef BILLINGCOMMON_H
#define BILLINGCOMMON_H
#ifdef _WIN32
#pragma once
#endif



#define CLIENTBILLING_INTERFACE_VERSION "CLIENTBILLING_INTERFACE_VERSION001"

#define STEAMBILLING_INTERFACE_VERSION_001 "SteamBilling001"
#define STEAMBILLING_INTERFACE_VERSION_002 "SteamBilling002"



// Flags for licenses - BITS
typedef enum ELicenseFlags
{
	k_ELicenseFlagNone = 0,
	k_ELicenseFlagRenew = 0x01,				// Renew this license next period
	k_ELicenseFlagRenewalFailed = 0x02,		// Auto-renew failed
	k_ELicenseFlagPending = 0x04,			// Purchase or renewal is pending
	k_ELicenseFlagExpired = 0x08,			// Regular expiration (no renewal attempted)
	k_ELicenseFlagCancelledByUser = 0x10,	// Cancelled by the user
	k_ELicenseFlagCancelledByAdmin = 0x20,	// Cancelled by customer support
	k_ELicenseFlagLowViolence = 0x40,
	k_ELicenseFlagImportedFromSteam2 = 0x80,
} ELicenseFlags;

// Payment methods for purchases
typedef enum EPaymentMethod
{
	k_EPaymentMethodNone = 0,
	k_EPaymentMethodActivationCode = 1,		
	k_EPaymentMethodCreditCard = 2,
	k_EPaymentMethodGiropay = 3,
	k_EPaymentMethodPayPal = 4,
	k_EPaymentMethodIdeal = 5,
	k_EPaymentMethodPaySafeCard = 6,
	k_EPaymentMethodSofort = 7,
	k_EPaymentMethodGuestPass = 8,
	k_EPaymentMethodWebMoney = 9,
	k_EPaymentMethodMoneyBookers = 10,
	k_EPaymentMethodAliPay = 11,
	k_EPaymentMethodYandex = 12,
	k_EPaymentMethodKiosk = 13,
	k_EPaymentMethodQIWI = 14,
	k_EPaymentMethodGameStop = 15,
	k_EPaymentMethodHardwarePromo = 16,
	k_EPaymentMethodMopay = 17,
	k_EPaymentMethodBoletoBancario = 18,
	k_EPaymentMethodBoaCompraGold = 19,
	k_EPaymentMethodBancoDoBrasilOnline = 20,
	k_EPaymentMethodItauOnline = 21,
	k_EPaymentMethodBradescoOnline = 22,
	k_EPaymentMethodPagseguro = 23,
	k_EPaymentMethodVisaBrazil = 24,
	k_EPaymentMethodAmexBrazil = 25,
	k_EPaymentMethodAura = 26,
	k_EPaymentMethodHipercard = 27,
	k_EPaymentMethodMastercardBrazil = 28,
	k_EPaymentMethodDinerSCardBrazil = 29,
	k_EPaymentMethodAuthorizedDevice = 30,
	k_EPaymentMethodMOLPoints = 31,
	k_EPaymentMethodClickAndBuy = 32,
	k_EPaymentMethodBeeline = 33,
	k_EPaymentMethodKonbini = 34,
	k_EPaymentMethodEClubPoints = 35,
	k_EPaymentMethodCreditCardJapan = 36,
	k_EPaymentMethodBankTransferJapan = 37,
	k_EPaymentMethodPayEasyJapan = 38,

	k_EPaymentMethodSteamPressMaster = 130,

	k_EPaymentMethodAutoGrant = 64,
	k_EPaymentMethodWallet = 128,
	k_EPaymentMethodValve = 129,
	k_EPaymentMethodOEMTicket = 256,
	k_EPaymentMethodSplit = 512,
	k_EPaymentMethodComplimentary = 1024,
} EPaymentMethod;

typedef enum EPurchaseResultDetail
{
	k_EPurchaseResultNoDetail = 0,
	k_EPurchaseResultAVSFailure = 1,
	k_EPurchaseResultInsufficientFunds = 2,
	k_EPurchaseResultContactSupport = 3,
	k_EPurchaseResultTimeout = 4,

	// these are mainly used for testing
	k_EPurchaseResultInvalidPackage = 5,
	k_EPurchaseResultInvalidPaymentMethod = 6,
	k_EPurchaseResultInvalidData = 7,
	k_EPurchaseResultOthersInProgress = 8,
	k_EPurchaseResultAlreadyPurchased = 9,
	k_EPurchaseResultWrongPrice = 10,

	k_EPurchaseResultFraudCheckFailed = 11,
	k_EPurchaseResultCancelledByUser = 12,
	k_EPurchaseResultRestrictedCountry = 13,
	k_EPurchaseResultBadActivationCode = 14,
	k_EPurchaseResultDuplicateActivationCode = 15,
	k_EPurchaseResultUseOtherPaymentMethod = 16,
	k_EPurchaseResultUseOtherFundingSource = 17,
	k_EPurchaseResultInvalidShippingAddress = 18,
	k_EPurchaseResultRegionNotSupported = 19,
	k_EPurchaseResultAcctIsBlocked = 20,
	k_EPurchaseResultAcctNotVerified = 21,
	k_EPurchaseResultInvalidAccount = 22,
	k_EPurchaseResultStoreBillingCountryMismatch = 23,
	k_EPurchaseResultDoesNotOwnRequiredApp = 24,
	k_EPurchaseResultCanceledByNewTransaction = 25,
	k_EPurchaseResultForceCanceledPending = 26,
	k_EPurchaseResultFailCurrencyTransProvider = 27,
	k_EPurchaseResultFailedCyberCafe = 28,
	k_EPurchaseResultNeedsPreApproval = 29,
	k_EPurchaseResultPreApprovalDenied = 30,
	k_EPurchaseResultWalletCurrencyMismatch = 31,
	k_EPurchaseResultEmailNotValidated = 32,
	k_EPurchaseResultExpiredCard = 33,
	k_EPurchaseResultTransactionExpired = 34,
	k_EPurchaseResultWouldExceedMaxWallet = 35,
	k_EPurchaseResultMustLoginPS3AppForPurchase = 36,
	k_EPurchaseResultCannotShipToPOBox = 37,
} EPurchaseResultDetail;

typedef enum EPurchaseStatus
{
	k_EPurchasePending = 0,
	k_EPurchaseSucceeded = 1,
	k_EPurchaseFailed = 2,
	k_EPurchaseRefunded = 3,
	k_EPurchaseInit = 4,
	k_EPurchaseChargedback = 5,
	k_EPurchaseRevoked = 6,
	k_EPurchaseInDispute = 7,
	k_EPurchasePartialRefund = 8,
	k_EPurchaseRefundToWallet = 9,
} EPurchaseStatus;

typedef enum ECreditCardType
{
	k_ECreditCardTypeUnknown = 0,
	k_ECreditCardTypeVisa = 1,
	k_ECreditCardTypeMaster = 2,
	k_ECreditCardTypeAmericanExpress = 3,
	k_ECreditCardTypeDiscover = 4,
	k_ECreditCardTypeDinersClub = 5,
	k_ECreditCardTypeJCB = 6,
	k_ECreditCardTypeCarteBleue = 7,
	k_ECreditCardTypeDankort = 8,
	k_ECreditCardTypeMaestro = 9,
	k_ECreditCardTypeSolo = 10,
	k_ECreditCardTypeLaser = 11,
} ECreditCardType;

enum ELicenseType
{
	k_ENoLicense = 0,
	k_ESinglePurchase = 1,
	k_ESinglePurchaseLimitedUse = 2,
	k_ERecurringCharge = 3,
	k_ERecurringChargeLimitedUse = 4,
	k_ERecurringChargeLimitedUseWithOverages = 5,
};


#pragma pack( push, 8 )
//-----------------------------------------------------------------------------
// Purpose: called when this client has received a finalprice message from a Billing
//-----------------------------------------------------------------------------
struct OBSOLETE_CALLBACK FinalPriceMsg_t
{
		enum { k_iCallback = k_iSteamBillingCallbacks + 1 };

		uint32 m_bSuccess;
		uint32 m_nBaseCost;
		uint32 m_nTotalDiscount;
		uint32 m_nTax;
		uint32 m_nShippingCost;
};

struct OBSOLETE_CALLBACK PurchaseMsg_t
{
		enum { k_iCallback = k_iSteamBillingCallbacks + 2 };

		uint32 m_bSuccess;
		int32 m_EPurchaseResultDetail;			// Detailed result information
};

// Sent in response to PurchaseWithActivationCode
struct PurchaseResponse_t
{
	enum { k_iCallback = k_iSteamBillingCallbacks + 4 };
	
	EResult m_EResult;
	int32 m_EPurchaseResultDetail;
	int32 m_iReceiptIndex;
};

struct CancelLicenseMsg_t
{
	enum { k_iCallback = k_iSteamBillingCallbacks + 9 };

	enum EResult m_EResult;
};

struct RequestFreeLicenseResponse_t
{
	enum { k_iCallback = k_iSteamBillingCallbacks + 12 };

	EResult m_EResult;
	AppId_t m_uAppId;
};

struct OEMTicketActivationResponse_t
{
	enum { k_iCallback = k_iSteamBillingCallbacks + 14 };

	EResult m_EResult;
	EPurchaseResultDetail m_EPurchaseResultDetail;
	PackageId_t m_nPackageID;
	int m_iReceiptIndex;
};
#pragma pack( pop )


#endif // BILLINGCOMMON_H
