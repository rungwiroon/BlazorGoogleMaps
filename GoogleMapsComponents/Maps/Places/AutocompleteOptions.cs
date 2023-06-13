namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// google.maps.places.AutocompleteOptions interface
/// </summary>
public class AutocompleteOptions
{
    /// <summary>
    /// (optional) The area in which to search for places.
    /// </summary>
    public LatLngBoundsLiteral Bounds { get; set; }

    /// <summary>
    /// (optional) The component restrictions. Component restrictions are used to restrict predictions
    /// to only those within the parent component. For example, the country.
    /// </summary>
    public ComponentRestrictions ComponentRestrictions { get; set; }

    /// <summary>
    /// (optional) Fields to be included for the Place in the details response when the details are
    /// successfully retrieved, which will be billed for. If ['ALL'] is passed in,
    /// all available fields will be returned and billed for (this is not recommended
    /// for production deployments). For a list of fields see PlaceResult. Nested fields
    /// can be specified with dot-paths (for example, "geometry.location").
    /// </summary>
    public string[] Fields { get; set; }

    /// <summary>
    /// (optional) A boolean value, indicating that the Autocomplete widget should only return
    /// those places that are inside the bounds of the Autocomplete widget at the time
    /// the query is sent. Setting strictBounds to false (which is the default) will
    /// make the results biased towards, but not restricted to, places contained within
    /// the bounds.
    /// </summary>
    public bool StrictBounds { get; set; }

    /// <summary>
    /// (optional) The types of predictions to be returned. For a list of supported types,
    /// see the developer's guide. If nothing is specified, all types are returned.
    /// In general only a single type is allowed. The exception is that you can safely
    /// mix the 'geocode' and 'establishment' types, but note that this will have the same
    /// effect as specifying no types.
    /// </summary>
    public string[] Types { get; set; }
}