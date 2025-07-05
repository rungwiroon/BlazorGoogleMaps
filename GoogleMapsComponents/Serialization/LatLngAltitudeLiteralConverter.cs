using GoogleMapsComponents.Maps;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Serialization;

/// <summary>
/// JSON Converter for <see cref="LatLngAltitudeLiteral"/> objects.
/// </summary>
internal sealed class LatLngAltitudeLiteralConverter : JsonConverter<LatLngAltitudeLiteral>
{
    /// <inheritdoc />
    public override LatLngAltitudeLiteral Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType != JsonTokenType.StartObject)
        {
            throw new JsonException("Expected start of object.");
        }

        double? lat = null;
        double? lng = null;
        double? altitude = null;

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
                case "altitude":
                    altitude = reader.GetDouble();
                    break;
                default:
                    reader.Skip();
                    break;
            }
        }

        if (lat == null || lng == null || altitude == null)
        {
            throw new JsonException("Missing required properties for LatLngAltitudeLiteral.");
        }

        return new LatLngAltitudeLiteral(lat.Value, lng.Value, altitude.Value);
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, LatLngAltitudeLiteral value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteNumber("lat", value.Lat);
        writer.WriteNumber("lng", value.Lng);
        writer.WriteNumber("altitude", value.Altitude);
        writer.WriteEndObject();
    }
}
