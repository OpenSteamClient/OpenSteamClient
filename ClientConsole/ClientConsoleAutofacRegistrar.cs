using Autofac;
using Common.Autofac;
using Common.Managers;

namespace ClientConsole;

public class ClientConsoleAutofacRegistrar : IAutofacRegistrar
{
    public static void Register(ref ContainerBuilder builder)
    {
        Common.CommonAutofacRegistrar.Register(ref builder);
    }
}