using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// This object is returned from various mouse events on the map and overlays.
/// </summary>
public class MouseEvent : IActionArgument
{
    public JsObjectRef JsObjectRef { get; set; }

    /// <summary>
    /// The latitude/longitude that was below the cursor when the event occurred.
    /// </summary>
    public LatLngLiteral LatLng { get; set; }

    /// <summary>
    /// Prevents this event from propagating further.
    /// </summary>
    public Task Stop()
    {
        return JsObjectRef.InvokeAsync("stop");
    }
}