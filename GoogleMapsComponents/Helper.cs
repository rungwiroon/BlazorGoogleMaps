using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
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

        internal static Task<TRes> MyInvokeAsync<TRes>(
            this IJSRuntime jsRuntime,
            string identifier, 
            params object[] args)
        {
            var jsFriendlyArgs = args
                .Select(arg =>
                {
                    var argType = arg.GetType();

                    if(argType == typeof(ElementRef)
                        || argType == typeof(string)
                        || argType == typeof(int)
                        || argType == typeof(long)
                        || argType == typeof(double)
                        || argType == typeof(float)
                        || argType == typeof(decimal)
                        || argType == typeof(DateTime))
                    {
                        return arg;
                    }
                    else if(argType == typeof(Action))
                    {
                        return new DotNetObjectRef(
                            new JsCallableAction((Action)arg));
                    }
                    else if(argType.IsGenericType 
                        && (argType.GetGenericTypeDefinition() == typeof(Action<>)
                        || argType.GetGenericTypeDefinition() == typeof(Action<,>)
                        || argType.GetGenericTypeDefinition() == typeof(Action<,,>)
                        || argType.GetGenericTypeDefinition() == typeof(Action<,,,>)))
                    {
                        var genericArguments = argType.GetGenericArguments();

                        //Debug.WriteLine($"Generic args : {genericArguments.Count()}");

                        return new DotNetObjectRef(new JsCallableAction((Delegate)arg, genericArguments));
                    }
                    else if(argType == typeof(JsCallableAction))
                    {
                        return new DotNetObjectRef(arg);
                    }
                    else
                    {
                        return JsonConvert.SerializeObject(
                                arg,
                                Formatting.None,
                                new JsonSerializerSettings
                                {
                                    NullValueHandling = NullValueHandling.Ignore,
                                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                                });
                    }
                });

            return jsRuntime.InvokeAsync<TRes>(identifier, jsFriendlyArgs);
        }

        //internal static Task<TRes> InvokeWithDefinedGuidAsync<TRes>(
        //    this IJSRuntime jsRuntime,
        //    string identifier, 
        //    string guid, 
        //    params object[] args)
        //{
        //    var argsJson = JsonConvert.SerializeObject(args,
        //                    Formatting.None,
        //                    new JsonSerializerSettings
        //                    {
        //                        NullValueHandling = NullValueHandling.Ignore,
        //                        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //                    });

        //    return jsRuntime.InvokeAsync<TRes>(identifier, guid, argsJson);
        //}

        //internal static Task<TRes> InvokeWithDefinedGuidAndMethodAsync<TRes>(
        //    this IJSRuntime jsRuntime,
        //    string identifier, 
        //    string guid, 
        //    string method, 
        //    params object[] args)
        //{
        //    var argsJson = JsonConvert.SerializeObject(args,
        //                    Formatting.None,
        //                    new JsonSerializerSettings
        //                    {
        //                        NullValueHandling = NullValueHandling.Ignore,
        //                        ContractResolver = new CamelCasePropertyNamesContractResolver()
        //                    });

        //    return jsRuntime.InvokeAsync<TRes>(identifier, guid, method, argsJson);
        //}

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
