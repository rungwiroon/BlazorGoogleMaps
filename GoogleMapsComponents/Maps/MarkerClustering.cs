using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace GoogleMapsComponents.Maps;

/// <summary>
/// https://github.com/googlemaps/js-markerclusterer
/// </summary>
public class MarkerClustering : EventEntityBase, IJsObjectRef
{
    public Guid Guid => _jsObjectRef.Guid;

    public static async Task<MarkerClustering> CreateAsync(
        IJSRuntime jsRuntime,
        Map map,
        IEnumerable<IMarker> markers,
        MarkerClustererOptions? options = null
    )
    {
        options ??= new MarkerClustererOptions();

        var guid = Guid.NewGuid();
        var jsObjectRef = new JsObjectRef(jsRuntime, guid);
        await jsRuntime.InvokeVoidAsync("blazorGoogleMaps.objectManager.createClusteringMarkers", guid.ToString(), map.Guid.ToString(), markers, options);
        var obj = new MarkerClustering(jsObjectRef);
        return obj;
    }

    internal MarkerClustering(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }

    /// <summary>
    /// Add additional markers to an existing MarkerClusterer
    /// </summary>
    /// <param name="markers"></param>
    /// <param name="noDraw">when true, clusters will not be rerendered on the next map idle event rather than immediately after markers are added</param>
    public virtual async Task AddMarkers(IEnumerable<IMarker>? markers, bool noDraw = false)
    {
        if (markers == null)
        {
            return;
        }

        await _jsObjectRef.JSRuntime.InvokeVoidAsync("blazorGoogleMaps.objectManager.addClusteringMarkers", _jsObjectRef.Guid.ToString(), markers, noDraw);
    }

    public virtual async Task SetMap(Map map)
    {
        await _jsObjectRef.InvokeAsync("setMap", map);
    }

    /// <summary>
    /// Removes provided markers from the clusterer's internal list of source markers.
    /// </summary>
    public virtual async Task RemoveMarkers(IEnumerable<IMarker> markers, bool noDraw = false)
    {
        await _jsObjectRef.JSRuntime.InvokeVoidAsync("blazorGoogleMaps.objectManager.removeClusteringMarkers", _jsObjectRef.Guid.ToString(), markers, noDraw);
    }

    /// <summary>
    /// Removes all clusters and markers from the map and also removes all markers managed by the clusterer.
    /// </summary>
    public virtual async Task ClearMarkers(bool noDraw = false)
    {
        await _jsObjectRef.InvokeAsync("clearMarkers", noDraw);
    }

    /// <summary>
    /// https://googlemaps.github.io/js-markerclusterer/interfaces/Renderer.html#render
    /// </summary>
    /// <returns></returns>
    public virtual Task Render()
    {
        return _jsObjectRef.InvokeAsync("render");
    }
}