using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal static class Helper
    {
        internal static T ToEnum<T>(string str)
        {
            var enumType = typeof(T);
            foreach (var name in Enum.GetNames(enumType))
            {
                var enumMemberAttribute = ((EnumMemberAttribute[])enumType.GetField(name)
                    .GetCustomAttributes(typeof(EnumMemberAttribute), true)).Single();
                if (enumMemberAttribute.Value == str) return (T)Enum.Parse(enumType, name);
            }

            throw new Exception($"Can't matched Enum member of {typeof(T)} with given value");
        }

        internal static T? ToNullableEnum<T>(string? str)
            where T : struct
        {
            if (str == null || str == "null")
                return default;

            try
            {
                return ToEnum<T>(str);
            }
            catch
            {
                return default;
            }
        }

        internal static T? ToNullableEnum<T>(int? value)
            where T : struct
        {
            if (value == null)
                return default;

            var enumType = typeof(T);

            return (T)Enum.Parse(enumType, value.ToString());
        }

        // this delegate is just, so you don't have to pass an object array. _(params)_
        public delegate object ConstructorDelegate(params object[] args);

        // https://stackoverflow.com/questions/840261/passing-arguments-to-c-sharp-generic-new-of-templated-type
        public static ConstructorDelegate CreateConstructor(Type type, params Type[] parameters)
        {
            // Get the constructor info for these parameters
            var constructorInfo = type.GetConstructor(
                BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                parameters);

            if (constructorInfo == null)
                throw new Exception("Constructor with given signatures not found");

            // define a object[] parameter
            var paramExpr = Expression.Parameter(typeof(Object[]));

            // To feed the constructor with the right parameters, we need to generate an array 
            // of parameters that will be read from the initialize object array argument.
            var constructorParameters = parameters.Select((paramType, index) =>
                // convert the object[index] to the right constructor parameter type.
                Expression.Convert(
                    // read a value from the object[index]
                    Expression.ArrayAccess(
                        paramExpr,
                        Expression.Constant(index)),
                    paramType)).ToArray();

            // just call the constructor.
            var body = Expression.New(constructorInfo, constructorParameters);

            var constructor = Expression.Lambda<ConstructorDelegate>(body, paramExpr);
            return constructor.Compile();
        }

        internal static object? MakeArgJsFriendly(object? arg)
        {
            if (arg == null)
                return arg;

            if (arg is IOneOf oneof)
                arg = oneof.Value;

            return arg switch
            {
                JsInvokableAction _ => DotNetObjectReference.Create(arg),
                JsObjectRef jsObjectRef => jsObjectRef.Reference,
                _ => arg,
            };
        }

        internal static IEnumerable<object?> MakeArgJsFriendly(IJSRuntime jsRuntime, IEnumerable<object> args)
        {
            var jsFriendlyArgs = args.Select(MakeArgJsFriendly);

            return jsFriendlyArgs;
        }

        internal static async ValueTask<object> MyAddListenerAsync(
            this IJSRuntime jsRuntime,
            string identifier,
            params object[] args)
        {

            var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

            return await jsRuntime.InvokeAsync<object>(identifier, jsFriendlyArgs);
        }

        //public static DotNetObjectReference<JsInvokableAction<T>> MakeArgJsFriendly<T>(Action<T> action)
        //{
        //    return DotNetObjectReference.Create(new JsInvokableAction<T>(action));
        //}

        //public static DotNetObjectReference<JsInvokableAction<T, U>> MakeArgJsFriendly<T, U>(Action<T, U> action)
        //{
        //    return DotNetObjectReference.Create(new JsInvokableAction<T, U>(action));
        //}

        //private static string SerializeObject(object obj)
        //{
        //    var value = JsonConvert.SerializeObject(
        //                obj,
        //                Formatting.None,
        //                new JsonSerializerSettings
        //                {
        //                    NullValueHandling = NullValueHandling.Ignore,
        //                    ContractResolver = new CamelCasePropertyNamesContractResolver()
        //                });

        //    return value;
        //}

        //internal static async ValueTask<TRes> MyInvokeAsync<TRes>(
        //    this IJSRuntime jsRuntime,
        //    string identifier,
        //    params object[] args)
        //{

        //    var jsFriendlyArgs = MakeArgJsFriendly(jsRuntime, args);

        //    //if (typeof(IJsObjectRef).IsAssignableFrom(typeof(TRes)))
        //    //{
        //    //    var guid = await jsRuntime.InvokeAsync<string>(identifier, jsFriendlyArgs);

        //    //    return (TRes)JsObjectRefInstances.GetInstance(guid);
        //    //}

        //    if (typeof(IOneOf).IsAssignableFrom(typeof(TRes)))
        //    {
        //        var resultObject = await jsRuntime.InvokeAsync<string>(identifier, jsFriendlyArgs);
        //        object result = null;

        //        if (resultObject is string someText)
        //        {
        //            try
        //            {
        //                var jo = JObject.Parse(someText);

        //                if (jo.ContainsKey("dotnetTypeName"))
        //                {
        //                    var typeName = jo.SelectToken("dotnetTypeName").Value<string>();
        //                    var asm = typeof(Map).Assembly;
        //                    var type = asm.GetType(typeName);
        //                    result = jo.ToObject(type);
        //                }
        //                else
        //                {
        //                    result = someText;
        //                }
        //            }
        //            catch
        //            {
        //                result = someText;
        //            }
        //        }

        //        return (TRes)result;
        //    }
        //    else
        //    {
        //        return await jsRuntime.InvokeAsync<TRes>(identifier, jsFriendlyArgs);
        //    }
        //}

        //internal static async ValueTask MyInvokeAsync(
        //    this IJSRuntime jsRuntime,
        //    string identifier,
        //    params object[] args)
        //{
        //    await jsRuntime.MyInvokeAsync<object>(identifier, args);
        //}

        //private static async ValueTask<object> InvokeAsync(
        //    this IJSRuntime jsRuntime,
        //    string identifier,
        //    params object[] args)
        //{
        //    var resultObject = await jsRuntime.MyInvokeAsync<string>(identifier, args);
        //    object result = null;

        //    if (resultObject is string someText)
        //    {
        //        try
        //        {
        //            var jo = JObject.Parse(someText);

        //            if (jo.ContainsKey("dotnetTypeName"))
        //            {
        //                var typeName = jo.SelectToken("dotnetTypeName").Value<string>();
        //                var asm = typeof(Map).Assembly;
        //                var type = asm.GetType(typeName);
        //                result = jo.ToObject(type);
        //            }
        //            else
        //            {
        //                result = someText;
        //            }
        //        }
        //        catch
        //        {
        //            result = someText;
        //        }
        //    }

        //    return result;
        //}

        ///// <summary>
        ///// For use when returned result will be one of multiple type
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="U"></typeparam>
        ///// <typeparam name="V"></typeparam>
        ///// <param name="jsRuntime"></param>
        ///// <param name="identifier"></param>
        ///// <param name="args"></param>
        ///// <returns>Discriminated union of specified types</returns>
        //internal static async ValueTask<OneOf<T, U>> MyInvokeAsync<T, U>(
        //    this IJSRuntime jsRuntime,
        //    string identifier,
        //    params object[] args)
        //{
        //    var result = await jsRuntime.InvokeAsync(identifier, args);

        //    return result switch
        //    {
        //        T t => t,
        //        U u => u,
        //        _ => default,
        //    };
        //}

        ///// <summary>
        ///// For use when returned result will be one of multiple type
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <typeparam name="U"></typeparam>
        ///// <param name="jsRuntime"></param>
        ///// <param name="identifier"></param>
        ///// <param name="args"></param>
        ///// <returns>Discriminated union of specified types</returns>
        //internal static async ValueTask<OneOf<T, U, V>> MyInvokeAsync<T, U, V>(
        //    this IJSRuntime jsRuntime,
        //    string identifier,
        //    params object[] args)
        //{
        //    var result = await jsRuntime.InvokeAsync(identifier, args);

        //    return result switch
        //    {
        //        T t => t,
        //        U u => u,
        //        V v => v,
        //        _ => default,
        //    };
        //}
    }
}
