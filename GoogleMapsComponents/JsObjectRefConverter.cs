using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents
{
    internal class JsObjectRefConverter<T> : JsonConverter<T>
        where T : IJsObjectRef
    {
        //public override void WriteJson(JsonWriter writer, T value, JsonSerializer serializer)
        //{
        //    serializer.Serialize(writer, new JsObjectRef1(value.Guid));
        //}

        //public override T ReadJson(JsonReader reader, Type objectType, T existingValue, bool hasExistingValue, JsonSerializer serializer)
        //{
        //    throw new NotSupportedException();
        //}
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            var json = Helper.SerializeObject(new JsObjectRef1(value.Guid));
            writer.WriteStringValue(json);
        }
    }
}
