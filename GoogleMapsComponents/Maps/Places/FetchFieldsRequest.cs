using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// Options for fetching Place fields.
/// </summary>
public class FetchFieldsRequest
{
    /// <summary>
    /// List of fields to be fetched.
    /// </summary>
    public IReadOnlyList<string>? Fields { get; set; }
}