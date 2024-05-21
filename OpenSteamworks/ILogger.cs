using System;

namespace OpenSteamworks;

public interface ILogger {
    /// <summary>
    /// Write a (partial) message without a newline.
    /// </summary>
    public void Write(string message);
    public void Debug(string message, params object?[] formatObjs);
    public void Debug(string message);
    public void Info(string message, params object?[] formatObjs);
    public void Info(string message);
    public void Warning(string message, params object?[] formatObjs);
    public void Warning(string message);
    public void Warning(Exception e);
    public void Error(string message, params object?[] formatObjs);
    public void Error(string message);
    public void Error(Exception e);
    public void Fatal(string message, params object?[] formatObjs);
    public void Fatal(string message);
    public void Fatal(Exception e);
}