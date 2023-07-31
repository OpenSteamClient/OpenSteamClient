using Autofac;
using Autofac.Core;
using Common.Autofac;
using Common.Startup;
using Common.Utils;

namespace Common.Startup;

public static class StartupController {
    public static async Task<IContainer> Bootstrap<UIAutofacT>(IExtendedProgress<int> bootstrapperProgress) where UIAutofacT : IAutofacRegistrar {
        ContainerBuilder builder = new ContainerBuilder();

        CommonAutofacRegistrar.RegisterPreBootstrap(ref builder);
        builder.RegisterType<Bootstrapper>().PropertiesAutowired().SingleInstance();

        CommonAutofacRegistrar.Register(ref builder);
        
        UIAutofacT.Register(ref builder);

        builder.RegisterType<StartupTasksRunner>().PropertiesAutowired().SingleInstance();
        
        IContainer container = builder.Build();
        await container.Resolve<Bootstrapper>().RunBootstrap(bootstrapperProgress);
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
    