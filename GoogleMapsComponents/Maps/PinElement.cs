using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A PinElement represents a DOM element that consists of a shape and a glyph. The shape has the same balloon style as seen in the default AdvancedMarkerElement. The glyph is an optional DOM element displayed in the balloon shape. A PinElement may have a different aspect ratio depending on its PinElement.scale.
/// https://developers.google.com/maps/documentation/javascript/reference/advanced-markers#PinElement.element
/// </summary>
public class PinElement// : EventEntityBase
{
    public string? Background { get; set; }
    public string? BorderColor { get; set; }
    // private ElementReference Element { get; set; }

    public PinElement() : base()
    {

    }
    public static async Task<PinElement> CreateAsync(IJSRuntime jsRuntime, PinElementOptions? opts = null)
    {
        var id = Guid.NewGuid();
        var jsObjectRef = new JsObjectRef(jsRuntime, id);
        var result = await jsObjectRef.InvokeAsync<PinElement>("google.maps.marker.PinElement", opts);
        //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.marker.PinElement", opts);
        //var obj = new PinElement(jsObjectRef);
        return result;
    }


    protected PinElement(JsObjectRef jsObjectRef) : base()
    {
    }


    public async Task<string> GetContent()
    {
        //_jsObjectRef.JSRuntime.
        //await _jsObjectRef.InvokeAsync("element");
        //var ss = element.ToString();

        //return element.ToString();
        return "";
    }
}