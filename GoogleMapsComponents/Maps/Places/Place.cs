namespace GoogleMapsComponents.Maps.Places;

public class Place
{
    /// <summary>
    /// Gets or sets the unique place ID.
    /// </summary>
    public string Id { get; set; } = null!;

    /// <summary>
    /// Gets or sets the language identifier for the language in which details should be returned.
    /// </summary>
    /// <remarks>
    /// See the list of <see href="https://developers.google.com/maps/faq#languagesupport">supported languages</see>.
    /// </remarks>
    public string? RequestedLanguage { get; set; }

    /// <summary>
    /// Gets or sets the region code of the user's region.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This can affect which photos may be returned, and possibly other things. The region code accepts a 
    /// ccTLD ("top-level domain") two-character value. Most ccTLD codes are identical to ISO 3166-1 codes, 
    /// with some notable exceptions. For example, the United Kingdom's ccTLD is "uk" (.co.uk) while its 
    /// ISO 3166-1 code is "gb" (technically for the entity of "The United Kingdom of Great Britain and Northern Ireland").
    /// </para>
    /// </remarks>
    public string? RequestedRegion { get; set; }

    /// <summary>
    /// The location's display name. null if there is no name. undefined if the name data has not been loaded from the server.
    /// </summary>
    public string? DisplayName { get; set; }
    public LatLngLiteral? Location { get; set; }
}