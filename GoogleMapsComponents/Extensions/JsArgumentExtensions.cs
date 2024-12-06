using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Extensions;
internal static class JsArgumentExtensions
{
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

    internal static DotNetObjectReference<JsAsyncCallableAction<T>> MakeCallbackJsFriendly<T>(this IJSRuntime jsRuntime, EventCallback<T> callback)
    {
        return DotNetObjectReference.Create(new JsAsyncCallableAction<T>(jsRuntime, callback));
    }

    internal static IEnumerable<object> MakeArgJsFriendly(this IJSRuntime jsRuntime, IEnumerable<object?> args)
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
                    case EventCallback callback:
                        return DotNetObjectReference.Create(new JsAsyncCallableAction(callback));
                    default:
                        {
                            if (argType.IsGenericType && argType.GetGenericTypeDefinition() == typeof(Action<>))
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
                                        return JsonExtensions.SerializeObject(new JsObjectRef1(guid));
                                    }
                                default:
                                    return JsonExtensions.SerializeObject(arg);
                            }
                        }
                }
            });

        return jsFriendlyArgs;
    }
}
