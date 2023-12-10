using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Styling;
using ClientUI.Controls;
using ClientUI.Translation;
using ClientUI.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using OpenSteamworks;
using OpenSteamworks.Callbacks.Structs;
using OpenSteamworks.Client;
using OpenSteamworks.Client.Apps;
using OpenSteamworks.Client.Enums;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.Client.Startup;
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
    private BasePage currentPage;

    private PageHeaderViewModel? CurrentPageHeader;
    private readonly Dictionary<Type, BasePage> LoadedPages = new();
    public ObservableCollection<PageHeaderViewModel> PageList { get; } = new() { };
    
    public bool CanLogonOffline => client.NativeClient.IClientUser.CanLogonOffline() == 1;
    public bool IsOfflineMode => client.NativeClient.IClientUtils.GetOfflineMode();
    private Action? openSettingsWindow;
    private TranslationManager tm;
    private SteamClient client;
    private LoginManager loginManager;
    private AppsManager appsManager;
    private MainWindow mainWindow;

    public MainWindowViewModel(MainWindow mainWindow, SteamClient client, AppsManager appsManager, TranslationManager tm, LoginManager loginManager, Action openSettingsWindowAction) {
        this.mainWindow = mainWindow;
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
        this.openSettingsWindow = openSettingsWindowAction;
        this.appsManager = appsManager;

        PageList.Add(new(this, "Store", typeof(StorePage), typeof(StorePageViewModel)));
        PageList.Add(new(this, "Library", typeof(LibraryPage), typeof(LibraryPageViewModel)));
        PageList.Add(new(this, "Community", typeof(CommunityPage), typeof(CommunityPageViewModel)));
        PageList.Add(new(this, "Console", typeof(ConsolePage), typeof(ConsolePageViewModel)));

        SwitchToPage(typeof(LibraryPage));
        // SwitchToPage("Library");
        // this.CurrentPage = new StorePage()
        // {
        //     DataContext = AvaloniaApp.Container.ConstructOnly<StorePageViewModel>()
        // };

        // this.CurrentPage = new LibraryPage() {
        //     DataContext = AvaloniaApp.Container.ConstructOnly<LibraryPageViewModel>()
        // };
    }

#pragma warning disable MVVMTK0034
    [MemberNotNull(nameof(currentPage))]
    [MemberNotNull(nameof(CurrentPage))]
#pragma warning restore MVVMTK0034
    internal void SwitchToPage(Type pageType) {
        PageHeaderViewModel model = this.PageList.Where(item => item.PageType == pageType).First();
        var (type, page) = LoadedPages.Where(item => item.Key == model.PageType).FirstOrDefault();
        if (page == null) {
            page = model.PageCtor();
            page.DataContext = model.ViewModelCtor();
            LoadedPages.Add(model.PageType, page);
        }

        // Set selected button
        model.ButtonBackground = Brushes.Green;
        if (CurrentPageHeader != null) {
            CurrentPageHeader.ButtonBackground = AvaloniaApp.Theme!.ButtonBackground;
            if (CurrentPageHeader.IsWebPage) {
                CurrentPageHeader.ButtonBackground = AvaloniaApp.Theme!.AccentButtonBackground;
            }
        }

        CurrentPage = page;
        CurrentPageHeader = model;
    }

    internal void UnloadPage(Type pageType) {
        if (!LoadedPages.ContainsKey(pageType)) {
            return;
        }

        PageHeaderViewModel model = this.PageList.Where(item => item.PageType == pageType).First();
        var loadedPage = LoadedPages[pageType];
        loadedPage.Free();
        loadedPage.DataContext = null;
        LoadedPages.Remove(pageType);
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
        // ClientApps apps = AvaloniaApp.Container.Get<ClientApps>();
        // foreach (var item in apps.GetMultipleAppDataSectionsSync(730, new [] { EAppInfoSection.Common, EAppInfoSection.Extended, EAppInfoSection.Config }))
        // {
        //     Console.WriteLine("item: " + item.ToString());
        // }
        IClientDeviceAuth devauth = AvaloniaApp.Container.Get<IClientDeviceAuth>();

        uint[] owners = new uint[5];
        Console.WriteLine("ret: " + devauth.GetSharedLibraryOwners(owners, 5));
        foreach (var item in owners)
        {
            Console.WriteLine("i: " + item);
        }

        uint[] owners2 = new uint[5];
        Console.WriteLine("ret2: " + devauth.GetAuthorizedBorrowers(owners2, 5));
        foreach (var item in owners2)
        {
            Console.WriteLine("i2: " + item);
        }

        uint[] owners3 = new uint[10];
        Console.WriteLine("ret3: " + devauth.GetLocalUsers(owners3, 10));
        StringBuilder builder = new(256);
        bool isSharingLibrary = false;
        foreach (var item in owners3)
        {
            Console.WriteLine("i3: " + item);
            Console.WriteLine("ret i3: " + devauth.GetBorrowerInfo(item, builder, builder.Capacity, out isSharingLibrary));
            Console.WriteLine("info: " + builder.ToString());
            Console.WriteLine("isSharingLibrary: " + isSharingLibrary);
        }

        //await this.appsManager.LaunchApp(730, 1, "gamemoderun %command% -dev -sdlaudiodriver pipewire");
    }

    public async void DBG_LaunchSpel2() {
        //await this.appsManager.LaunchApp(418530, 0, "");
        IClientUser user = AvaloniaApp.Container.Get<IClientUser>();
        unsafe {
            CMsgCellList list;
            using (var hack = ProtobufHack.Create<CMsgCellList>())
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
        HTMLSurfaceTest testWnd = new();
        testWnd.Show();
        await testWnd.Init("Valve Steam Client", "https://google.com/");
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
