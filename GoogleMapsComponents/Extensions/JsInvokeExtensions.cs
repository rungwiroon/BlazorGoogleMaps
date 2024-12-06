using GoogleMapsComponents.Maps.Strings;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Extensions;
internal static class JsInvokeExtensions
{
    internal static async Task<TRes> AddListener<TRes, T>(
        this IJSRuntime jsRuntime,
        EventCallback<T> callback, 
        params object?[] args)
    {
        var callBackJs = jsRuntime.MakeCallbackJsFriendly(callback);
        var jsFriendlyArgs = jsRuntime.MakeArgJsFriendly(args).Append(callBackJs);
        

        if (typeof(IJsObjectRef).IsAssignableFrom(typeof(TRes)))
        {
            var guid = await jsRuntime.InvokeAsync<string?>(Methods.InvokeWithReturnedObjectRef, jsFriendlyArgs);

            return guid == null ? default : (TRes)JsObjectRefInstances.GetInstance(guid);
        }

        if (typeof(IOneOf).IsAssignableFrom(typeof(TRes)))
        {
            var resultObject = await jsRuntime.InvokeAsync<string>(Methods.InvokeWithReturnedObjectRef, jsFriendlyArgs);
            object? result = null;

            if (resultObject is string someText)
            {
                try
                {
                    var jo = JsonDocument.Parse(someText);
                    var typeToken = jo.RootElement.GetProperty("dotnetTypeName").GetString();
                    if (typeToken != null)
                    {
                        result = JsonExtensions.DeSerializeObject<TRes>(typeToken);
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
            return await jsRuntime.InvokeAsync<TRes>(Methods.InvokeWithReturnedObjectRef, jsFriendlyArgs);
        }
    }
}
