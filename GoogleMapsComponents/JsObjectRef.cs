using GoogleMapsComponents.Maps;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class JsObjectRef : IJSObjectReference
    {
        private readonly IJSObjectReference _jsObjectRef;

        internal IJSObjectReference Reference => _jsObjectRef;

        public JsObjectRef(IJSObjectReference jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
        }

        public async ValueTask DisposeAsync()
        {
            await _jsObjectRef.DisposeAsync();
        }

        public ValueTask<TValue> InvokeAsync<TValue>(string identifier, params object?[]? args)
        {
            return _jsObjectRef.InvokeAsync<TValue>(identifier, args);
        }

        public ValueTask<TValue> InvokeAsync<TValue>(
            string identifier, CancellationToken cancellationToken, params object?[]? args)
        {
            return _jsObjectRef.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }

        public ValueTask InvokeVoidAsync(string identifier, params object?[]? args)
        {
            return _jsObjectRef.InvokeVoidAsync(identifier, args);
        }

        /// <summary>
        /// Use when returned result will be one of defined types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <param name="jsRuntime"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns>Discriminated union of specified types</returns>
        public async ValueTask<OneOf<T, U>> InvokeAsync<T, U>(string identifier, params object?[]? args)
        {
            var value = await InvokeAsync<object>(identifier, args);

            return value switch
            {
                T t => t,
                U u => u,
                object obj => throw new Exception($"Unspecify return type {obj.GetType()}")
            };
        }

        /// <summary>
        /// Use when returned result will be one of defined types
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="jsRuntime"></param>
        /// <param name="identifier"></param>
        /// <param name="args"></param>
        /// <returns>Discriminated union of specified types</returns>
        public async ValueTask<OneOf<T, U, V>> InvokeAsync<T, U, V>(string identifier, params object?[]? args)
        {
            var value = await InvokeAsync<JsonElement>(identifier, args);

            return value switch
            {
                T t => t,
                U u => u,
                V v => v,
                object obj => throw new Exception($"Unspecify return type {obj.GetType()}")
            };
        }

        public async ValueTask<TValue?> InvokeWithReturnedObjectRefAsync<TValue>(
            string identifier,
            Func<IJSObjectReference, TValue> objectCreator,
            params object?[]? args)
        {
            var returnedValue = await InvokeAsync<IJSObjectReference?>(identifier, args);

            return returnedValue != null
                ? objectCreator(returnedValue)
                : default;
        }
    }

    internal static class JsObjectRefExtension
    {
        public static ValueTask<IJSObjectReference> AddListenerAsync(
            this JsObjectRef jsObjectRef,
            string eventName,
            Action handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JsInvokableAction(handler));

            return jsObjectRef.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addListenerNoArgument",
                eventName,
                dotNetObjRef);
        }

        //public static ValueTask<IJSObjectReference> AddListenerAsync(
        //    this JsObjectRef jsObjectRef,
        //    string eventName,
        //    Func<Task> handler)
        //{
        //    var dotNetObjRef = DotNetObjectReference.Create(
        //        new JsInvokableAsyncAction(handler));

        //    return jsObjectRef.InvokeAsync<IJSObjectReference>(
        //        "extensionFunctions.addAsyncListenerNoArgument",
        //        eventName,
        //        dotNetObjRef);
        //}

        public static async ValueTask<IJSObjectReference> AddListenerAsync<T>(
            this MVCObject jsObjectRef,
            string eventName,
            Action<T> handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JsInvokableFunc<T, bool?>(wrapHandler));

            return await jsObjectRef.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addListenerWithArgument",
                eventName,
                dotNetObjRef);

            bool? wrapHandler(T obj)
            {
                handler(obj);

                if (obj is MouseEvent mouseEvent)
                {
                    return mouseEvent.StopStatus;
                }

                return null;
            }
        }

        public static async ValueTask<IJSObjectReference> AddListenerAsync<T>(
            this MVCObject jsObjectRef,
            string eventName,
            Func<T, Task> handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JsInvokableAsyncFunc<T, bool?>(wrapHandler));

            return await jsObjectRef.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addAsyncListenerWithArgument",
                eventName,
                dotNetObjRef);

            async Task<bool?> wrapHandler(T obj)
            {
                await handler(obj);

                if (obj is MouseEvent mouseEvent)
                {
                    return mouseEvent.StopStatus;
                }

                return null;
            }
        }

        public static async ValueTask<ReferenceAndValue<TValue>> InvokeAsyncReturnedReferenceAndValue<TValue>(
            this JsObjectRef jsObjectRef,
            string identifier,
            params object?[]? args)
        {
            var returnedValue = await jsObjectRef.InvokeAsync<ReferenceAndValue<TValue>>(
                "extensionFunctions.invokeAsyncReturnReferenceAndValue",
                identifier,
                args);

            return returnedValue;
        }
    }
}
