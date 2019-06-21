using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// google.maps.Data.SetPropertyEvent interface 
    /// </summary>
    public class SetPropertyEvent
    {
        /// <summary>
        /// The feature whose property was set.
        /// </summary>
        public Feature Feature { get; set; }

        /// <summary>
        /// The property name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The new value.
        /// </summary>
        public object NewValue { get; set; }

        /// <summary>
        /// The previous value. Will be undefined if the property was added.
        /// </summary>
        public object OldValue { get; set; }
    }
}
