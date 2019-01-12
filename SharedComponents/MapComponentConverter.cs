using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    internal class MapComponentConverter : JsonConverter<MapComponent>
    {
        public override void WriteJson(JsonWriter writer, MapComponent value, JsonSerializer serializer)
        {
            writer.WriteValue(value.DivId);
        }

        public override MapComponent ReadJson(JsonReader reader, Type objectType, MapComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var mapId = reader.ReadAsString();

            return (GoogleMap)MapComponentInstances.GetInstance(mapId);
        }
    }
}
