using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Avalonia.Controls;
using CommunityToolkit.Mvvm.Input;
using OpenSteamworks.Utils;

namespace ClientUI.Views;

public partial class InterfaceList : Window
{
    private Dictionary<string, Type> knownInterfaces = new();
    private readonly StackPanel stackpanel;
    public InterfaceList()
    {
        InitializeComponent();
        var stackpanel2 = this.FindControl<StackPanel>("StackPanel");
        UtilityFunctions.AssertNotNull(stackpanel2);
        stackpanel = stackpanel2;

        var jit = GetAssemblyByName("OpenSteamworksJIT");
        var osw = GetAssemblyByName("OpenSteamworks");

        UtilityFunctions.AssertNotNull(osw);
        UtilityFunctions.AssertNotNull(jit);

        var validInterfaces = osw.GetTypes().Where(type => (type.Name.StartsWith("IClient") || type.Name.StartsWith("ISteam")) && type.IsInterface);
        List<Button> buttons = new();
        foreach (var type in validInterfaces)
        {
            var hasField = HasFieldInClientNative(type);
            knownInterfaces.Add(type.Name, type);
            buttons.Add(new Button()
            {
                Content = type.Name + (!hasField ? " (Not implemented in ClientNative)" : ""),
                CommandParameter = type.Name,
                Command = new RelayCommand<string>(this.ButtonClicked),
                IsEnabled = hasField
            });
        }
        
        int i = 0;
        do
        {
            Button button1 = buttons[i];
            i++;
            Button? button2 = null;
            if (buttons.Count > i) {
                button2 = buttons[i];
                i++;
            }

            Grid grid = new()
            {
                ColumnDefinitions = ColumnDefinitions.Parse("*,*"),
                RowDefinitions = RowDefinitions.Parse("*"),
            };
            
            stackpanel.Children.Add(grid);
            
            button1[Grid.ColumnProperty] = 0;
            grid.Children.Add(button1);

            if (button2 != null) {
                button2[Grid.ColumnProperty] = 1;
                grid.Children.Add(button2);
            }
        } while (i < buttons.Count);
    }
    public void ButtonClicked(string? interfacename) {
        UtilityFunctions.AssertNotNull(interfacename);
        Type iface = knownInterfaces[interfacename];
        var debugger = new InterfaceDebugger(iface);
        debugger.Show();
    }
    private bool HasFieldInClientNative(Type iface) {
        var implementorFields = typeof(OpenSteamworks.Native.ClientNative).GetProperties().Where(f => f.PropertyType == iface);
        return implementorFields.Count() != 0;
    }
    
    private Assembly? GetAssemblyByName(string name)
    {
        return AppDomain.CurrentDomain.GetAssemblies().
            SingleOrDefault(assembly => assembly.GetName().Name == name);
    }
}