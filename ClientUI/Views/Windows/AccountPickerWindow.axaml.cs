using System.IO;
using Avalonia.Controls;
using ClientUI.Extensions;
using ClientUI.ViewModels;
using QRCoder;

namespace ClientUI.Views;

public partial class AccountPickerWindow : Window
{
    public AccountPickerWindow()
    {
        InitializeComponent();
        this.TranslatableInit();
    }

}