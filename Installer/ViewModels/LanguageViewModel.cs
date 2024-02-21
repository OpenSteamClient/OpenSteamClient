using Installer.Translation;

namespace Installer.ViewModels;

public partial class LanguageViewModel : AvaloniaCommon.ViewModelBase {
    public string Name { get; init; }
    public ELanguage ELang { get; init; }
    public LanguageViewModel(ELanguage lang) {
        this.ELang = lang;
        this.Name = AvaloniaApp.TranslationManager.GetPrettyNameForLanguage(lang);
    }
}