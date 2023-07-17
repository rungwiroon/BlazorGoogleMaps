using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using GoogleMapsComponents.Maps.Places;
using GoogleMapsComponents.Maps.Extension;
using System.Text.Json.Serialization;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A service for converting between an address and a LatLng.
/// <example> 
/// Use as follows: 
/// <code> 
/// var kmlLayer = KmlLayer.CreateAsync(map.JsRuntime, "<KML_REFERENCE>");
/// kmlLayer.AddEventListener<KmlMouseEvent>("click", e => {
/// })
/// </code>
/// </example>
/// </summary>
public class KmlLayer : EventEntityBase, IJsObjectRef
{
    public Guid Guid => _jsObjectRef.Guid;
    /// <summary>
    /// Creates a new instance of a KmlLayer
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <returns>A KmlLayer</returns>
    public async static Task<KmlLayer> CreateAsync(IJSRuntime jsRuntime, string src, KmlLayerOptions? options = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.KmlLayer", src, options);
        var obj = new KmlLayer(jsObjectRef);

        return obj;
    }

    /// <summary>
    /// Get the metadata associated with this layer, as specified in the layer markup.
    /// </summary>
    /// <returns>Metadata for a single KML layer</returns>
    public Task<KmlLayerMetadata> GetMetadata()
    {
        return _jsObjectRef.InvokeAsync<KmlLayerMetadata>("getMetadata");
    }

    /// <summary>
    /// Get the status of the layer, set once the requested document has loaded.
    /// </summary>
    /// <returns>KmlLayerStatus constant, see constant for more info</returns>
    public Task<string> GetStatus()
    {
        //TODO: Make constant -> https://developers.google.com/maps/documentation/javascript/reference/kml#KmlLayerStatus
        return _jsObjectRef.InvokeAsync<string>("getStatus");
    }

    /// <summary>
    /// Get the default viewport for the layer being displayed.
    /// </summary>
    /// <returns>LatLngBounds for the layer</returns>
    public Task<LatLngBounds> GetDefaultViewport()
    {
        return _jsObjectRef.InvokeAsync<LatLngBounds>("getDefaultViewport");
    }

    /// <summary>
    /// Gets the URL of the KML file being displayed.
    /// </summary>
    /// <returns>Url</returns>
    public Task<string> GetUrl()
    {
        return _jsObjectRef.InvokeAsync<string>("getUrl");
    }

    /// <summary>
    /// Gets the z-index of the KML Layer.
    /// </summary>
    /// <returns>z-index</returns>
    public Task<int> GetZIndex()
    {
        return _jsObjectRef.InvokeAsync<int>("getZIndex");
    }

    public Task SetOptions(KmlLayerOptions? options)
    {
        return _jsObjectRef.InvokeAsync("setOptions", options);
    }

    public Task SetUrl(string url)
    {
        return _jsObjectRef.InvokeAsync("setUrl", url);
    }

    public Task SetZIndex(int zIndex)
    {
        return _jsObjectRef.InvokeAsync("setZIndex", zIndex);
    }

    /// <summary>
    /// Creates a new instance of a KmlLayer
    /// </summary>)
    /// <param name="jsObjectRef"></param>
    private KmlLayer(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }
}

public class KmlLayerOptions
{
    /// <summary>
    /// The URL of the KML document to display.
    /// </summary>
    public string? Url { get; set; }
    /// <summary>
    /// Suppress the rendering of info windows when layer features are clicked.
    /// </summary>
    public bool? SuppressInfoWindow { get; set; }
    /// <summary>
    /// Whether to render the screen overlays.
    /// </summary>
    public bool? ScreenOverlays { get; set; }
    /// <summary>
    /// If this option is set to true or if the map's center and zoom were never set, the input map is centered and zoomed to the bounding box of the contents of the layer.
    /// </summary>
    public bool? PreserveViewport { get; set; }
    /// <summary>
    /// If true, the layer receives mouse events.
    /// </summary>
    public bool? Clickable { get; set; }

    /// <summary>
    /// Map on which to display the Entity.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map Map { get; set; }

    /// <summary>
    /// All entities are displayed on the map in order of their zIndex, with higher values displaying in front of entities with lower values. 
    /// By default, entities are displayed according to their vertical position on screen, with lower entities appearing in front of entities further up the screen.
    /// </summary>
    public int? ZIndex { get; set; }
}

public class KmlMouseEvent
{
    /// <summary>
    /// A KmlFeatureData object, containing information about the clicked feature.
    /// </summary>
    public KmlFeatureData FeatureData { get; set; }
    /// <summary>
    /// The position at which to anchor an infowindow on the clicked feature.
    /// </summary>
    public LatLngLiteral LatLng { get; set; }
    /// <summary>
    /// The offset to apply to an infowindow anchored on the clicked feature.
    /// </summary>
    public Size Size { get; set; }
}

public abstract class KmlFeatureBase
{
    /// <summary>
    /// The feature's <atom:author>, extracted from the layer markup (if specified).
    /// </summary>
    public KmlAuthor? Author { get; set; }
    /// <summary>
    /// The feature's <description>, extracted from the layer markup.
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// The feature's <name>, extracted from the layer markup.
    /// </summary>
    public string? Name { get; set; }
    /// <summary>
    /// The feature's <Snippet>, extracted from the layer markup.
    /// </summary>
    public string? Snippet { get; set; }
}

public class KmlFeatureData : KmlFeatureBase
{
    /// <summary>
    /// The feature's <id>, extracted from the layer markup. If no <id> has been specified, a unique ID will be generated for this feature.
    /// </summary>
    public string Id { get; set; } = string.Empty; //Should always be present.
    /// <summary>
    /// The feature's balloon styled text, if set.
    /// </summary>
    public string? InfoWindowHtml { get; set; }
}

public class KmlLayerMetadata : KmlFeatureBase
{
    /// <summary>
    /// Whether the layer has any screen overlays.
    /// </summary>
    public bool HasScreenOverlay { get; set; }
}

public class KmlAuthor
{
    /// <summary>
    /// The author's e-mail address, or an empty string if not specified.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// The author's name, or an empty string if not specified.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The author's home page, or an empty string if not specified.
    /// </summary>
    public string Uri { get; set; } = string.Empty;
}

public static class KmlLayerStatus
{
    /// <summary>
    /// The document could not be found. Most likely it is an invalid URL, or the document is not publicly available.
    /// </summary>
    public const string DOCUMENT_NOT_FOUND = "DOCUMENT_NOT_FOUND";
    /// <summary>
    /// The document exceeds the file size limits of KmlLayer.
    /// </summary>
    public const string DOCUMENT_TOO_LARGE = "DOCUMENT_TOO_LARGE";
    /// <summary>
    /// The document could not be fetched.
    /// </summary>
    public const string FETCH_ERROR = "FETCH_ERROR";
    /// <summary>
    /// The document is not a valid KML, KMZ or GeoRSS document.
    /// </summary>
    public const string INVALID_DOCUMENT = "INVALID_DOCUMENT";
    /// <summary>
    /// The KmlLayer is invalid. 
    /// </summary>
    public const string INVALID_REQUEST = "INVALID_REQUEST";
    /// <summary>
    /// The document exceeds the feature limits of KmlLayer.
    /// </summary>
    public const string LIMITS_EXCEEDED = "LIMITS_EXCEEDED";
    /// <summary>
    /// The layer loaded successfully.
    /// </summary>
    public const string OK = "OK";
    /// <summary>
    /// The document could not be loaded within a reasonable amount of time.
    /// </summary>
    public const string TIMED_OUT = "TIMED_OUT";
    /// <summary>
    /// The document failed to load for an unknown reason.
    /// </summary>
    public const string UNKNOWN = "UNKNOWN";
}