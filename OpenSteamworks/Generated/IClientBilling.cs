//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientBilling
{
    // WARNING: Arguments are unknown!
    public unknown_ret PurchaseWithActivationCode();  // argc: 1, index: 1, ipc args: [string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret HasActiveLicense();  // argc: 1, index: 2, ipc args: [bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public unknown_ret GetLicenseInfo();  // argc: 9, index: 3, ipc args: [bytes4], ipc returns: [bytes1, bytes4, bytes4, bytes4, bytes4, bytes4, bytes4, bytes4, bytes3]
    // WARNING: Arguments are unknown!
    public unknown_ret EnableTestLicense();  // argc: 1, index: 4, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret DisableTestLicense();  // argc: 1, index: 5, ipc args: [bytes4], ipc returns: []
    // WARNING: Arguments are unknown!
    public unknown_ret GetAppsInPackage();  // argc: 3, index: 6, ipc args: [bytes4, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public unknown_ret RequestFreeLicenseForApps();  // argc: 2, index: 7, ipc args: [bytes4, bytes_length_from_reg], ipc returns: [bytes8]
}