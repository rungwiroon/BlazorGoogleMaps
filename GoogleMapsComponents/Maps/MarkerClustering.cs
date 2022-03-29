using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// https://github.com/googlemaps/js-markerclusterer
    /// </summary>
    public class MarkerClustering : IJsObjectRef
    {
        private readonly JsObjectRef _jsObjectRef;
        public Guid Guid => _jsObjectRef.Guid;
        private Map _map;
        private readonly IEnumerable<Marker> _originalMarkers;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public static async Task<MarkerClustering> CreateAsync(
            IJSRuntime jsRuntime,
            Map map,
            IEnumerable<Marker> markers,
            MarkerClustererOptions options = null
           )
        {
            if (options == null)
            {
                options = new MarkerClustererOptions();
            }
            var guid = System.Guid.NewGuid();
            var jsObjectRef = new JsObjectRef(jsRuntime, guid);
            await jsRuntime.InvokeVoidAsync("googleMapsObjectManager.addClusteringMarkers", guid.ToString(), map.Guid.ToString(), markers, options);
            var obj = new MarkerClustering(jsObjectRef, map, markers);
            return obj;
        }

        internal MarkerClustering(JsObjectRef jsObjectRef, Map map, IEnumerable<Marker> markers)
        {
            _jsObjectRef = jsObjectRef;
            _map = map;
            _originalMarkers = markers;
            EventListeners = new Dictionary<string, List<MapEventListener>>();
        }

        
        public virtual async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            JsObjectRef listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }

            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public virtual async Task<MapEventListener> AddListener<V>(string eventName, Action<V> handler)
        {
            JsObjectRef listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync("addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }

            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public virtual async Task SetMap(Map map)
        {
            _map = map;
            await _jsObjectRef.InvokeAsync("setMap", map);
        }

        /// <summary>
        /// Removes all clusters and markers from the map and also removes all markers managed by the clusterer.
        /// </summary>
        public virtual async Task ClearMarkers()
        {
            await _jsObjectRef.InvokeAsync("clearMarkers");
            await _jsObjectRef.InvokeAsync("render");
        }

        /// <summary>
        /// Fits the map to the bounds of the markers managed by the clusterer.
        /// </summary>
        /// <param name="padding"></param>
        [Obsolete("Deprecated: Center map based on unclustered Markers before clustering. Latest js-markerclusterer lib doesn't support this. Workaround is slow. ")]
        public virtual async Task FitMapToMarkers(int padding)
        {
            var newBounds = new LatLngBoundsLiteral(await _originalMarkers.First().GetPosition());
            foreach(var marker in _originalMarkers)
            {
                newBounds.Extend(await marker.GetPosition());
            }

            await _map.FitBounds(newBounds, padding);
        }

        /// <summary>
        /// Recalculates and redraws all the marker clusters from scratch. Call this after changing any properties.
        /// </summary>
        [Obsolete("Deprecated in favor of Redraw() to match latest js-markerclusterer")]
        public virtual async Task Repaint()
        {
            await Redraw();
        }

        /// <summary>
        /// Recalculates and redraws all the marker clusters from scratch. Call this after changing any properties.
        /// </summary>
        public virtual async Task Redraw()
        {
            await _jsObjectRef.InvokeAsync("redraw");

        }

        public virtual async Task ClearListeners(string eventName)
        {
            if (EventListeners.ContainsKey(eventName))
            {
                await _jsObjectRef.InvokeAsync("clearListeners", eventName);

                //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
                //so, instead of clearing the list and removing the key from dictionary, I prefer to leave the key with an empty list
                EventListeners[eventName].Clear();
            }
        }
    }
}