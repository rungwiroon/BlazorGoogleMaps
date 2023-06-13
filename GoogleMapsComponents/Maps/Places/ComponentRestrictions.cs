namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Defines the component restrictions that can be used with the autocomplete service.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class ComponentRestrictions
{
    /// <summary>
    /// (optional) Restricts predictions to the specified country (ISO 3166-1 Alpha-2 country code, case insensitive).
    /// For example, 'us', 'br', or 'au'. You can provide a single one, or an array of up to five country code strings.
    /// </summary>
    public string[]? Country { get; set; }
}