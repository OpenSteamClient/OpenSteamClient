using System;
using Autofac;
using ClientUI.Views;
using OpenSteamworks;
using OpenSteamworks.Enums;
using OpenSteamworks.Structs;

namespace ClientUI.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    public void DBG_Crash() {
        throw new Exception("test");
    }
    public void DBG_LaunchFactorio() {
        var client = Common.Utils.UtilityFunctions.AssertNotNull(App.DIContainer?.Resolve<SteamClient>());
        var gameid = new CGameID(427520);
        EAppUpdateError launchresult = client.NativeClient.IClientAppManager.LaunchApp(gameid, 3, 0, "");
        MessageBox.Show("result", launchresult.ToString());
    }
    public void DBG_OpenInterfaceList() {
        var ifacelist = new InterfaceList();
        ifacelist.Show();
    }
    public void DBG_ChangeLanguage() {
        // Very simple logic, just switches between english and finnish. 
        Translation.TranslationManager? tm = App.DIContainer?.Resolve<Translation.TranslationManager>();
        if (tm == null) {
            return;
        }

        ELanguage lang = tm.CurrentTranslation.Language;
        Console.WriteLine(string.Format(tm.GetTranslationForKey("#SettingsWindow_YourCurrentLanguage"), tm.GetTranslationForKey("#LanguageNameTranslated"), tm.CurrentTranslation.LanguageFriendlyName));
        if (lang == ELanguage.English) {
            tm.SetLanguage(ELanguage.Finnish);
        } else {
            tm.SetLanguage(ELanguage.English);
        }
    }
}
