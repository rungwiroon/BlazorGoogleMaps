using OneOf;

namespace GoogleMapsComponents.Maps.Data
{
    public class FeatureOptions
    {
        /// <summary>
        /// The feature geometry. 
        /// If none is specified when a feature is constructed, the feature's geometry will be null. 
        /// If a LatLng object or LatLngLiteral is given, this will be converted to a Data.Point geometry.
        /// </summary>
        public OneOf<Geometry, LatLngLiteral>? Geometry { get; set; }

        /// <summary>
        /// Feature ID is optional. 
        /// If provided, it can be used to look up the feature in a Data object using the getFeatureById() method. 
        /// Note that a feature's ID cannot be subsequently changed.
        /// </summary>
        public OneOf<int, string>? Id { get; set; }
    }

    public class FeatureOptions<T>
        where T : Geometry
    {
        /// <summary>
        /// The feature geometry. 
        /// If none is specified when a feature is constructed, the feature's geometry will be null. 
        /// If a LatLng object or LatLngLiteral is given, this will be converted to a Data.Point geometry.
        /// </summary>
        public T? Geometry { get; set; }

        /// <summary>
        /// Feature ID is optional. 
        /// If provided, it can be used to look up the feature in a Data object using the getFeatureById() method. 
        /// Note that a feature's ID cannot be subsequently changed.
        /// </summary>
        public OneOf<int, string>? Id { get; set; }
    }
}
