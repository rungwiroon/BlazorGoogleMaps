using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Object : IAsyncDisposable
    {
        internal readonly IJSObjRefWrapper refWrapper;

        internal IJSObjectReference Reference => refWrapper.Value;

        public Object(IJSObjectReference jsObjectRef)
        {
            this.refWrapper = new JSObjRefWrapper(jsObjectRef);
        }

        internal Object(IJSObjRefWrapper jsObjectRef)
        {
            this.refWrapper = jsObjectRef;
        }

        public async ValueTask DisposeAsync()
        {
            await refWrapper.DisposeAsync();
        }

        internal ValueTask<TValue> InvokeAsync<TValue>(
            string identifier, params object?[]? args)
        {
            return refWrapper.InvokeAsync<TValue>(identifier, args);
        }

        internal ValueTask<TValue> InvokeAsync<TValue>(
            string identifier, CancellationToken cancellationToken, params object?[]? args)
        {
            return refWrapper.InvokeAsync<TValue>(identifier, cancellationToken, args);
        }
    }

    internal static class ObjectExtension
    {
        public static async ValueTask InvokeVoidAsync(
             this Object obj,
             string identifier,
             params object?[]? args)
        {
            await obj.refWrapper.InvokeAsync<object?>(identifier, args);
        }

        public static async ValueTask<TValue?> InvokeWithReturnedObjectRefAsync<TValue>(
            this Object obj,
            string identifier,
            Func<IJSObjectReference, TValue> objectCreator,
            params object?[]? args)
        {
            var returnedValue = await obj.InvokeAsync<IJSObjectReference?>(identifier, args);

            return returnedValue != null
                ? objectCreator(returnedValue)
                : default;
        }

        public static async ValueTask<ReferenceAndValue<TValue>> InvokeAsyncReturnedReferenceAndValue<TValue>(
            this Object obj,
            string identifier,
            IEnumerable<string>? ignoredProperties,
            params object?[]? args)
        {
            var returnedValue = await obj.InvokeAsync<ReferenceAndValue<TValue>>(
                "extensionFunctions.invokeAsyncReturnReferenceAndValue",
                Enumerable.Empty<object?>()
                    .Append(identifier)
                    .Append(ignoredProperties)
                    .Concat(args ?? Enumerable.Empty<object?>())
                    .ToArray());

            return returnedValue;
        }

        public static async ValueTask<TValue> InvokeAsync<TValue>(
            this Object obj,
            string identifier,
            IEnumerable<string>? ignoredProperties,
            params object?[]? args)
        {
            var returnedValue = await obj.InvokeAsync<TValue>(
                "extensionFunctions.invokeAsyncReturnFilteredValue",
                Enumerable.Empty<object?>()
                    .Append(identifier)
                    .Append(ignoredProperties)
                    .Concat(args ?? Enumerable.Empty<object?>())
                    .ToArray());

            return returnedValue;
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
        public static async ValueTask<OneOf<T, U>> InvokeAsync<T, U>(
            this Object obj,
            string identifier,
            params object?[]? args)
        {
            var value = await obj.InvokeAsync<object>(identifier, args);

            return value switch
            {
                T t => t,
                U u => u,
                object o => throw new Exception($"Unspecify return type {o.GetType()}")
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
        public static async ValueTask<OneOf<T, U, V>> InvokeAsync<T, U, V>(
            this Object obj,
            string identifier,
            params object?[]? args)
        {
            var value = await obj.InvokeAsync<JsonElement>(identifier, args);

            return value switch
            {
                T t => t,
                U u => u,
                V v => v,
                object o => throw new Exception($"Unspecify return type {o.GetType()}")
            };
        }
    }
}
