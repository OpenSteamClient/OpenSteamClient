using System;
using System.Collections.Generic;
using Autofac;
using ClientUI.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace ClientUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.TryTranslateSelf();
    }
}