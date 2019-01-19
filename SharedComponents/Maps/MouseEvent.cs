using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// This object is returned from various mouse events on the map and overlays, and contains all the fields shown below.
    /// </summary>
    public class MouseEvent : MapEventArgs
    {
        internal string Id { get; private set; }

        /// <summary>
        /// The latitude/longitude that was below the cursor when the event occurred.
        /// </summary>
        public LatLngLiteral LatLng { get; set; }

        public MouseEvent(string id)
        {
            Id = id;
        }

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
