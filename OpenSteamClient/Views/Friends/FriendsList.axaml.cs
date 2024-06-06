using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using OpenSteamClient.Extensions;

namespace OpenSteamClient.Views.Friends;

public partial class FriendsList : Window
{
    public FriendsList()
    {
        InitializeComponent();
        this.TranslatableInit();
    }

    public void HandlePointerPressed(object sender, RoutedEventArgs e) {
        Console.WriteLine("OnPointerPressed");
    }
    
    private void TestOnPointerReleased(object sender, RoutedEventArgs e) {

    }
}