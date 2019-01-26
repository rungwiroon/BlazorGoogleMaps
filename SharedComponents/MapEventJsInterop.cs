using Microsoft.JSInterop;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    public static class MapEventJsInterop
    {
        private static readonly Dictionary<Guid, Action<JObject>> registeredEvents 
            = new Dictionary<Guid, Action<JObject>>();

        public static async Task<Guid> SubscribeMapEvent(string mapId, string eventName, Action<JObject> action)
        {
            var guid = Guid.NewGuid();

            await JSRuntime.Current.InvokeAsync<bool>(
                "googleMapEventJsFunctions.addListener",
                guid,
                mapId,
                eventName);

            registeredEvents.Add(guid, action);

            return guid;
        }

        public static async Task<Guid> SubscribeMapEventOnce(string mapId, string eventName, Action<JObject> action)
        {
            var guid = Guid.NewGuid();

            await JSRuntime.Current.InvokeAsync<bool>(
                "googleMapEventJsFunctions.addListenerOnce",
                guid,
                mapId,
                eventName);

            registeredEvents.Add(guid, action);

            return guid;
        }

        public static async Task UnsubscribeMapEvent(string guid)
        {
            await Helper.MyInvokeAsync<bool>(
                "googleMapEventJsFunctions.removeListener",
                guid);
        }

        [JSInvokable]
        public static Task NotifyMapEvent(string guidString, string eventArgs)
        {
            var guid = new Guid(guidString);

            if (eventArgs == null)
            {
                registeredEvents[guid].Invoke(null);
            }
            else
            {
                registeredEvents[guid].Invoke(JObject.Parse(eventArgs));
            }

            return Task.FromResult(true);
        }

        public static async Task<Guid> SubscribeMarkerEvent(
            string markerGuid, 
            string eventName, 
            Action<JObject> action)
        {
            var eventGuid = Guid.NewGuid();

            await JSRuntime.Current.InvokeAsync<bool>(
                "googleMapEventJsFunctions.addMarkerListener",
                eventGuid,
                markerGuid,
                eventName);

            registeredEvents.Add(eventGuid, action);

            return eventGuid;
        }

        //public static async Task UnsubscribeMarkerEvent(string guid)
        //{
        //    await Helper.MyInvokeAsync<bool>(
        //        "googleMapEventJsFunctions.removeMarkerListener",
        //        guid);
        //}

        [JSInvokable]
        public static Task NotifyMarkerEvent(string guidString, string eventArgs)
        {
            var guid = new Guid(guidString);
            registeredEvents[guid].Invoke(JObject.Parse(eventArgs));

            return Task.FromResult(true);
        }
    }
}
