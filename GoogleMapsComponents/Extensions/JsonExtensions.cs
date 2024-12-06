using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Extensions;
internal static class JsonExtensions
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    static JsonExtensions()
    {
        Options.Converters.Add(new OneOfConverterFactory());
        Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    public static object? DeSerializeObject(JsonElement json, Type type)
    {
        var obj = json.Deserialize(type, Options);
        return obj;
    }

    public static T? DeSerializeObject<T>(JsonElement json)
    {
        return json.Deserialize<T>(Options);
    }

    public static object? DeSerializeObject(string? json, Type type)
    {
        if (json == null)
        {
            return default;
        }

        var obj = JsonSerializer.Deserialize(json, type, Options);
        return obj;
    }

    public static TObject? DeSerializeObject<TObject>(string? json)
    {
        if (json == null)
        {
            return default;
        }

        var value = JsonSerializer.Deserialize<TObject>(json, Options);
        return value;
    }

    public static string SerializeObject(object obj)
    {
        var value = JsonSerializer.Serialize(obj, Options);
        return value;
    }
}
