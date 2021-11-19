using OneOf;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps
{
    public class MarkerOptions : ListableEntityOptionsBase
    {
        /// <summary>
        /// The offset from the marker's position to the tip of an InfoWindow that has been opened with the marker as anchor.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Point? AnchorPoint { get; init; }

        /// <summary>
        /// Which animation to play when marker is added to a map.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Animation? Animation { get; set; }

        /// <summary>
        /// If false, disables cross that appears beneath the marker when dragging. 
        /// This option is true by default.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? CrossOnDrag { get; set; }

        /// <summary>
        /// Mouse cursor to show on hover
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Cursor { get; set; }

        /// <summary>
        /// Icon for the foreground. 
        /// If a string is provided, it is treated as though it were an Icon with the string as url.
        /// </summary>
        [JsonConverter(typeof(OneOfConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OneOf<string, Icon, Symbol>? Icon { get; set; }

        /// <summary>
        /// Adds a label to the marker. The label can either be a string, or a MarkerLabel object.
        /// </summary>
        [JsonConverter(typeof(OneOfConverter))]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public OneOf<string, MarkerLabel>? Label { get; set; }

        /// <summary>
        /// The marker's opacity between 0.0 and 1.0.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public float? Opacity { get; set; }

        /// <summary>
        /// Optimization renders many markers as a single static element. 
        /// Optimized rendering is enabled by default. 
        /// Disable optimized rendering for animated GIFs or PNGs, or when each marker must be rendered as a separate DOM element (advanced usage only)
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Optimized { get; set; }

        /// <summary>
        /// Marker position. Required.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public LatLngLiteral? Position { get; set; }

        /// <summary>
        /// 2021-07 supported only in beta google maps version
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public CollisionBehavior? CollisionBehavior { get; set; }

        /// <summary>
        /// Image map region definition used for drag/click.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MarkerShape? Shape { get; set; }

        /// <summary>
        /// Rollover text
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? Title { get; set; }
    }
}
