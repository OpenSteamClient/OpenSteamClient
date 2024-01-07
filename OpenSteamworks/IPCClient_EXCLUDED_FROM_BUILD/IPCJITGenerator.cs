using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.FileProviders;
using OpenSteamworks.Native.JIT;

namespace OpenSteamworks.IPCClient;

public static class IPCJITGenerator {
    private static ModuleBuilder moduleBuilder;

    /// <summary>
    /// Maps a list of interfaces to a list of our generated implementations of it
    /// </summary>
    private static Dictionary<Type, Type> generatedTypes = new();

    static IPCJITGenerator()
    {
        //TODO: re-add AssemblyBuilderAccess.RunAndSave when it is implemented
        AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("OpenSteamworksIPCJIT"), AssemblyBuilderAccess.Run);

#if DEBUG
        Type daType = typeof(DebuggableAttribute);
        ConstructorInfo? daCtor = daType.GetConstructor(new Type[] { typeof(DebuggableAttribute.DebuggingModes) });
        if (daCtor != null) {
            CustomAttributeBuilder daBuilder = new CustomAttributeBuilder(daCtor, new object[] { 
            DebuggableAttribute.DebuggingModes.DisableOptimizations | 
            DebuggableAttribute.DebuggingModes.Default });
            assemblyBuilder.SetCustomAttribute(daBuilder);
        }
#endif

        moduleBuilder = assemblyBuilder.DefineDynamicModule("OpenSteamworksIPCJIT");
    }

    public static TClass GenerateClass<TClass>(IPCClient ipcclient, HSteamUser user) where TClass : class
    {
        Type targetInterface = typeof(TClass);

        if (generatedTypes.ContainsKey(targetInterface)) {
            return (TClass)GenerateClassForImplementor(generatedTypes[targetInterface], ipcclient, user);
        }

        // Fetch the interface info from OpenSteamworks.dll
        var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
        IFileInfo fileInfo = embeddedProvider.GetFileInfo($"{targetInterface.Name}Map.json");
        if (!fileInfo.Exists) {
            throw new Exception($"Cannot find {targetInterface.Name}Map.json as an embedded resource.");
        }

        JsonElement interfaceJson;
        using (var stream = fileInfo.CreateReadStream())
        {
            interfaceJson = JsonDocument.Parse(stream).RootElement;
        }

        TypeBuilder builder = moduleBuilder.DefineType(targetInterface.Name + "_IPC",
                                                TypeAttributes.Class, null, new Type[] { targetInterface });
        

        FieldBuilder ipcclientField = builder.DefineField("IPCClient", typeof(IPCClient), FieldAttributes.Public);
        FieldBuilder steamuserField = builder.DefineField("SteamUser", typeof(UInt32), FieldAttributes.Public);

        var methods = targetInterface.GetMethods();
        for (int i = 0; i < methods.Length; i++)
        {
            byte interfaceid = 0;
            uint fencepost = 0;
            uint functionid = 0;

            foreach (var item in interfaceJson.GetProperty("functions").EnumerateArray())
            {
                if (item.GetProperty("name").GetString() == methods[i].Name) {
                    interfaceid = (byte)uint.Parse(item.GetProperty("interfaceid").GetString()!);
                    functionid = uint.Parse(item.GetProperty("functionid").GetString()!);
                    fencepost = uint.Parse(item.GetProperty("fencepost").GetString()!);
                    break;
                }
            }

            EmitClassMethod(methods[i], builder, ipcclientField, steamuserField, interfaceid, fencepost, functionid);
        }

        Type implClass = builder.CreateType();
        generatedTypes[targetInterface] = implClass;
        return (TClass)GenerateClassForImplementor(implClass, ipcclient, user);
    }

    private static object GenerateClassForImplementor(Type implClass, IPCClient ipcclient, HSteamUser user) {
        object? instClass = Activator.CreateInstance(implClass);
        if (instClass == null) {
            throw new Native.JIT.JITEngineException("Failed to CreateInstance of implClass");
        }

        FieldInfo ipcclientField = implClass.GetField("IPCClient", BindingFlags.Public | BindingFlags.Instance)!;
        ipcclientField.SetValue(instClass, ipcclient);

        FieldInfo steamuserField = implClass.GetField("SteamUser", BindingFlags.Public | BindingFlags.Instance)!;
        steamuserField.SetValue(instClass, (UInt32)(int)user);

        return instClass;
    }

    private static void EmitClassMethod(MethodInfo methodToGenerate, TypeBuilder builder, FieldBuilder ipcclientField, FieldBuilder steamuserField, byte interfaceid, uint fencepost, uint functionid)
    {
        var paramInfos = methodToGenerate.GetParameters();
        MethodBuilder mbuilder = builder.DefineMethod(methodToGenerate.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.HasThis);

        mbuilder.SetSignature(methodToGenerate.ReturnType, methodToGenerate.ReturnParameter.GetRequiredCustomModifiers(), methodToGenerate.ReturnParameter.GetOptionalCustomModifiers(), paramInfos.Select(pi => pi.ParameterType).ToArray(), paramInfos.Select(pi => pi.GetRequiredCustomModifiers()).ToArray(), paramInfos.Select(pi => pi.GetOptionalCustomModifiers()).ToArray());
        builder.DefineMethodOverride(mbuilder, methodToGenerate);

        ILGeneratorEx ilgen = new(mbuilder, paramInfos.Select(pi => pi.ParameterType).ToArray());

        // Emit a call to ThrowIfRemotePipe if we have BlacklistedInCrossProcessIPCAttribute
        if (methodToGenerate.GetCustomAttributes(typeof(BlacklistedInCrossProcessIPCAttribute), false).Any()) {
            ilgen.Call(typeof(InteropHelp).GetMethod(nameof(InteropHelp.ThrowIfRemotePipe))!);
        }

        ilgen.EmitWriteLine("Function " + methodToGenerate.DeclaringType!.Name + "::" + methodToGenerate.Name);

        ilgen.Ldarg(0);
        ilgen.Ldfld(ipcclientField);
        ilgen.Ldarg(0);
        ilgen.Ldfld(steamuserField);

        ilgen.Emit(OpCodes.Ldc_I4, (int)interfaceid);
        ilgen.Emit(OpCodes.Ldc_I4, (int)functionid);
        ilgen.Emit(OpCodes.Ldc_I4, (int)fencepost);

        // Create array of paramInfos.Length
        if (paramInfos.Length > 0) {
            ilgen.Ldc_I4(paramInfos.Length);
            ilgen.Emit(OpCodes.Newarr, typeof(object));
        } else {
            ilgen.Call(typeof(Array).GetMethod(nameof(Array.Empty))!.MakeGenericMethod(typeof(object)));
        }

        // Emit all arguments into the array
        for (int i = 0; i < paramInfos.Length; i++)
        {
            ilgen.AddComment("Array element " + paramInfos[i].ParameterType.Name);
            ilgen.Emit(OpCodes.Dup);
            ilgen.Ldc_I4(i);
            ilgen.Ldarg(i+1);
            if (paramInfos[i].ParameterType.IsValueType) {
                ilgen.AddComment("Is value type");
                ilgen.Box(paramInfos[i].ParameterType);
            }

            ilgen.Emit(OpCodes.Stelem_Ref);
        }

        if (methodToGenerate.ReturnType == typeof(void)) {
            ilgen.Callvirt(typeof(IPCClient).GetMethod(nameof(IPCClient.CallIPCFunctionNoReturn))!);
        } else {
            ilgen.Callvirt(typeof(IPCClient).GetMethod(nameof(IPCClient.CallIPCFunction))!.MakeGenericMethod(methodToGenerate.ReturnType));
        }

        ilgen.Return();
        if (methodToGenerate.Name == "SetLanguage") {
            Console.WriteLine(ilgen.ToString());
        }
    }
}