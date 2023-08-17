using System;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ClientUI.ViewModels;

public partial class ViewModelBase : ObservableObject
{
    // Fake property that returns a translated string when the caller has a TranslationKey attached property.
    public string GetTranslation {
        get {
            
            StackTrace stackTrace = new StackTrace();
            Console.WriteLine(stackTrace.ToString());
            return "";
        }
    }
}
