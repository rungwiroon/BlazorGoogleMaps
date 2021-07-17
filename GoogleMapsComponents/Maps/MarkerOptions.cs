using Newtonsoft.Json;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MarkerOptions : ListableEntityOptionsBase
    {
        /// <summary>
        /// The offset from the marker's position to the tip of an InfoWindow that has been opened with the marker as anchor.
        /// </summary>
        public Point AnchorPoint { get; set; }

        /// <summary>
        /// Which animation to play when marker is added to a map.
        /// </summary>
        public Animation? Animation { get; set; }

        /// <summary>
        /// If false, disables cross that appears beneath the marker when dragging. 
        /// This option is true by default.
        /// </summary>
        public bool? CrossOnDrag { get; set; }

        /// <summary>
        /// Mouse cursor to show on hover
        /// </summary>
        public string Cursor { get; set; }

        /// <summary>
        /// Icon for the foreground. 
        /// If a string is provided, it is treated as though it were an Icon with the string as url.
        /// </summary>
        [JsonConverter(typeof(OneOfConverter))]
        public OneOf<string, Icon, Symbol> Icon { get; set; }

        /// <summary>
        /// Adds a label to the marker. The label can either be a string, or a MarkerLabel object.
        /// </summary>
        [JsonConverter(typeof(OneOfConverter))]
        public OneOf<string, MarkerLabel> Label { get; set; }

        /// <summary>
        /// The marker's opacity between 0.0 and 1.0.
        /// </summary>
        public float? Opacity { get; set; }

        /// <summary>
        /// Optimization renders many markers as a single static element. 
        /// Optimized rendering is enabled by default. 
        /// Disable optimized rendering for animated GIFs or PNGs, or when each marker must be rendered as a separate DOM element (advanced usage only)
        /// </summary>
        public bool? Optimized { get; set; }

        /// <summary>
        /// Marker position. Required.
        /// </summary>
        public LatLngLiteral Position { get; set; }

        /// <summary>
        /// 2021-07 supported only in beta google maps version
        /// </summary>
        public CollisionBehavior? CollisionBehavior { get; set; }

        /// <summary>
        /// Image map region definition used for drag/click.
        /// </summary>
        public MarkerShape Shape { get; set; }

        /// <summary>
        /// Rollover text
        /// </summary>
        public string Title { get; set; }
    }
}
