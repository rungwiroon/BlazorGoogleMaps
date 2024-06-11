using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Serialization;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

internal static class Helper
{
    private static readonly JsonSerializerOptions Options = new JsonSerializerOptions()
    {
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    };

    static Helper()
    {
        Options.Converters.Add(new OneOfConverterFactory());
        Options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));
    }

    internal static Task MyInvokeAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object?[] args)
    {
        return jsRuntime.MyInvokeAsync<object>(identifier, args);
    }

    internal static T? ToNullableEnum<T>(string? str)
        where T : struct
    {
        var enumType = typeof(T);

        if (int.TryParse(str, out var enumIntValue))
        {
            return (T)Enum.Parse(enumType, enumIntValue.ToString());
        }

        if (str == "null")
        {
            return null;
        }

        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str)
            {
                return (T)Enum.Parse(enumType, name);
            }
        }

        //throw exception or whatever handling you want
        return default;
    }

    public static object? DeSerializeObject(JsonElement json, Type type)
    {
        var obj = json.Deserialize(type, Options);
        return obj;
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

    private static IEnumerable<object> MakeArgJsFriendly(IJSRuntime jsRuntime, IEnumerable<object?> args)
    {
        var jsFriendlyArgs = args
            .Select(arg =>
            {
                if (arg == null)
                {
                    return arg;
                }

                if (arg is IOneOf oneof)
                {
                    arg = oneof.Value;
                }

                var argType = arg.GetType();

                switch (arg)
                {
                    case Enum: return GetEnumValue(arg);
                    case ElementReference _:
                    case string _:
                    case int _:
                    case long _:
                    case double _:
                    case float _:
                    case decimal _:
                    case DateTime _:
                    case bool _:
                        return arg;
                    case Action action:
                        return DotNetObjectReference.Create(new JsCallableAction(jsRuntime, action));
                    default:
                        {
                            if (argType.IsGenericType
                                && (argType.GetGenericTypeDefinition() == typeof(Action<>)))
                            {
                                var genericArguments = argType.GetGenericArguments();

                                //Debug.WriteLine($"Generic args : {genericArguments.Count()}");

                                return DotNetObjectReference.Create(new JsCallableAction(jsRuntime, (Delegate)arg, genericArguments));
                            }

                            switch (arg)
                            {
                                case JsCallableAction _:
                                    return DotNetObjectReference.Create(arg);
                                case IJsObjectRef jsObjectRef:
                                    {
                                        //Debug.WriteLine("Serialize IJsObjectRef");

                                        var guid = jsObjectRef.Guid;
                                        return SerializeObject(new JsObjectRef1(guid));
                                    }
                                default:
                                    return SerializeObject(arg);
                            }
                        }
                }
            });

        return jsFriendlyArgs;
    }

    private static string? GetEnumValue(object? enumItem)
    {
        //what happens if enumItem is null.
        //Shouldnt we take 0 value of enum
        //Also is it even possible to have null enumItem
        //So far looks like only MapLegend Add controll reach here
        if (enumItem == null)
        {
            return null;
        }

        if (enumItem is not Enum enumItem2)
        {
            return enumItem.ToString();
        }

        var memberInfo = enumItem2.GetType().GetMember(enumItem2.ToString());
        if (memberInfo.Length == 0)
        {
            return null;
        }

        foreach (var attr in memberInfo[0].GetCustomAttributes(false))
        {
            if (attr is EnumMemberAttribute val)
            {
                return val.Value;
            }
        }

        return null;
    }

    internal static async Task<TRes?> MyInvokeAsync<TRes>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object?[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

        if (typeof(IJsObjectRef).IsAssignableFrom(typeof(TRes)))
        {
            var guid = await jsRuntime.InvokeAsync<string?>(identifier, jsFriendlyArgs);

            return guid == null ? default : (TRes)JsObjectRefInstances.GetInstance(guid);
        }

        if (typeof(IOneOf).IsAssignableFrom(typeof(TRes)))
        {
            var resultObject = await jsRuntime.InvokeAsync<string>(identifier, jsFriendlyArgs);
            object? result = null;

            if (resultObject is string someText)
            {
                try
                {
                    var jo = JsonDocument.Parse(someText);
                    var typeToken = jo.RootElement.GetProperty("dotnetTypeName").GetString();
                    if (typeToken != null)
                    {
                        result = DeSerializeObject<TRes>(typeToken);
                    }
                    else
                    {
                        result = someText;
                    }
                }
                catch
                {
                    result = someText;
                }
            }

            return (TRes?)result;
        }
        else
        {
            return await jsRuntime.InvokeAsync<TRes>(identifier, jsFriendlyArgs);
        }
    }

    internal static async Task<object> MyAddListenerAsync(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

        return await jsRuntime.InvokeAsync<object>(identifier, jsFriendlyArgs);
    }

    /// <summary>
    /// For use when returned result will be one of multiple type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    internal static async Task<OneOf<T, U>> MyInvokeAsync<T, U>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, args);
        object? result = null;

        if (resultObject is JsonElement jsonElement)
        {
            string? json;
            if (jsonElement.ValueKind == JsonValueKind.Number)
            {
                json = jsonElement.GetRawText();
            }
            else if (jsonElement.ValueKind == JsonValueKind.String)
            {
                json = jsonElement.GetString();
                //Not sure if this is ok
                //Basically fails for marker GetLabel if skip this
                if (typeof(T) == typeof(string))
                {
                    result = json ?? "";
                    return (T)result;
                }

                if (typeof(U) == typeof(string))
                {
                    result = json ?? "";
                    return (T)result;
                }
            }
            else
            {
                json = jsonElement.GetString();
            }

            var propArray = Helper.DeSerializeObject<Dictionary<string, object>>(json);
            if (propArray?.TryGetValue("dotnetTypeName", out var typeName) ?? false)
            {
                var asm = typeof(Map).Assembly;
                var typeNameString = typeName.ToString();
                var type = asm.GetType(typeNameString);
                result = Helper.DeSerializeObject(json, type);
            }
        }

        switch (result)
        {
            case T t:
                return t;
            case U u:
                return u;
            default:
                return default;
        }
    }

    /// <summary>
    /// For use when returned result will be one of multiple type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="jsRuntime"></param>
    /// <param name="identifier"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    internal static async Task<OneOf<T, U, V>> MyInvokeAsync<T, U, V>(
        this IJSRuntime jsRuntime,
        string identifier,
        params object[] args)
    {
        var resultObject = await jsRuntime.MyInvokeAsync<object>(identifier, args);
        object? result = null;

        if (resultObject is JsonElement jsonElement)
        {
            var json = jsonElement.GetString();
            var propArray = Helper.DeSerializeObject<Dictionary<string, object>>(json);
            if (propArray?.TryGetValue("dotnetTypeName", out var typeName) ?? false)
            {
                var asm = typeof(Map).Assembly;
                var typeNameString = typeName.ToString();
                var type = asm.GetType(typeNameString);
                result = Helper.DeSerializeObject(json, type);
            }
        }

        switch (result)
        {
            case T t:
                return t;
            case U u:
                return u;
            case V v:
                return v;
            default:
                return default;
        }
    }

    internal static T? ToEnum<T>(string str)
    {
        var enumType = typeof(T);
        foreach (var name in Enum.GetNames(enumType))
        {
            var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
            if (enumMemberAttribute.Value == str)
            {
                return (T)Enum.Parse(enumType, name);
            }

            if (string.Equals(name, str, StringComparison.InvariantCultureIgnoreCase))
            {
                return (T)Enum.Parse(enumType, name);
            }
        }

        //throw exception or whatever handling you want
        return default;
    }
}