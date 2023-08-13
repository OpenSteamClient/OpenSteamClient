using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using OpenSteamworks.Attributes;
using OpenSteamworks.Callbacks;

namespace OpenSteamworks.ClientInterfaces;

public abstract class ClientInterface {

    private SteamClient client;
    private List<CallbackManager.CallbackHandler> registeredHandlers = new();
    public ClientInterface(SteamClient client) {
        this.client = client;
        foreach (var func in this.GetType().GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly))
        {
            Attribute? attribute = func.GetCustomAttribute(typeof(CallbackListenerAttribute<>));
            if (attribute == null) {
                continue;
            }
            Type expectedType = attribute.GetType().GetGenericArguments()[0];
            Type argType = func.GetParameters()[0].ParameterType;
            if (expectedType != argType) {
                throw new ArgumentException("Function " + func.Name + " has invalid arguments for CallbackListenerAttribute. Expected " + expectedType.Name + ", had " + argType.Name);
            }
            var actType = typeof(Action<>).MakeGenericType(expectedType);
            Console.WriteLine("Registering");
            registeredHandlers.Add(client.CallbackManager.RegisterHandler(func.CreateDelegate(actType, this), expectedType, false));
        }
    }
    internal virtual void RunShutdownTasks() {
        foreach (var handler in registeredHandlers)
        {
            this.client.CallbackManager.DeregisterHandler(handler);
        }

        registeredHandlers.Clear();
    }
}