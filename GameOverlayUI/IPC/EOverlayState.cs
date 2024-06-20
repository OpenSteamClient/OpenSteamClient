namespace GameOverlayUI.IPC;

public enum EOverlayState : int {
    NotRunning = 0,

    /// <summary>
    /// Phase 1 of the overlay loop.
    /// </summary>
    ClientRequestLoopStart,

    /// <summary>
    /// Phase 2 of the overlay loop. The client will re-allocate the display's shared memory segment if the resolution has changed
    /// </summary>
    ServerRequestDisplayAllocation,

    /// <summary>
    /// Phase 3 of the overlay loop. The client will tell the server how big the input buffer is in MemoryResized or 0 if it is unchanged.
    /// </summary>
    ServerRequestInputData,

    /// <summary>
    /// Phase 4 of the overlay loop. Set by the server once rendering is done and the results are ready in memory to be displayed by the client.
    /// After the client finishes rendering, the client will reset the state to NotRunning or ClientRequestLoopStart
    /// </summary>
    RenderDataAvailable,
    

    ResponseAvailable,
}
