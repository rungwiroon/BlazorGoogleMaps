using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// https://googlemaps.github.io/v3-utility-library/modules/_google_markerclustererplus.html
    /// </summary>
    public class MarkerClustering : MVCObject
    {
        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public static async Task<MarkerClustering> CreateAsync(
            IJSRuntime jsRuntime,
            Map map,
            IEnumerable<Marker> markers,
            MarkerClustererOptions? options = null
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

        internal MarkerClustering(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
            //_jsObjectRef = jsObjectRef;
            EventListeners = new();
        }

        public virtual async Task SetMap(Map map)
        {
            await this.InvokeVoidAsync("setMap", map);
        }

        /// <summary>
        /// Removes all clusters and markers from the map and also removes all markers managed by the clusterer.
        /// </summary>
        public virtual async Task ClearMarkers()
        {
            await this.InvokeVoidAsync("clearMarkers");
        }

        /// <summary>
        /// Fits the map to the bounds of the markers managed by the clusterer.
        /// </summary>
        /// <param name="padding"></param>
        public virtual async Task FitMapToMarkers(int padding)
        {
            await this.InvokeVoidAsync("fitMapToMarkers", padding);
        }

        /// <summary>
        /// Recalculates and redraws all the marker clusters from scratch. Call this after changing any properties.
        /// </summary>
        public virtual async Task Repaint()
        {
            await this.InvokeVoidAsync("repaint");
        }

        public virtual async Task ClearListeners(string eventName)
        {
            if (EventListeners.ContainsKey(eventName))
            {
                await this.InvokeVoidAsync("clearListeners", eventName);

                //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
                //so, instead to clear the list and remove the key from dictionary, I prefer to leave the key with an empty list
                EventListeners[eventName].Clear();
            }
        }
    }
}