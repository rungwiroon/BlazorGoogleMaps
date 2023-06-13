using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// ReSharper disable UnusedMember.Global

namespace GoogleMapsComponents.Maps;

/// <summary>
/// https://github.com/googlemaps/js-markerclusterer
/// </summary>
public class MarkerClustering : EventEntityBase, IJsObjectRef
{
    public Guid Guid => _jsObjectRef.Guid;
    private Map _map;
    private readonly IEnumerable<Marker> _originalMarkers;

    public static async Task<MarkerClustering> CreateAsync(
        IJSRuntime jsRuntime,
        Map map,
        IEnumerable<Marker> markers,
        MarkerClustererOptions? options = null
    )
    {
        options ??= new MarkerClustererOptions();

        var guid = Guid.NewGuid();
        var jsObjectRef = new JsObjectRef(jsRuntime, guid);
        await jsRuntime.InvokeVoidAsync("blazorGoogleMaps.objectManager.createClusteringMarkers", guid.ToString(), map.Guid.ToString(), markers, options);
        var obj = new MarkerClustering(jsObjectRef, map, markers);
        return obj;
    }

    internal MarkerClustering(JsObjectRef jsObjectRef, Map map, IEnumerable<Marker> markers) : base(jsObjectRef)
    {
        _map = map;
        _originalMarkers = markers;
    }

    /// <summary>
    /// Add additional markers to an existing MarkerClusterer
    /// </summary>
    /// <param name="markers"></param>
    /// <param name="noDraw">when true, clusters will not be rerendered on the next map idle event rather than immediately after markers are added</param>
    public virtual async Task AddMarkers(IEnumerable<Marker>? markers, bool noDraw = false)
    {
        if (markers == null)
        {
            return;
        }

        await _jsObjectRef.JSRuntime.InvokeVoidAsync("blazorGoogleMaps.objectManager.addClusteringMarkers", _jsObjectRef.Guid.ToString(), markers, noDraw);
    }

    public virtual async Task SetMap(Map map)
    {
        _map = map;
        await _jsObjectRef.InvokeAsync("setMap", map);
    }

    /// <summary>
    /// Removes provided markers from the clusterer's internal list of source markers.
    /// </summary>
    public virtual async Task RemoveMarkers(IEnumerable<Marker> markers, bool noDraw = false)
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
    /// Fits the map to the bounds of the markers managed by the clusterer.
    /// </summary>
    /// <param name="padding"></param>
    [Obsolete("Deprecated: Center map based on unclustered Markers before clustering. Latest js-markerclusterer lib doesn't support this. Workaround is slow. ")]
    public virtual async Task FitMapToMarkers(int padding)
    {
        var newBounds = new LatLngBoundsLiteral(await _originalMarkers.First().GetPosition());
        foreach (var marker in _originalMarkers)
        {
            newBounds.Extend(await marker.GetPosition());
        }

        await _map.FitBounds(newBounds, padding);
    }


    //[Obsolete("Deprecated in favor of Redraw() to match latest js-markerclusterer")]
    //public virtual Task Repaint()
    //{
    //    return Render();
    //}


    //[Obsolete("Deprecated in favor of Render() to match latest js-markerclusterer")]
    //public virtual Task Redraw()
    //{
    //    return Render();
    //}

    /// <summary>
    /// https://googlemaps.github.io/js-markerclusterer/interfaces/Renderer.html#render
    /// </summary>
    /// <returns></returns>
    public virtual Task Render()
    {
        return _jsObjectRef.InvokeAsync("render");
    }
}