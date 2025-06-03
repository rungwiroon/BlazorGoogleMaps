using GoogleMapsComponents.Maps;
using System; 
using System.Text.Json;
using System.Text.Json.Serialization; 

namespace GoogleMapsComponents.Serialization;

/// <summary>
/// JSON Converter for <see cref="LatLngLiteral"/> objects.
/// </summary>
internal sealed class LatLngLiteralConverter : JsonConverter<LatLngLiteral>
{
    /// <inheritdoc />
    public override LatLngLiteral Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object.");
        }

        if (!reader.Read())
        {
            throw new JsonException("Expected property name.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected latitude value");
        }
        
        var lat = reader.GetDouble();

        if (!reader.Read())
        {
            throw new JsonException("Expected property name.");
        }

        if (!reader.Read() || reader.TokenType != JsonTokenType.Number)
        {
            throw new JsonException("Expected latitude value");
        }
        var lon = reader.GetDouble();

        if (!reader.Read() || reader.TokenType != JsonTokenType.EndObject)
        {
            throw new JsonException("Expected end of object.");
        }
        
        return new LatLngLiteral(lat, lon);
    }

    // <inheritdoc />
    public override void Write(Utf8JsonWriter writer, LatLngLiteral value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("lat", value.Lat);
        writer.WriteNumber("lng", value.Lng);
        writer.WriteEndObject();
    }
}
