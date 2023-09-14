using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ClientUI.Views;

public partial class WebviewPage : UserControl
{
    public WebviewPage()
    {
        InitializeComponent();
        this.TranslatableInit();
        //TODO: how to implement webviews in Avalonia?
        // Should we use SteamHTML/Steamwebhelper?
        // Should we use CEFSharp?
        // Should we use something else?
        // Should we home cook our own CEF bindings?
        // Could we use Firefox's engine instead?
    }
}