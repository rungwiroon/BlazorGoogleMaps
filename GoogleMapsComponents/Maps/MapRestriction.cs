namespace GoogleMapsComponents.Maps;

/// <summary>
/// A restriction that can be applied to the Map. 
/// The map's viewport will not exceed these restrictions.
/// </summary>
public class MapRestriction
{
    /// <summary>
    /// When set, a user can only pan and zoom inside the given bounds. 
    /// Bounds can restrict both longitude and latitude, or can restrict latitude only. 
    /// For latitude-only bounds use west and east longitudes of -180 and 180, respectively. 
    /// </summary>
    public LatLngBoundsLiteral latLngBounds { get; set; }

    /// <summary>
    /// By default bounds are relaxed, meaning that a user can zoom out until the entire bounded area is in view. 
    /// Bounds can be made more restrictive by setting the strictBounds flag to true. 
    /// This reduces how far a user can zoom out, ensuring that everything outside of the restricted bounds stays hidden.
    /// </summary>
    public bool strictBounds { get; set; }
}