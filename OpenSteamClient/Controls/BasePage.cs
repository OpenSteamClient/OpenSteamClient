using System;
using System.Collections.Generic;
using OpenSteamClient.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using Avalonia;

namespace OpenSteamClient.Controls;

public class BasePage : UserControl
{
    public BasePage() : base() { }

    public virtual void Free()
    {
        // No-op in BasePage
    }
}