using OneOf;

namespace GoogleMapsComponents.Maps.Data;

public class FeatureOptions
{
    /// <summary>
    /// The feature geometry. 
    /// If none is specified when a feature is constructed, the feature's geometry will be null. 
    /// If a LatLng object or LatLngLiteral is given, this will be converted to a Data.Point geometry.
    /// </summary>
    public OneOf<Geometry, LatLngLiteral>? Geometry { get; set; }

    public OneOf<int, string> Id { get; set; }
}