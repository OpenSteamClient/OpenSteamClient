using System.Text.Json;
using OpenSteamworks.Client.Utils;

namespace OpenSteamworks.Client.Apps.Library;

public class FilterGroup<T> where T: notnull {
    public List<T> FilterOptions { get; set; } = new();
    public bool AcceptUnion { get; set; }

    internal static FilterGroup<T> FromJSONFilterGroup(JSONFilterGroup json) {
        FilterGroup<T> filterGroup = new();
        filterGroup.AcceptUnion = json.bAcceptUnion;
        foreach (var item in json.rgOptions)
        {
            filterGroup.FilterOptions.Add(UtilityFunctions.AssertNotNull(item.Deserialize<T>()));
        }

        return filterGroup;
    }

    internal JSONFilterGroup ToJSON() {
        JSONFilterGroup json = new();
        json.bAcceptUnion = this.AcceptUnion;
        foreach (var item in this.FilterOptions)
        {
            json.rgOptions.Add(JsonSerializer.SerializeToElement(item));
        }

        return json;
    }
}