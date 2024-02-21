using System.IO;
using Avalonia.Controls;
using OpenSteamClient.Extensions;
using OpenSteamClient.ViewModels;
using QRCoder;

namespace OpenSteamClient.Views;

public partial class AccountPickerWindow : Window
{
    public AccountPickerWindow()
    {
        InitializeComponent();
        this.TranslatableInit();
    }

}