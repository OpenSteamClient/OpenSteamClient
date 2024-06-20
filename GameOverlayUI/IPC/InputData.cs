using System.Runtime.InteropServices;
using Avalonia.Input;

namespace GameOverlayUI.IPC;

[StructLayout(LayoutKind.Explicit)]
public unsafe struct InputData {
    [FieldOffset(0)]    
    public EInputType Type;

    [FieldOffset(4)]
    public RawInputModifiers Modifiers;

    // Mouse specific

    [FieldOffset(8)]
    public MouseButton MouseButton;

    [FieldOffset(12)]
    public uint X;

    [FieldOffset(16)]
    public uint Y;

    // Keyboard specific

    [FieldOffset(8)]
    public Key Key;

    [FieldOffset(12)]
    public PhysicalKey PhysicalKey;

    public InputData(EInputType type, RawInputModifiers modifiers) {
        this.Type = type;
        this.Modifiers = modifiers;
    }

    public InputData(RawInputModifiers modifiers, uint x, uint y) : this(EInputType.MouseMove, modifiers) {
        this.X = x;
        this.Y = y;
    }

    public InputData(RawInputModifiers modifiers, uint x, uint y, MouseButton button, bool down = true) : this(modifiers, x, y) {
        this.Type = down ? EInputType.MouseDown : EInputType.MouseUp;
        this.MouseButton = button;
    }

    public InputData(RawInputModifiers modifiers, Key key, PhysicalKey physicalKey, bool down = true) : this(EInputType.KeyDown, modifiers) {
        this.Type = down ? EInputType.KeyDown : EInputType.KeyUp;
        this.Key = key;
        this.PhysicalKey = physicalKey;
    }
}