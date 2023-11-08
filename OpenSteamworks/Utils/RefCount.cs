using System.Threading;

namespace OpenSteamworks.Utils;

/// <summary>
/// A reference count helper class.
/// Prevents the Count from going under 0.
/// </summary>
public class RefCount {
    public int Count { get; private set; } = 0;
    /// <summary>
    /// Call this function when the reference count should increase.
    /// </summary>
    /// <returns>If initialisation should be done</returns>
    public bool Increment() {
        Count++;
        return Count > 0;
    }

    /// <summary>
    /// Call this function when the reference count should decrease.
    /// </summary>
    /// <returns>If deconstrution should be done</returns>
    public bool Decrement() {
        bool shouldDeconstruct = false;
        if (Count - 1 < 1) {
            Count = 0;
            shouldDeconstruct = true;
        } else {
            Count--;
        }
        return shouldDeconstruct;
    }
}