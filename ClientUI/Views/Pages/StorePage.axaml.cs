using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ClientUI.Views;

public partial class StorePage : UserControl
{
    public StorePage()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}