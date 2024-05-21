using System.Runtime.InteropServices;

namespace OpenSteamworks.ConCommands;

//-----------------------------------------------------------------------------
// Any executable that wants to use ConVars need to implement one of
// these to hook up access to console variables.
//-----------------------------------------------------------------------------
[StructLayout(LayoutKind.Sequential)]
public unsafe struct IConCommandBaseAccessor
{
    public delegate* unmanaged[Cdecl]<IConCommandBaseAccessor*, nint, byte>* accessorFunc;
};
