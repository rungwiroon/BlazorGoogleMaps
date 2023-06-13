using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps;

public class Projection : IDisposable
{
    protected readonly JsObjectRef _jsObjectRef;

    public Guid Guid => _jsObjectRef.Guid;

    public Projection(IJSRuntime jsRuntime, Guid id)
    {
        _jsObjectRef = new JsObjectRef(jsRuntime, id);
    }

    public async Task<Point> FromLatLngToPoint(LatLngLiteral literal)
    {
        var result = await _jsObjectRef.InvokeAsync<Point>("fromLatLngToPoint", literal);
        return result;
    }

    public async Task<LatLngLiteral> FromPointToLatLng(Point point)
    {
        var result = await _jsObjectRef.InvokeAsync<LatLngLiteral>("fromPointToLatLng", point);
        return result;
    }

    public void Dispose()
    {
        _jsObjectRef?.Dispose();
    }
}