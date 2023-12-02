using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Controls;
using ClientUI.Translation;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Utils;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Protobuf;
using OpenSteamworks.Protobuf.WebUI;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool showGoOffline = false;

    [ObservableProperty]
    private bool showGoOnline = false;

    [ObservableProperty]
    private Control currentPage;
    public bool CanLogonOffline => client.NativeClient.IClientUser.CanLogonOffline() == 1;
    public bool IsOfflineMode => client.NativeClient.IClientUtils.GetOfflineMode();
    private Action? openSettingsWindow;
    private TranslationManager tm;
    private SteamClient client;
    private LoginManager loginManager;
    private AppsManager appsManager;
    
    public MainWindowViewModel(SteamClient client, AppsManager appsManager, TranslationManager tm, LoginManager loginManager, Action openSettingsWindowAction) {
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
        this.openSettingsWindow = openSettingsWindowAction;
        this.appsManager = appsManager;
        this.CurrentPage = new LibraryPage() {
            DataContext = AvaloniaApp.Container.ConstructOnly<LibraryPageViewModel>()
        };
    }
    public void DBG_Crash() {
        throw new Exception("test");
    }

    public async void DBG_LaunchFactorio() {
        IClientShortcuts shortcuts = AvaloniaApp.Container.Get<IClientShortcuts>();
        //CUtlVector<AppId_t> appids = new(1024, 0);
        shortcuts.AddShortcut("wtf", "name", "exe", "workingdir", "unk");
        uint appidslen = 0;
        unsafe
        {
            uint* appids = null;
            appidslen = shortcuts.GetShortcutAppIds(appids);
            Console.WriteLine("ret: " + appidslen);
            Console.WriteLine("ptr: " + (nint)appids);
        }

        // foreach (var item in appids.ToManagedAndFree())
        // {
        //     Console.WriteLine("Item: " + item);
        // }
        
        //await this.appsManager.LaunchApp(427520, 3, "gamemoderun %command%");
    }

    public async void DBG_LaunchCS2() {
        ClientApps apps = AvaloniaApp.Container.Get<ClientApps>();
        foreach (var item in apps.GetMultipleAppDataSectionsSync(730, new [] { EAppInfoSection.Common, EAppInfoSection.Extended, EAppInfoSection.Config }))
        {
            Console.WriteLine("item: " + item.ToString());
        }
        
        //await this.appsManager.LaunchApp(730, 1, "gamemoderun %command% -dev -sdlaudiodriver pipewire");
    }

    public async void DBG_LaunchSpel2() {
        //await this.appsManager.LaunchApp(418530, 0, "");
        IClientUser user = AvaloniaApp.Container.Get<IClientUser>();
        unsafe {
            CMsgCellList list;
            using (var hack = ProtobufHack.Create_CMsgCellList())
            {
                Console.WriteLine("ptr: " + hack.ptr);
                Console.WriteLine("pre");
                user.GetCellList(hack.ptr);
                Console.WriteLine("post");
                list = hack.GetManaged();
            }

            foreach (var item in list.Cells)
            {
                Console.WriteLine("i: " + item.CellId + " n: " + item.LocName);
            }

            // IntPtr ptr = ProtobufHack.CMsgCellList_Construct();
            // Console.WriteLine("ptr: " + ptr);
            // Console.WriteLine("pre");
            // user.GetCellList(ptr);
            // Console.WriteLine("post");
            // var length = ProtobufHack.Protobuf_ByteSizeLong(ptr);

            // CMsgCellList list;
            // var bytes = new byte[length];
            // fixed (byte* bptr = bytes) {
            //     if (!ProtobufHack.Protobuf_SerializeToArray(ptr, bptr, length)) {
            //         throw new Exception("Failed");
            //     }
            // }
            // list = CMsgCellList.Parser.ParseFrom(bytes);
            
            // foreach (var item in list.Cells)
            // {
            //     Console.WriteLine("i: " + item.CellId + " n: " + item.LocName);
            // }
        }
    }

    public void DBG_OpenInterfaceList() => AvaloniaApp.Current?.OpenInterfaceList();
    public void DBG_ChangeLanguage() {
        // Very simple logic, just switches between english and finnish. 
        var tm = AvaloniaApp.Container.Get<TranslationManager>();

        ELanguage lang = tm.CurrentTranslation.Language;
        Console.WriteLine(string.Format(tm.GetTranslationForKey("#SettingsWindow_YourCurrentLanguage"), tm.GetTranslationForKey("#LanguageNameTranslated"), tm.CurrentTranslation.LanguageFriendlyName));
        if (lang == ELanguage.English) {
            tm.SetLanguage(ELanguage.Finnish);
        } else {
            tm.SetLanguage(ELanguage.English);
        }
    }
    public async void DBG_TestHTMLSurface() {
        HTMLSurfaceTest testWnd = new(this.client);
        testWnd.Show();
        await testWnd.Init("Valve Steam Client", "https://google.com");
        // Console.WriteLine("init returned: " + this.client.NativeClient.IClientHTMLSurface.Init());
        // this.client.CallbackManager.PauseThread();

        // var callHandle = this.client.NativeClient.IClientHTMLSurface.CreateBrowser("Valve Steam Client", null);
        // Console.WriteLine("IClientHTMLSurface::CreateBrowser got call handle " + callHandle);

        // var result = await this.client.CallbackManager.WaitForAPICallResult<HTML_BrowserReady_t>(callHandle);
        // if (result.failed) {
        //     Console.WriteLine("Call failed");
        //     return;
        // }

        // var handle = result.data.unBrowserHandle;
        // Console.WriteLine("result: " + handle);
        // this.client.NativeClient.IClientHTMLSurface.AllowStartRequest(handle, true);
    }

    public void Quit() {
        AvaloniaApp.Current?.ExitEventually();
    }

    public void OpenSettings() {
        this.openSettingsWindow?.Invoke();
    }

    public void GoOffline() {
        client.NativeClient.IClientUtils.SetOfflineMode(true);
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
    }
    public void GoOnline() {
        client.NativeClient.IClientUtils.SetOfflineMode(false);
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
    }

    public async void SignOut() {
        ExtendedProgress<int> progress = new(0, 100, "Logging off");
        AvaloniaApp.Current?.ForceProgressWindow(new ProgressWindowViewModel(progress, "Logging off"));
        await this.loginManager.LogoutAsync(progress, true);
    }

    public async void ChangeAccount() {
        await this.loginManager.LogoutAsync();
    }
}
