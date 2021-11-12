using Microsoft.JSInterop;
using OneOf;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class Marker : ListableEntityBase<MarkerOptions>
    {
        public static async Task<Marker> CreateAsync(IJSRuntime jsRuntime, MarkerOptions opts = null)
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
            var animation = await _jsObjectRef.InvokeAsync<object>(
                "getAnimation");

            return Helper.ToNullableEnum<Animation>(animation?.ToString());
        }

        public ValueTask<bool> GetClickable()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getClickable");
        }

        public ValueTask<string> GetCursor()
        {
            return _jsObjectRef.InvokeAsync<string>(
                "getCursor");
        }

        public ValueTask<bool> GetDraggable()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getDraggable");
        }

        public async ValueTask<OneOf<string, Icon, Symbol>> GetIcon()
        {
            var result = await _jsObjectRef.InvokeAsync<string, Icon, Symbol>(
                "getIcon");

            return result;
        }

        public ValueTask<OneOf<string, MarkerLabel>> GetLabel()
        {
            return _jsObjectRef.InvokeAsync<string, MarkerLabel>("getLabel");
        }

        public async ValueTask<string> GetLabelText()
        {
            OneOf<string, MarkerLabel> markerLabel = await GetLabel();
            return markerLabel.IsT0 ? markerLabel.AsT0 : markerLabel.AsT1.Text;
        }

        public async ValueTask<MarkerLabel> GetLabelMarkerLabel()
        {
            OneOf<string, MarkerLabel> markerLabel = await GetLabel();
            return markerLabel.IsT1 ?
                markerLabel.AsT1 :
                new MarkerLabel { Text = markerLabel.AsT0 };
        }

        public ValueTask<LatLngLiteral> GetPosition()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>(
                "getPosition");
        }

        public ValueTask<MarkerShape> GetShape()
        {
            return _jsObjectRef.InvokeAsync<MarkerShape>(
                "getShape");
        }

        public ValueTask<string> GetTitle()
        {
            return _jsObjectRef.InvokeAsync<string>(
                "getTitle");
        }

        public ValueTask<bool> GetVisible()
        {
            return _jsObjectRef.InvokeAsync<bool>(
                "getVisible");
        }

        public ValueTask<int> GetZIndex()
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
        public ValueTask SetAnimation(Animation animation)
        {
            int animationCode = 0;
            if (animation == Animation.Bounce)
            {
                animationCode = 1;
            }
            return _jsObjectRef.InvokeAsync(
                "setAnimation",
                animationCode);
        }

        public ValueTask SetClickable(bool flag)
        {
            return _jsObjectRef.InvokeAsync(
                "setClickable",
                flag);
        }

        public ValueTask SetCursor(string cursor)
        {
            return _jsObjectRef.InvokeAsync(
                "setCursor",
                cursor);
        }

        public ValueTask SetDraggable(bool flag)
        {
            return _jsObjectRef.InvokeAsync(
                "setDraggable",
                flag);
        }

        public ValueTask SetIcon(string icon)
        {
            return _jsObjectRef.InvokeAsync(
                "setIcon",
                icon);
        }

        public ValueTask SetIcon(Icon icon)
        {
            return _jsObjectRef.InvokeAsync(
                "setIcon",
                icon);
        }

        public ValueTask SetLabel(OneOf<string, MarkerLabel> label)
        {
            return _jsObjectRef.InvokeAsync(
                "setLabel",
                label);
        }

        public async ValueTask SetLabelText(string labelText)
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

        public ValueTask SetOpacity(float opacity)
        {
            return _jsObjectRef.InvokeAsync(
                "setOpacity",
                opacity);
        }

        public ValueTask SetOptions(MarkerOptions options)
        {
            return _jsObjectRef.InvokeAsync(
                "setOptions",
                options);
        }

        public ValueTask SetPosition(LatLngLiteral latLng)
        {
            return _jsObjectRef.InvokeAsync(
                "setPosition",
                latLng);
        }

        public ValueTask SetShape(MarkerShape shape)
        {
            return _jsObjectRef.InvokeAsync(
                "setShape",
                shape);
        }

        public ValueTask SetTitle(string title)
        {
            return _jsObjectRef.InvokeAsync(
                "setTitle",
                title);
        }

        public ValueTask SetVisible(bool visible)
        {
            return _jsObjectRef.InvokeAsync(
                "setVisible",
                visible);
        }

        public ValueTask SetZIndex(int zIndex)
        {
            return _jsObjectRef.InvokeAsync(
                "setZIndex",
                zIndex);
        }
    }
}