using GoogleMapsComponents.Maps.Extension;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public class DirectionsRenderer : EventEntityBase, IDisposable
{
    public static async Task<DirectionsRenderer> CreateAsync(IJSRuntime jsRuntime, DirectionsRendererOptions opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.DirectionsRenderer", opts);
        var obj = new DirectionsRenderer(jsObjectRef);

        return obj;
    }

    private DirectionsRenderer(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
    /// <returns></returns>
    public async Task<DirectionsResult?> Route(DirectionsRequest request, DirectionsRequestOptions? directionsRequestOptions = null)
    {
        if (directionsRequestOptions == null)
        {
            directionsRequestOptions = new DirectionsRequestOptions();
        }

        var response = await _jsObjectRef.InvokeAsync<string>(
            "blazorGoogleMaps.directionService.route",
            request, directionsRequestOptions);
        try
        {
            var dirResult = Helper.DeSerializeObject<DirectionsResult>(response);
            return dirResult;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error parsing DirectionsResult Object. Message: " + e.Message);
            return null;
        }

    }

    public Task<Map> GetMap()
    {
        return _jsObjectRef.InvokeAsync<Map>(
            "getMap");
    }

    public Task<int> GetRouteIndex()
    {
        return _jsObjectRef.InvokeAsync<int>(
            "getRouteIndex");
    }

    public async Task SetDirections(DirectionsResult? directions)
    {
        await _jsObjectRef.InvokeAsync(
            "setDirections",
            directions);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="directionsRequestOptions">Lets you specify which route response paths to opt out from clearing.</param>
    /// <returns></returns>
    public async Task<DirectionsResult> GetDirections(DirectionsRequestOptions directionsRequestOptions = null)
    {
        if (directionsRequestOptions == null)
        {
            directionsRequestOptions = new DirectionsRequestOptions();
        }

        var response = await _jsObjectRef.InvokeAsync<string>(
            "getDirections",
            directionsRequestOptions);
        try
        {
            var dirResult = Helper.DeSerializeObject<DirectionsResult>(response);
            return dirResult;
        }
        catch (Exception e)
        {
            Console.WriteLine("Error parsing DirectionsResult Object. Message: " + e.Message);
            return null;
        }
    }

    public async Task SetMap(Map? map)
    {
        await _jsObjectRef.InvokeAsync(
            "setMap",
            map);
    }

    public async Task SetRouteIndex(int routeIndex)
    {
        await _jsObjectRef.InvokeAsync(
            "setRouteIndex",
            routeIndex);
    }
}