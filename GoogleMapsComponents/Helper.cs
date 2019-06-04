using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal static class Helper
    {
        internal static Task MyInvokeAsync(
            this IJSRuntime jsRuntime,
            string identifier,
            params object[] args)
        {
            return jsRuntime.MyInvokeAsync<object>(identifier, args);
        }

        private static string SerializeObject(object obj)
        {
            return JsonConvert.SerializeObject(
                        obj,
                        Formatting.None,
                        new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore,
                            ContractResolver = new CamelCasePropertyNamesContractResolver()
                        });
        }

        internal static Task<TRes> MyInvokeAsync<TRes>(
            this IJSRuntime jsRuntime,
            string identifier, 
            params object[] args)
        {
            var jsFriendlyArgs = args
                .Select(arg =>
                {
                    var argType = arg.GetType();

                    if (arg is ElementRef
                        || arg is string
                        || arg is int
                        || arg is long
                        || arg is double
                        || arg is float
                        || arg is decimal
                        || arg is DateTime
                        || arg is null)
                    {
                        return arg;
                    }
                    else if (arg is Action)
                    {
                        return new DotNetObjectRef(
                            new JsCallableAction((Action)arg));
                    }
                    else if (argType.IsGenericType
                        && (argType.GetGenericTypeDefinition() == typeof(Action<>)
                        || argType.GetGenericTypeDefinition() == typeof(Action<,>)
                        || argType.GetGenericTypeDefinition() == typeof(Action<,,>)
                        || argType.GetGenericTypeDefinition() == typeof(Action<,,,>)))
                    {
                        var genericArguments = argType.GetGenericArguments();

                        //Debug.WriteLine($"Generic args : {genericArguments.Count()}");

                        return new DotNetObjectRef(new JsCallableAction((Delegate)arg, genericArguments));
                    }
                    else if (arg is JsCallableAction)
                    {
                        return new DotNetObjectRef(arg);
                    }
                    else if (arg is IJsObjectRef)
                    {
                        Debug.WriteLine("Serialize IJsObjectRef");

                        var guid = ((IJsObjectRef)arg).Guid;
                        return SerializeObject(new JsObjectRef1(guid));
                    }
                    else
                    {
                        return SerializeObject(arg);
                    }
                });

            return jsRuntime.InvokeAsync<TRes>(identifier, jsFriendlyArgs);
        }

        internal static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name).GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }

            //throw exception or whatever handling you want or
            return default;
        }
    }
}
