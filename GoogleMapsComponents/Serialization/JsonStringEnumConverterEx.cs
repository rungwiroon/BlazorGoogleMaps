using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Serialization;

/// <summary>
/// TODO apply to all enum with EnumMember attribute
/// </summary>
/// <typeparam name="TEnum"></typeparam>
public class JsonStringEnumConverterEx<TEnum> : JsonConverter<TEnum> where TEnum : struct, Enum
{

    private readonly Dictionary<TEnum, string> _enumToString = new Dictionary<TEnum, string>();
    private readonly Dictionary<string, TEnum> _stringToEnum = new Dictionary<string, TEnum>();

    public JsonStringEnumConverterEx()
    {
        var type = typeof(TEnum);
        var values = Enum.GetValues(typeof(TEnum));

        foreach (var value in values)
        {
            var enumMember = type.GetMember(value.ToString())[0];
            var attr = enumMember
                .GetCustomAttributes(typeof(EnumMemberAttribute), false)
                .Cast<EnumMemberAttribute>()
                .FirstOrDefault();

            _stringToEnum.Add(value.ToString(), (TEnum)value);

            if (attr?.Value != null)
            {
                _enumToString.Add((TEnum)value, attr.Value);
                _stringToEnum.Add(attr.Value, (TEnum)value);
            }
            else
            {
                _enumToString.Add((TEnum)value, value.ToString());
            }
        }
    }

    public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var stringValue = reader.GetString();

        if (_stringToEnum.TryGetValue(stringValue, out var enumValue))
        {
            return enumValue;
        }

        return default;
    }

    public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(_enumToString[value]);
    }

    public static string ToLowerFirstChar(string str)
    {
        if (String.IsNullOrEmpty(str) || Char.IsLower(str, 0))
            return str;

        return Char.ToLowerInvariant(str[0]) + str.Substring(1);
    }
}