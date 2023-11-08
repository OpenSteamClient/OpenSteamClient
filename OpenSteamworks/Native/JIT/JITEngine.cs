using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Linq;
using System.Diagnostics.CodeAnalysis;

namespace OpenSteamworks.Native.JIT
{
    class JITEngineException : Exception
    {
        public JITEngineException(string message) : base(message) { }
    }

    /// <summary>
    /// The heart of OpenSteamworks. 
    /// If you have problems with dotnet coredumping and not providing error messages, build it from source (with the debug configuration) and run your project with it (/path/to/built/runtime/artifacts/bin/testhost/net7.0-Linux-Debug-x64/dotnet bin/Debug/net7.0/ClientUI.dll)
    /// </summary>
    public static class JITEngine
    {
        private static ModuleBuilder moduleBuilder;

        /// <summary>
        /// Maps a list of interfaces to a list of our generated implementors for it
        /// </summary>
        private static Dictionary<Type, Type> generatedTypes = new();

        static JITEngine()
        {
            //TODO: re-add AssemblyBuilderAccess.RunAndSave when it is implemented
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(new AssemblyName("OpenSteamworksJIT"), AssemblyBuilderAccess.Run);

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

            moduleBuilder = assemblyBuilder.DefineDynamicModule("OpenSteamworksJIT");
        }

        /// <summary>
        /// Generates a class from an interface and a pointer to that class/interface in native memory
        /// Will generate code at runtime when a specific type is first created
        /// </summary>
        /// <typeparam name="TClass">The class to generate an instance of</typeparam>
        /// <param name="classptr">The pointer to the class</param>
        /// <returns>An instance of the class</returns>
        /// <exception cref="JITEngineException"></exception>
        public static TClass GenerateClass<TClass>(IntPtr classptr) where TClass : class
        {
            Type targetInterface = typeof(TClass);
            if (classptr == IntPtr.Zero)
            {
                throw new JITEngineException("GenerateClass called with NULL ptr");
            }

            if (generatedTypes.ContainsKey(targetInterface)) {
                return (TClass)GenerateClassForImplementor(generatedTypes[targetInterface], classptr);
            }

            IntPtr vtable_ptr = Marshal.ReadIntPtr(classptr);

            TypeBuilder builder = moduleBuilder.DefineType(targetInterface.Name + "_" + (IntPtr.Size * 8).ToString(),
                                                    TypeAttributes.Class, null, new Type[] { targetInterface });
            

            FieldBuilder fbuilder = builder.DefineField("ObjectAddress", typeof(IntPtr), FieldAttributes.Public);

            ClassJITInfo classInfo = new(targetInterface);

            for (int i = 0; i < classInfo.Methods.Count; i++)
            {
                IntPtr vtableMethod = Marshal.ReadIntPtr(vtable_ptr, IntPtr.Size * classInfo.Methods[i].VTableSlot);
                MethodJITInfo methodInfo = classInfo.Methods[i];
                EmitClassMethod(methodInfo, classptr, vtableMethod, builder, fbuilder);
            }

            Type implClass = builder.CreateType();
            generatedTypes[targetInterface] = implClass;
            return (TClass)GenerateClassForImplementor(implClass, classptr);
        }

        private static object GenerateClassForImplementor(Type implClass, IntPtr classptr) {
            object? instClass = Activator.CreateInstance(implClass);
            if (instClass == null) {
                throw new JITEngineException("Failed to CreateInstance of implClass");
            }

            FieldInfo addressField = implClass.GetField("ObjectAddress", BindingFlags.Public | BindingFlags.Instance)!;
            addressField.SetValue(instClass, classptr);

            return instClass;
        }

        private class MethodState
        {
            public struct RefArgLocal
            {
                public LocalBuilderEx builder;
                public int argIndex;
                public Type paramType;
            }

            public MethodState(MethodJITInfo method)
            {
                this.Method = method;
                MethodArgs = new();
                NativeArgs = new();
                unmanagedMemory = new();
                refargLocals = new();
            }

            public MethodJITInfo Method;
            public List<Type> MethodArgs;
            public Type? MethodReturn;

            public List<Type> NativeArgs;
            public Type? NativeReturn;

            public List<LocalBuilderEx> unmanagedMemory;
            public List<RefArgLocal> refargLocals;
        }

        private static void EmitClassMethod(MethodJITInfo method, IntPtr objectptr, IntPtr methodptr, TypeBuilder builder, FieldBuilder addressAssistant)
        {
            MethodState state = new(method);

            state.NativeArgs.Add(typeof(IntPtr)); // thisptr

            if (method.ReturnType.IsStringClass)
            {
                // special case for strings, we will marshal it in ourselves
                state.NativeReturn = typeof(IntPtr);
            }
            else
            {
                state.NativeReturn = method.ReturnType.NativeType;
            }

            state.MethodReturn = method.ReturnType.Type;

            foreach (TypeJITInfo typeInfo in method.Args)
            {
                // Handle managed args (should match interface declaration exactly)
                state.MethodArgs.Add(typeInfo.Type);

                // Handle unmanaged args (should match natives)
                if (typeInfo.IsStringClass)
                {
                    // we need to specially marshal strings
                    state.NativeArgs.Add(typeof(IntPtr));
                } else if (!typeInfo.IsParams) {
                    if (typeInfo.IsByRef && typeInfo.NativeType.IsValueType)
                    {
                        state.NativeArgs.Add(typeInfo.NativeType.MakeByRefType());
                    }
                    else
                    {
                        state.NativeArgs.Add(typeInfo.NativeType);
                    }
                }
            }

            var paramInfos = method.MethodInfo.GetParameters();
            MethodBuilder mbuilder = builder.DefineMethod(method.Name, MethodAttributes.Public | MethodAttributes.HideBySig | MethodAttributes.NewSlot | MethodAttributes.Virtual, CallingConventions.HasThis);
            if (method.ReturnType.IsGeneric)
            {
                // create generic param
                GenericTypeParameterBuilder[] gtypeParameters;
                gtypeParameters = mbuilder.DefineGenericParameters(new string[] { "TClass" });

                state.MethodReturn = gtypeParameters[0];
                gtypeParameters[0].SetGenericParameterAttributes(GenericParameterAttributes.ReferenceTypeConstraint);
            }

            mbuilder.SetSignature(state.MethodReturn, method.MethodInfo.ReturnParameter.GetRequiredCustomModifiers(), method.MethodInfo.ReturnParameter.GetOptionalCustomModifiers(), state.MethodArgs.ToArray(), paramInfos.Select(pi => pi.GetRequiredCustomModifiers()).ToArray(), paramInfos.Select(pi => pi.GetOptionalCustomModifiers()).ToArray());

            builder.DefineMethodOverride(mbuilder, method.MethodInfo);

            ILGeneratorEx ilgen = new(mbuilder, state.MethodArgs.ToArray());

            // Emit a call to ThrowIfRemotePipe if we have BlacklistedInCrossProcessIPCAttribute
            if (method.MethodInfo.GetCustomAttributes(typeof(BlacklistedInCrossProcessIPCAttribute), false).Any()) {
                ilgen.Call(typeof(InteropHelp).GetMethod(nameof(InteropHelp.ThrowIfRemotePipe))!);
            }

            // load object pointer
            ilgen.AddComment("Load native object pointer");
            ilgen.EmitPlatformLoad(objectptr);

            int argindex = 0;
            foreach (TypeJITInfo typeInfo in method.Args)
            {
                argindex++;

                // perform any conversions necessary
                if (typeInfo.NativeType != typeInfo.Type && typeInfo.IsByRef)
                {
                    LocalBuilderEx localArg = ilgen.DeclareLocal(typeInfo.NativeType);
                    localArg.SetLocalSymInfo("byrefarg" + argindex);

                    var helper = new MethodState.RefArgLocal
                    {
                        builder = localArg,
                        argIndex = argindex,
                        paramType = typeInfo.PierceType
                    };

                    state.refargLocals.Add(helper);

                    // Store the previous value if not an out var
                    if (!typeInfo.IsOut) {
                        ilgen.AddComment("Store previous value in refarg var for all ref non-out args");
                        ilgen.Ldarg(argindex);
                        ilgen.Ldobj(typeInfo.PierceType);
                        if (typeInfo.IsCustomValueType) {
                            ilgen.Call(typeInfo.CustomValueTypeToNativeTypeOperator);
                        } else {
                            ilgen.Ldtoken(typeInfo.NativeType);
                            ilgen.Call(typeof(Convert).GetMethod(nameof(Convert.ChangeType), new[] { typeof(object), typeof(Type) })!);
                        }

                        ilgen.Stloc(localArg.LocalIndex);
                    }

                    ilgen.Ldloca(localArg, true);
                }
                else if (typeInfo.NativeType != typeInfo.Type && !(typeInfo.Type.IsEnum || typeInfo.IsDelegate))
                {
                    ilgen.Ldarg(argindex);
                    if (typeInfo.IsCustomValueType) {
                        // Create backing type from the custom value type
                        ilgen.Call(typeInfo.CustomValueTypeToNativeTypeOperator);
                    } else {
                        var getvalueFunc = typeInfo.Type.GetMethod("GetValue");
                        if (getvalueFunc == null) {
                            throw new JITEngineException(typeInfo.Type + " does not implement GetValue");
                        }
                        ilgen.Call(getvalueFunc);
                    }
                }
                else
                {
                    ilgen.Ldarg(argindex);
                }
                
                if (typeInfo.IsStringClass && typeInfo.IsParams && method.HasParams)
                {
                    ilgen.Call(typeof(string).GetMethod(nameof(string.Format), BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string), typeof(object[]) }, null)!);
                }

                if (typeInfo.IsStringClass || typeInfo.IsParams)
                {
                    LocalBuilderEx localString = ilgen.DeclareLocal(typeof(GCHandle));
                    localString.SetLocalSymInfo("nativeString" + argindex);

                    state.unmanagedMemory.Add(localString);

                    // we need to specially marshal strings
                    ilgen.Ldloca(localString);
                    ilgen.Call(typeof(InteropHelp).GetMethod(nameof(InteropHelp.EncodeUTF8String))!);
                }
                else if (typeInfo.IsCreatableClass)
                {
                    // if this argument is a class we understand: get the object pointer
                    ilgen.Ldfld(addressAssistant);
                }
            }

            if (method.ReturnType.IsGeneric)
            {
                ilgen.Ldtoken(method.ReturnType.Type);
                ilgen.Call(typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle), BindingFlags.Static | BindingFlags.Public)!);
            }

            // load vtable method pointer
            ilgen.AddComment("load vtable method pointer");
            ilgen.EmitPlatformLoad(methodptr);

            CallingConvention ccv = CallingConvention.ThisCall;

            if (method.HasParams)
                ccv = CallingConvention.Cdecl;

            if (state.NativeReturn == typeof(bool))
                state.NativeReturn = typeof(byte);

            ilgen.Calli(ccv, state.NativeReturn, state.NativeArgs.ToArray());

            // populate byref args
            foreach (var local in state.refargLocals)
            {
                var paramTypeInfo = TypeJITInfo.FromType(local.paramType);
                // Load the argument we got from the native function
                ilgen.AddComment("Load arg of ptr type ");
                ilgen.Ldarg(local.argIndex);

                ilgen.AddComment("Load local variable of type " + local.builder.LocalType.Name);
                // Load the previously created field's address
                ilgen.Ldloc(local.builder.LocalIndex);

                if (paramTypeInfo.IsCustomValueType) {
                    ilgen.AddComment("Custom value type arg path");
                    ilgen.Call(paramTypeInfo.CustomValueTypeFromNativeTypeOperator);
                    // Store the new value object into the argument
                    ilgen.Stobj(local.paramType);
                } else {
                    // Create a new value type from it
                    ilgen.AddComment("Regular arg path for type " + local.builder.LocalType.ToString());
                    var ctor = local.paramType.GetConstructor(new Type[] { local.builder.LocalType });
                    if (ctor == null) {
                        throw new NullReferenceException("No constructor for " + local.paramType + " that takes " + local.builder.LocalType);
                    }

                    ilgen.Newobj(ctor);
                    // And store it's address to the local
                    // STIND_REF can be used to store TYP_INT, TYP_I_IMPL, TYP_REF, or TYP_BYREF (not value types)
                    ilgen.Stind_Ref();
                }

            }

            // clean up unmanaged memory
            foreach (LocalBuilderEx localbuilder in state.unmanagedMemory)
            {
                ilgen.Ldloca(localbuilder);
                ilgen.Call(typeof(InteropHelp).GetMethod(nameof(InteropHelp.FreeString))!);
            }

        if (method.ReturnType.IsCreatableClass)
            {
                if (method.ReturnType.IsGeneric)
                {
                    ilgen.Call(typeof(JITEngine).GetMethod(nameof(JITEngine.GenerateClass), BindingFlags.Static | BindingFlags.Public)!);
                }
                else if (method.ReturnType.IsDelegate)
                {
                    ilgen.Ldtoken(method.ReturnType.Type);
                    ilgen.Call(typeof(Type).GetMethod(nameof(Type.GetTypeFromHandle))!);
                    ilgen.Call(typeof(Marshal).GetMethod(nameof(Marshal.GetDelegateForFunctionPointer), BindingFlags.Static | BindingFlags.Public)!);
                    ilgen.Castclass(method.ReturnType.Type);
                }
                else
                {
                    ilgen.Call(typeof(JITEngine).GetMethod(nameof(JITEngine.GenerateClass), BindingFlags.Static | BindingFlags.Public)!.MakeGenericMethod(method.ReturnType.Type));
                }
            }
            else if (method.ReturnType.IsStringClass)
            {
                // marshal string return
                ilgen.Call(typeof(InteropHelp).GetMethod(nameof(InteropHelp.DecodeUTF8String))!);
            } else if (method.ReturnType.IsCustomValueType) {
                // Create custom value type from the backing type
                ilgen.Call(method.ReturnType.CustomValueTypeFromNativeTypeOperator);
            }

            ilgen.Return();
        }
    }
}
