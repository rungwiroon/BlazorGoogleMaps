using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// MarkerClustererOptions object used to define the properties that can be passed to a MarkerClusterer.
    /// </summary>
    public class MarkerClustererOptions
    {
        /// <summary>
        /// The maximum zoom level at which clustering is enabled or if
        /// clustering is to be enabled at all zoom levels.
        /// </summary>
        public int? MaxZoom { get; set; }

        /// <summary>
        /// Whether the position of a cluster marker should be
        /// the average position of all markers in the cluster. If set to `false`, the
        /// cluster marker is positioned at the location of the first marker added to the cluster.
        /// </summary>
        public bool? AverageCenter { get; set; }

        /// <summary>
        /// Set this property to the number of markers to be processed in a single batch
        /// </summary>
        public int? BatchSize { get; set; }

        /// <summary>
        /// The name of the CSS class defining general styles for the cluster markers.
        /// </summary>
        public string? ClusterClass { get; set; }

        /// <summary>
        /// Whether to allow the use of cluster icons that
        /// have sizes that are some multiple(typically double) of their actual display size. Icons such
        /// as these look better when viewed on high-resolution monitors such as Apple's Retina displays.
        /// Note: if this property is `true`, sprites cannot be used as cluster icons.
        /// </summary>
        public bool? EnableRetinaIcons { get; set; }

        /// <summary>
        ///The grid size of a cluster in pixels. The grid is a square.
        /// </summary>
        public int? GridSize { get; set; }

        /// <summary>
        /// Whether to ignore hidden markers in clusters. You
        /// may want to set this to `true` to ensure that hidden markers are not included
        /// in the marker count that appears on a cluster marker.
        /// </summary>
        public bool? IgnoreHidden { get; set; }

        /// <summary>
        /// The full URL of the root name of the group of image files to use for cluster icons.
        /// </summary>
        public string ImagePath { get; set; } = "_content/BlazorGoogleMaps/m";

        /// <summary>
        /// The extension name for the cluster icon image files (e.g., `"png"` or `"jpg"`).
        /// </summary>
        public string? ImageExtension { get; set; }

        /// <summary>
        /// The minimum number of markers needed in a cluster
        /// before the markers are hidden and a cluster marker appears.
        /// </summary>
        public int? MinimumClusterSize { get; set; }

        /// <summary>
        /// The tooltip to display when the mouse moves over a cluster marker.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// The z-index of a cluster.
        /// </summary>
        public int? ZIndex { get; set; }

        /// <summary>
        /// Whether to zoom the map when a cluster marker is clicked. You may want to
        /// set this to `false` if you have installed a handler for the `click` event
        /// and it deals with zooming on its own.
        /// </summary>
        public bool? ZoomOnClick { get; set; }
    }
}
