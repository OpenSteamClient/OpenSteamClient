using System.Reflection;
using OpenSteamworks.Client.Utils.Interfaces;

public interface IContainer {
    public void RegisterComponentInstance<T>(T component);
    public T ConstructAndRegisterComponent<T>();
    public object GetComponent(Type type);
    public T GetComponent<T>();
}

public class Container : IContainer {
    internal Dictionary<Type, object> components { get; init; } = new();
    internal List<IComponent> componentOrder = new();

    public Container() {
        this.RegisterComponentInstance<IContainer>(this);
    }
    public void RegisterComponentInstance<T>(T component) {
        if (this.components.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }
        
        if (component == null) {
            throw new NullReferenceException("component is null");
        }

        this.components.Add(typeof(T), component);        
        if (component is IComponent) {
            componentOrder.Add((IComponent)component);
        }
    }
    public T ConstructAndRegisterComponent<T>() {
        if (this.components.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }

        Type componentType = typeof(T);
        ConstructorInfo[] ctors = componentType.GetConstructors();
        if (ctors.Length == 0) {
            throw new ArgumentException("No constructors for " + componentType.Name);
        }

        if (ctors.Length > 1) {
            Console.WriteLine("More than one constructor for " + componentType.Name + ", issues may arise!");
        }

        ConstructorInfo ctor = ctors.First();

        List<object> constructorDependencies = new();
        foreach (var constructorArg in ctor.GetParameters())
        {
            constructorDependencies.Add(GetComponent(constructorArg.ParameterType));
        }

        T component = (T)ctor.Invoke(constructorDependencies.ToArray());
        this.RegisterComponentInstance(component);
        return component;
    }

    public object GetComponent(Type type) {
        if (this.components.ContainsKey(type)) {
            return this.components[type];
        }

        throw new InvalidOperationException("Component '" + type + "' not registered.");
    }

    public T GetComponent<T>() {
        return (T)GetComponent(typeof(T));
    }

    public async Task RunStartupForComponents() {
        foreach (var component in componentOrder)
        {
            await component.RunStartup();
        }
    }

    public async Task RunShutdownForComponents() {
        foreach (var component in componentOrder)
        {
            await component.RunShutdown();
        }
    }
}