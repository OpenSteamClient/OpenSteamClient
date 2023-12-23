using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Google.Protobuf;
using OpenSteamworks.Protobuf;
using ValveKeyValue;

public class Program
{
    public static void Main(string[] args)
    {
        if (args.Length < 3) {
            PrintHelp();
            return;
        }

        if (args.Length > 3) {
            Console.WriteLine("Too many arguments");
            return;
        }

        byte[] bytes;
        switch (args[0])
        {
            case "f":
                if (!File.Exists(args[1])) {
                    Console.WriteLine($"filepath ({args[1]}) does not exist");
                    return;
                }

                bytes = File.ReadAllBytes(args[1]);
                break;

            case "h":
                bytes = Convert.FromHexString(args[1]);
                break;

            default:
                Console.WriteLine("Invalid mode '" + args[0] + "'");
                return;
        }

        int len = bytes.Length;

        var type = GetTypeFromString(args[2]);
        dynamic parser = typeof(Program).GetMethod(nameof(CreateParser), BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)!.MakeGenericMethod(type).Invoke(null, null)!;
        // This doesn't work when invoking. Why?
        //var parseFrom = typeof(MessageParser<>).GetMethod("ParseFrom", BindingFlags.Instance | BindingFlags.Public, new Type[1] { typeof(byte[]) })!;

        for (int i = 0; i < len; i++)
        {
            var trimmed = bytes[i..];
            try
            {
                var result = parser.ParseFrom(trimmed);
                Console.WriteLine($"Succeeded ({i}/{len})");
                Console.WriteLine(result == null ? "null" : result.ToString());
            }
            catch (System.Exception e)
            {
                Console.WriteLine($"Failed ({i}/{len}): " + e.Message);
            }
        }
    }
    
    private static Assembly GetOSWProtoAssembly()
    {
        AppDomain.CurrentDomain.Load("OpenSteamworks.Protobuf");
        var protoAssembly = AppDomain.CurrentDomain.GetAssemblies().
            SingleOrDefault(assembly => assembly.GetName().Name == "OpenSteamworks.Protobuf");
        
        if (protoAssembly == null) {
            throw new NullReferenceException("protoAssembly is null. Loaded assemblies: " + string.Join(",", (IEnumerable<Assembly>)AppDomain.CurrentDomain.GetAssemblies()));
        }

        return protoAssembly;
    }

    private static Type GetTypeFromString(string name) {
        var assembly = GetOSWProtoAssembly();
        foreach (var item in assembly.GetTypes())
        {
            if (item.Name == name) {
                return item;
            }
        }

        throw new InvalidOperationException("Couldn't find " + name);
    }

    private static MessageParser<T> CreateParser<T>() where T: IMessage<T>, new() {
        return new MessageParser<T>(() => new T());
    }

    private static void PrintHelp() {
        Console.WriteLine("Needs three arguments: mode, [filepath, hex string], classname");
        Console.WriteLine("Valid modes: ");
        Console.WriteLine("f: Find in file");
        Console.WriteLine("h: Find in hex");
    }
}