using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
// ReSharper disable UnusedMember.Global

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
            MarkerClustererOptions? options = null
           )
        {
            options ??= new MarkerClustererOptions();

            var guid = Guid.NewGuid();
            var jsObjectRef = new JsObjectRef(jsRuntime, guid);
            await jsRuntime.InvokeVoidAsync("googleMapsObjectManager.createClusteringMarkers", guid.ToString(), map.Guid.ToString(), markers, options);
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

        /// <summary>
        /// Add additional markers to an existing MarkerClusterer
        /// </summary>
        /// <param name="markers"></param>
        /// <param name="noDraw">when true, clusters will not be rerendered on the next map idle event rather than immediately after markers are added</param>
        public virtual async Task AddMarkers(IEnumerable<Marker>? markers, bool noDraw = false)
        {
            if (markers == null)
            {
                return;
            }

            await _jsObjectRef.JSRuntime.InvokeVoidAsync("googleMapsObjectManager.addClusteringMarkers", _jsObjectRef.Guid.ToString(), markers, noDraw);
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

        public virtual async Task<MapEventListener> AddListener<TAction>(string eventName, Action<TAction> handler)
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
        /// Removes provided markers from the clusterer's internal list of source markers.
        /// </summary>
        public virtual async Task RemoveMarkers(IEnumerable<Marker> markers, bool noDraw = false)
        {
            await _jsObjectRef.JSRuntime.InvokeVoidAsync("googleMapsObjectManager.removeClusteringMarkers", _jsObjectRef.Guid.ToString(), markers, noDraw);
        }

        /// <summary>
        /// Removes all clusters and markers from the map and also removes all markers managed by the clusterer.
        /// </summary>
        public virtual async Task ClearMarkers(bool noDraw = false)
        {
            await _jsObjectRef.InvokeAsync("clearMarkers", noDraw);
        }

        /// <summary>
        /// Fits the map to the bounds of the markers managed by the clusterer.
        /// </summary>
        /// <param name="padding"></param>
        [Obsolete("Deprecated: Center map based on unclustered Markers before clustering. Latest js-markerclusterer lib doesn't support this. Workaround is slow. ")]
        public virtual async Task FitMapToMarkers(int padding)
        {
            var newBounds = new LatLngBoundsLiteral(await _originalMarkers.First().GetPosition());
            foreach (var marker in _originalMarkers)
            {
                newBounds.Extend(await marker.GetPosition());
            }

            await _map.FitBounds(newBounds, padding);
        }

        /// <summary>
        /// Recalculates and redraws all the marker clusters from scratch. Call this after changing any properties.
        /// </summary>
        [Obsolete("Deprecated in favor of Redraw() to match latest js-markerclusterer")]
        public virtual Task Repaint()
        {
            return Render();
        }

        /// <summary>
        /// Recalculates and redraws all the marker clusters from scratch. Call this after changing any properties.
        /// </summary>
        [Obsolete("Deprecated in favor of Render() to match latest js-markerclusterer")]
        public virtual Task Redraw()
        {
            return Render();
        }

        /// <summary>
        /// https://googlemaps.github.io/js-markerclusterer/interfaces/Renderer.html#render
        /// </summary>
        /// <returns></returns>
        public virtual Task Render()
        {
            return _jsObjectRef.InvokeAsync("render");

        }

        public virtual async Task ClearListeners(string eventName)
        {
            if (EventListeners.ContainsKey(eventName))
            {
                //await _jsObjectRef.InvokeAsync("clearListeners", eventName);

                foreach (MapEventListener listener in EventListeners[eventName])
                {
                    await listener.RemoveAsync();
                }
                //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
                //so, instead of clearing the list and removing the key from dictionary, I prefer to leave the key with an empty list
                EventListeners[eventName].Clear();
            }
        }
    }
}