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

        double? lat = null;
        double? lng = null;

        while (reader.Read())
        {
            if (reader.TokenType == JsonTokenType.EndObject)
            {
                break;
            }

            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException("Expected property name.");
            }

            var propertyName = reader.GetString();
            if (!reader.Read())
            {
                throw new JsonException("Expected value.");
            }

            switch (propertyName)
            {
                case "lat":
                    lat = reader.GetDouble();
                    break;
                case "lng":
                    lng = reader.GetDouble();
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (lat == null || lng == null)
        {
            throw new JsonException("Missing required properties for LatLngLiteral.");
        }

        return new LatLngLiteral(lat.Value, lng.Value);
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
