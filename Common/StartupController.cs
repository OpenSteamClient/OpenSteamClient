using Autofac;
using Common.Autofac;
using Common.Startup;
using Common.Utils;

namespace Common;

public static class StartupController {
    // Keeps bootstrapper from being GC'd
    internal static Bootstrapper? bootstrapper;
    public static async Task<IContainer> Bootstrap<UIAutofacT>(IExtendedProgress<int> bootstrapperProgress) where UIAutofacT : IAutofacRegistrar {
        bootstrapper = new Bootstrapper();

        ContainerBuilder builder = new ContainerBuilder();
        builder.Register(c => bootstrapper);
        await bootstrapper.RunBootstrap(bootstrapperProgress);

        CommonAutofacRegistrar.Register(ref builder);
        UIAutofacT.Register(ref builder);

        builder.RegisterType<StartupTasksRunner>().SingleInstance();
        IContainer container = builder.Build();
        container.Resolve<StartupTasksRunner>().RunStartup();
        return container;
    }
}

internal class StartupTasksRunner {
    IEnumerable<IHasStartupTasks> tasks;
    public StartupTasksRunner(IEnumerable<IHasStartupTasks> tasks) {
        this.tasks = tasks;
    }

    public void RunStartup()
    {
        foreach (var task in tasks)
        {
            task.RunStartup();
        }
    }
}
    