namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Represents a place prediction from the Google Maps Places API.
/// </summary>
public class PlacePrediction
{
    /// <summary>
    /// The length of the geodesic in meters from origin if origin is specified.
    /// </summary>
    public int? DistanceMeters { get; set; }

    /// <summary>
    /// Represents the name of the Place.
    /// </summary>
    public string? MainText { get; set; }

    /// <summary>
    /// The unique identifier of the suggested Place. This identifier can be used in other APIs that accept Place IDs.
    /// </summary>
    public string? PlaceId { get; set; }

    /// <summary>
    /// Represents additional disambiguating features (such as a city or region) to further identify the Place.
    /// </summary>
    public string? SecondaryText { get; set; }

    /// <summary>
    /// Contains the human-readable name for the returned result. For establishment results, this is usually the business name and address.
    /// </summary>
    /// <remarks>
    /// This is recommended for developers who wish to show a single UI element. Developers who wish to show two separate, but related, UI elements
    /// may want to use <see cref="MainText"/> and <see cref="SecondaryText"/> instead.
    /// </remarks>
    public string? Text { get; set; }

    /// <summary>
    /// List of types that apply to this Place.
    /// </summary>
    /// <remarks>
    /// See <see href="https://developers.google.com/maps/documentation/places/web-service/place-types">place types</see> for more information.
    /// </remarks>
    public string[]? Types { get; set; }
}