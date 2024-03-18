using Profiler;

namespace ProfilerTest;

public class Program
{
    private static Random rand = new Random();

    public static void Main(string[] args)
    {
        CProfiler.CurrentProfiler?.Reset();

        Init();

        CProfiler.CurrentProfiler?.PrintStats();
    }

    private static void Init() {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Program.Init");
        User_Init();
        Apps_Init();
        Library_Init();
    }

    private static void Apps_Init() {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Apps_Init");
        Thread.Sleep(rand.Next(1, 1000));
        {
            using var subscope = CProfiler.CurrentProfiler?.EnterScope("Apps_Init - Create app data");
            for (int i = 0; i < rand.Next(250, 1600); i++)
            {
                using var subscope1 = CProfiler.CurrentProfiler?.EnterScope("App_Create");
                Thread.Sleep(rand.Next(1, 5));
            }
        }
    }

    private static void User_Init() {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("User_Init");
        Thread.Sleep(rand.Next(1, 1000));
    }

    private static void Library_Init() {
        using var scope = CProfiler.CurrentProfiler?.EnterScope("Library_Init");
        Thread.Sleep(rand.Next(1, 1000));
        {
            using var subscope = CProfiler.CurrentProfiler?.EnterScope("Library_Init - Load library data");
            Thread.Sleep(rand.Next(1, 500));
        }

        {
            using var subscope = CProfiler.CurrentProfiler?.EnterScope("Library_Init - Load assets");
            Thread.Sleep(rand.Next(1, 500));
        }

        {
            using var subscope = CProfiler.CurrentProfiler?.EnterScope("Library_Init - Create missing assets");
            Thread.Sleep(rand.Next(1, 1000));
        }
    }
}