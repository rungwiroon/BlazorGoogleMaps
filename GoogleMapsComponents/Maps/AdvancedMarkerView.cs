using Microsoft.JSInterop;
using OneOf;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Represents an advanced marker element for Google Maps.
/// As in 2023-09 Available only in the v=beta channel.
/// </summary>
public class AdvancedMarkerElement : ListableEntityBase<AdvancedMarkerElementOptions>, IMarker
{
    /// <summary>
    /// The name of the Google Maps AdvancedMarkerElement JavaScript class.
    /// See: https://developers.google.com/maps/documentation/javascript/reference/advanced-markers
    /// </summary>
    public const string GoogleMapAdvancedMarkerName = "google.maps.marker.AdvancedMarkerElement";

    /// <summary>
    /// Asynchronously creates a new <see cref="AdvancedMarkerElement"/> instance.
    /// </summary>
    /// <param name="jsRuntime">The JS runtime to use for interop calls.</param>
    /// <param name="opts">The options for the advanced marker element. Optional.</param>
    /// <returns>A task that represents the asynchronous creation operation. The task result contains the created <see cref="AdvancedMarkerElement"/>.</returns>
    public static async Task<AdvancedMarkerElement> CreateAsync(IJSRuntime jsRuntime, AdvancedMarkerElementOptions? opts = null)
    {
        if (opts != null)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (opts.Content.Value == null)
            {
                opts.Content = "<div>&nbsp;</div>";
            }

        }
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, GoogleMapAdvancedMarkerName, opts);
        var obj = new AdvancedMarkerElement(jsObjectRef);
        return obj;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AdvancedMarkerElement"/> class.
    /// </summary>
    /// <param name="jsObjectRef">The JavaScript object reference.</param>
    internal AdvancedMarkerElement(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    /// <summary>
    /// Gets the z-index of the marker.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the z-index value.</returns>
    public Task<int> GetZIndex()
    {
        return _jsObjectRef.InvokeAsync<int>("getZIndex");
    }

    /// <summary>
    /// Sets the z-index of the marker.
    /// </summary>
    /// <param name="zIndex"></param>
    /// <returns></returns>
    public Task SetZIndex(int? zIndex)
    {
        return _jsObjectRef.InvokePropertyAsync("zIndex", zIndex);
    }

    /// <summary>
    /// Sets the map of the marker.
    /// AdvancedMarkerElement uses the map property instead of setMap.
    /// </summary>
    /// <param name="map"></param>
    /// <returns></returns>
    public override Task SetMap(Map? map)
    {
        return _jsObjectRef.InvokePropertyAsync("map", map);
    }

    /// <summary>
    /// Sets the position of the marker.
    /// </summary>
    /// <param name="newPosition">The new position as a <see cref="LatLngLiteral"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetPosition(LatLngLiteral newPosition)
    {
        await _jsObjectRef.InvokePropertyAsync("position", newPosition);
    }

    /// <summary>
    /// Sets the content of the marker.
    /// </summary>
    /// <param name="newContent">The new content, either a string (HTML) or a <see cref="PinElement"/>.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetContent(OneOf<string, PinElement> newContent)
    {
        await _jsObjectRef.InvokePropertyAsync("content", newContent);
    }

    /// <summary>
    /// Sets whether the marker is clickable.
    /// </summary>
    /// <param name="newValue">True to make the marker clickable; otherwise, false.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetGmpClickable(bool newValue)
    {
        await _jsObjectRef.InvokePropertyAsync("gmpClickable", newValue);
    }

    /// <summary>
    /// Gets whether the marker is clickable.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if clickable; otherwise, false.</returns>
    public async Task<bool> GetGmpClickable()
    {
        return await _jsObjectRef.InvokePropertyAsync<bool>("gmpClickable");
    }

    /// <summary>
    /// Sets whether the marker is draggable.
    /// </summary>
    /// <param name="newValue">True to make the marker draggable; otherwise, false.</param>
    /// <returns>A task that represents the asynchronous operation.</returns>
    public async Task SetGmpDraggable(bool newValue)
    {
        await _jsObjectRef.InvokePropertyAsync("gmpDraggable", newValue);
    }

    /// <summary>
    /// Gets whether the marker is draggable.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains true if draggable; otherwise, false.</returns>
    public async Task<bool> GetGmpDraggable()
    {
        return await _jsObjectRef.InvokePropertyAsync<bool>("gmpDraggable");
    }

    /// <summary>
    /// Gets the position of the marker.
    /// </summary>
    /// <returns>A task that represents the asynchronous operation. The task result contains the position as a <see cref="LatLngAltitudeLiteral"/>.</returns>
    public async Task<LatLngAltitudeLiteral> GetPosition()
    {
        return await _jsObjectRef.InvokePropertyAsync<LatLngAltitudeLiteral>("position");
    }
}
