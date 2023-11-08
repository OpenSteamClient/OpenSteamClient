using System;

namespace OpenSteamworks;

public class DefaultConsoleLogger : ILogger
{
    public void Debug(string message)
    {
        Console.WriteLine(message);
    }

    public void Debug(string message, params object?[] formatObjs)
    {
        Console.WriteLine(message, formatObjs);
    }

    public void Error(string message)
    {
        Console.WriteLine(message);
    }

    public void Error(string message, params object?[] formatObjs)
    {
        Console.WriteLine(message, formatObjs);
    }

    public void Error(Exception e)
    {
        Console.WriteLine(e);
    }

    public void Fatal(string message)
    {
        Console.WriteLine(message);
    }

    public void Fatal(string message, params object?[] formatObjs)
    {
        Console.WriteLine(message, formatObjs);
    }

    public void Fatal(Exception e)
    {
        Console.WriteLine(e);
    }

    public void Info(string message)
    {
        Console.WriteLine(message);
    }

    public void Info(string message, params object?[] formatObjs)
    {
        Console.WriteLine(message, formatObjs);
    }

    public void Warning(string message)
    {
        Console.WriteLine(message);
    }

    public void Warning(string message, params object?[] formatObjs)
    {
        Console.WriteLine(message, formatObjs);
    }

    public void Warning(Exception e)
    {
        Console.WriteLine(e);
    }
}