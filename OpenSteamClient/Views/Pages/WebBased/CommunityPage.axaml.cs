using System;
using System.Collections.Generic;
using OpenSteamClient.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using OpenSteamClient.Controls;

namespace OpenSteamClient.Views;

public partial class CommunityPage : BaseWebPage
{
    public CommunityPage() : base()
    {
        this.URL = "https://steamcommunity.com";
        this.SetSteamCookies = true;
    }
}