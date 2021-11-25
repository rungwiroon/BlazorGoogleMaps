using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// The types of overlay that may be created by the DrawingManager. 
    /// </summary>
    [JsonConverter(typeof(CustomJsonStringEnumConverter))]
    public enum OverlayType
    {
        /// <summary>
        /// Specifies that the DrawingManager creates circles, and that the overlay given in the overlaycomplete event is a circle.
        /// </summary>
        [EnumMember(Value = "circle")]
        Circle,

        /// <summary>
        /// Specifies that the DrawingManager creates markers, and that the overlay given in the overlaycomplete event is a marker.
        /// </summary>
        [EnumMember(Value = "marker")]
        Marker,

        /// <summary>
        /// Specifies that the DrawingManager creates polygons, and that the overlay given in the overlaycomplete event is a polygon.
        /// </summary>
        [EnumMember(Value = "polygon")]
        Polygon,

        /// <summary>
        /// Specifies that the DrawingManager creates polylines, and that the overlay given in the overlaycomplete event is a polyline.
        /// </summary>
        [EnumMember(Value = "polyline")]
        Polyline,

        /// <summary>
        /// Specifies that the DrawingManager creates rectangles, and that the overlay given in the overlaycomplete event is a rectangle.
        /// </summary>
        [EnumMember(Value = "rectangle")]
        Rectangle
    }
}
