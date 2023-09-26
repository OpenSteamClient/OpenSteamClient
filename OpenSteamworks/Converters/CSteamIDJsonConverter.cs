using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using OpenSteamworks.Structs;

namespace OpenSteamworks.Converters;

public class CSteamIDJsonConverter : JsonConverter<CSteamID>
{
    public override CSteamID Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return new CSteamID(reader.GetUInt64());
    }

    public override void Write(Utf8JsonWriter writer, CSteamID val, JsonSerializerOptions options)
    {
        writer.WriteNumberValue(val.SteamID64);
    }
}