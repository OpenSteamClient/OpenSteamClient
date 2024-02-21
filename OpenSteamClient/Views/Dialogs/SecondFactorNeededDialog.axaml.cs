using Avalonia.Controls;
using OpenSteamClient.Extensions;

namespace OpenSteamClient.Views;

public partial class SecondFactorNeededDialog : Window
{
    public SecondFactorNeededDialog()
    {
        InitializeComponent();
        this.TranslatableInit();
    }
}