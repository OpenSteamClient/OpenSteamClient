namespace GameOverlayUI.IPC;

public enum EInputType : uint {
    KeyDown,
    KeyUp,
    MouseDown,
    MouseUp,
    MouseMove,

    //TODO: Cannot support smooth scrolling
    MouseScrollDown,
    MouseScrollUp,
}