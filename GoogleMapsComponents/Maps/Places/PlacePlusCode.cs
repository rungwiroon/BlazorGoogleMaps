using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

public class PlacePlusCode
{
    /// <summary>
    /// A plus code with a 1/8000th of a degree by 1/8000th of a degree area. For example, "8FVC9G8F+5W".
    /// </summary>
    [JsonPropertyName("global_code")]
    public string GlobalCode { get; set; } = default!;

    /// <summary>
    /// (optional) A plus code with a 1/8000th of a degree by 1/8000th of a degree area where the first four characters (the area code) are dropped and replaced with a locality description.
    /// For example, "9G8F+5W Zurich, Switzerland". If no suitable locality that can be found to shorten the code then this field is omitted.
    /// </summary>
    [JsonPropertyName("compound_code")]
    public string? CompoundCode { get; set; }

}