using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// The properties of an overlaycomplete event on a DrawingManager.
    /// https://developers.google.com/maps/documentation/javascript/reference/drawing#OverlayCompleteEvent
    /// </summary>
    public class OverlayCompleteEvent
    {
        /// <summary>
        /// The completed overlay.
        /// It could be one of Marker|Polygon|Polyline|Rectangle|Circle
        /// Overlay object is JObject with all properties,
        /// so need to serialize, extract required info depending on your needs
        /// </summary>
        public object Overlay { get; set; }

        /// <summary>
        /// The completed overlay's type.
        /// </summary>
        public OverlayType Type { get; set; }
    }
}
