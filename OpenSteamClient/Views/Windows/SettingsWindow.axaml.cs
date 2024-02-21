using System;
using System.Collections.Generic;
using OpenSteamClient.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace OpenSteamClient.Views;

public partial class SettingsWindow : Window
{
    public SettingsWindow()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}