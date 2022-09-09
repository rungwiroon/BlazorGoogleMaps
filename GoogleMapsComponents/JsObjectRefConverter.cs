using Newtonsoft.Json;
using System;

namespace GoogleMapsComponents
{
    internal class JsObjectRefConverter<T> : JsonConverter<T>
        where T : IJsObjectRef
    {
        public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new JsObjectRef1(value.Guid));
        }

        public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
