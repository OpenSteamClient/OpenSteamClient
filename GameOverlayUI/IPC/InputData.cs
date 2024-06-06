using System.Runtime.InteropServices;

namespace GameOverlayUI.IPC;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct InputData {
    [FieldOffset(0)]    
    public EInputType Type;

    // Mouse specific
    
    [FieldOffset(4)]
    public EMouseButton MouseButton;

    [FieldOffset(4)]
    public uint X;

    [FieldOffset(8)]
    public uint Y;

    // Keyboard specific

    [FieldOffset(4)]
    public uint KeyCode;
}