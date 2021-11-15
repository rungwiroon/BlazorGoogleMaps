using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// This object is returned from various mouse events on the map and overlays.
    /// </summary>
    public class MouseEvent
    {
        //public MouseEvent(JsObjectRef jsObjectRef)
        //    : base(jsObjectRef)
        //{
        //}

        /// <summary>
        /// The latitude/longitude that was below the cursor when the event occurred.
        /// </summary>
        public LatLngLiteral LatLng { get; init; }

        internal bool StopStatus { get; private set; } = false;

        /// <summary>
        /// Prevents this event from propagating further.
        /// </summary>
        public void Stop()
        {
            StopStatus = true;
        }
    }
}
