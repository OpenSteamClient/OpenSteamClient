using System;
using System.Collections.Generic;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using ClientUI.Controls;

namespace ClientUI.Views;

public partial class StorePage : BaseWebPage
{
    public StorePage() : base()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}