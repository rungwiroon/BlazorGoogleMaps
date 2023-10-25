namespace GoogleMapsComponents.Maps
{
    public class AdvancedMarkerViewOptions : ListableEntityOptionsBase
    {
        public LatLngLiteral? Position { get; set; }
        /// <summary>
        /// Currently only html content is supported
        /// Svg, images url could not work
        /// </summary>
        public string? Content { get; set; }
    }
}
