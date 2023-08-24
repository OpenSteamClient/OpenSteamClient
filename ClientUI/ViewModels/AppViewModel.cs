using System;
using System.Threading.Tasks;

namespace ClientUI.ViewModels;

public class AppViewModel : ViewModelBase {
    public async Task Exit() {
        await App.Current?.Exit();
    }
}