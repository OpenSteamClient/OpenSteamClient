using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;
using ValveKeyValue;

namespace OpenSteamworks.Client.Managers;

public struct BetaBranch {
    public string Name;
    public string Description;
    public string BuildID;
    public UInt32 TimeUpdated;
    public bool PasswordRequired;
}

public struct LaunchOption {
    public int ID = -1;
    public int Index = -1;
    public string Description = "";
    public ELaunchOptionType Type = ELaunchOptionType.k_ELaunchOptionTypeNone;
    public LaunchOption() {

    }   
}

/// <summary>
/// An app. Can mean basically anything, like a trailer, legacy media or a game.
/// </summary>
public class App
{
    public UInt32 AppID { get; init; }
    public string Name { get; private set; } = "";
    public EAppType Type { get; private set; } = EAppType.k_EAppTypeInvalid;
    public string ClientIconHash { get; private set; } = "";
    public bool HasWorkshop { get; private set; } = false;
    public bool HasCommunityHub { get; private set; } = false;
    public LaunchOption[] LaunchOptions { get; private set; } = Array.Empty<LaunchOption>();
    //TODO
    //public BetaBranch[] Betas { get; private set; } = Array.Empty<BetaBranch>();

    internal App(UInt32 appid)
    {
        this.AppID = appid;
    }
    
    internal void FillWithAppInfoBinary(byte[] commonBytes, byte[] configBytes) {
        var serializer = KVSerializer.Create(KVSerializationFormat.KeyValues1Binary);

        KVObject common;
        using (var stream = new MemoryStream(commonBytes))
        {
            common = serializer.Deserialize(stream);
        }

        KVObject config;
        using (var stream = new MemoryStream(configBytes))
        {
            config = serializer.Deserialize(stream);
        }

        //TODO: support localized names via name_localized/LANGUAGE
        this.Name = (string)common["name"];

        this.Type = AppTypeFromString((string)common["type"]);

        // Process game type specific keys
        if (this.Type == EAppType.k_EAppTypeGame) {
            if (common["clienticon"] != null) {
                this.ClientIconHash = (string)common["clienticon"];
            }

            if (common["workshop_visible"] != null) {
                this.HasWorkshop = (bool)common["workshop_visible"];
            }

            if (common["community_hub_visible"] != null) {
                this.HasCommunityHub = (bool)common["community_hub_visible"];
            }

            if (config["launch"] != null) {
                List<LaunchOption> launchOptions = new();
                foreach (var launchOpt in (IEnumerable<KVObject>)config["launch"])
                {
                    LaunchOption opt = new();
                    opt.ID = int.Parse(launchOpt.Name);
                    opt.Index = launchOptions.Count;

                    // Get initial name
                    {
                        if (launchOpt["description"] != null) {
                            opt.Description = (string)launchOpt["description"];
                        }
                    }
                    
                    // Get filters to save
                    {
                        var launchOptionConfig = launchOpt["config"];
                        if (launchOptionConfig != null) {
                            if (launchOptionConfig["oslist"] != null) {
                                
                            }
                        }
                    }

                    // Parse type and set description and index based on it
                    {
                        var typeObj = launchOpt["type"];
                        if (typeObj != null) {
                            string type = (string)typeObj;

                            // Some of these are guessed, since I haven't found apps that use these.
                            opt.Type = type.ToLowerInvariant() switch
                            {
                                "default" => ELaunchOptionType.k_ELaunchOptionTypeDefault,
                                "safemode" => ELaunchOptionType.k_ELaunchOptionTypeSafeMode,
                                "multiplayer" => ELaunchOptionType.k_ELaunchOptionTypeMultiplayer,
                                "config" => ELaunchOptionType.k_ELaunchOptionTypeConfig,
                                "vr" => ELaunchOptionType.k_ELaunchOptionTypeVR,
                                "server" => ELaunchOptionType.k_ELaunchOptionTypeServer,
                                "editor" => ELaunchOptionType.k_ELaunchOptionTypeEditor,
                                "manual" => ELaunchOptionType.k_ELaunchOptionTypeManual,
                                "benchmark" => ELaunchOptionType.k_ELaunchOptionTypeBenchmark,
                                "option1" => ELaunchOptionType.k_ELaunchOptionTypeOption1,
                                "option2" => ELaunchOptionType.k_ELaunchOptionTypeOption2,
                                "option3" => ELaunchOptionType.k_ELaunchOptionTypeOption3,
                                "othervr" => ELaunchOptionType.k_ELaunchOptionTypeOthervr,
                                "openvroverlay" => ELaunchOptionType.k_ELaunchOptionTypeOpenvroverlay,
                                "osvr" => ELaunchOptionType.k_ELaunchOptionTypeOsvr,
                                "openxr" => ELaunchOptionType.k_ELaunchOptionTypeOpenxr,
                                "dialog" => ELaunchOptionType.k_ELaunchOptionTypeDialog,
                                _ => ELaunchOptionType.k_ELaunchOptionTypeNone
                            };

                            switch (opt.Type)
                            {
                                case ELaunchOptionType.k_ELaunchOptionTypeDefault:
                                    opt.Index = 0;
                                    if (string.IsNullOrEmpty(opt.Description))
                                    {
                                        opt.Description = $"Play {this.Name}";
                                    }
                                    else
                                    {
                                        opt.Description = $"Play {this.Name} ({opt.Description})";
                                    }
                                    break;

                                case ELaunchOptionType.k_ELaunchOptionTypeVR:
                                    opt.Description = $"Play {this.Name} in VR";
                                    break;

                                case ELaunchOptionType.k_ELaunchOptionTypeSafeMode:
                                    opt.Description = $"Play {this.Name} in Safe Mode";
                                    break;

                                case ELaunchOptionType.k_ELaunchOptionTypeMultiplayer:
                                    opt.Description = $"Play {this.Name} in Multiplayer";
                                    break;

                                case ELaunchOptionType.k_ELaunchOptionTypeOpenxr:
                                    opt.Description = $"Play {this.Name} in VR with OpenXR";
                                    break;

                                case ELaunchOptionType.k_ELaunchOptionTypeOption1:
                                    opt.Index = 1;
                                    opt.Description = $"Play {this.Name} ({opt.Description})";
                                    break;

                                case ELaunchOptionType.k_ELaunchOptionTypeOption2:
                                    opt.Index = 2;
                                    opt.Description = $"Play {this.Name} ({opt.Description})";
                                    break;
                                    
                                case ELaunchOptionType.k_ELaunchOptionTypeOption3:
                                    opt.Index = 3;
                                    opt.Description = $"Play {this.Name} ({opt.Description})";
                                    break;

                                default:
                                    opt.Description += $" ({opt.Type})";
                                    break;
                            }
                        }
                    }
                }

                this.LaunchOptions = launchOptions.ToArray();
            }
        }
    }

    public static EAppType AppTypeFromString(string typeStr)
    {
        return typeStr.ToLowerInvariant() switch
        {
            "game" => EAppType.k_EAppTypeGame,
            "demo" => EAppType.k_EAppTypeDemo,
            "beta" => EAppType.k_EAppTypeBeta,
            "tool" => EAppType.k_EAppTypeTool,
            "application" => EAppType.k_EAppTypeApplication,
            "music" => EAppType.k_EAppTypeMusic,
            "config" => EAppType.k_EAppTypeConfig,
            "dlc" => EAppType.k_EAppTypeDlc,
            "media" => EAppType.k_EAppTypeMedia,
            "video" => EAppType.k_EAppTypeVideo,
            _ => throw new ArgumentOutOfRangeException(nameof(typeStr), $"Unexpected type: {typeStr}"),
        };
    }

    public override string ToString()
    {
        return string.Format("AppID {0}, Name: {1}, Type: {2}", this.AppID, this.Name, this.Type);
    }
}