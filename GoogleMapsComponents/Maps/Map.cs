using GoogleMapsComponents.Maps.Coordinates;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using OneOf;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// google.maps.Map class
/// </summary>
public class Map : EventEntityBase, IJsObjectRef, IAsyncDisposable
{
    public Guid Guid => _jsObjectRef.Guid;

    private bool _isDisposed;
    public MapData Data { get; private set; }

    public static async Task<Map> CreateAsync(
        IJSRuntime jsRuntime,
        ElementReference mapDiv,
        MapOptions? opts = null)
    {
        if (opts?.ApiLoadOptions != null)
        {
            MapApiLoadOptions apiOpts = opts.ApiLoadOptions;
            await jsRuntime.InvokeVoidAsync("blazorGoogleMaps.objectManager.initMap",
                apiOpts);
        }
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Map", mapDiv, opts);
        var dataObjectRef = await jsObjectRef.GetObjectReference("data");
        var data = new MapData(dataObjectRef);
        var map = new Map(jsObjectRef, data);

        JsObjectRefInstances.Add(map);

        return map;
    }

    private Map(JsObjectRef jsObjectRef, MapData data) : base(jsObjectRef)
    {
        Data = data;
    }

    public async Task AddControl(ControlPosition position, ElementReference reference)
    {
        await _jsObjectRef.JSRuntime.MyInvokeAsync<object>("blazorGoogleMaps.objectManager.addControls", this.Guid.ToString(), position, reference);
    }

    public async Task RemoveControl(ControlPosition position, ElementReference reference)
    {
        await _jsObjectRef.JSRuntime.MyInvokeAsync<object>("blazorGoogleMaps.objectManager.removeControl", this.Guid.ToString(), position, reference);
    }

    public async Task RemoveControls(ControlPosition position)
    {
        await _jsObjectRef.JSRuntime.MyInvokeAsync<object>("blazorGoogleMaps.objectManager.removeControls", this.Guid.ToString(), position);
    }

    public async Task AddImageLayer(ImageMapType reference)
    {
        await _jsObjectRef.JSRuntime.MyInvokeAsync<object>("blazorGoogleMaps.objectManager.addImageLayer", this.Guid.ToString(), reference.Guid.ToString());
    }
    public async Task RemoveImageLayer(ImageMapType reference)
    {
        await _jsObjectRef.JSRuntime.MyInvokeAsync<object>("blazorGoogleMaps.objectManager.removeImageLayer", this.Guid.ToString(), reference.Guid.ToString());
    }
    public async Task RemoveAllImageLayers()
    {
        await _jsObjectRef.JSRuntime.MyInvokeAsync<object>("blazorGoogleMaps.objectManager.removeAllImageLayers", this.Guid.ToString());
    }

    /// <summary>
    /// Sets the viewport to contain the given bounds.
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="padding"></param>
    /// <returns></returns>
    public Task FitBounds(LatLngBoundsLiteral? bounds, OneOf<int, Padding>? padding = null)
    {
        return _jsObjectRef.InvokeAsync("fitBounds", bounds, padding);
    }

    /// <summary>
    /// Changes the center of the map by the given distance in pixels.
    /// If the distance is less than both the width and height of the map, the transition will be smoothly animated.
    /// Note that the map coordinate system increases from west to east (for x values) and north to south (for y values).
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <returns></returns>
    public Task PanBy(int x, int y)
    {
        return _jsObjectRef.InvokeAsync("panBy", x, y);
    }

    /// <summary>
    /// Changes the center of the map to the given LatLng.
    /// If the change is less than both the width and height of the map, the transition will be smoothly animated.
    /// </summary>
    /// <param name="latLng"></param>
    /// <returns></returns>
    public Task PanTo(LatLngLiteral latLng)
    {
        return _jsObjectRef.InvokeAsync("panTo", latLng);
    }

    /// <summary>
    /// Returns the current Projection.
    /// If the map is not yet initialized then the result is undefined.
    /// Listen to the projection_changed event and check its value to ensure it is not undefined.
    /// </summary>
    /// <returns></returns>
    public async Task<Projection> GetProjection()
    {
        var id = Guid.NewGuid();
        await _jsObjectRef.InvokeAsync("getProjection", id.ToString());
        //projection is returned and created on js
        var projection = new Projection(_jsObjectRef.JSRuntime, id);

        return projection;
    }

    /// <summary>
    /// Pans the map by the minimum amount necessary to contain the given LatLngBounds.
    /// It makes no guarantee where on the map the bounds will be, except that the map will be panned to show as much of the bounds as possible inside {currentMapSizeInPx} - {padding}.
    /// </summary>
    /// <param name="latLngBounds"></param>
    /// <returns></returns>
    public Task PanToBounds(LatLngBoundsLiteral latLngBounds)
    {
        return _jsObjectRef.InvokeAsync("panToBounds", latLngBounds);
    }

    /// <summary>
    /// Returns the lat/lng bounds of the current viewport.
    /// If more than one copy of the world is visible, the bounds range in longitude from -180 to 180 degrees inclusive.
    /// If the map is not yet initialized (i.e. the mapType is still null), or center and zoom have not been set then the result is null.
    /// </summary>
    /// <returns></returns>
    public Task<LatLngBoundsLiteral> GetBounds()
    {
        return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>("getBounds");
    }

    /// <summary>
    /// Returns the position displayed at the center of the map.
    /// Note that this LatLng object is not wrapped.
    /// </summary>
    /// <returns></returns>
    public Task<LatLngLiteral> GetCenter()
    {
        return _jsObjectRef.InvokeAsync<LatLngLiteral>("getCenter");
    }

    public Task SetCenter(LatLngLiteral latLng)
    {
        return _jsObjectRef.InvokeAsync("setCenter", latLng);
    }

    /// <summary>
    /// Returns the compass heading of aerial imagery.
    /// The heading value is measured in degrees (clockwise) from cardinal direction North.
    /// </summary>
    /// <returns></returns>
    public Task<int> GetHeading()
    {
        return _jsObjectRef.InvokeAsync<int>("getHeading");
    }

    /// <summary>
    /// Sets the compass heading for aerial imagery measured in degrees from cardinal direction North.
    /// </summary>
    /// <param name="heading"></param>
    /// <returns></returns>
    public Task SetHeading(int heading)
    {
        return _jsObjectRef.InvokeAsync("setHeading", heading);
    }

    public async Task<MapTypeId> GetMapTypeId()
    {
        var mapTypeIdStr = await _jsObjectRef.InvokeAsync<string>("getMapTypeId");

        return Helper.ToEnum<MapTypeId>(mapTypeIdStr);
    }

    public Task SetMapTypeId(MapTypeId mapTypeId)
    {
        return _jsObjectRef.InvokeAsync("setMapTypeId", mapTypeId);
    }

    /// <summary>
    /// Returns the current angle of incidence of the map, in degrees from the viewport plane to the map plane.
    /// The result will be 0 for imagery taken directly overhead or 45 for 45° imagery. 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels.
    /// Note: This method does not return the value set by setTilt.
    /// See setTilt for details.
    /// </summary>
    /// <returns></returns>
    public Task<int> GetTilt()
    {
        return _jsObjectRef.InvokeAsync<int>("getTilt");
    }

    /// <summary>
    /// Controls the automatic switching behavior for the angle of incidence of the map.
    /// The only allowed values are 0 and 45.
    /// setTilt(0) causes the map to always use a 0° overhead view regardless of the zoom level and viewport.
    /// setTilt(45) causes the tilt angle to automatically switch to 45 whenever 45° imagery is available for the current zoom level and viewport, and switch back to 0 whenever 45° imagery is not available (this is the default behavior).
    /// 45° imagery is only available for satellite and hybrid map types, within some locations, and at some zoom levels. Note: getTilt returns the current tilt angle, not the value set by setTilt.
    /// Because getTilt and setTilt refer to different things, do not bind() the tilt property; doing so may yield unpredictable effects.
    /// </summary>
    /// <param name="tilt"></param>
    /// <returns></returns>
    public Task SetTilt(int tilt)
    {
        return _jsObjectRef.InvokeAsync("setTilt", tilt);
    }

    public Task<double> GetZoom()
    {
        return _jsObjectRef.InvokeAsync<double>("getZoom");
    }

    public Task SetZoom(double zoom)
    {
        return _jsObjectRef.InvokeAsync("setZoom", zoom);
    }

    public Task SetOptions(MapOptions mapOptions)
    {
        return _jsObjectRef.InvokeAsync("setOptions", mapOptions);
    }

    public override async ValueTask DisposeAsync()
    {
        // Perform async cleanup.
        await DisposeAsyncCore();

        // Dispose of unmanaged resources.
        Dispose(false);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    protected override async ValueTask DisposeAsyncCore()
    {
        await _jsObjectRef.JSRuntime.InvokeAsync<object>("blazorGoogleMaps.objectManager.disposeMapElements", Guid.ToString());
        await base.DisposeAsyncCore();
        JsObjectRefInstances.Remove(_jsObjectRef.Guid.ToString());
    }

    protected override void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            base.Dispose(disposing);
            //_jsObjectRef.Dispose();

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _isDisposed = true;
        }
    }

    public override void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}