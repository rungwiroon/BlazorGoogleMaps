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
            writer.WriteValue(value.Guid);
        }

        public override MapComponent ReadJson(JsonReader reader, Type objectType, MapComponent existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var mapId = reader.ReadAsString();

            return MapComponentInstances.GetInstance(mapId);
        }
    }
}
