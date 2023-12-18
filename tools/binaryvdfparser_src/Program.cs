using System;
using System.Collections.Generic;
using System.IO;
using ValveKeyValue;

public class Program
{
    public static KVSerializer kvt = KVSerializer.Create(KVSerializationFormat.KeyValues1Text);
    public static KVSerializer kvb = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);
    public static void Main(string[] args)
    {
        if (args.Length < 2) {
            PrintHelp();
            return;
        }

        if (args.Length > 2) {
            Console.WriteLine("Too many arguments");
            return;
        }

        switch (args[0])
        {
            case "f":
                if (!File.Exists(args[1])) {
                    Console.WriteLine($"filepath ({args[1]}) does not exist");
                    return;
                }

                using (var stream = File.OpenRead(args[1])) {
                    PrintKV(stream);
                }
                break;

            case "h":
                byte[] bytes = Convert.FromHexString(args[1]);
                using (var stream = new MemoryStream(bytes)) {
                    PrintKV(stream);
                }
                break;

            default:
                Console.WriteLine("Invalid mode '" + args[0] + "'");
                break;
        }
    }

    private static void PrintKV(Stream stream) {
        KVObject data = kvb.Deserialize(stream);
        using var ms = new MemoryStream();
        kvt.Serialize(ms, data);
        Console.WriteLine(System.Text.Encoding.UTF8.GetString(ms.ToArray()));
    }

    private static void PrintHelp() {
        Console.WriteLine("Needs two arguments: mode, [filepath, hex string]");
        Console.WriteLine("Valid modes: ");
        Console.WriteLine("f: Display from file");
        Console.WriteLine("h: Display from hex");
    }
}