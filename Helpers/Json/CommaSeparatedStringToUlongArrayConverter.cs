
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Hanum.Core.Helpers.Json;

public class CommaSeparatedStringToUlongArrayConverter : JsonConverter<ulong[]> {
    public override ulong[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var value = reader.GetString();
        return value?.Split(',').Select(ulong.Parse).ToArray() ?? [];
    }

    public override void Write(Utf8JsonWriter writer, ulong[] value, JsonSerializerOptions options) {
        writer.WriteStringValue(string.Join(',', value));
    }
}