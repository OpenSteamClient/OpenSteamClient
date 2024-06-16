using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Generated;

namespace OpenSteamClient.ViewModels;

public partial class LibraryFolderViewModel : AvaloniaCommon.ViewModelBase {
    public LibraryFolder_t ID { get; init; }
    public string Name { get; init; }
    public string Path { get; init; }

    [ObservableProperty]
    private int installedApps;

    public LibraryFolderViewModel(ClientApps clientApps, LibraryFolder_t folderID) {
        ID = folderID;
        Name = clientApps.GetLibraryFolderLabel(folderID);
        Path = clientApps.GetLibraryFolderPath(folderID);
        InstalledApps = clientApps.GetNumAppsInFolder(folderID);
    }

    public static List<LibraryFolderViewModel> GetLibraryFolders(ClientApps clientApps) {
        var list = new List<LibraryFolderViewModel>();
        for (int i = 0; i < clientApps.GetNumLibraryFolders(); i++)
        {
            if (!clientApps.NativeClientAppManager.BGetLibraryFolderInfo(i, out bool mounted, out _, out _)) {
                continue;
            }

            if (!mounted) {
                continue;
            }

            list.Add(new LibraryFolderViewModel(clientApps, i));
        }

        return list;
    }
}