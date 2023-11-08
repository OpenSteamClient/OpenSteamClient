using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using OpenSteamworks.Attributes;

namespace OpenSteamworks.Native.JIT
{
    class JITInfoException : Exception
    {
        public JITInfoException(string message) : base(message) { }
    }

    class TypeJITInfo
    {
        public Type Type { get; private set; }
        public Type PierceType { get; private set; }
        public Type NativeType { get; private set; }

        public bool IsParams { get; internal set; }

        public static TypeJITInfo FromType(Type type) {
            return new TypeJITInfo(type);
        }
        
        public TypeJITInfo(Type type)
        {
            Type = type;
            PierceType = Type.IsByRef ? Type.GetElementType()! : Type;
            IsParams = false;
            DetermineProps();
        }

        public MethodInfo? CustomValueTypeFromNativeTypeOperator { get; private set; }
        public MethodInfo? CustomValueTypeToNativeTypeOperator { get; private set; }

        public bool IsArray { get { return Type.IsArray; } }
        public bool IsStringClass { get { return Type.GetTypeCode(Type) == TypeCode.String; } }
        public bool IsAutoClass { get { return Type == typeof(StringBuilder); } }
        [MemberNotNullWhen(true, nameof(CustomValueTypeFromNativeTypeOperator))]
        [MemberNotNullWhen(true, nameof(CustomValueTypeToNativeTypeOperator))]
        public bool IsCustomValueType { get { return PierceType.IsValueType && PierceType.GetCustomAttribute<CustomValueTypeAttribute>(false) != null; } }
        public bool IsUnknownClass { get { return PierceType.IsClass && !TypeJITInfo.FromType(PierceType).IsStringClass; } }
        public bool IsCreatableClass { get { return IsGeneric || Type.IsInterface || IsDelegate; } }
        public bool IsGeneric { get { return Type.IsGenericParameter; } }
        public bool IsDelegate { get { return Type.IsSubclassOf(typeof(MulticastDelegate)); } }
        public bool IsOut { get { return Type.GetCustomAttribute<OutAttribute>() != null; } }
        public bool IsByRef { get { return Type.IsByRef; } }
        public bool IsUnsafePtr { get { return Type.IsPointer; } }

        // determine whether this type will fit in a register and what the native type should be
        [MemberNotNull(nameof(NativeType))]
        public void DetermineProps()
        {
            // strings and arrays.
            if (IsStringClass || IsArray || IsAutoClass || IsUnsafePtr)
            {
                NativeType = Type;
                return;
            }

            // for a generic return or interface (to construct) return an IntPtr
            if (IsCreatableClass)
            {
                NativeType = typeof(IntPtr);
                return;
            }

            if (IsCustomValueType) {
                var customValueTypeAttrib = PierceType.GetCustomAttribute<CustomValueTypeAttribute>(false);
                if (customValueTypeAttrib != null) {
                    var _valueField = PierceType.GetField("_value", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                    if (_valueField == null) {
                        throw new InvalidOperationException(PierceType.FullName + " has CustomValueTypeAttribute but doesn't have a _value field.");
                    }

                    NativeType = _valueField.FieldType;

                    CustomValueTypeFromNativeTypeOperator = PierceType.GetMethod("op_Implicit", new[] { NativeType });
                    CustomValueTypeToNativeTypeOperator = PierceType.GetMethod("op_Implicit", new[] { PierceType });
                    if (CustomValueTypeFromNativeTypeOperator == null || CustomValueTypeToNativeTypeOperator == null) {
                        throw new InvalidOperationException(PierceType.FullName + " has CustomValueTypeAttribute but doesn't implement the correct implicit operators.");
                    }

                    return;
                }
            } else if (IsUnknownClass) {
                // for a class (not a value type) we need to figure out what to do, CSteamID might implement InteropHelp.NativeType for example to tell us the value type
                var nativeAttribs = PierceType.GetCustomAttributes(typeof(NativeTypeAttribute), false);
                
                if (nativeAttribs.Length > 0)
                {
                    NativeType = ((NativeTypeAttribute)nativeAttribs[0]).NativeType;
                }
                else
                {
                    SteamClient.JITLogger.Error("Don't know what to do with type " + Type);
                    SteamClient.JITLogger.Error("IsArray: " + IsArray);
                    SteamClient.JITLogger.Error("IsStringClass: " + IsStringClass);
                    SteamClient.JITLogger.Error("IsAutoClass: " + IsAutoClass);
                    SteamClient.JITLogger.Error("IsUnknownClass: " + IsUnknownClass);
                    SteamClient.JITLogger.Error("IsCreatableClass: " + IsCreatableClass);
                    SteamClient.JITLogger.Error("IsGeneric: " + IsGeneric);
                    SteamClient.JITLogger.Error("IsDelegate: " + IsDelegate);
                    SteamClient.JITLogger.Error("IsByRef: " + IsByRef);

                    var pt = new TypeJITInfo(PierceType);
                    SteamClient.JITLogger.Error("PierceType.IsArray: " + pt.IsArray);
                    SteamClient.JITLogger.Error("PierceType.IsStringClass: " + pt.IsStringClass);
                    SteamClient.JITLogger.Error("PierceType.IsAutoClass: " + pt.IsAutoClass);
                    SteamClient.JITLogger.Error("PierceType.IsUnknownClass: " + pt.IsUnknownClass);
                    SteamClient.JITLogger.Error("PierceType.IsCreatableClass: " + pt.IsCreatableClass);
                    SteamClient.JITLogger.Error("PierceType.IsGeneric: " + pt.IsGeneric);
                    SteamClient.JITLogger.Error("PierceType.IsDelegate: " + pt.IsDelegate);
                    SteamClient.JITLogger.Error("PierceType.IsByRef: " + pt.IsByRef);
                    SteamClient.JITLogger.Error("PierceType=" + PierceType);

                    throw new JITInfoException("Not sure what to do with this type: " + Type);
                }

                return;
            }
            else if (Type.IsEnum)
            {
                NativeType = Enum.GetUnderlyingType(Type);
                return;
            }

            // otherwise, native type is the type
            NativeType = Type;

            // byref won't have a size
            if (IsByRef)
            {
                return;
            }
        }
    }

    class MethodJITInfo
    {
        public int VTableSlot { get; private set; }
        public TypeJITInfo ReturnType { get; private set; }
        public List<TypeJITInfo> Args { get; private set; }
        public string Name { get; private set; }
        public MethodInfo MethodInfo { get; private set; }

        public bool HasParams { get; private set; }

        public MethodJITInfo(int slot, MethodInfo method)
        {
            VTableSlot = slot;
            ReturnType = new TypeJITInfo(method.ReturnType);
            Name = method.Name;
            MethodInfo = method;

            Args = new List<TypeJITInfo>();

            foreach (ParameterInfo paramInfo in method.GetParameters())
            {
                TypeJITInfo typeInfo = new(paramInfo.ParameterType);

                if (paramInfo.GetCustomAttributes(typeof(ParamArrayAttribute), false).Length > 0)
                {
                    HasParams = true;
                    typeInfo.IsParams = true;
                }

                Args.Add(typeInfo);
            }
        }
    }

    class ClassJITInfo
    {
        public List<MethodJITInfo> Methods { get; private set; }

        public ClassJITInfo(Type classType)
        {
            MethodInfo[] methods = classType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);

            Methods = new List<MethodJITInfo>(methods.Length);

            for (int i = 0; i < methods.Length; i++)
            {
                Methods.Add(new MethodJITInfo(i, methods[i]));
            }
        }
    }
}
