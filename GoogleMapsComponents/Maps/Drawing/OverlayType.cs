using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// The types of overlay that may be created by the DrawingManager. 
    /// </summary>
    public enum OverlayType
    {
        /// <summary>
        /// Specifies that the DrawingManager creates circles, and that the overlay given in the overlaycomplete event is a circle.
        /// </summary>
        Circle,

        /// <summary>
        /// Specifies that the DrawingManager creates markers, and that the overlay given in the overlaycomplete event is a marker.
        /// </summary>
        Marker,

        /// <summary>
        /// Specifies that the DrawingManager creates polygons, and that the overlay given in the overlaycomplete event is a polygon.
        /// </summary>
        Polygon,

        /// <summary>
        /// Specifies that the DrawingManager creates polylines, and that the overlay given in the overlaycomplete event is a polyline.
        /// </summary>
        Polyline,

        /// <summary>
        /// Specifies that the DrawingManager creates rectangles, and that the overlay given in the overlaycomplete event is a rectangle.
        /// </summary>
        Rectangle
    }
}
