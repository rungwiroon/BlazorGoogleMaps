using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// https://googlemaps.github.io/v3-utility-library/modules/_google_markerclustererplus.html
    /// </summary>
    public class MarkerClustering : JsObjectRef
    {
        //private readonly JsObjectRef _jsObjectRef;
        //public Guid Guid => _jsObjectRef.Guid;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public static async Task<MarkerClustering> CreateAsync(
            IJSRuntime jsRuntime,
            Map map,
            IEnumerable<Marker> markers,
            MarkerClustererOptions options = null
           )
        {
            //if (options == null)
            //{
            //    options = new MarkerClustererOptions();
            //}

            //var guid = System.Guid.NewGuid();
            //var jsObjectRef = new JsObjectRef(jsRuntime, guid);
            //await jsRuntime.InvokeVoidAsync("googleMapsObjectManager.addClusteringMarkers", guid.ToString(), map.Guid.ToString(), markers, options);
            //var obj = new MarkerClustering(jsObjectRef);
            //return obj;

            throw new NotImplementedException();
        }

        internal MarkerClustering(JsObjectRef jsObjectRef)
            : base(jsObjectRef)
        {
            //_jsObjectRef = jsObjectRef;
            EventListeners = new();
        }

        public virtual async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>("addListener", eventName, handler);
            var eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }

            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public virtual async Task<MapEventListener> AddListener<V>(string eventName, Action<V> handler)
        {
            var listenerRef = await InvokeAsync<IJSObjectReference>("addListener", eventName, handler);
            var eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }

            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public virtual async Task SetMap(Map map)
        {
            await InvokeVoidAsync("setMap", map);
        }

        /// <summary>
        /// Removes all clusters and markers from the map and also removes all markers managed by the clusterer.
        /// </summary>
        public virtual async Task ClearMarkers()
        {
            await InvokeVoidAsync("clearMarkers");
        }

        /// <summary>
        /// Fits the map to the bounds of the markers managed by the clusterer.
        /// </summary>
        /// <param name="padding"></param>
        public virtual async Task FitMapToMarkers(int padding)
        {
            await InvokeVoidAsync("fitMapToMarkers", padding);
        }

        /// <summary>
        /// Recalculates and redraws all the marker clusters from scratch. Call this after changing any properties.
        /// </summary>
        public virtual async Task Repaint()
        {
            await InvokeVoidAsync("repaint");
        }

        public virtual async Task ClearListeners(string eventName)
        {
            if (EventListeners.ContainsKey(eventName))
            {
                await InvokeVoidAsync("clearListeners", eventName);

                //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
                //so, instead to clear the list and remove the key from dictionary, I prefer to leave the key with an empty list
                EventListeners[eventName].Clear();
            }
        }
    }
}