//using GoogleMapsComponents.Maps;
//using OneOf;
//using System;
//using System.Text.Json;
//using System.Text.Json.Serialization;

//namespace GoogleMapsComponents.Serialization
//{
//    //internal class OneOfConverterOneOf3<T0, T1, T2> : JsonConverter<OneOf<T0, T1, T2>>
//    //{
//    //    public override OneOf<T0, T1, T2> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//    //    {
//    //        throw new NotImplementedException();
//    //    }

//    //    public override void Write(Utf8JsonWriter writer, OneOf<T0, T1, T2> value, JsonSerializerOptions options)
//    //    {
//    //        writer.WriteStringValue("HIiiii");
//    //    }
//    //}
//    internal class OneOfConverter : JsonConverter<IOneOf>
//    {
//        //public override IOneOf ReadJson(JsonReader reader, Type objectType, IOneOf existingValue, bool hasExistingValue, JsonSerializer serializer)
//        //{
//        //    throw new NotSupportedException();
//        //}

//        //public override void WriteJson(JsonWriter writer, IOneOf value, JsonSerializer serializer)
//        //{
//        //    if (value.Value == null
//        //        || value.Value is string
//        //        || value.Value is int
//        //        || value.Value is long
//        //        || value.Value is double
//        //        || value.Value is float
//        //        || value.Value is decimal
//        //        || value.Value is SymbolPath
//        //        || value.Value is DateTime)
//        //    {
//        //        serializer.Serialize(writer, value.Value);
//        //    }
//        //    else
//        //    {
//        //        var jo = JObject.FromObject(value.Value, serializer);
//        //        var typeNameProperty = new JProperty("dotnetTypeName", value.Value.GetType().FullName);

//        //        jo.Add(typeNameProperty);
//        //        jo.WriteTo(writer);
//        //    }
//        //}
//        public override IOneOf? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
//        {
//            throw new NotImplementedException();
//        }

//        public override void Write(Utf8JsonWriter writer, IOneOf value, JsonSerializerOptions options)
//        {
//            if (value.Value == null
//                    || value.Value is string
//                    || value.Value is int
//                    || value.Value is long
//                    || value.Value is double
//                    || value.Value is float
//                    || value.Value is decimal
//                    || value.Value is SymbolPath
//                    || value.Value is DateTime)
//            {
//                writer.WriteStringValue(value.Value?.ToString());
//            }
//            else
//            {
//                //var jo = JObject.FromObject(value.Value, serializer);
//                //var typeNameProperty = new JProperty("dotnetTypeName", value.Value.GetType().FullName);
//                var jo = JsonDocument.Parse(value.Value.ToString());
//                var typeToken = jo.RootElement.GetProperty("dotnetTypeName").GetString();

//                //jo.Add(typeNameProperty);
//                //jo.WriteTo(writer);
//            }
//        }
//    }
//}
