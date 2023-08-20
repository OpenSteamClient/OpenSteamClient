using System.Reflection;

namespace OpenSteamworks.Client.Utils.Interfaces;

public interface IComponent {
    public IContainer Container { get; init; }
    public Task RunStartup();
    public Task RunShutdown();
}
public abstract class Component : IComponent
{
    public abstract Task RunStartup();
    public abstract Task RunShutdown();
    public IContainer Container { get; init; }

    public Component(IContainer container) {
        this.Container = container;
    }
    
    public async Task EmptyAwaitable() {
        await Task.CompletedTask;
    }

    public void RegisterSubComponentInstance<T>(T component)
    {
        Container.RegisterComponentInstance(component);
    }

    public T ConstructAndRegisterSubComponent<T>()
    {
        return Container.ConstructAndRegisterComponent<T>();
    }

    public object GetComponent(Type type)
    {
        return Container.GetComponent(type);
    }

    public T GetComponent<T>()
    {
        return Container.GetComponent<T>();
    }
}