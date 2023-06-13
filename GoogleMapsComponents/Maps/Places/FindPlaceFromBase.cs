using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapsComponents.Maps.Places;

public abstract class FindPlaceFromBase
{
    /// <summary>
    /// Fields to be included in the response, which will be billed for.
    /// If ['ALL'] is passed in, all available fields will be returned and billed for (this is not recommended for production deployments).
    /// For a list of fields see <see cref="PlaceResult"></see>.
    /// Nested fields can be specified with dot-paths (for example, "geometry.location").
    /// </summary>
    public string[] Fields { get; set; } = default!;

    /// <summary>
    /// A language identifier for the language in which names and addresses should be returned, when possible.
    /// Find list of supported languages at:
    /// https://developers.google.com/maps/faq#languagesupport
    /// </summary>
    public string? Language { get; set; }

    /// <summary>
    /// (optional) The bias used when searching for Place. The result will be biased towards, but not restricted to, this value.
    /// </summary>
    public LocationBias? LocationBias { get; set; }
}