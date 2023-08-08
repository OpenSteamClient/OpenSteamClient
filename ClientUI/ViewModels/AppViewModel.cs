using System;

namespace ClientUI.ViewModels;

public class AppViewModel : ViewModelBase {
    public void Exit() {
        App.Current?.Exit();
    }
}