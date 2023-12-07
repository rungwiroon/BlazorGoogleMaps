using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;
using System.Runtime.Serialization;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace GoogleMapsComponents.Serialization;
public class JsonStringEnumConverterEx : JsonConverterFactory
{
    private readonly ConcurrentDictionary<Type, EnumConverter> _converters = new ConcurrentDictionary<Type, EnumConverter>();

    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsEnum;
    }

    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
        return _converters.GetOrAdd(typeToConvert, (type) => new EnumConverter(type));
    }

    private class EnumConverter : JsonConverter<Enum>
    {
        private readonly Dictionary<string, Enum> _stringToEnum;
        private readonly Dictionary<Enum, string> _enumToString;

        public EnumConverter(Type enumType)
        {
            _stringToEnum = new Dictionary<string, Enum>();
            _enumToString = new Dictionary<Enum, string>();

            var enumValues = Enum.GetValues(enumType);
            foreach (Enum value in enumValues)
            {
                var name = value.ToString();
                var enumMemberAttr = enumType.GetField(name)
                    .GetCustomAttribute<EnumMemberAttribute>();

                var stringKey = enumMemberAttr?.Value ?? name;
                _stringToEnum[stringKey] = value;
                _enumToString[value] = stringKey;
            }
        }

        public override Enum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            if (_stringToEnum.TryGetValue(stringValue, out var enumValue))
            {
                return enumValue;
            }

            throw new JsonException($"Unable to convert \"{stringValue}\" to enum {typeToConvert}.");
        }

        public override void Write(Utf8JsonWriter writer, Enum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(_enumToString[value]);
        }
    }
}
