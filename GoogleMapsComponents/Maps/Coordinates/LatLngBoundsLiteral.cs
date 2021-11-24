namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// Object literals are accepted in place of LatLngBounds objects throughout the API.
    /// These are automatically converted to LatLngBounds objects. All south, west, north and east must be set, otherwise an exception is thrown.
    /// </summary>
    public record struct LatLngBoundsLiteral
    {
        /// <summary>
        /// East longitude in degrees. Values outside the range [-180, 180] will be wrapped to the range [-180, 180). 
        /// For example, a value of -190 will be converted to 170. 
        /// A value of 190 will be converted to -170. 
        /// This reflects the fact that longitudes wrap around the globe.
        /// </summary>
        public double East { get; init; }

        /// <summary>
        /// North latitude in degrees. Values will be clamped to the range [-90, 90]. 
        /// This means that if the value specified is less than -90, it will be set to -90. 
        /// And if the value is greater than 90, it will be set to 90.
        /// </summary>
        public double North { get; init; }

        /// <summary>
        /// South latitude in degrees. Values will be clamped to the range [-90, 90]. 
        /// This means that if the value specified is less than -90, it will be set to -90. 
        /// And if the value is greater than 90, it will be set to 90.
        /// </summary>
        public double South { get; init; }

        /// <summary>
        /// West longitude in degrees. 
        /// Values outside the range [-180, 180] will be wrapped to the range [-180, 180). 
        /// For example, a value of -190 will be converted to 170. 
        /// A value of 190 will be converted to -170. 
        /// This reflects the fact that longitudes wrap around the globe.
        /// </summary>
        public double West { get; init; }

        /// <summary>
        /// Constructor with one or two given coordinate points.
        /// If the second point is null, the bounds are set to the first point.
        /// The points may be positioned arbitrarily.
        /// </summary>
        public LatLngBoundsLiteral(LatLngLiteral latLng1, LatLngLiteral? latLng2 = null)
        {
            East = latLng1.Lng;
            West = latLng1.Lng;
            South = latLng1.Lat;
            North = latLng1.Lat;

            if (latLng2 != null)
              Extend(latLng2.Value);
        }       

        /// <summary>
        /// Extend these boundaries by a given coordinate point.
        /// </summary>
        public LatLngBoundsLiteral Extend(double lng, double lat)
        {
            var west = (lng < West) ? lng : West;
            var east = (lng > East) ? lng : East;
            var south = (lat < South) ? lat : South;
            var north = (lat > North) ? lat : North;

            return new LatLngBoundsLiteral()
            {
                East = east,
                West = west,
                South = south,
                North = north,
            };
        }

        /// <summary>
        /// Extend these boundaries by a given coordinate point.
        /// </summary>
        public LatLngBoundsLiteral Extend(LatLngLiteral latLng)
        {
            return Extend(latLng.Lng,latLng.Lat);
        }
        
        /// <summary>
        /// Is the area zero?
        /// </summary>
        public bool IsEmpty()
        {
            return (West == East || South == North);
        }
        
        public override string ToString()
        {
            return $"{North} {East} {South} {West}";
        }
    }
}
