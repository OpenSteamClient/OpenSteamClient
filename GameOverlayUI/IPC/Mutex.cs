using GameOverlayUI.Platform;

namespace GameOverlayUI.IPC;

//TODO: Windows version with WaitOnAddress (length set to 4 bytes, get threadid pointer)
public unsafe struct OverlayMutex {
    /// <summary>
    /// Who currently has the mutex?
    /// This should be a thread ID.
    /// The important bit is to interlockedly check for 0, and acquire if 0.
    /// </summary>
    public int ThreadID;
    public robust_list robust_list;
}