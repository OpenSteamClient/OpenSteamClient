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

#ifndef ICLIENTINVENTORY_H
#define ICLIENTINVENTORY_H
#ifdef _WIN32
#pragma once
#endif

#include "SteamTypes.h"
#include "InventoryCommon.h"

abstract_class IClientInventory
{
public:
    // INVENTORY ASYNC RESULT MANAGEMENT
    //
    // Asynchronous inventory queries always output a result handle which can be used with
    // GetResultStatus, GetResultItems, etc. A SteamInventoryResultReady_t callback will
    // be triggered when the asynchronous result becomes ready (or fails).
    //
    
    // Find out the status of an asynchronous inventory result handle. Possible values:
    //  k_EResultPending - still in progress
    //  k_EResultOK - done, result ready
    //  k_EResultExpired - done, result ready, maybe out of date (see DeserializeResult)
    //  k_EResultInvalidParam - ERROR: invalid API call parameters
    //  k_EResultServiceUnavailable - ERROR: service temporarily down, you may retry later
    //  k_EResultLimitExceeded - ERROR: operation would exceed per-user inventory limits
    //  k_EResultFail - ERROR: unknown / generic error
    virtual EResult GetResultStatus(SteamInventoryResult_t resultHandle) = 0; //argc: 1, index 1
    
    // Destroys a result handle and frees all associated memory.
    virtual void DestroyResult(SteamInventoryResult_t resultHandle) = 0; //argc: 1, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetResultItems() = 0; //argc: 4, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetResultItemProperty() = 0; //argc: 6, index 4
    
    // Copies the contents of a result set into a flat array. The specific
    // contents of the result set depend on which query which was used.
    virtual bool GetResultItems(SteamInventoryResult_t resultHandle,
    SteamItemDetails_t *pOutItemsArray,
    uint32 *punOutItemsArraySize) = 0;
    
    // Returns the server time at which the result was generated. Compare against
    // the value of IClientUtils::GetServerRealTime() to determine age.
    virtual uint32 GetResultTimestamp(SteamInventoryResult_t resultHandle) = 0; //argc: 1, index 5
    
    // Returns true if the result belongs to the target steam ID, false if the
    // result does not. This is important when using DeserializeResult, to verify
    // that a remote player is not pretending to have a different user's inventory.
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool CheckResultSteamID(SteamInventoryResult_t resultHandle, CSteamID steamIDExpected) = 0; //argc: 3, index 6
    
    
    // RESULT SERIALIZATION AND AUTHENTICATION
    //
    // Serialized result sets contain a short signature which can't be forged
    // or replayed across different game sessions. A result set can be serialized
    // on the local client, transmitted to other players via your game networking,
    // and deserialized by the remote players. This is a secure way of preventing
    // hackers from lying about posessing rare/high-value items.
    
    // Serializes a result set with signature bytes to an output buffer. Pass
    // NULL as an output buffer to get the required size via punOutBufferSize.
    // The size of a serialized result depends on the number items which are being
    // serialized. When securely transmitting items to other players, it is
    // recommended to use "GetItemsByID" first to create a minimal result set.
    // Results have a built-in timestamp which will be considered "expired" after
    // an hour has elapsed. See DeserializeResult for expiration handling.
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool SerializeResult(SteamInventoryResult_t resultHandle, void *pOutBuffer, uint32 *punOutBufferSize) = 0; //argc: 4, index 7
    
    // Deserializes a result set and verifies the signature bytes. Returns false
    // if bRequireFullOnlineVerify is set but Steam is running in Offline mode.
    // Otherwise returns true and then delivers error codes via GetResultStatus.
    //
    // The bRESERVED_MUST_BE_FALSE flag is reserved for future use and should not
    // be set to true by your game at this time.
    //
    // DeserializeResult has a potential soft-failure mode where the handle status
    // is set to k_EResultExpired. GetResultItems() still succeeds in this mode.
    // The "expired" result could indicate that the data may be out of date - not
    // just due to timed expiration (one hour), but also because one of the items
    // in the result set may have been traded or consumed since the result set was
    // generated. You could compare the timestamp from GetResultTimestamp() to
    // ISteamUtils::GetServerRealTime() to determine how old the data is. You could
    // simply ignore the "expired" result code and continue as normal, or you
    // could challenge the player with expired data to send an updated result set.
    virtual bool DeserializeResult(SteamInventoryResult_t *pOutResultHandle, const void *pBuffer, uint32 unBufferSize, bool bRESERVED_MUST_BE_FALSE = false) = 0; //argc: 4, index 8
    
    
    // INVENTORY ASYNC QUERY
    //
    
    // Captures the entire state of the current user's Steam inventory.
    // You must call DestroyResult on this handle when you are done with it.
    // Returns false and sets *pResultHandle to zero if inventory is unavailable.
    // Note: calls to this function are subject to rate limits and may return
    // cached results if called too frequently. It is suggested that you call
    // this function only when you are about to display the user's full inventory,
    // or if you expect that the inventory may have changed.
    virtual bool GetAllItems(SteamInventoryResult_t *pResultHandle) = 0; //argc: 1, index 9
    
    
    // Captures the state of a subset of the current user's Steam inventory,
    // identified by an array of item instance IDs. The results from this call
    // can be serialized and passed to other players to "prove" that the current
    // user owns specific items, without exposing the user's entire inventory.
    // For example, you could call GetItemsByID with the IDs of the user's
    // currently equipped cosmetic items and serialize this to a buffer, and
    // then transmit this buffer to other players upon joining a game.
    virtual bool GetItemsByID(SteamInventoryResult_t *pResultHandle, const SteamItemInstanceID_t *pInstanceIDs, uint32 unCountInstanceIDs) = 0; //argc: 3, index 10
    
    
    // INVENTORY ASYNC MODIFICATION
    //
    
    // GenerateItems() creates one or more items and then generates a SteamInventoryCallback_t
    // notification with a matching nCallbackContext parameter. This API is insecure, and could
    // be abused by hacked clients. It is, however, very useful as a development cheat or as
    // a means of prototyping item-related features for your game. The use of GenerateItems can
    // be restricted to certain item definitions or fully blocked via the Steamworks website.
    // If punArrayQuantity is not NULL, it should be the same length as pArrayItems and should
    // describe the quantity of each item to generate.
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool GenerateItems(SteamInventoryResult_t *pResultHandle, const SteamItemDef_t *pArrayItemDefs, const uint32 *punArrayQuantity, uint32 unArrayLength) = 0; //argc: 5, index 11
    virtual bool AddPromoItems(SteamInventoryResult_t *pResultHandle, const SteamItemDef_t *pArrayItemDefs, uint32 unArrayLength) = 0; //argc: 3, index 12
    
    // ConsumeItem() removes items from the inventory, permenantly. They cannot be recovered.
    // Not for the faint of heart - if your game implements item removal at all, a high-friction
    // UI confirmation process is highly recommended. Similar to GenerateItems, punArrayQuantity
    // can be NULL or else an array of the same length as pArrayItems which describe the quantity
    // of each item to destroy. ConsumeItem can be restricted to certain item definitions or
    // fully blocked via the Steamworks website to minimize support/abuse issues such as the
    // clasic "my brother borrowed my laptop and deleted all of my rare items".
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool ConsumeItem(SteamInventoryResult_t *pResultHandle, SteamItemInstanceID_t itemConsume, uint32 unQuantity) = 0; //argc: 4, index 13
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret ExchangeItems() = 0; //argc: 9, index 14
    
    // ExchangeItems() is an atomic combination of GenerateItems and DestroyItems. It can be
    // used to implement crafting recipes or transmutations, or items which unpack themselves
    // into other items. Like GenerateItems, this is a flexible and dangerous API which is
    // meant for rapid prototyping. You can configure restrictions on ExchangeItems via the
    // Steamworks website, such as limiting it to a whitelist of input/output combinations
    // corresponding to recipes.
    // (Note: although GenerateItems may be hard or impossible to use securely in your game,
    // ExchangeItems is perfectly reasonable to use once the whitelists are set accordingly.)
    virtual bool ExchangeItems(SteamInventoryResult_t *pResultHandle,
    const SteamItemDef_t *pArrayGenerate, const uint32 *punArrayGenerateQuantity, uint32 unArrayGenerateLength,
    const SteamItemInstanceID_t *pArrayDestroy, const uint32 *punArrayDestroyQuantity, uint32 unArrayDestroyLength) = 0;
    
    
    // TransferItemQuantity() is intended for use with items which are "stackable" (can have
    // quantity greater than one). It can be used to split a stack into two, or to transfer
    // quantity from one stack into another stack of identical items. To split one stack into
    // two, pass k_SteamItemInstanceIDInvalid for itemIdDest and a new item will be generated.
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual bool TransferItemQuantity(SteamInventoryResult_t *pResultHandle, SteamItemInstanceID_t itemIdSource, uint32 unQuantity, SteamItemInstanceID_t itemIdDest) = 0; //argc: 6, index 15
    
    
    // TIMED DROPS AND PLAYTIME CREDIT
    //
    
    // Applications which use timed-drop mechanics should call SendItemDropHeartbeat() when
    // active gameplay begins, and at least once every two minutes afterwards. The backend
    // performs its own time calculations, so the precise timing of the heartbeat is not
    // critical as long as you send at least one heartbeat every two minutes. Calling the
    // function more often than that is not harmful, it will simply have no effect. Note:
    // players may be able to spoof this message by hacking their client, so you should not
    // attempt to use this as a mechanism to restrict playtime credits. It is simply meant
    // to distinguish between being in any kind of gameplay situation vs the main menu or
    // a pre-game launcher window. (If you are stingy with handing out playtime credit, it
    // will only encourage players to run bots or use mouse/kb event simulators.)
    //
    // Playtime credit accumulation can be capped on a daily or weekly basis through your
    // Steamworks configuration.
    //
    virtual void SendItemDropHeartbeat() = 0; //argc: 0, index 16
    
    // Playtime credit must be consumed and turned into item drops by your game. Only item
    // definitions which are marked as "playtime item generators" can be spawned. The call
    // will return an empty result set if there is not enough playtime credit for a drop.
    // Your game should call TriggerItemDrop at an appropriate time for the user to receive
    // new items, such as between rounds or while the player is dead. Note that players who
    // hack their clients could modify the value of "dropListDefinition", so do not use it
    // to directly control rarity. It is primarily useful during testing and development,
    // where you may wish to perform experiments with different types of drops.
    virtual bool TriggerItemDrop(SteamInventoryResult_t *pResultHandle, SteamItemDef_t dropListDefinition) = 0; //argc: 2, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret TradeItems() = 0; //argc: 11, index 1
    
    
    // IN-GAME TRADING
    //
    // TradeItems() implements limited in-game trading of items, if you prefer not to use
    // the overlay or an in-game web browser to perform Steam Trading through the website.
    // You should implement a UI where both players can see and agree to a trade, and then
    // each client should call TradeItems simultaneously (+/- 5 seconds) with matching
    // (but reversed) parameters. The result is the same as if both players performed a
    // Steam Trading transaction through the web. Each player will get an inventory result
    // confirming the removal or quantity changes of the items given away, and the new
    // item instance id numbers and quantities of the received items.
    // (Note: new item instance IDs are generated whenever an item changes ownership.)
    virtual bool TradeItems(SteamInventoryResult_t *pResultHandle, CSteamID steamIDTradePartner,
    const SteamItemInstanceID_t *pArrayGive, const uint32 *pArrayGiveQuantity, uint32 nArrayGiveLength,
    const SteamItemInstanceID_t *pArrayGet, const uint32 *pArrayGetQuantity, uint32 nArrayGetLength) = 0;
    
    
    // ITEM DEFINITIONS
    //
    // Item definitions are a mapping of "definition IDs" (integers between 1 and 1000000)
    // to a set of string properties. Some of these properties are required to display items
    // on the Steam community web site. Other properties can be defined by applications.
    // Use of these functions is optional; there is no reason to call LoadItemDefinitions
    // if your game hardcodes the numeric definition IDs (eg, purple face mask = 20, blue
    // weapon mod = 55) and does not allow for adding new item types without a client patch.
    //
    
    // LoadItemDefinitions triggers the automatic load and refresh of item definitions.
    // Every time new item definitions are available (eg, from the dynamic addition of new
    // item types while players are still in-game), a SteamInventoryDefinitionUpdate_t
    // callback will be fired.
    virtual bool LoadItemDefinitions() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemDefinitionIDs() = 0; //argc: 3, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemDefinitionProperty() = 0; //argc: 5, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RequestEligiblePromoItemDefinitionsIDs() = 0; //argc: 2, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetEligiblePromoItemDefinitionIDs() = 0; //argc: 5, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret StartPurchase() = 0; //argc: 4, index 4
    virtual unknown_ret RequestPrices() = 0; //argc: 0, index 5
    virtual unknown_ret GetNumItemsWithPrices() = 0; //argc: 0, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemsWithPrices() = 0; //argc: 4, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret GetItemPrice() = 0; //argc: 3, index 1
    virtual unknown_ret StartUpdateProperties() = 0; //argc: 0, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret RemoveProperty() = 0; //argc: 5, index 0
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetProperty() = 0; //argc: 6, index 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetProperty() = 0; //argc: 6, index 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    // WARNING: Do not use this function! Unknown behaviour will occur!
    virtual unknown_ret Unknown_32_DONTUSE() = 0; //argc: -1, index 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SetProperty() = 0; //argc: 6, index 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret SubmitUpdateProperties() = 0; //argc: 3, index 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    virtual unknown_ret InspectItem() = 0; //argc: 2, index 6
};

#endif // ICLIENTINVENTORY_H