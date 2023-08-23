using System.Reflection;
using OpenSteamworks.Client.Utils.Interfaces;

public interface IContainer {
    public void RegisterComponentFactoryMethod<T>(Func<T> factoryMethod);
    public void RegisterComponentInstance<T>(T component);
    public T ConstructAndRegisterComponent<T>();
    public void LazyConstructAndRegisterComponent<T>();
    public object GetComponent(Type type);
    public T GetComponent<T>();
}


public class Container : IContainer {
    internal Dictionary<Type, object> components { get; init; } = new();
    internal Dictionary<Type, object> componentFactories { get; init; } = new();
    internal List<IComponent> componentOrder = new();

    public Container() {
        this.RegisterComponentInstance<IContainer>(this);
    }

    public void RegisterComponentFactoryMethod<T>(Func<T> factoryMethod) {
        if (this.components.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }

        if (this.componentFactories.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Factory '" + typeof(T) + "' already registered.");
        }

        this.componentFactories.Add(typeof(T), factoryMethod);
    }

    private T RunFactoryFor<T>() {
        if (!this.componentFactories.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Factory '" + typeof(T) + "' not registered");
        }

        T ret = ((Func<T>)componentFactories[typeof(T)]).Invoke();
        this.componentFactories.Remove(typeof(T));
        this.RegisterComponentInstance<T>(ret);
        return ret;
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

    private ConstructorInfo GetConstructorFor<T>() {
        ConstructorInfo[] ctors = typeof(T).GetConstructors();
        if (ctors.Length == 0) {
            throw new ArgumentException("No constructors for " + typeof(T).Name);
        }

        if (ctors.Length > 1) {
            Console.WriteLine("More than one constructor for " + typeof(T).Name + ", issues may arise!");
        }

        return ctors.First();
    }
    public T ConstructAndRegisterComponent<T>() {
        if (this.components.ContainsKey(typeof(T))) {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }

        Type componentType = typeof(T);
        ConstructorInfo ctor = GetConstructorFor<T>();

        List<object> constructorDependencies = new();
        foreach (var constructorArg in ctor.GetParameters())
        {
            constructorDependencies.Add(GetComponent(constructorArg.ParameterType));
        }

        T component = (T)ctor.Invoke(constructorDependencies.ToArray());
        this.RegisterComponentInstance(component);
        return component;
    }

    public void LazyConstructAndRegisterComponent<T>() {
        ConstructorInfo ctor = GetConstructorFor<T>();
        this.RegisterComponentFactoryMethod<T>(() => {
            List<object> constructorDependencies = new();
            foreach (var constructorArg in ctor.GetParameters())
            {
                constructorDependencies.Add(GetComponent(constructorArg.ParameterType));
            }

            return (T)ctor.Invoke(constructorDependencies.ToArray());
        });
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