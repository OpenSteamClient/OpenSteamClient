using Avalonia.Controls;
using ClientUI.Extensions;
using ClientUI.ViewModels;
using OpenSteamworks.Client.Utils;

namespace ClientUI.Views;

public partial class SecondFactorNeededDialog : Window
{
    public SecondFactorNeededDialog()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}