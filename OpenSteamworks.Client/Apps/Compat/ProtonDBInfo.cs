using System.Text.Json.Serialization;

namespace OpenSteamworks.Client.Apps.Compat;

public class ProtonDBInfo
{
    public enum ETier {
        [JsonPropertyName("platinum")]
        Platinum,

        [JsonPropertyName("gold")]
        Gold,
        
        [JsonPropertyName("silver")]
        Silver,

        [JsonPropertyName("bronze")]
        Bronze,

        [JsonPropertyName("borked")]
        Borked,
    }

    public enum EConfidence {
        [JsonPropertyName("strong")]
        Strong,
        [JsonPropertyName("moderate")]
        Moderate,
        [JsonPropertyName("good")]
        Good,
        [JsonPropertyName("low")]
        Low
    }

    [JsonPropertyName("bestReportedTier")]
    public ETier BestReportedTier { get; set; }

    [JsonPropertyName("confidence")]
    public EConfidence Confidence { get; set; }

    [JsonPropertyName("score")]
    public double Score { get; set; }

    [JsonPropertyName("tier")]
    public ETier Tier { get; set; }

    [JsonPropertyName("total")]
    public int TotalReports { get; set; }

    [JsonPropertyName("trendingTier")]
    public ETier TrendingTier { get; set; }
}