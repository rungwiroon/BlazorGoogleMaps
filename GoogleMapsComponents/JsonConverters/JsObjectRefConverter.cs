using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents
{
    internal sealed class JSObjectRefConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(Maps.Object));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new JsObjectRefConverterInner();
        }

        private class JsObjectRefConverterInner : JsonConverter<object>
        {
            public override object? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                throw new NotSupportedException();
            }

            public override void Write(Utf8JsonWriter writer, object jsObjectRefValue, JsonSerializerOptions options)
            {
                var reference = (jsObjectRefValue as Maps.Object)?.Reference;

                JsonSerializer.Serialize(writer, reference, options);
            }
        }
    }
}
