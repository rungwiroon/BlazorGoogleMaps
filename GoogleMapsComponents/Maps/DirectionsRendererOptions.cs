using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// This object defines the properties that can be set on a DirectionsRenderer object.
    /// </summary>
    public class DirectionsRendererOptions
    {
        /// <summary>
        /// The directions to display on the map and/or in a <div> panel, retrieved as a DirectionsResult object from DirectionsService.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public DirectionsResult? Directions { get; set; }

        /// <summary>
        /// If true, allows the user to drag and modify the paths of routes rendered by this DirectionsRenderer.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? Draggable { get; set; }

        /// <summary>
        /// This property indicates whether the renderer should provide UI to select amongst alternative routes. By default, this flag is false and a user-selectable list of routes will be shown in the directions' associated panel. To hide that list, set hideRouteList to true.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? HideRouteList { get; set; }

        /// <summary>
        /// The InfoWindow in which to render text information when a marker is clicked. Existing info window content will be overwritten and its position moved. If no info window is specified, the DirectionsRenderer will create and use its own info window. This property will be ignored if suppressInfoWindows is set to true.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public InfoWindow? InfoWindow { get; set; }

        /// <summary>
        /// Map on which to display the directions.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public Map? Map { get; set; }

        /// <summary>
        /// Options for the markers. All markers rendered by the DirectionsRenderer will use these options.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public MarkerOptions? MarkerOptions { get; set; }

        /// <summary>
        /// The <div> in which to display the directions steps.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public ElementReference? Panel { get; set; }

        /// <summary>
        /// Options for the polylines. All polylines rendered by the DirectionsRenderer will use these options.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public PolylineOptions? PolylineOptions { get; set; }

        /// <summary>
        /// By default, the input map is centered and zoomed to the bounding box of this set of directions. If this option is set to true, the viewport is left unchanged, unless the map's center and zoom were never set.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? PreserveViewport { get; set; }

        /// <summary>
        /// The index of the route within the DirectionsResult object. The default value is 0.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public int? RouteIndex { get; set; }

        /// <summary>
        /// Suppress the rendering of the BicyclingLayer when bicycling directions are requested.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SuppressBicyclingLayer { get; set; }

        /// <summary>
        /// Suppress the rendering of info windows.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SuppressInfoWindows { get; set; }

        /// <summary>
        /// Suppress the rendering of markers.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SuppressMarkers { get; set; }

        /// <summary>
        /// Suppress the rendering of polylines.
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public bool? SuppressPolylines { get; set; }
    }
}
