using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// Optional parameters for importing GeoJSON.
    /// </summary>
    public class GeoJsonOptions
    {
        /// <summary>
        /// The name of the Feature property to use as the feature ID. 
        /// If not specified, the GeoJSON Feature id will be used.
        /// </summary>
        public string IdPropertyName { get; set; }
    }
}
