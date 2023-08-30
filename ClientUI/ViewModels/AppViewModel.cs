using System;
using System.Threading.Tasks;

namespace ClientUI.ViewModels;

public class AppViewModel : ViewModelBase {
    public async Task Exit() {
        // This is stupid. TODO: Pending support for "await?" to clean up.
        await (App.Current == null ? Task.CompletedTask : App.Current.Exit(1));
    }

    public void OpenInterfaceList() {
        App.Current?.OpenInterfaceList();
    }
}