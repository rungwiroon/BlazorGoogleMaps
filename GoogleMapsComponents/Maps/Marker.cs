using Microsoft.JSInterop;
using OneOf;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public class Marker : ListableEntityBase<MarkerOptions>
{
    public static async Task<Marker> CreateAsync(IJSRuntime jsRuntime, MarkerOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Marker", opts);
        var obj = new Marker(jsObjectRef);
        return obj;
    }

    internal Marker(JsObjectRef jsObjectRef)
        : base(jsObjectRef)
    {
    }

    public async Task<Animation?> GetAnimation()
    {
        var animation = await _jsObjectRef.InvokeAsync<object?>("getAnimation");

        return Helper.ToNullableEnum<Animation>(animation?.ToString());
    }

    public Task<bool> GetClickable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getClickable");
    }

    public Task<string> GetCursor()
    {
        return _jsObjectRef.InvokeAsync<string>(
            "getCursor");
    }

    public Task<bool> GetDraggable()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getDraggable");
    }

    public async Task<OneOf<string, Icon, Symbol>> GetIcon()
    {
        var result = await _jsObjectRef.InvokeAsync<string, Icon, Symbol>(
            "getIcon");

        return result;
    }

    public Task<OneOf<string, MarkerLabel>> GetLabel()
    {
        return _jsObjectRef.InvokeAsync<string, MarkerLabel>("getLabel");
    }

    public async Task<string> GetLabelText()
    {
        OneOf<string, MarkerLabel> markerLabel = await GetLabel();
        return markerLabel.IsT0 ? markerLabel.AsT0 : markerLabel.AsT1.Text;
    }

    public async Task<MarkerLabel> GetLabelMarkerLabel()
    {
        OneOf<string, MarkerLabel> markerLabel = await GetLabel();
        return markerLabel.IsT1 ?
            markerLabel.AsT1 :
            new MarkerLabel { Text = markerLabel.AsT0 };
    }

    public Task<LatLngLiteral> GetPosition()
    {
        return _jsObjectRef.InvokeAsync<LatLngLiteral>(
            "getPosition");
    }

    public Task<MarkerShape> GetShape()
    {
        return _jsObjectRef.InvokeAsync<MarkerShape>(
            "getShape");
    }

    public Task<string> GetTitle()
    {
        return _jsObjectRef.InvokeAsync<string>(
            "getTitle");
    }

    public Task<bool> GetVisible()
    {
        return _jsObjectRef.InvokeAsync<bool>(
            "getVisible");
    }

    public Task<int> GetZIndex()
    {
        return _jsObjectRef.InvokeAsync<int>(
            "getZIndex");
    }

    /// <summary>
    /// Start an animation. 
    /// Any ongoing animation will be cancelled. 
    /// Currently supported animations are: BOUNCE, DROP. 
    /// Passing in null will cause any animation to stop.
    /// </summary>
    /// <param name="animation"></param>
    public Task SetAnimation(Animation? animation)
    {
        int? animationCode;

        switch (animation)
        {
            case null: animationCode = null; break;
            case Animation.Bounce: animationCode = 1; break;
            case Animation.Drop: animationCode = 2; break;
            default: animationCode = 0; break;
        }

        return _jsObjectRef.InvokeAsync(
            "setAnimation",
            animationCode);
    }

    public Task SetClickable(bool flag)
    {
        return _jsObjectRef.InvokeAsync(
            "setClickable",
            flag);
    }

    public Task SetCursor(string cursor)
    {
        return _jsObjectRef.InvokeAsync(
            "setCursor",
            cursor);
    }

    public Task SetDraggable(bool flag)
    {
        return _jsObjectRef.InvokeAsync(
            "setDraggable",
            flag);
    }

    public Task SetIcon(string icon)
    {
        return _jsObjectRef.InvokeAsync(
            "setIcon",
            icon);
    }

    public Task SetIcon(Icon icon)
    {
        return _jsObjectRef.InvokeAsync(
            "setIcon",
            icon);
    }

    public Task SetLabel(OneOf<string, MarkerLabel> label)
    {
        return _jsObjectRef.InvokeAsync(
            "setLabel",
            label);
    }

    public async Task SetLabelText(string labelText)
    {
        OneOf<string, MarkerLabel> markerLabel = await GetLabel();
        if (markerLabel.IsT1)
        {
            MarkerLabel label = markerLabel.AsT1;
            label.Text = labelText;
            await SetLabel(label);
        }
        else
            await SetLabel(labelText);
    }

    public Task SetOpacity(float opacity)
    {
        return _jsObjectRef.InvokeAsync(
            "setOpacity",
            opacity);
    }

    public Task SetOptions(MarkerOptions options)
    {
        return _jsObjectRef.InvokeAsync(
            "setOptions",
            options);
    }

    public Task SetPosition(LatLngLiteral latLng)
    {
        return _jsObjectRef.InvokeAsync(
            "setPosition",
            latLng);
    }

    public Task SetShape(MarkerShape shape)
    {
        return _jsObjectRef.InvokeAsync(
            "setShape",
            shape);
    }

    public Task SetTitle(string title)
    {
        return _jsObjectRef.InvokeAsync(
            "setTitle",
            title);
    }

    public Task SetVisible(bool visible)
    {
        return _jsObjectRef.InvokeAsync(
            "setVisible",
            visible);
    }

    public Task SetZIndex(int zIndex)
    {
        return _jsObjectRef.InvokeAsync(
            "setZIndex",
            zIndex);
    }
}