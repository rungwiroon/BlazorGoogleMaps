using Microsoft.JSInterop;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// 2023-09
/// Notice: Available only in the v=beta channel.
/// </summary>
public class AdvancedMarkerView : ListableEntityBase<AdvancedMarkerViewOptions>
{
    public static async Task<Marker> CreateAsync(IJSRuntime jsRuntime, AdvancedMarkerViewOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.marker.AdvancedMarkerView", opts);
        var obj = new Marker(jsObjectRef);
        return obj;
    }

    internal AdvancedMarkerView(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    public Task<int> GetZIndex()
    {
        return _jsObjectRef.InvokeAsync<int>("getZIndex");
    }
}