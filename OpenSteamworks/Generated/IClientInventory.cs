//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientInventory
{
    // WARNING: Arguments are unknown!
    public unknown_ret GetResultStatus();  // argc: 1, index: 1, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DestroyResult();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetResultItems();  // argc: 4, index: 3, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_reg, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetResultItemProperty();  // argc: 6, index: 4, ipc args: [bytes4, bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetResultTimestamp();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret CheckResultSteamID();  // argc: 3, index: 6, ipc args: [bytes4, uint64], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SerializeResult();  // argc: 4, index: 7, ipc args: [bytes4, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret DeserializeResult();  // argc: 4, index: 8, ipc args: [bytes4, bytes1, bytes_length_from_mem], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetAllItems();  // argc: 1, index: 9, ipc args: [], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemsByID();  // argc: 3, index: 10, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GenerateItems();  // argc: 5, index: 11, ipc args: [bytes4, bytes4, bytes_length_from_reg, bytes_length_from_reg], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret AddPromoItems();  // argc: 3, index: 12, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ConsumeItem();  // argc: 4, index: 13, ipc args: [bytes8, bytes4], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret ExchangeItems();  // argc: 9, index: 14, ipc args: [bytes4, bytes4, bytes4, bytes4, bytes_length_from_reg, bytes_length_from_reg, bytes_length_from_reg, bytes_length_from_reg], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TransferItemQuantity();  // argc: 6, index: 15, ipc args: [bytes8, bytes4, bytes8], ipc returns: [bytes1, bytes4]
    public unknown_ret SendItemDropHeartbeat();  // argc: 0, index: 16, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret TriggerItemDrop();  // argc: 2, index: 17, ipc args: [bytes4], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret TradeItems();  // argc: 11, index: 18, ipc args: [uint64, bytes4, bytes4, bytes4, bytes4, bytes_length_from_reg, bytes_length_from_reg, bytes_length_from_reg, bytes_length_from_reg], ipc returns: [bytes1, bytes4]
    public unknown_ret LoadItemDefinitions();  // argc: 0, index: 19, ipc args: [], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemDefinitionIDs();  // argc: 3, index: 20, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_reg, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemDefinitionProperty();  // argc: 5, index: 21, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1, bytes_length_from_mem, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestEligiblePromoItemDefinitionsIDs();  // argc: 2, index: 22, ipc args: [uint64], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret GetEligiblePromoItemDefinitionIDs();  // argc: 5, index: 23, ipc args: [uint64, bytes4], ipc returns: [bytes1, bytes_length_from_reg, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret StartPurchase();  // argc: 4, index: 24, ipc args: [bytes4, bytes4, bytes_length_from_reg, bytes_length_from_reg], ipc returns: [bytes8]
    public unknown_ret RequestPrices();  // argc: 0, index: 25, ipc args: [], ipc returns: [bytes8]
    public unknown_ret GetNumItemsWithPrices();  // argc: 0, index: 26, ipc args: [], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemsWithPrices();  // argc: 4, index: 27, ipc args: [bytes4], ipc returns: [bytes1, bytes_length_from_reg, bytes_length_from_reg, bytes_length_from_mem]
    // WARNING: Arguments are unknown!
    public unknown_ret GetItemPrice();  // argc: 3, index: 28, ipc args: [bytes4], ipc returns: [bytes1, bytes8, bytes8]
    public unknown_ret StartUpdateProperties();  // argc: 0, index: 29, ipc args: [], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public unknown_ret RemoveProperty();  // argc: 5, index: 30, ipc args: [bytes8, bytes8, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetProperty(bool unk1);  // argc: 6, index: 31, ipc args: [bytes8, bytes8, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SetProperty(double unk1, bool unk2);  // argc: 6, index: 32, ipc args: [bytes8, bytes8, string, bytes1], ipc returns: [bytes1]
    // WARNING: Do not use this function! Unknown behaviour will occur!
    public unknown_ret Unknown_32_DONTUSE();  // argc: -1, index: 33, ipc args: [], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret SetProperty(int unk1, bool unk2, double unk3);  // argc: 6, index: 34, ipc args: [bytes8, bytes8, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret SubmitUpdateProperties();  // argc: 3, index: 35, ipc args: [bytes8], ipc returns: [bytes1, bytes4]
    // WARNING: Arguments are unknown!
    public unknown_ret InspectItem();  // argc: 2, index: 36, ipc args: [string], ipc returns: [bytes1, bytes4]
}