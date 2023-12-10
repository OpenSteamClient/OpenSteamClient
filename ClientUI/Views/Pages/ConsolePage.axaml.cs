using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientUI.Controls;

namespace ClientUI.Views;

public partial class ConsolePage : BasePage
{
    public ConsolePage() : base()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}