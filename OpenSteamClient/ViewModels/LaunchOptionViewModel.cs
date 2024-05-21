namespace OpenSteamClient.ViewModels;

public partial class LaunchOptionViewModel : AvaloniaCommon.ViewModelBase
{
    public int ID { get; init; }
    public string Name { get; init; }
    public string? Description { get; init; }
    public bool HasDescription => !string.IsNullOrEmpty(Description);

    public LaunchOptionViewModel(int id, string name, string? description) {
        this.ID = id;
        this.Name = name;
        this.Description = description;
    }
}