using System;
using System.Collections.Generic;
using OpenSteamClient.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OpenSteamClient.Controls;

namespace OpenSteamClient.Views;

public partial class LibraryPage : BasePage
{
    public LibraryPage() : base()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}