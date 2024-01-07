using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientUI.Controls;
using Avalonia.Media.Imaging;
using OpenSteamworks.Structs;
using OpenSteamworks.Client.Apps;

namespace ClientUI.Views.Library;

public partial class FocusedAppPane : BasePage
{
    public FocusedAppPane() : base()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}