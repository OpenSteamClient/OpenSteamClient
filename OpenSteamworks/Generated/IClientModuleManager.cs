//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;

namespace OpenSteamworks.Generated;

public unsafe interface IClientModuleManager
{
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret LoadModule();  // argc: 3, index: 1
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret UnloadModule();  // argc: 1, index: 2
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret CallFunction();  // argc: 8, index: 3
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret CallFunctionAsync();  // argc: 9, index: 4
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret PollResponseAsync();  // argc: 5, index: 5
    // WARNING: Argument count doesn't match argc! Remove this once this has been corrected!
    public unknown_ret SetProtonEnvironment();  // argc: 2, index: 6
}