using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// The MapTypeStyle is a collection of selectors and stylers that define how the map should be styled. 
    /// Selectors specify the map features and/or elements that should be affected, and stylers specify how those features and elements should be modified.
    /// </summary>
    public class MapTypeStyle
    {
        /// <summary>
        /// The element to which a styler should be applied. 
        /// An element is a visual aspect of a feature on the map. 
        /// Example: a label, an icon, the stroke or fill applied to the geometry, and more. 
        /// Optional. 
        /// If elementType is not specified, the value is assumed to be 'all'.
        /// </summary>
        public string elementType { get; set; }

        /// <summary>
        /// The feature, or group of features, to which a styler should be applied. Optional. 
        /// If featureType is not specified, the value is assumed to be 'all'.
        /// </summary>
        public string featureType { get; set; }

        /// <summary>
        /// The style rules to apply to the selected map features and elements. 
        /// The rules are applied in the order that you specify in this array.
        /// </summary>
        public object[] stylers { get; set; }
    }
}
