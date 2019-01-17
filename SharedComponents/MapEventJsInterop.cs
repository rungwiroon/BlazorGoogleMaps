using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    public static class MapEventJsInterop
    {
        private static readonly Dictionary<Guid, Action<Dictionary<string, object>>> registeredEvents 
            = new Dictionary<Guid, Action<Dictionary<string, object>>>();

        public static async Task<Guid> SubscribeMapEvent(string mapId, string eventName, Action<Dictionary<string, object>> action)
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

        public static async Task UnsubscribeMapEvent(string guid)
        {
            await Helper.MyInvokeAsync<bool>(
                "googleMapEventJsFunctions.removeListener",
                guid);
        }

        [JSInvokable]
        public static Task NotifyMapEvent(string guidString, Dictionary<string, object> eventArgs)
        {
            var guid = new Guid(guidString);
            registeredEvents[guid].Invoke(eventArgs);

            return Task.FromResult(true);
        }

        public static async Task<Guid> SubscribeMarkerEvent(
            string markerGuid, 
            string eventName, 
            Action<Dictionary<string, object>> action)
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
        public static Task NotifyMarkerEvent(string guidString, Dictionary<string, object> eventArgs)
        {
            var guid = new Guid(guidString);
            registeredEvents[guid].Invoke(eventArgs);

            return Task.FromResult(true);
        }
    }
}
