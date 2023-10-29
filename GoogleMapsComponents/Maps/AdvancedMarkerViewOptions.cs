using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps
{
    public class AdvancedMarkerViewOptions : ListableEntityOptionsBase
    {
        /// <summary>
        /// Sets the AdvancedMarkerElement's position. An AdvancedMarkerElement may be constructed without a position, but will not be displayed until its position is provided - for example, by a user's actions or choices. An AdvancedMarkerElement's position can be provided by setting AdvancedMarkerElement.position if not provided at the construction.
        /// Note: AdvancedMarkerElement with altitude is only supported on vector maps.
        /// </summary>
        public LatLngLiteral? Position { get; set; }

        /// <summary>
        /// An enumeration specifying how an AdvancedMarkerElement should behave when it collides with another AdvancedMarkerElement or with the basemap labels on a vector map.
        /// Note: AdvancedMarkerElement to AdvancedMarkerElement collision works on both raster and vector maps, however, AdvancedMarkerElement to base map's label collision only works on vector maps.
        /// </summary>

        [JsonConverter(typeof(GoogleMapsComponents.Serialization.JsonStringEnumConverterEx<CollisionBehavior>))]
        public CollisionBehavior? CollisionBehavior { get; set; }

        /// <summary>
        /// 2023-10-29 Currently only html content is supported
        /// Svg, images url could not work
        /// </summary>
        public string? Content { get; set; }

        /// <summary>
        /// If true, the AdvancedMarkerElement can be dragged.
        /// Note: AdvancedMarkerElement with altitude is not draggable.
        /// </summary>
        public bool GmpDraggable { get; set; }

        /// <summary>
        /// Rollover text. If provided, an accessibility text (e.g. for use with screen readers) will be added to the
        /// </summary>
        public string? Title { get; set; }
    }
}
