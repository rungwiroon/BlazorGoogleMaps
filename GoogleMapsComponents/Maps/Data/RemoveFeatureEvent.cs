using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// google.maps.Data.RemoveFeatureEvent interface 
    /// </summary>
    public class RemoveFeatureEvent
    {
        /// <summary>
        /// The feature that was removed from the FeatureCollection.
        /// </summary>
        public Feature Feature { get; set; }
    }
}
