using OneOf;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// A QueryAutocompletion request to be sent to the QueryAutocompleteService.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class QueryAutocompletionRequest
{
    /// <summary>
    /// The user entered input string.
    /// </summary>
    public string Input { get; set; } = default!;

    /// <summary>
    /// (optional) Bounds for prediction biasing. Predictions will be biased towards, but not restricted to, the given bounds.
    /// Both location and radius will be ignored if bounds is set.
    /// </summary>
    public OneOf<LatLngBounds, LatLngBoundsLiteral>? Bounds { get; set; }

    /// <summary>
    /// (optional) Location for prediction biasing. Predictions will be biased towards the given location and radius.
    /// Alternatively, bounds can be used.
    /// </summary>
    public LatLngBoundsLiteral? Location { get; set; }

    /// <summary>
    /// (optional) The character position in the input term at which the service uses text for predictions (the position of the cursor in the input field).
    /// </summary>
    public int? Offset { get; set; }

    /// <summary>
    /// (optional) The radius of the area used for prediction biasing. The radius is specified in meters, and must always be accompanied by a location property.
    /// Alternatively, bounds can be used.
    /// </summary>
    public decimal? Radius { get; set; }
}