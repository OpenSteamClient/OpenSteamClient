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
using OpenSteamworks.Client;
using OpenSteamworks.Client.Managers;
using OpenSteamworks.ClientInterfaces;
using OpenSteamworks.Enums;
using OpenSteamworks.Generated;
using OpenSteamworks.Messaging;
using OpenSteamworks.NativeTypes;
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
    public MainWindowViewModel(SteamClient client, TranslationManager tm, LoginManager loginManager, Action openSettingsWindowAction) {
        this.client = client;
        this.tm = tm;
        this.loginManager = loginManager;
        this.ShowGoOffline = CanLogonOffline && !IsOfflineMode;
        this.ShowGoOnline = CanLogonOffline && IsOfflineMode;
        this.openSettingsWindow = openSettingsWindowAction;
        this.CurrentPage = new LibraryPage() {
            DataContext = AvaloniaApp.Container.ConstructOnly<LibraryPageViewModel>()
        };
    }
    public void DBG_Crash() {
        throw new Exception("test");
    }
    public void DBG_LaunchFactorio() {
        var gameid = new CGameID(427520);
        EAppUpdateError launchresult = client.NativeClient.IClientAppManager.LaunchApp(gameid, 3, 0, "");
        MessageBox.Show("result", launchresult.ToString());
    }
    public void DBG_OpenInterfaceList() => AvaloniaApp.Current?.OpenInterfaceList();
    public void DBG_ChangeLanguage() {
        // Very simple logic, just switches between english and finnish. 
        Translation.TranslationManager tm = AvaloniaApp.Container.GetComponent<Translation.TranslationManager>();

        ELanguage lang = tm.CurrentTranslation.Language;
        Console.WriteLine(string.Format(tm.GetTranslationForKey("#SettingsWindow_YourCurrentLanguage"), tm.GetTranslationForKey("#LanguageNameTranslated"), tm.CurrentTranslation.LanguageFriendlyName));
        if (lang == ELanguage.English) {
            tm.SetLanguage(ELanguage.Finnish);
        } else {
            tm.SetLanguage(ELanguage.English);
        }
    }
    public async void DBG_TestMaps() {
        unsafe {
            var map = new CUtlMap<uint, uint>(1, 80000);
            Console.WriteLine("LastPlayedMap: " + client.NativeClient.IClientUser.BGetAppsLastPlayedMap(&map));
            var asManaged = map.ToManagedAndFree();
            foreach (var item in asManaged)
            {
                Console.WriteLine(item.Key+":"+item.Value);
            }
        }

        unsafe {
            var map = new CUtlMap<uint, ulong>(1, 80000);
            Console.WriteLine("LastPlayedMap: " + client.NativeClient.IClientUser.BGetAppPlaytimeMap(&map));
            var asManaged = map.ToManagedAndFree();
            foreach (var item in asManaged)
            {
                Console.WriteLine(item.Key+":"+item.Value);
            }
        }

        unsafe {
            CUtlStringList compatToolsVec = new();
            client.NativeClient.IClientCompat.GetAvailableCompatTools(&compatToolsVec);
            List<string?> compatTools = compatToolsVec.ToManagedAndFree();
            foreach (var tool in compatTools)
            {
                Console.WriteLine(tool);
            }
        }

        // {
        //     for (int i = 0; i < 20; i++)
        //     {
        //         StringBuilder sb = new StringBuilder("", 1024);
        //         var ret = client.NativeClient.IClientUser.GetConfigString(i, "user-collections.favorite", sb, 1024);
        //         Console.WriteLine(i + ": " + ret + " " + sb.ToString());

        //         unsafe {
        //             CUtlBuffer buf = new CUtlBuffer(10000);
        //             var ret2 = client.NativeClient.IClientUser.GetConfigBinaryBlob(i, "user-collections.favorite", &buf);
        //             Console.WriteLine(i + ": " + ret2 + " " + buf.m_Put);
        //             buf.Free();
        //         }

        //         StringBuilder sb3 = new StringBuilder("", 1024);
        //         var ret3 = client.NativeClient.IClientUser.GetConfigStoreKeyName(i, "user-collections.favorite", sb3, 1024);
        //         Console.WriteLine(i + ": " + ret3 + " " + sb3.ToString());
        //     }
        // }

        //CloudConfigStoreManager mgr = AvaloniaApp.Container.ConstructOnly<CloudConfigStoreManager>();
        // var nm = new OpenSteamworks.Protobuf.WebUI.CCloudConfigStore_NamespaceVersion();
        // nm.Enamespace = 2138;
        // var test = await mgr.Download(new() { nm });

        var connection = AvaloniaApp.Container.GetComponent<ClientMessaging>().AllocateConnection();
        ProtoMsg<CCloudConfigStore_Download_Request> msg = new("CloudConfigStore.Download#1");
        // msg.body.Versions.Add(versions);
        // Console.WriteLine("sending");
        // Console.WriteLine(msg.ToString());
        var resp = await connection.ProtobufSendMessageAndAwaitResponse<CCloudConfigStore_Download_Response, CCloudConfigStore_Download_Request>(msg);
        connection.RegisterServiceMethodHandler("CloudConfigStoreClient.NotifyChange#1");
        connection.RegisterServiceMethodHandler("CloudConfigStoreClient.Download#1");
        connection.RegisterServiceMethodHandler("CloudConfigStoreClient.Upload#1");
        Console.WriteLine("got resp");
        Console.WriteLine(resp.ToString());
        connection.StartPollThread();
        //connection.ProtobufSendMessage<CCloudConfigStore_Download_Request>(msg);
        
        // unsafe {
        //     var map = new CUtlMap<uint, AppTags_t>(1, 80000, &LessFunc);
        //     Console.WriteLine("AppTagsMap: " + client.NativeClient.IClientUser.BGetAppTagsMap(&map));
        //     var asManaged = map.ToManagedAndFree();
        //     foreach (var item in asManaged)
        //     {
        //         Console.WriteLine(item.Key+": { unk1: " + item.Value.unk1 + ", unk2: " + string.Format("0x{0:X}", (IntPtr)item.Value.unk2).ToLower() + ", unk3: " + item.Value.unk3 + ", unk4: " + item.Value.unk4 + "}");
        //         Console.WriteLine("reading " + Marshal.PtrToStructure<CUtlString>((IntPtr)item.Value.unk2).ToManaged());
        //         var size = 256;
        //         byte[] managedArray = new byte[size];
        //         Marshal.Copy((IntPtr)item.Value.unk2, managedArray, 0, size);
        //         System.IO.File.WriteAllBytes("/tmp/" + item.Key.ToString() + ".bin", managedArray);
        //     }
        // }
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

    public void SignOut() {
        this.loginManager.Logout(true);
    }
    public void ChangeAccount() {
        this.loginManager.Logout();
    }
}
