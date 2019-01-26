using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// Object literals are accepted in place of LatLngBounds objects throughout the API.
    /// These are automatically converted to LatLngBounds objects. All south, west, north and east must be set, otherwise an exception is thrown.
    /// </summary>
    public class LatLngBoundsLiteral
    {
        /// <summary>
        /// East longitude in degrees. Values outside the range [-180, 180] will be wrapped to the range [-180, 180). 
        /// For example, a value of -190 will be converted to 170. 
        /// A value of 190 will be converted to -170. 
        /// This reflects the fact that longitudes wrap around the globe.
        /// </summary>
        public double East { get; set; }

        /// <summary>
        /// North latitude in degrees. Values will be clamped to the range [-90, 90]. 
        /// This means that if the value specified is less than -90, it will be set to -90. 
        /// And if the value is greater than 90, it will be set to 90.
        /// </summary>
        public double North { get; set; }

        /// <summary>
        /// South latitude in degrees. Values will be clamped to the range [-90, 90]. 
        /// This means that if the value specified is less than -90, it will be set to -90. 
        /// And if the value is greater than 90, it will be set to 90.
        /// </summary>
        public double South { get; set; }

        /// <summary>
        /// West longitude in degrees. 
        /// Values outside the range [-180, 180] will be wrapped to the range [-180, 180). 
        /// For example, a value of -190 will be converted to 170. 
        /// A value of 190 will be converted to -170. 
        /// This reflects the fact that longitudes wrap around the globe.
        /// </summary>
        public double West { get; set; }

        public override string ToString()
        {
            return $"{North} {East} {South} {West}";
        }
    }
}
