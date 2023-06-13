using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public class PolylinePath
{
    private readonly JsObjectRef _jsObjectRef;

    public PolylinePath(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    public Task Push(LatLngLiteral coordinate)
    {
        return _jsObjectRef.InvokeAsync("push", coordinate);
    }

    public Task Clear()
    {
        return _jsObjectRef.InvokeAsync("clear");
    }
}