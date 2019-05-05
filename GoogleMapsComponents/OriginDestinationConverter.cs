using GoogleMapsComponents.Maps;
using Newtonsoft.Json;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal class OrginDestinationConverter : JsonConverter<OneOf<string, LatLngLiteral, Place>>
    {
        public override void WriteJson(JsonWriter writer, OneOf<string, LatLngLiteral, Place> value, JsonSerializer serializer)
        {
            writer.WriteValue(value.Value);
        }

        public override OneOf<string, LatLngLiteral, Place> ReadJson(
            JsonReader reader, 
            Type objectType, 
            OneOf<string, LatLngLiteral, Place> existingValue, 
            bool hasExistingValue, 
            JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }
    }
}
