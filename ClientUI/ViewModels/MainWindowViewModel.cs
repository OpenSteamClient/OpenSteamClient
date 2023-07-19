using System;

namespace ClientUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public string Greeting => "Welcome to Avalonia!";
    public void Crash() {
        throw new Exception("test");
    }
}
