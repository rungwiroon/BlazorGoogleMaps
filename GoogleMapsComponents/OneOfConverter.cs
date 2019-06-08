using GoogleMapsComponents.Maps;
using Newtonsoft.Json;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal class OneOfConverter : JsonConverter<IOneOf>
    {
        public override IOneOf ReadJson(JsonReader reader, Type objectType, IOneOf existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotSupportedException();
        }

        public override void WriteJson(JsonWriter writer, IOneOf value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.Value);
        }
    }
}
