using System;

namespace OpenSteamworks.Utils;

public abstract class IncrementingBase<T> {
    public abstract T Data { get; set; }
    public abstract int Length { get; }
    public uint UIntLength => (uint)Length;

    public abstract T Allocate(int size);

    /// <summary>
    /// Runs a function until our buffer fits it's output. <br/>
    /// </summary>
    /// <param name="func"></param>
    public int RunUntilFits(Func<int> func) {
        int lastResult;
        while (true)
        {
            lastResult = func();

            if (lastResult == 0) {
                return 0;
            }

            if (lastResult < Length) {
                break;
            }

            if (lastResult == Length) {
                Data = Allocate(Length * 2);
            }
        }

        return lastResult;
    }

    /// <summary>
    /// Runs a function until our buffer fits it's output. <br/>
    /// </summary>
    /// <param name="func"></param>
    public uint RunUntilFits(Func<uint> func) {
        uint lastResult;
        while (true)
        {
            lastResult = func();

            if (lastResult == 0) {
                return 0;
            }

            if (lastResult < Length) {
                break;
            }

            if (lastResult == Length) {
                Data = Allocate(Length * 2);
            }
        }

        return lastResult;
    }

    /// <summary>
    /// Runs a function and resizes our buffer to it's outputted length.
    /// </summary>
    /// <param name="func"></param>
    public int RunToFit(Func<int> func) {
        int lastResult;
        while (true)
        {
            lastResult = func();

            if (lastResult == 0) {
                return 0;
            }
            
            if (Length >= lastResult) {
                break;
            }

            if (lastResult > Length) {
                Data = Allocate(lastResult);
            }
        }

        return lastResult;
    }

    /// <summary>
    /// Runs a function and resizes our buffer to it's outputted length.
    /// </summary>
    /// <param name="func"></param>
    public uint RunToFit(Func<uint> func) {
        uint lastResult;
        while (true)
        {
            lastResult = func();

            if (lastResult == 0) {
                return 0;
            }
            
            if (Length >= lastResult) {
                break;
            }

            if (lastResult > Length) {
                checked
                {
                    Data = Allocate((int)lastResult);
                }
            }
        }

        return lastResult;
    }
}