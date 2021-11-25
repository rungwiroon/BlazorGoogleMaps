using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.JsonConverters
{
    internal interface IBridgeDto<T>
    {
        T ConvertToDestinationType();
    }

    internal sealed class JSObjPropsRefConverter<TBridge, TDestinationValue> : JsonConverterFactory
        where TBridge : IBridgeDto<TDestinationValue>
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return typeToConvert.IsAssignableTo(typeof(TDestinationValue));
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            return new OneOfConverterInner();
        }

        private class OneOfConverterInner : JsonConverter<TDestinationValue>
        {
            public override TDestinationValue? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var dto = JsonSerializer.Deserialize<TBridge>(ref reader, options);

                return dto != null ? dto.ConvertToDestinationType() : default;
            }

            public override void Write(Utf8JsonWriter writer, TDestinationValue oneOfValue, JsonSerializerOptions options)
            {
                throw new NotSupportedException();
            }
        }
    }
}
