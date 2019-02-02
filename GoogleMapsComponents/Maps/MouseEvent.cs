using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// This object is returned from various mouse events on the map and overlays.
    /// </summary>
    public class MouseEventArgs : MapEventArgs
    {
        public string Id { get; set; }

        /// <summary>
        /// The latitude/longitude that was below the cursor when the event occurred.
        /// </summary>
        public LatLngLiteral LatLng { get; set; }

        //public MouseEventArgs(string id)
        //{
        //    Id = id;
        //}

        /// <summary>
        /// Prevents this event from propagating further.
        /// </summary>
        public void Stop()
        {
            JSRuntime.Current.InvokeAsync<bool>(
                   "googleMapEventJsFunctions.invokeEventArgsFunction",
                   Id, 
                   "stop");
        }
    }
}
