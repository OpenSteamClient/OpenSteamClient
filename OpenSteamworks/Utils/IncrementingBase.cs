using System;

namespace OpenSteamworks.Utils;

public abstract class IncrementingBase<T> {
    public abstract T Data { get; set; }
    public abstract int Length { get; }
    public abstract T Allocate(int size);

    /// <summary>
    /// Runs a function until our buffer fits it's output. <br/>
    /// </summary>
    /// <param name="func"></param>
    public void RunUntilFits(Func<int> func) {
        int lastResult;
        while (true)
        {
            lastResult = func();

            if (lastResult == 0) {
                throw new ZeroLengthResultException();
            }

            if (lastResult < Length) {
                break;
            }

            if (lastResult == Length) {
                Data = Allocate(Length * 2);
            }
        }
    }

    /// <summary>
    /// Runs a function and resizes our buffer to it's outputted length.
    /// </summary>
    /// <param name="func"></param>
    public void RunToFit(Func<int> func) {
        int lastResult;
        while (true)
        {
            lastResult = func();

            if (lastResult == 0) {
                throw new ZeroLengthResultException();
            }
            
            if (Length >= lastResult) {
                break;
            }

            if (lastResult > Length) {
                Data = Allocate(lastResult);
            }
        }
    }
}