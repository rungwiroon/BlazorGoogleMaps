using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MarkerOptions
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
        /// If true, the marker receives mouse and touch events. 
        /// Default value is true.
        /// </summary>
        public bool? Clickable { get; set; }

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
        /// If true, the marker can be dragged. Default value is false.
        /// </summary>
        public bool? Draggable { get; set; }

        /// <summary>
        /// Icon for the foreground. 
        /// If a string is provided, it is treated as though it were an Icon with the string as url.
        /// </summary>
        public object Icon { get; set; }

        /// <summary>
        /// Adds a label to the marker. The label can either be a string, or a MarkerLabel object.
        /// </summary>
        public object Label { get; set; }

        /// <summary>
        /// Map on which to display Marker.
        /// </summary>
        [JsonConverter(typeof(MapComponentConverter))]
        public MapComponent Map { get; set; }

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
        /// Image map region definition used for drag/click.
        /// </summary>
        public MarkerShape Shape { get; set; }

        /// <summary>
        /// Rollover text
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// If true, the marker is visible
        /// </summary>
        public bool? Visible { get; set; }

        /// <summary>
        /// All markers are displayed on the map in order of their zIndex, with higher values displaying in front of markers with lower values. 
        /// By default, markers are displayed according to their vertical position on screen, with lower markers appearing in front of markers further up the screen.
        /// </summary>
        public int? ZIndex { get; set; }
    }
}
