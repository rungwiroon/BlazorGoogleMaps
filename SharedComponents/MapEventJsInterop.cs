using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    public static class MapEventJsInterop
    {
        private static readonly Dictionary<Guid, Action> mapRegisterdBoundsChangedEvents = new Dictionary<Guid, Action>();

        public static async Task<Guid> SubscribeEvent(string mapId, string eventName, Action action)
        {
            var guid = Guid.NewGuid();

            await JSRuntime.Current.InvokeAsync<bool>(
                "googleMapEventJsFunctions.addListener",
                guid,
                mapId,
                eventName);

            mapRegisterdBoundsChangedEvents.Add(guid, action);

            return guid;
        }

        public static async Task UnsubscribeEvent(string guid)
        {
            await Helper.MyInvokeAsync<bool>(
                "googleMapEventJsFunctions.removeListener",
                guid);
        }

        [JSInvokable]
        public static Task NotifyEvent(string guidString)
        {
            var guid = new Guid(guidString);
            mapRegisterdBoundsChangedEvents[guid].Invoke();

            return Task.FromResult(true);
        }
    }
}
