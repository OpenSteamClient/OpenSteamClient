using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.Interfaces;
public class Container
{
    public bool IsShuttingDown { get; private set; } = false;
    private readonly object factoryPlaceholderObject = "factory";
    internal Dictionary<Type, object> registeredObjects { get; init; } = new();
    internal Dictionary<Type, Delegate> factories { get; init; } = new();
    internal List<Type> clientLifetimeOrder = new();
    internal List<Type> logonLifetimeOrder = new();
    private static Logger logger = new Logger("Container");

    public Container()
    {
        this.RegisterInstance(this);
    }

    public void RegisterFactoryMethod<T>(Delegate factoryMethod)
    {
        if (this.registeredObjects.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Type '" + typeof(T) + "' already registered.");
        }

        if (this.factories.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Factory '" + typeof(T) + "' already registered.");
        }

        this.factories.Add(typeof(T), factoryMethod);
        this.registeredObjects.Add(typeof(T), factoryPlaceholderObject);
        if (typeof(IClientLifetime).IsAssignableFrom(typeof(T)))
        {
            if (!this.clientLifetimeOrder.Contains(typeof(T)))
            {
                this.clientLifetimeOrder.Add(typeof(T));
            }
        }

        if (typeof(ILogonLifetime).IsAssignableFrom(typeof(T)))
        {
            if (!this.logonLifetimeOrder.Contains(typeof(T)))
            {
                this.logonLifetimeOrder.Add(typeof(T));
            }
        }
    }

    private object RunFactoryFor(Type type)
    {
        if (!this.factories.ContainsKey(type))
        {
            throw new InvalidOperationException("Factory '" + type + "' not registered");
        }
        Delegate factoryMethod = factories[type];
        object? ret = factoryMethod.DynamicInvoke(FillArrayWithDependencies(factoryMethod.GetMethodInfo().GetParameters()));
        if (ret == null)
        {
            throw new NullReferenceException("Factory for " + type + " returned null.");
        }

        this.factories.Remove(type);
        if (this.registeredObjects.ContainsKey(type))
        {
            if (object.ReferenceEquals(this.registeredObjects[type], factoryPlaceholderObject))
            {
                this.registeredObjects.Remove(type);
            }
            else
            {
                throw new InvalidOperationException("Type '" + type + "' already registered (and not factory placeholder)");
            }
        }

        this.RegisterInstance(type, ret);
        return ret;
    }

    private object?[] FillArrayWithDependencies(ParameterInfo[] args, bool withOptionals = true, object[]? extraArgs = null)
    {
        List<object?> constructorDependencies = new();
        Dictionary<Type, object> extras = new();
        if (extraArgs != null) {
            foreach (var item in extraArgs)
            {
                extras.Add(item.GetType(), item);
            }
        }
        for (int i = 0; i < args.Length; i++)
        {
            var constructorArg = args[i];
            if (TryGet(constructorArg.ParameterType, out object? obj)) {
                constructorDependencies.Add(obj);
            } else {
                if (extras.ContainsKey(constructorArg.ParameterType)) {
                    constructorDependencies.Add(extras[constructorArg.ParameterType]);
                } else if (constructorArg.IsOptional) {
                    if (withOptionals) {
                        constructorDependencies.Add(null);
                    }
                } else {
                    throw new InvalidOperationException("Failed to get required object " + constructorArg.ParameterType.FullName);
                }
            }
        }
        return constructorDependencies.ToArray();
    }
    private T RunFactoryFor<T>()
    {
        return (T)RunFactoryFor(typeof(T));
    }

    public object RegisterInstance(Type type, object instance)
    {
        if (this.registeredObjects.ContainsKey(type))
        {
            throw new InvalidOperationException("Type '" + type + "' already registered.");
        }

        if (instance == null)
        {
            throw new NullReferenceException("component is null");
        }

        this.registeredObjects.Add(type, instance);
        if (typeof(IClientLifetime).IsAssignableFrom(type))
        {
            if (!this.clientLifetimeOrder.Contains(type))
            {
                this.clientLifetimeOrder.Add(type);
            }
        }

        if (typeof(ILogonLifetime).IsAssignableFrom(type))
        {
            if (!this.logonLifetimeOrder.Contains(type))
            {
                this.logonLifetimeOrder.Add(type);
            }
        }

        return instance;
    }

    public T RegisterInstance<T>(T instance)
    {
        return (T)RegisterInstance(typeof(T), instance!);
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
            logger.Warning("More than one constructor for " + type.Name + ", issues may arise!");
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
        var dependencies = FillArrayWithDependencies(ctor.GetParameters(), extraArgs == null, extraArgs);
        return (T)ctor.Invoke(dependencies);
    }

    public T ConstructAndRegisterImmediate<T>()
    {
        if (this.registeredObjects.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Type '" + typeof(T) + "' already registered.");
        }

        Type componentType = typeof(T);
        T component = ConstructOnly<T>();
        this.RegisterInstance(component);
        return component;
    }

    public void ConstructAndRegister<T>()
    {
        if (this.registeredObjects.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Type '" + typeof(T) + "' already registered.");
        }

        if (this.factories.ContainsKey(typeof(T)))
        {
            throw new InvalidOperationException("Factory '" + typeof(T) + "' already registered.");
        }

        ConstructorInfo ctor = GetConstructorFor<T>();
        this.RegisterFactoryMethod<T>(() =>
        {
            List<object> constructorDependencies = new();
            foreach (var constructorArg in ctor.GetParameters())
            {
                constructorDependencies.Add(Get(constructorArg.ParameterType));
            }

            return (T)ctor.Invoke(constructorDependencies.ToArray());
        });
    }

    public bool Has(Type type) {
        return this.registeredObjects.ContainsKey(type);
    }

    public bool HasNonFactory(Type type) {
        if (!this.registeredObjects.ContainsKey(type)) {
            return false;
        }

        var obj = this.registeredObjects[type];
        if (object.ReferenceEquals(obj, factoryPlaceholderObject)) {
            return false;
        }

        return true;
    }

    public object Get(Type type)
    {
        if (this.registeredObjects.ContainsKey(type))
        {
            var obj = this.registeredObjects[type];
            if (object.ReferenceEquals(obj, factoryPlaceholderObject))
            {
                return RunFactoryFor(type);
            }
            return this.registeredObjects[type];
        }

        throw new InvalidOperationException("Type '" + type + "' not registered.");
    }

    public bool TryGet(Type type, [NotNullWhen(true)] out object? obj) {
        obj = null;

        if (this.Has(type)) {
            obj = Get(type);
            return true;
        }

        return false;
    }

    public T Get<T>() {
        return (T)Get(typeof(T));
    }

    private bool hasRanStartup;
    public async Task RunClientStartup() {
        foreach (var component in clientLifetimeOrder)
        {
            logger.Info("Running startup for " + component.Name);
            await ((IClientLifetime)Get(component)).RunStartup();
        }
        hasRanStartup = true;
    }

    public async Task RunClientShutdown() {
        if (!hasRanStartup) {
            throw new InvalidOperationException("Cannot run shutdown if startup was never run");
        }

        IsShuttingDown = true;
        foreach (var component in clientLifetimeOrder)
        {
            logger.Info("Shutting down " + component.Name);
            if (this.HasNonFactory(component)) {
                await ((IClientLifetime)Get(component)).RunShutdown();
                logger.Info("Shutdown for " + component.Name + " finished");
            } else {
                logger.Warning("Shutdown for " + component.Name + " skipped, factory was never ran prior.");
            } 
        }
    }

    public async Task RunLogon(IExtendedProgress<int> progress, LoggedOnEventArgs e) {
        foreach (var component in logonLifetimeOrder)
        {
            logger.Info("Running logon for component " + component.Name);
            await ((ILogonLifetime)Get(component)).OnLoggedOn(progress, e);
        }
    }

    public async Task RunLogoff(IExtendedProgress<int> progress) {
        int shutdownComponents = 0;
        progress.SetMaxProgress(logonLifetimeOrder.Count);
        foreach (var component in logonLifetimeOrder)
        {
            logger.Info("Running logoff for component " + component.Name);
            await ((ILogonLifetime)Get(component)).OnLoggingOff(progress);
            logger.Info("Logoff for component " + component.Name + " finished");

            shutdownComponents++;
            progress.SetProgress(shutdownComponents);
        }
    }
}