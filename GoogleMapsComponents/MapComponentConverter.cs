using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal class MapComponentConverter : JsonConverter<MapComponent>
    {
        public override void WriteJson(JsonWriter writer, MapComponent value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, new JsObjectRef1(value.Guid));
        }

        public override MapComponent ReadJson(JsonReader reader, Type objectType, MapComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();

            //var mapId = reader.ReadAsString();

            //return MapComponentInstances.GetInstance(mapId);
        }
    }
}
