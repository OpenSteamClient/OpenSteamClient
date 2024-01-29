using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Client.Utils.DI;

namespace OpenSteamworks.Client.Utils.DI;
public class Container
{
    public bool IsShuttingDown { get; private set; } = false;
    private readonly object factoryPlaceholderObject = "factory";
    internal Dictionary<Type, object> registeredObjects { get; init; } = new();
    internal Dictionary<Type, Delegate> factories { get; init; } = new();
    internal List<Type> clientLifetimeOrder = new();
    internal List<Type> logonLifetimeOrder = new();
    private Logger logger;

    //TODO: constructor argument shenanigans are bad. How to make this less terrible while keeping logging immediate?
    public Container(InstallManager installManager)
    {
        this.logger = Logger.GetLogger("Container", installManager.GetLogPath("Container"));
        this.RegisterInstance(installManager);
        this.RegisterInstance(this);
    }

    public void RegisterFactoryMethod<T>(Func<T> factoryMethod)
    {
        RegisterFactoryMethod(typeof(T), factoryMethod);
    }

    public void RegisterFactoryMethod<T>(Delegate factoryMethodWithArgs)
    {
        RegisterFactoryMethod(typeof(T), factoryMethodWithArgs);
    }

    public void RegisterFactoryMethod(Type type, Delegate factoryMethod)
    {
        logger.Debug("Attempting to register factory for type '" + type.Name + "'");
        if (this.registeredObjects.ContainsKey(type))
        {
            logger.Error("Type '" + type.Name + "' already registered.");
            throw new InvalidOperationException("Type '" + type + "' already registered.");
        }

        if (this.factories.ContainsKey(type))
        {
            logger.Error("Factory for type '" + type.Name + "' already registered.");
            throw new InvalidOperationException("Factory for type '" + type + "' already registered.");
        }

        this.factories.Add(type, factoryMethod);
        this.registeredObjects.Add(type, factoryPlaceholderObject);
        logger.Debug("Registered factory for type '" + type.Name + "'");
        var implementedInterfacesAttrs = type.GetCustomAttributes(typeof(ImplementsInterfaceAttribute<>));
        foreach (var ifaceAttr in implementedInterfacesAttrs)
        {
            Type interfaceType = ifaceAttr.GetType().GetGenericArguments().First();
            logger.Debug("Registered implemented interface factory for type '" + interfaceType.Name + "'");
            this.factories.Add(interfaceType, factoryMethod);
            this.registeredObjects.Add(interfaceType, factoryPlaceholderObject);
        }

        if (typeof(IClientLifetime).IsAssignableFrom(type))
        {
            if (!this.clientLifetimeOrder.Contains(type))
            {
                this.clientLifetimeOrder.Add(type);
                logger.Debug("Registered factory of type '" + type.Name + "' for client lifetime at index " + this.clientLifetimeOrder.Count);
            }
        }

        if (typeof(ILogonLifetime).IsAssignableFrom(type))
        {
            if (!this.logonLifetimeOrder.Contains(type))
            {
                this.logonLifetimeOrder.Add(type);
                logger.Debug("Registered factory of type '" + type.Name + "' for logon lifetime at index " + this.logonLifetimeOrder.Count);
            }
        }
    }

    private object RunFactoryFor(Type type)
    {
        logger.Debug("Attempting to run factory for type '" + type.Name + "'");

        if (!this.factories.ContainsKey(type))
        {
            logger.Error("No factory for type '" + type.Name + "'");
            throw new InvalidOperationException("Factory '" + type + "' not registered");
        }

        Delegate factoryMethod = factories[type];
        object? ret = factoryMethod.DynamicInvoke(FillArrayWithDependencies(factoryMethod.GetMethodInfo().GetParameters()));
        if (ret == null)
        {
            logger.Error("Factory for type '" + type.Name + "' returned null");
            throw new NullReferenceException("Factory for " + type + " returned null.");
        }

        List<Type> toRemove = new();
        foreach (var f in this.factories)
        {
            if (f.Value == factoryMethod) {
                toRemove.Add(f.Key);
            }
        }

        foreach (var item in toRemove)
        {
            this.factories.Remove(item);

            if (this.registeredObjects.ContainsKey(item))
            {
                if (object.ReferenceEquals(this.registeredObjects[item], factoryPlaceholderObject))
                {
                    this.registeredObjects.Remove(item);
                }
                else
                {
                    logger.Error("Type '" + item + "' already registered (and not factory placeholder)");
                    throw new InvalidOperationException("Type '" + item + "' already registered (and not factory placeholder)");
                }
            }
        }

        logger.Debug("Factory for type '" + type.Name + "' ran successfully. Registering result");
        this.RegisterInstance(type, ret);
        return ret;
    }

    private object?[] FillArrayWithDependencies(ParameterInfo[] args, bool withOptionals = true, object[]? extraArgs = null)
    {
        List<object?> constructorDependencies = new();
        Dictionary<Type, object> extras = new();
        if (extraArgs != null)
        {
            foreach (var item in extraArgs)
            {
                extras.Add(item.GetType(), item);
            }
        }
        for (int i = 0; i < args.Length; i++)
        {
            var constructorArg = args[i];
            if (TryGet(constructorArg.ParameterType, out object? obj))
            {
                constructorDependencies.Add(obj);
            }
            else
            {
                if (extras.ContainsKey(constructorArg.ParameterType))
                {
                    constructorDependencies.Add(extras[constructorArg.ParameterType]);
                }
                else if (constructorArg.IsOptional)
                {
                    if (withOptionals)
                    {
                        constructorDependencies.Add(Type.Missing);
                    }
                }
                else
                {
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
        logger.Debug("Attempting to register type '" + type.Name + "'");
        if (this.registeredObjects.ContainsKey(type))
        {
            logger.Error("Type '" + type.Name + "' already registered.");
            throw new InvalidOperationException("Type '" + type + "' already registered.");
        }

        if (instance == null)
        {
            logger.Error("Component is null");
            throw new NullReferenceException("component is null");
        }


        this.registeredObjects.Add(type, instance);
        logger.Debug("Registered type '" + type.Name + "'");
        if (typeof(IClientLifetime).IsAssignableFrom(type))
        {
            if (!this.clientLifetimeOrder.Contains(type))
            {
                this.clientLifetimeOrder.Add(type);
                logger.Debug("Registered type '" + type.Name + "' for client lifetime at index " + this.clientLifetimeOrder.Count);
            }
        }

        if (typeof(ILogonLifetime).IsAssignableFrom(type))
        {
            if (!this.logonLifetimeOrder.Contains(type))
            {
                this.logonLifetimeOrder.Add(type);
                logger.Debug("Registered type '" + type.Name + "' for logon lifetime at index " + this.clientLifetimeOrder.Count);
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
            logger.Error("No constructors for type '" + type.Name + "'");
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

    public object ConstructOnly(Type type, params object[] extraArgs)
    {
        logger.Debug("Attempting to construct type '" + type.Name + "'");
        ConstructorInfo ctor = GetConstructorFor(type);
        var dependencies = FillArrayWithDependencies(ctor.GetParameters(), !extraArgs.Any(), extraArgs);
        return ctor.Invoke(dependencies);
    }

    public T ConstructOnly<T>(params object[] extraArgs)
    {
        logger.Debug("Attempting to construct type '" + typeof(T).Name + "'");
        ConstructorInfo ctor = GetConstructorFor<T>();
        var dependencies = FillArrayWithDependencies(ctor.GetParameters(), !extraArgs.Any(), extraArgs);
        return (T)ctor.Invoke(dependencies);
    }

    public T ConstructAndRegisterImmediate<T>()
    {
        if (this.registeredObjects.ContainsKey(typeof(T)))
        {
            logger.Error("Type '" + typeof(T) + "' already registered.");
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
            logger.Error("Type '" + typeof(T) + "' already registered.");
            throw new InvalidOperationException("Type '" + typeof(T) + "' already registered.");
        }

        if (this.factories.ContainsKey(typeof(T)))
        {
            logger.Error("Factory for type '" + typeof(T) + "' already registered.");
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

    public bool Has(Type type)
    {
        return this.registeredObjects.ContainsKey(type);
    }

    public bool HasNonFactory(Type type)
    {
        if (!this.registeredObjects.ContainsKey(type))
        {
            return false;
        }

        var obj = this.registeredObjects[type];
        if (object.ReferenceEquals(obj, factoryPlaceholderObject))
        {
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

    public bool TryGet<T>([NotNullWhen(true)] out T? obj) {
        obj = default;
        if (TryGet(typeof(T), out object? obji)) {
            obj = (T)obji;
            return true;
        }

        return false;
    }

    public T? GetNullable<T>() {
        TryGet<T>(out T? obj);
        return obj;
    }

    public bool TryGet(Type type, [NotNullWhen(true)] out object? obj)
    {
        obj = null;

        if (this.Has(type))
        {
            obj = Get(type);
            return true;
        }

        return false;
    }

    public T Get<T>()
    {
        return (T)Get(typeof(T));
    }

    private bool hasRanStartup;
    public async Task RunClientStartup()
    {
        foreach (var component in clientLifetimeOrder)
        {
            logger.Info("Running startup for " + component.Name);
            await ((IClientLifetime)Get(component)).RunStartup();
        }
        hasRanStartup = true;
    }

    public async Task RunClientShutdown()
    {
        if (!hasRanStartup)
        {
            throw new InvalidOperationException("Cannot run shutdown if startup was never run");
        }

        IsShuttingDown = true;
        foreach (var component in clientLifetimeOrder)
        {
            logger.Info("Shutting down " + component.Name);
            if (this.HasNonFactory(component))
            {
                await ((IClientLifetime)Get(component)).RunShutdown();
                logger.Info("Shutdown for " + component.Name + " finished");
            }
            else
            {
                logger.Warning("Shutdown for " + component.Name + " skipped, factory was never ran prior.");
            }
        }
    }

    public async Task RunLogon(IExtendedProgress<int> progress, LoggedOnEventArgs e)
    {
        foreach (var component in logonLifetimeOrder)
        {
            logger.Info("Running logon for component " + component.Name);
            await ((ILogonLifetime)Get(component)).OnLoggedOn(progress, e);
        }
    }

    public async Task RunLogoff(IExtendedProgress<int> progress)
    {
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