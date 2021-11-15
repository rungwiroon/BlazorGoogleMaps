using OneOf;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents
{
    internal sealed class OneOfConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(IOneOf))
                || (typeToConvert.IsGenericType 
                    && typeToConvert.GetGenericTypeDefinition() == typeof(Nullable<>)
                    && typeToConvert.GetGenericArguments()[0].IsAssignableTo(typeof(IOneOf)));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new OneOfConverterInner();
        }

        private class OneOfConverterInner : JsonConverter<IOneOf>
        {
            public override IOneOf? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotSupportedException();
            }

            public override void Write(Utf8JsonWriter writer, IOneOf oneOfValue, JsonSerializerOptions options)
            {
                JsonSerializer.Serialize(writer, oneOfValue.Value, options);
            }
        }
    }
}
