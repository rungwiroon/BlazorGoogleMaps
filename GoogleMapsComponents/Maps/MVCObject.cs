using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// google.maps.MVCObject class
    /// Base class implementing KVO.
    /// </summary>
    public class MVCObject : Object
    {
        internal MVCObject(IJSObjectReference jsObjectRef) : base(jsObjectRef)
        {
        }

        internal MVCObject(IJSObjRefWrapper jsObjectRef) : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Adds the given listener function to the given event name.
        /// Returns an identifier for this listener that can be used with google.maps.event.removeListener.
        /// </summary>
        public async ValueTask<MapEventListener> AddListenerAsync(
            string eventName,
            Action handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JSInvokableAction(handler));

            var listenerRef = await this.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addListenerNoArgument",
                eventName,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);
        }

        /// <summary>
        /// Adds the given listener function to the given event name.
        /// Returns an identifier for this listener that can be used with google.maps.event.removeListener.
        /// </summary>
        public async ValueTask<MapEventListener> AddListenerAsync(
            string eventName,
            Func<Task> handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JSInvokableAsyncAction(handler));

            var listenerRef = await this.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addAsyncListenerNoArgument",
                eventName,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);
        }

        /// <summary>
        /// Adds the given listener function to the given event name.
        /// Returns an identifier for this listener that can be used with google.maps.event.removeListener.
        /// </summary>
        public virtual async ValueTask<MapEventListener> AddListenerAsync<T>(
            string eventName,
            Action<T> handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JSInvokableFunc<T, bool?>(wrapHandler));

            var listenerRef = await this.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addListenerWithArgument",
                eventName,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);

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

        /// <summary>
        /// Adds the given listener function to the given event name.
        /// Returns an identifier for this listener that can be used with google.maps.event.removeListener.
        /// </summary>
        public virtual async ValueTask<MapEventListener> AddListenerAsync<T>(
            string eventName,
            Func<T, Task> handler)
        {
            var dotNetObjRef = DotNetObjectReference.Create(
                new JSInvokableAsyncFunc<T, bool?>(wrapHandler));

            var listenerRef = await this.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addAsyncListenerWithArgument",
                eventName,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);

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
    }

    internal static class ListenerExtension
    {
        public static async ValueTask<MapEventListener> AddListenerAsync<T>(
            this MVCObject mvcObject,
            string eventName,
            DotNetObjectReference<T> dotNetObjRef)
            where T : class
        {
            var listenerRef = await mvcObject.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addListenerWithArgument",
                eventName,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);
        }

        public static async ValueTask<MapEventListener> AddListenerAsync<T>(
            this MVCObject mvcObject,
            string eventName,
            IEnumerable<string> referenceProperties,
            DotNetObjectReference<T> dotNetObjRef)
            where T : class
        {
            var listenerRef = await mvcObject.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addListenerWithArgument2",
                eventName,
                referenceProperties,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);
        }

        public static async ValueTask<MapEventListener> AddAsyncListenerAsync<T>(
            this MVCObject mvcObject,
            string eventName,
            DotNetObjectReference<T> dotNetObjRef)
            where T : class
        {
            var listenerRef = await mvcObject.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addAsyncListenerWithArgument",
                eventName,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);
        }

        public static async ValueTask<MapEventListener> AddAsyncListenerAsync<T>(
            this MVCObject mvcObject,
            string eventName,
            IEnumerable<string> referenceProperties,
            DotNetObjectReference<T> dotNetObjRef)
            where T : class
        {
            var listenerRef = await mvcObject.InvokeAsync<IJSObjectReference>(
                "extensionFunctions.addAsyncListenerWithArgument2",
                eventName,
                referenceProperties,
                dotNetObjRef);

            return new MapEventListener(listenerRef, dotNetObjRef);
        }
    }
}
