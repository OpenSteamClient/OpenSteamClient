using System.Collections.ObjectModel;
using OpenSteamworks.Enums;
using OpenSteamworks.NativeTypes;
using OpenSteamworks.Structs;
using OpenSteamworks.Utils;
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
    public ELaunchOptionType Type = ELaunchOptionType.None;
    public LaunchOption() {

    }   
}

/// <summary>
/// An app. Can mean basically anything, like a trailer, legacy media, game, tools, dedicated servers, configs, etc.
/// Can also be non-steam apps.
/// </summary>
public class App
{
    public AppId_t AppID { get; init; }
    public CGameID GameID { get; init; }
    public string Name { get; private set; } = "";
    public EAppType Type { get; private set; } = EAppType.k_EAppTypeInvalid;
    public string ClientIconHash { get; private set; } = "";
    public bool HasWorkshop { get; private set; } = false;
    public bool HasCommunityHub { get; private set; } = false;
    public ReadOnlyCollectionEx<LaunchOption> LaunchOptions {
        get {
            return new ReadOnlyCollectionEx<LaunchOption>(launchOptions);
        }
    }

    private List<LaunchOption> launchOptions = new();

    public ReadOnlyCollectionEx<string> Aliases {
        get {
            return new ReadOnlyCollectionEx<string>(aliases);
        }
    }
    private List<string> aliases = new();
    //TODO
    //public BetaBranch[] Betas { get; private set; } = Array.Empty<BetaBranch>();

    internal App(UInt32 appid, CGameID nonSteamGameID = default)
    {
        this.AppID = appid;
        if (!nonSteamGameID.IsValid()) {
            this.GameID = new CGameID(appid);
        } else {
            this.GameID = nonSteamGameID;
        }
    }

    //TODO: This is a mess. Split this.
    internal void FillWithAppInfoBinary(byte[] commonBytes, byte[] configBytes, byte[] extendedBytes, byte[] depotsBytes) {
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

        KVObject? extended = null;
        if (extendedBytes.Any()) {
            using (var stream = new MemoryStream(extendedBytes))
            {
                extended = serializer.Deserialize(stream);
            }
        }

        KVObject? depots = null;
        if (depotsBytes.Any()) {
            using (var stream = new MemoryStream(depotsBytes))
            {
                depots = serializer.Deserialize(stream);
            }
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
                this.launchOptions.Clear();
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
                                "default" => ELaunchOptionType.Default,
                                "safemode" => ELaunchOptionType.SafeMode,
                                "multiplayer" => ELaunchOptionType.Multiplayer,
                                "config" => ELaunchOptionType.Config,
                                "vr" => ELaunchOptionType.VR,
                                "server" => ELaunchOptionType.Server,
                                "editor" => ELaunchOptionType.Editor,
                                "manual" => ELaunchOptionType.Manual,
                                "benchmark" => ELaunchOptionType.Benchmark,
                                "option1" => ELaunchOptionType.Option1,
                                "option2" => ELaunchOptionType.Option2,
                                "option3" => ELaunchOptionType.Option3,
                                "othervr" => ELaunchOptionType.Othervr,
                                "openvroverlay" => ELaunchOptionType.Openvroverlay,
                                "osvr" => ELaunchOptionType.Osvr,
                                "openxr" => ELaunchOptionType.Openxr,
                                "dialog" => ELaunchOptionType.Dialog,
                                _ => ELaunchOptionType.None
                            };

                            switch (opt.Type)
                            {
                                case ELaunchOptionType.Default:
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

                                case ELaunchOptionType.VR:
                                    opt.Description = $"Play {this.Name} in VR";
                                    break;

                                case ELaunchOptionType.SafeMode:
                                    opt.Description = $"Play {this.Name} in Safe Mode";
                                    break;

                                case ELaunchOptionType.Multiplayer:
                                    opt.Description = $"Play {this.Name} in Multiplayer";
                                    break;

                                case ELaunchOptionType.Openxr:
                                    opt.Description = $"Play {this.Name} in VR with OpenXR";
                                    break;

                                case ELaunchOptionType.Option1:
                                    opt.Index = 1;
                                    opt.Description = $"Play {this.Name} ({opt.Description})";
                                    break;

                                case ELaunchOptionType.Option2:
                                    opt.Index = 2;
                                    opt.Description = $"Play {this.Name} ({opt.Description})";
                                    break;
                                    
                                case ELaunchOptionType.Option3:
                                    opt.Index = 3;
                                    opt.Description = $"Play {this.Name} ({opt.Description})";
                                    break;

                                default:
                                    opt.Description += $" ({opt.Type})";
                                    break;
                            }
                        }
                    }

                    this.launchOptions.Add(opt);
                }
            }
        }

        if (extended != null) {
            if (extended["aliases"] != null) {
                string ogAliases = (string)extended["aliases"];
                ogAliases = ogAliases.Replace(" ", "");
                foreach (var item in ogAliases.Split(','))
                {
                    this.aliases.Add(item);
                }
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