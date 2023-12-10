using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Controls.Primitives;
using Avalonia;

namespace ClientUI.Controls;

public class BasePage : UserControl
{
    public BasePage() : base() {} 

    public virtual void Free() {
        // No-op in BasePage
    }
}