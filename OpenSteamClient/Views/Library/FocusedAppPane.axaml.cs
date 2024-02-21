using System;
using System.Collections.Generic;
using OpenSteamClient.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OpenSteamClient.Controls;
using Avalonia.Media.Imaging;
using OpenSteamworks.Structs;
using OpenSteamworks.Client.Apps;

namespace OpenSteamClient.Views.Library;

public partial class FocusedAppPane : BasePage
{
    public FocusedAppPane() : base()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}