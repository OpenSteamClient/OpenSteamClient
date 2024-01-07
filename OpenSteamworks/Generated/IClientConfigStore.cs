//==========================  AUTO-GENERATED FILE  ================================
//
// This file is partially auto-generated.
// If functions are removed, your changes to that function will be lost.
// Parameter types and names however are preserved if the function stays unchanged.
// Feel free to change parameters to be more accurate. 
//=============================================================================

using System;
using OpenSteamworks.Attributes;
using OpenSteamworks.Enums;


namespace OpenSteamworks.Generated;

public unsafe interface IClientConfigStore
{
    public bool IsSet( EConfigStore eConfigStore, string pszKeyNameIn );  // argc: 2, index: 1, ipc args: [bytes4, string], ipc returns: [boolean]
    // WARNING: Arguments are unknown!
    public bool GetBool( EConfigStore eConfigStore, string pszKeyNameIn, bool defaultValue );  // argc: 3, index: 2, ipc args: [bytes4, string, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public Int32 GetInt( EConfigStore eConfigStore, string pszKeyName, Int32 defaultValue );  // argc: 3, index: 3, ipc args: [bytes4, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public UInt64 GetUint64( EConfigStore eConfigStore, string pszKeyName, UInt64 defaultValue );  // argc: 4, index: 4, ipc args: [bytes4, string, bytes8], ipc returns: [bytes8]
    // WARNING: Arguments are unknown!
    public float GetFloat(EConfigStore eConfigStore, string pszKeyName, float defaultValue);  // argc: 3, index: 5, ipc args: [bytes4, string, bytes4], ipc returns: [bytes4]
    // WARNING: Arguments are unknown!
    public string GetString(EConfigStore eConfigStore, string pszKeyName, string defaultValue);  // argc: 3, index: 6, ipc args: [bytes4, string, string], ipc returns: [string]
    // WARNING: Arguments are unknown!
    public uint GetBinary(EConfigStore eConfigStore, string pszKeyName, IntPtr pubBuf, UInt32 cubBuf);  // argc: 4, index: 7, ipc args: [bytes4, string, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public uint GetBinary(EConfigStore eConfigStore, string pszKeyName, [IPCOut] CUtlBuffer* pUtlBuf);  // argc: 3, index: 8, ipc args: [bytes4, string], ipc returns: [bytes4, unknown]
    // WARNING: Arguments are unknown!
    public uint GetBinaryWatermarked(EConfigStore eConfigStore, string pszKeyName, IntPtr pubBuf, UInt32 cubBuf);  // argc: 4, index: 9, ipc args: [bytes4, string, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public bool SetBool(EConfigStore eConfigStore, string pszKeyNameIn, bool value);  // argc: 3, index: 10, ipc args: [bytes4, string, bytes1], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetInt(EConfigStore eConfigStore, string pszKeyNameIn, Int32 nValue);  // argc: 3, index: 11, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetUint64(EConfigStore eConfigStore, string pszKeyNameIn, UInt64 unValue);  // argc: 4, index: 12, ipc args: [bytes4, string, bytes8], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetFloat(EConfigStore eConfigStore, string pszKeyNameIn, float flValue);  // argc: 3, index: 13, ipc args: [bytes4, string, bytes4], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetString(EConfigStore eConfigStore, string pszKeyNameIn, string pszValue);  // argc: 3, index: 14, ipc args: [bytes4, string, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetBinary(EConfigStore eConfigStore, string pszKeyName, IntPtr pubData, UInt32 cubData);  // argc: 4, index: 15, ipc args: [bytes4, string, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool SetBinaryWatermarked(EConfigStore eConfigStore, string pszKeyName, IntPtr pubData, UInt32 cubData);  // argc: 4, index: 16, ipc args: [bytes4, string, bytes4, bytes_length_from_mem], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public bool RemoveKey(EConfigStore eConfigStore, string pszKeyName);  // argc: 2, index: 17, ipc args: [bytes4, string], ipc returns: [bytes1]
    // WARNING: Arguments are unknown!
    public uint GetKeySerialized(EConfigStore eConfigStore, string pszKeyNameIn, IntPtr pchBuffer, Int32 cbBufferMax);  // argc: 4, index: 18, ipc args: [bytes4, string, bytes4], ipc returns: [bytes4, bytes_length_from_reg]
    // WARNING: Arguments are unknown!
    public bool FlushToDisk(bool bIsShuttingDown);  // argc: 1, index: 19, ipc args: [bytes1], ipc returns: [bytes1]
}