using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using Avalonia.Controls;
using Avalonia.VisualTree;
using ClientUI.Extensions;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks;
using OpenSteamworks.Attributes;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.Utils;

namespace ClientUI.Views;

public partial class InterfaceDebugger : Window
{
    private string ifaceName = "";
    public InterfaceDebugger(Type iface) : base()
    {
        InitializeComponent();
        var stackpanel = this.FindControl<StackPanel>("StackPanel");
        UtilityFunctions.AssertNotNull(stackpanel);

        this.Title = iface.Name + " Debugger";
        this.ifaceName = iface.Name;

        var funcs = iface.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        int i = 0;
        foreach (var func in funcs)
        {
            string funcidentifier = iface.Name + "_" + funcs[i].Name + funcs[i].GetParameters().Length;
            var columndef = CalculateDefinitions(func.GetParameters().Length + 1);
            Grid grid = new()
            {
                ColumnDefinitions = ColumnDefinitions.Parse(columndef),
                RowDefinitions = RowDefinitions.Parse("*"),
            };

            grid.Children.Add(new Button() 
            {
                Name = funcidentifier,
                Content = funcs[i].Name,
                Command = new RelayCommand<Tuple<Type,MethodInfo>>(this.MethodCalled),
                CommandParameter = new Tuple<Type,MethodInfo>(iface,  funcs[i]),
                [Grid.ColumnProperty] = 0
            });
            i++;

            int addedargs = 0;
            foreach (var param in func.GetParameters())
            {
                grid.Children.Add(new TextBox()
                {
                    Name = funcidentifier + "_Arg"+addedargs,
                    [Grid.ColumnProperty] = addedargs+1,
                    Watermark = param.ParameterType.Name + " " + param.Name
                });
                addedargs++;
            }
            
            stackpanel.Children.Add(grid);
        }
    }
    private string CalculateDefinitions(int count) {
        string columndef = "";
        for (int y = 0; y < count; y++)
        {
            columndef += "*" + (y+1 < count ? "," : "");
        }
        return columndef;
    }
    public void MethodCalled(Tuple<Type,MethodInfo>? info) {
        UtilityFunctions.AssertNotNull(info);
        string funcidentifier = ifaceName + "_" + info.Item2.Name + info.Item2.GetParameters().Length;
        var implementer = GetInterfaceImpl(info.Item1);
        List<object?> paramArr = new();
        Dictionary<int, ParameterInfo> refParams = new();
        var paramInfos = info.Item2.GetParameters();
        for (int i = 0; i < paramInfos.Length; i++)
        {
            var paramInfo = paramInfos[i];
            ParameterInfo? nextParamInfo = null;
            if (i+1 < paramInfos.Length) {
                nextParamInfo = paramInfos[i+1];
            }

            var paramIdentifier = funcidentifier + "_Arg" + i;
            var nextParamIdentifier = funcidentifier + "_Arg" + (i+1);
            Console.WriteLine("trying to find " + paramIdentifier);
            Console.WriteLine("next identifier" + nextParamIdentifier);
            var paramTextbox = this.FindControlNested<TextBox>(paramIdentifier);
            var nextParamTextbox = this.FindControlNested<TextBox>(nextParamIdentifier);
            UtilityFunctions.AssertNotNull(paramTextbox);

            var paramCurrentText = paramTextbox.Text;

            try {
                bool isStruct = false;
                Type pierceType = paramInfo.ParameterType.IsByRef ? paramInfo.ParameterType.GetElementType()! : paramInfo.ParameterType;
                bool isCustomValueType = pierceType.GetCustomAttribute<CustomValueTypeAttribute>() != null;
                Type? customValueType = null;

                if (isCustomValueType) {
                    customValueType = UtilityFunctions.AssertNotNull(pierceType.GetField("_value", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)).FieldType;
                }

                if (string.IsNullOrEmpty(paramCurrentText)) {
                    if (pierceType == typeof(string) || pierceType == typeof(StringBuilder) || paramInfo.IsOut) {
                        paramCurrentText = "";
                    } else {
                        MessageBox.Error("Function execution failed", "Failed to execute " + funcidentifier + "\n" + "Required argument " + paramInfo.Name + " missing!");
                        return;
                    }
                }

                if (paramInfo.IsOut || paramInfo.ParameterType.IsByRef) {
                    refParams.Add(paramArr.Count, paramInfo);
                }

                if (pierceType.IsValueType && !pierceType.IsPrimitive && !pierceType.IsEnum) {
                    isStruct = true;
                }

                if (paramInfo.IsOut) {
                    paramArr.Add(null);
                    continue;
                }

                if (isCustomValueType) {
                    // Custom value type, convert to int and then run implicit operator
                    paramArr.Add(UtilityFunctions.AssertNotNull(pierceType.GetMethod("op_Implicit", new [] {customValueType!})).Invoke(null, new [] { Convert.ChangeType(paramCurrentText, customValueType!) }));
                    continue;
                }

                if (isStruct && !isCustomValueType) {
                    // This is a struct, find InterfaceDebuggerSupport and run it
                    MethodInfo? ci;
                    ci = pierceType.GetMethod("InterfaceDebuggerSupport", BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public, new Type[1] {
                        typeof(string)
                    });

                    if (ci != null)
                    {
                        paramArr.Add(ci.Invoke(null, new object[1] { paramCurrentText }));
                        continue;
                    }

                    throw new NullReferenceException(pierceType.Name + "doesn't take a string");
                }

                if (pierceType == typeof(StringBuilder)) {
                    UtilityFunctions.AssertNotNull(nextParamTextbox);
                    UtilityFunctions.AssertNotNull(nextParamTextbox.Text);
                    refParams.Add(paramArr.Count, paramInfo);
                    paramArr.Add(new StringBuilder(int.Parse(nextParamTextbox.Text)));
                    continue;
                }

                // Enums need special handling...
                if (pierceType.IsEnum) {
                    paramArr.Add(Enum.Parse(pierceType, paramCurrentText));
                    continue;
                }

                paramArr.Add(Convert.ChangeType(paramCurrentText, pierceType));
            } catch (Exception e) {
                MessageBox.Error("Function execution failed", "Failed to execute " + funcidentifier + ": Conversion failed with param " + paramInfo.Name + Environment.NewLine + e.ToString());
                return;
            }
        }

        object? ret = info.Item2.Invoke(implementer, paramArr.ToArray());
        string refParamsAsStr = "";
        foreach (var tuple in refParams)
        {
            refParamsAsStr += tuple.Value.Name + ": " + paramArr[tuple.Key] + Environment.NewLine;
        }

        MessageBox.Show("Function executed successfully", info.Item2.Name + " returned " + ret + Environment.NewLine + refParamsAsStr);
    }

    private static object GetInterfaceImpl(Type iface) {
        var client = AvaloniaApp.Container.Get<SteamClient>();
        UtilityFunctions.AssertNotNull(client);
        var jitAssembly = GetJITAssembly();
        var implementorFields = typeof(OpenSteamworks.Native.ClientNative).GetFields().Where(f => f.FieldType == iface);
        if (!implementorFields.Any()) {
            throw new NotSupportedException("This interface is not implemented in ClientNative");
        }
        var implementorField = implementorFields.First();
        
        return UtilityFunctions.AssertNotNull(implementorField.GetValue(client.NativeClient));
    }

    private static Assembly GetJITAssembly()
    {
        var jitAssembly = AppDomain.CurrentDomain.GetAssemblies().
            SingleOrDefault(assembly => assembly.GetName().Name == "OpenSteamworksJIT");
        
        UtilityFunctions.AssertNotNull(jitAssembly);

        return jitAssembly;
    }
}
