using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OpenSteamworks.Client.Utils.Interfaces;

public interface IContainer {
    /// <summary>
    /// Registers a factory method. Can use any number of params that are registered (lazy or otherwise).
    /// Must return an instance of type T.
    /// </summary>
    public void RegisterComponentFactoryMethod<T>(Delegate factoryMethod);
    public void RegisterComponentInstance(Type type, object component);
    public void RegisterComponentInstance<T>(T component);
    /// <summary>
    /// Constructs a component, but does not register it. Useful for viewmodels.
    /// </summary>
    public T ConstructOnly<T>(object[]? extraArgs = null);
    /// <summary>
    /// Immediately constructs and registers a component.
    /// </summary>
    public T ConstructAndRegisterComponentImmediate<T>();
    /// <summary>
    /// Lazy constructs a component.
    /// </summary>
    public void ConstructAndRegisterComponent<T>();
    public object GetComponent(Type type);
    public T GetComponent<T>();
}


public class Container : IContainer
{
    private readonly object factoryPlaceholderComponent = "factory";
    internal Dictionary<Type, object> components { get; init; } = new();
    internal Dictionary<Type, Delegate> componentFactories { get; init; } = new();
    internal List<Type> componentOrder = new();

    public Container()
    {
        this.RegisterComponentInstance<IContainer>(this);
    }

    public void RegisterComponentFactoryMethod<T>(Delegate factoryMethod)
    {
        if (this.components.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }

        if (this.componentFactories.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Factory '" + typeof(T) + "' already registered.");
        }

        this.componentFactories.Add(typeof(T), factoryMethod);
        this.components.Add(typeof(T), factoryPlaceholderComponent);
        if (typeof(IComponent).IsAssignableFrom(typeof(T)))
        {
            if (!this.componentOrder.Contains(typeof(T)))
            {
                this.componentOrder.Add(typeof(T));
            }
        }
    }

    private object RunFactoryFor(Type type)
    {
        if (!this.componentFactories.ContainsKey(type))
        {
            throw new InvalidOperationException("Factory '" + type + "' not registered");
        }
        Delegate factoryMethod = componentFactories[type];
        object? ret = factoryMethod.DynamicInvoke(FillArrayWithDependencies(factoryMethod.GetMethodInfo().GetParameters()));
        if (ret == null)
        {
            throw new NullReferenceException("Factory for " + type + " returned null.");
        }

        this.componentFactories.Remove(type);
        if (this.components.ContainsKey(type))
        {
            if (object.ReferenceEquals(this.components[type], factoryPlaceholderComponent))
            {
                this.components.Remove(type);
            }
            else
            {
                throw new InvalidOperationException("Component '" + type + "' already registered (and not factory placeholder)");
            }
        }

        this.RegisterComponentInstance(type, ret);
        return ret;
    }

    private object?[] FillArrayWithDependencies(ParameterInfo[] args, bool withOptionals = true)
    {
        List<object?> constructorDependencies = new();
        for (int i = 0; i < args.Length; i++)
        {
            var constructorArg = args[i];
            if (TryGetComponent(constructorArg.ParameterType, out object? obj)) {
                constructorDependencies.Add(obj);
            } else {
                if (!constructorArg.IsOptional) {
                    throw new InvalidOperationException("Failed to get required component " + constructorArg.ParameterType.FullName);
                } else {
                    if (withOptionals) {
                        constructorDependencies.Add(null);
                    }
                }
            }
        }
        foreach (var constructorArg in args)
        {

        }
        return constructorDependencies.ToArray();
    }
    private T RunFactoryFor<T>()
    {
        return (T)RunFactoryFor(typeof(T));
    }

    public void RegisterComponentInstance(Type type, object component)
    {
        if (this.components.ContainsKey(type))
        {
            throw new InvalidOperationException("Component '" + type + "' already registered.");
        }

        if (component == null)
        {
            throw new NullReferenceException("component is null");
        }

        this.components.Add(type, component);
        if (typeof(IComponent).IsAssignableFrom(type))
        {
            if (!this.componentOrder.Contains(type))
            {
                this.componentOrder.Add(type);
            }
        }
    }

    public void RegisterComponentInstance<T>(T component)
    {
        RegisterComponentInstance(typeof(T), component!);
    }

    private ConstructorInfo GetConstructorFor(Type type)
    {
        ConstructorInfo[] ctors = type.GetConstructors();
        if (ctors.Length == 0)
        {
            throw new ArgumentException("No constructors for " + type.Name);
        }

        if (ctors.Length > 1)
        {
            Console.WriteLine("More than one constructor for " + type.Name + ", issues may arise!");
        }

        return ctors.First();
    }

    private ConstructorInfo GetConstructorFor<T>()
    {
        return GetConstructorFor(typeof(T));
    }

    public T ConstructOnly<T>(object[]? extraArgs = null)
    {
        ConstructorInfo ctor = GetConstructorFor<T>();
        List<object?> dependencies = FillArrayWithDependencies(ctor.GetParameters(), extraArgs == null).ToList();
        if (extraArgs != null)
        {
            dependencies.AddRange(extraArgs);
        }
        return (T)ctor.Invoke(dependencies.ToArray());
    }

    public T ConstructAndRegisterComponentImmediate<T>()
    {
        if (this.components.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }

        Type componentType = typeof(T);
        T component = ConstructOnly<T>();
        this.RegisterComponentInstance(component);
        return component;
    }

    public void ConstructAndRegisterComponent<T>()
    {
        if (this.components.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Component '" + typeof(T) + "' already registered.");
        }

        if (this.componentFactories.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Factory '" + typeof(T) + "' already registered.");
        }

        ConstructorInfo ctor = GetConstructorFor<T>();
        this.RegisterComponentFactoryMethod<T>(() =>
        {
            List<object> constructorDependencies = new();
            foreach (var constructorArg in ctor.GetParameters())
            {
                constructorDependencies.Add(GetComponent(constructorArg.ParameterType));
            }

            return (T)ctor.Invoke(constructorDependencies.ToArray());
        });
    }

    public object GetComponent(Type type)
    {
        if (this.components.ContainsKey(type))
        {
            var component = this.components[type];
            if (object.ReferenceEquals(component, factoryPlaceholderComponent))
            {
                return RunFactoryFor(type);
            }
            return this.components[type];
        }

        throw new InvalidOperationException("Component '" + type + "' not registered.");
    }

    public bool TryGetComponent(Type type, [NotNullWhen(true)] out object? obj) {
        obj = null;
        try
        {
            obj = GetComponent(type);
        }
        catch (System.Exception)
        {
            return false;
        }

        return true;
    }

    public T GetComponent<T>() {
        return (T)GetComponent(typeof(T));
    }

    public async Task RunStartupForComponents() {
        foreach (var component in componentOrder)
        {
            await ((IComponent)GetComponent(component)).RunStartup();
        }
    }

    public async Task RunShutdownForComponents() {
        foreach (var component in componentOrder)
        {
            await ((IComponent)GetComponent(component)).RunShutdown();
        }
    }
}