using GoogleMapsComponents.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            Console.WriteLine("What?");

            var jo = JObject.Load(reader);
            Console.WriteLine(jo);

            var guid = jo["guidString"].ToObject<string>();

            return (T)JsObjectRefInstances.GetInstance(guid);
        }
    }
}
