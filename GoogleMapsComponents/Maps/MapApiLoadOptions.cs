using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// All options
/// https://developers.google.com/maps/documentation/javascript/load-maps-js-api#required_parameters <br/>
/// </summary>
public class MapApiLoadOptions(string key)
{
    /// <summary>
    /// Default is weekly.
    /// https://developers.google.com/maps/documentation/javascript/versions
    /// </summary>
    [JsonPropertyName("v")]
    public string Version { get; set; } = "weekly";

    /// <summary>
    /// Comma separated list of libraries to load.
    /// https://developers.google.com/maps/documentation/javascript/libraries
    /// </summary>
    public string? Libraries { get; set; } = "places,visualization,drawing,marker";
    
    /// <summary>
    /// The language to use. This affects the names of controls, copyright notices, driving directions, and control labels, and the responses to service requests. See the list of supported languages.
    /// https://developers.google.com/maps/documentation/javascript/localization
    /// https://developers.google.com/maps/faq#languagesupport
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Language { get; set; }
    
    /// <summary>
    /// The region code to use. This alters the map's behavior based on a given country or territory.
    /// https://developers.google.com/maps/documentation/javascript/localization#Region
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Region { get; set; }
    
    /// <summary>
    /// Maps JS customers can configure HTTP Referrer Restrictions in the Cloud Console to limit which URLs are allowed
    /// to use a particular API Key. By default, these restrictions can be configured to allow only certain paths to
    /// use an API Key. If any URL on the same domain or origin may use the API Key, you can set
    /// authReferrerPolicy: "origin" to limit the amount of data sent when authorizing requests from the Maps
    /// JavaScript API. When this parameter is specified and HTTP Referrer Restrictions are enabled on Cloud Console,
    /// Maps JavaScript API will only be able to load if there is an HTTP Referrer Restriction that matches the
    /// current website's domain without a path specified.
    /// https://developers.google.com/maps/documentation/javascript/get-api-key#restrict_key
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? AuthReferrerPolicy { get; set; }
    
    /// <summary>
    /// An array of map Ids. Causes the configuration for the specified map Ids to be preloaded.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string[]? MapIds { get; set; }
    
    /// <summary>
    /// See Usage tracking per channel.
    /// https://developers.google.com/maps/reporting-and-monitoring/reporting#usage-tracking-per-channel
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Channel { get; set; }
    
    /// <summary>
    /// Google Maps Platform provides many types of sample code to help you get up and running quickly. To track
    /// adoption of our more complex code samples and improve solution quality, Google includes the solutionChannel
    /// query parameter in API calls in our sample code.
    /// </summary>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? SolutionChannel { get; set; }

    /// <summary>
    /// Synchronous key provider function used by the default implementation
    /// </summary>
    public string? Key { get; set; } = key;
}