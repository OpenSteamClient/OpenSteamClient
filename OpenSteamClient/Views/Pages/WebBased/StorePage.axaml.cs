using System;
using System.Collections.Generic;
using OpenSteamClient.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OpenSteamClient.Controls;

namespace OpenSteamClient.Views;

public partial class StorePage : BaseWebPage
{
    public StorePage() : base()
    {
        this.URL = "https://store.steampowered.com";
        this.SetSteamCookies = true;
    }
}