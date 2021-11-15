using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents
{
    internal sealed class JsObjectRefConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(JsObjectRef));
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
                var reference = (jsObjectRefValue as JsObjectRef)?.Reference;

                JsonSerializer.Serialize(writer, reference, options);
            }
        }
    }
}
