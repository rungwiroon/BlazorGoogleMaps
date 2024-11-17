using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents;

internal class EnumMemberConverter<T> : JsonConverter<T> where T : IComparable, IFormattable, IConvertible
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var jsonValue = reader.GetString();

        foreach (var fi in typeToConvert.GetFields())
        {
            var description = (EnumMemberAttribute?)fi.GetCustomAttribute(typeof(EnumMemberAttribute), false);

            if (description != null)
            {
                if (string.Equals(description.Value, jsonValue, StringComparison.OrdinalIgnoreCase))
                {
                    return (T?)fi.GetValue(null);
                }
            }
        }

        throw new JsonException($"string {jsonValue} was not found as a description in the enum {typeToConvert}");
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
    {
        var fi = value.GetType().GetField(value.ToString() ?? string.Empty);

        var description = (EnumMemberAttribute?)fi?.GetCustomAttribute(typeof(EnumMemberAttribute), false);

        writer.WriteStringValue(description?.Value);
    }
}