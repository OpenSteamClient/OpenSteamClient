using System;
using System.Collections.Generic;
using Installer.Extensions;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Installer.Views;

public partial class ChooseInstallDirectoryPage : UserControl
{
    public ChooseInstallDirectoryPage()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}