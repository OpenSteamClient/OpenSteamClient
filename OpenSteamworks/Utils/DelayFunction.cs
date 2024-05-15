using System;
using System.Threading.Tasks;

namespace OpenSteamworks.Utils;

public class DelayFunction {
    private readonly Action func;
    private TimeSpan delay;
    public Task? Task { get; private set; }

    /// <summary>
    /// If the task is finished.
    /// Can be set to true while the function is waiting to stop the function from executing.
    /// </summary>
    public bool Finished { get; set; } = false;
    
    public event EventHandler? OnFinished;

    /// <summary>
    /// Creates a DelayFunction, but does not start it
    /// </summary>
    public DelayFunction(Action func) {
        this.func = func;
    }

    /// <summary>
    /// Creates a DelayFunction and starts it
    /// </summary>
    public DelayFunction(Action func, TimeSpan delay) {
        this.func = func;
        this.Start(delay);
    }

    /// <summary>
    /// (Re)starts this DelayFunction
    /// </summary>
    public void Start(TimeSpan delay) {
        this.delay = delay;
        this.Task = Task.Run(Main);
    }

    private async void Main() {
    NewDelay:
        var lastDelay = delay;
        await Task.Delay(lastDelay);
        if (delay != lastDelay) {
            // If the delay has changed, restart the wait
            goto NewDelay;
        }

        if (Finished) {
            // If the task has been cancelled, exit now
            return;
        }

        Finished = true;
        OnFinished?.Invoke(this, EventArgs.Empty);

        func.Invoke();
    }

    public void AddTime(TimeSpan timeToAdd) {
        delay = timeToAdd;
    }
}