using Autofac;
using ClientUI.Translation;
using Common.Autofac;
using Common.Managers;

namespace ClientUI;

public class ClientUIAutofacRegistrar : IAutofacRegistrar
{
    public static void Register(ref ContainerBuilder builder)
    {
        builder.RegisterType<TranslationManager>().As<IHasStartupTasks>().AsSelf().PropertiesAutowired().SingleInstance();
    }
}