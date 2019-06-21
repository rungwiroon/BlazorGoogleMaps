using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// google.maps.Data.AddFeatureEvent interface 
    /// </summary>
    public class AddFeatureEvent
    {
        /// <summary>
        /// The feature that was added to the FeatureCollection.
        /// </summary>
        public Feature Feature { get; set; }
    }
}
