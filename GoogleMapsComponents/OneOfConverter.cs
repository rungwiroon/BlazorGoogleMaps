using GoogleMapsComponents.Maps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            if(value.Value == null
                || value.Value is string
                || value.Value is int
                || value.Value is long
                || value.Value is double
                || value.Value is float
                || value.Value is decimal
                || value.Value is SymbolPath
                || value.Value is DateTime)
            {
                serializer.Serialize(writer, value.Value);
            }
            else
            {
                var jo = JObject.FromObject(value.Value, serializer);
                var typeNameProperty = new JProperty("dotnetTypeName", value.Value.GetType().FullName);

                jo.Add(typeNameProperty);
                jo.WriteTo(writer);
            }
        }
    }
}
