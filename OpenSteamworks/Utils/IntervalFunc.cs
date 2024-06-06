
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace OpenSteamworks.Utils;

public class IntervalFunc
{
    private Timer? timer;

    /// <summary>
    /// Gets or sets the action that is called every interval
    /// </summary>
    public Action? Action { get; set; }

    /// <summary>
    /// Stops the timer after calling the function
    /// </summary>
    public bool StopAfterInvocation { get; set; }

    /// <summary>
    /// Is the timer being ran
    /// </summary>
    public bool Running => timer != null;

    private TimeSpan interval;
    public TimeSpan Interval {
        get => interval;
        set {
            RecreateTimer(value);
        }
    }

    /// <summary>
    /// Creates an IntervalFunc that is stopped
    /// </summary>
    public IntervalFunc(Action func) {
        this.Action = func;
    }

    /// <summary>
    /// Creates an IntervalFunc that is stopped
    /// </summary>
    public IntervalFunc(Action func, bool stopAfterInvocation = false)
    {
        this.Action = func;
        this.StopAfterInvocation = stopAfterInvocation;
    }

    /// <summary>
    /// Creates an IntervalFunc that is running
    /// </summary>
    public IntervalFunc(Action func, TimeSpan interval, bool stopAfterInvocation = false)
    {
        this.Action = func;
        this.StopAfterInvocation = stopAfterInvocation;
        RecreateTimer(interval);
    }

    [MemberNotNull(nameof(timer))]
    private void RecreateTimer(TimeSpan interval) {
        this.interval = interval;
        this.timer?.Dispose();
        this.timer = new(OnElapsed, null, System.TimeSpan.Zero, interval);
    }

    private void OnElapsed(object? state)
    {
        Action?.Invoke();
        if (StopAfterInvocation) {
            Stop();
        }
    }

    public void Start() {
        RecreateTimer(this.interval);
    }

    public void Stop() {
        this.timer?.Dispose();
        timer = null;
    }
}