using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Runtime.InteropServices;
using System.Text;

/// <summary>
/// Extended ILGenerator. Allows you to print the generated method.
/// TODO: validate args for functions as it gets generated to avoid a runtime segfault
/// </summary>
public class ILGeneratorEx {
    private struct CalliDBGData {
        public Type ReturnValue;
        public IEnumerable<Type> Arguments;
    }
    private StringBuilder ilDBGString = new();
    private ILGenerator ilgen;
    private List<LocalBuilderEx> locals = new();
    private MethodBuilder? mbuilder;
    private Type[]? methodParameters;

    public ILGeneratorEx(ILGenerator generator) {
        this.ilgen = generator;
    }

    public ILGeneratorEx(MethodBuilder mbuilder, Type[]? parameters) {
        this.mbuilder = mbuilder;
        this.methodParameters = parameters;
        this.ilgen = mbuilder.GetILGenerator();
    }

    private static readonly object NoArgs = new();

    private static string MethodToILStr(MethodBase mi, Type returnType) {
        return MethodToILStr(returnType, mi.DeclaringType?.Assembly.GetName().Name, mi.Name, mi.GetParameters().Select(e => e.ParameterType.ToString()));
    }

    private static string MethodToILStr(Type returnType, string? assemblyName, string? functionName, IEnumerable<string> paramTypes) {
        string assemblyNameStr = "";
        if (!string.IsNullOrEmpty(assemblyName)) {
            assemblyNameStr = $"[{assemblyName}]";
        } 

        string returnTypeStr = returnType.ToString();
        if (!string.IsNullOrEmpty(functionName) || !string.IsNullOrEmpty(assemblyName)) {
            returnTypeStr += " ";
        } 

        return $"{returnTypeStr}{assemblyNameStr}{functionName}({string.Join(", ", paramTypes)})";
    }

    private void AddOPCodeToDBGString(OpCode opcode, object args) {
        string argStr = "";
        try
        {
            if (object.ReferenceEquals(args, NoArgs)) {
                argStr = "";
            } else if (opcode == OpCodes.Call || opcode == OpCodes.Callvirt) {
                var mi = (MethodInfo)args;
                argStr = MethodToILStr(mi, mi.ReturnType);
            } else if (opcode == OpCodes.Calli) {
                if (args is CalliDBGData data) {
                    argStr = MethodToILStr(data.ReturnValue, "", "", data.Arguments.Select(e => e.ToString()));
                } else {
                    argStr = "<args not available: calli emitted outside Calli method>";
                }
            } else if (opcode == OpCodes.Ret) {
                argStr = "";
            } else if (opcode == OpCodes.Castclass) {
                argStr = ((Type)args).Name;
            } else if (opcode == OpCodes.Newobj) {
                argStr = MethodToILStr((ConstructorInfo)args, ((ConstructorInfo)args).DeclaringType!);
            } else if (opcode == OpCodes.Ldc_I4 || opcode == OpCodes.Ldc_I4_S || opcode == OpCodes.Ldc_I8 || opcode == OpCodes.Ldc_R4 || opcode == OpCodes.Ldc_R8) {
                argStr = args.ToString()!;
            } else if (opcode == OpCodes.Ldloca || opcode == OpCodes.Ldloca_S ) {
                LocalBuilderEx? local = locals.Find(e => e.LocalIndex == (int)args);
                if (local != null && !string.IsNullOrEmpty(local.LocalName)) {
                    argStr = local.LocalName;
                } else {
                    argStr = ((int)args).ToString();
                }
            } else if (opcode == OpCodes.Ldfld || opcode == OpCodes.Ldflda) {
                FieldInfo fieldInfo = (FieldInfo)args;
                argStr = fieldInfo.FieldType.Name + " " + (fieldInfo.DeclaringType != null ? fieldInfo.DeclaringType.Name + "::" : "") + fieldInfo.Name;
            } else {
                argStr = "<args not available: unsupported opcode " + opcode.ToString() + ">";
            }
        }
        catch (Exception e)
        {
            argStr = $"<args not available: error while converting {args.GetType()}:({e})>";
        }

        ilDBGString.AppendLine(string.Format("IL_{0,4:X4}", this.ilgen.ILOffset) + ": " + opcode.Name + " " + argStr);
    }

    public void Stobj(Type typeToStore) {
        this.Emit(OpCodes.Stobj, typeToStore);
    }

    public void Ldobj(Type typeToLoad) {
        this.Emit(OpCodes.Ldobj, typeToLoad);
    }

    public void Newobj(ConstructorInfo constructor) {
        this.Emit(OpCodes.Newobj, constructor);
    }

    public void Ldtoken(Type typeToMakeToken) {
        this.Emit(OpCodes.Ldtoken, typeToMakeToken);
    }

    public void Ldc_I4(int val) {
        switch (val)
        {
            case 0: this.Emit(OpCodes.Ldc_I4_0); break;
            case 1: this.Emit(OpCodes.Ldc_I4_1); break;
            case 2: this.Emit(OpCodes.Ldc_I4_2); break;
            case 3: this.Emit(OpCodes.Ldc_I4_3); break;
            case 4: this.Emit(OpCodes.Ldc_I4_4); break;
            case 5: this.Emit(OpCodes.Ldc_I4_5); break;
            case 6: this.Emit(OpCodes.Ldc_I4_6); break;
            case 7: this.Emit(OpCodes.Ldc_I4_7); break;
            case 8: this.Emit(OpCodes.Ldc_I4_8); break;
            case -1: this.Emit(OpCodes.Ldc_I4_M1); break;
            default: this.Emit(OpCodes.Ldc_I4, val); break;
        }
    }

    public void Return() {
        this.Emit(OpCodes.Ret);
    }

    public void Call(MethodInfo method) {
        this.Emit(OpCodes.Call, method);
    }

    public void Callvirt(MethodInfo method) {
        this.Emit(OpCodes.Callvirt, method);
    }

    public LocalBuilderEx DeclareLocal(Type localType, bool pinned = false) {
        var local = new LocalBuilderEx(this.ilgen.DeclareLocal(localType, pinned));
        locals.Add(local);
        return local;
    } 

    public void Ldloc(LocalBuilderEx local, bool shortForm = false) {
        this.Ldloc(local.LocalIndex, shortForm);
    }

    public void Ldloca(LocalBuilderEx local, bool shortForm = false) {
        this.Ldloca(local.LocalIndex, shortForm);
    }

    public void Ldstr(string value) {
        this.Emit(OpCodes.Ldstr, value);
    }

    public void Ldfld(FieldInfo field) {
        this.Emit(OpCodes.Ldfld, field);
    }

    public void Stind_Ref() {
        this.Emit(OpCodes.Stind_Ref);
    }

    public void Box(Type typeToBox) {
        this.Emit(OpCodes.Box, typeToBox);
    }

    public void Ldloca(int localIndex, bool shortForm = false) {
        OpCode code;
        if (shortForm) {
            code = OpCodes.Ldloca_S;
        } else {
            code = OpCodes.Ldloca;
        }
        
        this.Emit(code, localIndex);
    }

    public void Ldloc(int localIndex, bool shortForm = false) {
        OpCode code;
        if (shortForm) {
            code = OpCodes.Ldloc_S;
        } else {
            code = OpCodes.Ldloc;
        }
        
        this.Emit(code, localIndex);
    }

    public void Castclass(Type castTypeTarget) {
        this.Emit(OpCodes.Castclass, castTypeTarget);
    }

    public void Calli(CallingConvention ccv, Type? returnType, Type[]? parameterTypes) {
        this.ilgen.EmitCalli(OpCodes.Calli, ccv, returnType, parameterTypes);
        if (returnType == null) {
            returnType = typeof(void);
        }

        this.AddOPCodeToDBGString(OpCodes.Calli, new CalliDBGData() {ReturnValue = returnType, Arguments = parameterTypes == null ? [] : parameterTypes});
    }

    public void Ldarg(int index)
    {
        switch (index)
        {
            case 0: this.Emit(OpCodes.Ldarg_0); break;
            case 1: this.Emit(OpCodes.Ldarg_1); break;
            case 2: this.Emit(OpCodes.Ldarg_2); break;
            case 3: this.Emit(OpCodes.Ldarg_3); break;
            default: this.Emit(OpCodes.Ldarg_S, (byte)index); break;
        }
    }

    public void Ldloc(int index)
    {
        switch (index)
        {
            case 0: this.Emit(OpCodes.Ldloc_0); break;
            case 1: this.Emit(OpCodes.Ldloc_1); break;
            case 2: this.Emit(OpCodes.Ldloc_2); break;
            case 3: this.Emit(OpCodes.Ldloc_3); break;
            default: this.Emit(OpCodes.Ldloc_S, (byte)index); break;
        }
    }

    public void Stloc(int index)
    {
        switch (index)
        {
            case 0: this.Emit(OpCodes.Stloc_0); break;
            case 1: this.Emit(OpCodes.Stloc_1); break;
            case 2: this.Emit(OpCodes.Stloc_2); break;
            case 3: this.Emit(OpCodes.Stloc_3); break;
            default: this.Emit(OpCodes.Stloc_S, (byte)index); break;
        }
    }

    public void EmitPlatformLoad(IntPtr pointer)
    {
        switch (IntPtr.Size)
        {
            case 4: this.Emit(OpCodes.Ldc_I4, (int)pointer.ToInt32()); break;
            case 8: this.Emit(OpCodes.Ldc_I8, (long)pointer.ToInt64()); break;
            default: throw new Exception("Bad IntPtr size");
        }

        this.Emit(OpCodes.Conv_I);
    }

    public void Emit(OpCode opcode, Type cls) {
        this.AddOPCodeToDBGString(opcode, cls);
        this.ilgen.Emit(opcode, cls);
    }

    public void Emit(OpCode opcode, string str) {
        this.AddOPCodeToDBGString(opcode, str);
        this.ilgen.Emit(opcode, str);
    }

    public void Emit(OpCode opcode, float arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, sbyte arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, MethodInfo meth) {
        this.AddOPCodeToDBGString(opcode, meth);
        this.ilgen.Emit(opcode, meth);
    }

    public void Emit(OpCode opcode, FieldInfo field) {
        this.AddOPCodeToDBGString(opcode, field);
        this.ilgen.Emit(opcode, field);
    }

    public void Emit(OpCode opcode, Label[] labels) {
        this.AddOPCodeToDBGString(opcode, labels);
        this.ilgen.Emit(opcode, labels);
    }
    
    public void Emit(OpCode opcode, SignatureHelper signature) {
        this.AddOPCodeToDBGString(opcode, signature);
        this.ilgen.Emit(opcode, signature);
    }
    
    public void Emit(OpCode opcode, LocalBuilder local) {
        this.AddOPCodeToDBGString(opcode, local);
        this.ilgen.Emit(opcode, local);
    }

    public void Emit(OpCode opcode, ConstructorInfo con) {
        this.AddOPCodeToDBGString(opcode, con);
        this.ilgen.Emit(opcode, con);
    }

    public void Emit(OpCode opcode, long arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, int arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, short arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, double arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode, byte arg) {
        this.AddOPCodeToDBGString(opcode, arg);
        this.ilgen.Emit(opcode, arg);
    }

    public void Emit(OpCode opcode) {
        this.AddOPCodeToDBGString(opcode, NoArgs);
        this.ilgen.Emit(opcode);
    }

    public void Emit(OpCode opcode, Label label) {
        this.AddOPCodeToDBGString(opcode, label);
        this.ilgen.Emit(opcode, label);
    }

    // Adds a C#-style comment to the IL debug text
    public void AddComment(string comment) {
        this.ilDBGString.AppendLine("// " + comment);
    }

    public void EmitWriteLine(string value) {
        this.Ldstr(value);
        this.Call(typeof(Console).GetMethod(nameof(Console.WriteLine), new[] { typeof(string) })!);
    }

    /// <summary>
    /// Prints the generated IL as text in a human-readable format.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        StringBuilder finalStr = new();
        string linePrefix = "";
        if (this.mbuilder != null) {
            if (methodParameters == null) {
                methodParameters = Array.Empty<Type>();
            }

            string methodFlags = "";
            var attr = mbuilder.Attributes;
            if (attr.HasFlag(MethodAttributes.Public)) {
                methodFlags += " public";
            }

            if (attr.HasFlag(MethodAttributes.Private)) {
                methodFlags += " private";
            }

            if (attr.HasFlag(MethodAttributes.Final)) {
                methodFlags += " final";
            }

            if (attr.HasFlag(MethodAttributes.HideBySig)) {
                methodFlags += " hidebysig";
            }

            if (attr.HasFlag(MethodAttributes.Static)) {
                methodFlags += " static";
            }
            
            if (attr.HasFlag(MethodAttributes.Virtual)) {
                methodFlags += " virtual";
            }

            if (attr.HasFlag(MethodAttributes.NewSlot)) {
                methodFlags += " newslot";
            }

            finalStr.AppendLine(".method" + methodFlags + " " + mbuilder.ReturnType.Name + " " + mbuilder.Name + "(" + string.Join(", ", methodParameters.Select(e => e.Name)) + ") <cil and managed info unknown> {");
            linePrefix = "    ";
        }

        finalStr.AppendLine(linePrefix+".locals init (");
        foreach (var item in this.locals)
        {
            string nameStr = "";
            string valueTypeStr = "";
            if (!string.IsNullOrEmpty(item.LocalName)) {
                nameStr = " " + item.LocalName;
            }

            if (item.LocalType.IsValueType) {
                valueTypeStr = "valuetype ";
            }

            finalStr.AppendLine(linePrefix+$"    [{item.LocalIndex}] {valueTypeStr}{item.LocalType}{nameStr}");
        }
        finalStr.AppendLine(linePrefix+")");
        finalStr.AppendLine(linePrefix+"");
        foreach (var item in ilDBGString.ToString().Split(Environment.NewLine))
        {
            finalStr.AppendLine(linePrefix+item);
        }

        if (this.mbuilder != null) {
            finalStr.AppendLine("}");
        }

        return finalStr.ToString();
    }
}