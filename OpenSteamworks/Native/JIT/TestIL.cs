using System;

namespace OpenSteamworks.Native.JIT;

// Scratch pad for testing what kind of IL gets emitted for various things.
// Write testable code here, then open with ILSpy or AvaloniaILSpy.
public unsafe class TestIL {
    struct Fields {
        public void* S_VT;
        public uint S_unk;
    }
    
    public IntPtr ObjectPointer;
    public IntPtr VTPointer;

    public uint unk {
        get => ((Fields*)ObjectPointer)->S_unk;
        set => ((Fields*)ObjectPointer)->S_unk = value;
    }

    public unsafe HSteamPipe CreateSteamPipe() {
        var profile = InteropHelp.StartProfile("CreateSteamPipe");
        HSteamPipe ret = ((delegate* unmanaged[Thiscall]<nint, int>)InteropHelp.LoadVTPtr(VTPointer, 1))(ObjectPointer);
        InteropHelp.EndProfile(profile);
        return ret;
    }
}