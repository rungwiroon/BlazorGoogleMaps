using System;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using GoogleMapsComponents.Maps.Places;
using GoogleMapsComponents.Maps.Extension;

namespace GoogleMapsComponents.Maps.KmlLayer;

/// <summary>
/// A service for converting between an address and a LatLng.
/// <example> 
/// Use as follows: 
/// <code> 
/// var kmlLayer = KmlLayer.CreateAsync(map.JsRuntime, "KML_REFERENCE");
/// kmlLayer.AddEventListener&lt;KmlMouseEvent&gt;("click", e => {
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
    /// <param name="src">Link to a valid KML file</param>
    /// <param name="options">Optional options to configure the layer</param>
    /// <returns>A KmlLayer</returns>
    public static async Task<KmlLayer> CreateAsync(IJSRuntime jsRuntime, string src, KmlLayerOptions? options = null)
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