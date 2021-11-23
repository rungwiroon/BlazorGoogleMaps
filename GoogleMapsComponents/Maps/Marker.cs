using Microsoft.JSInterop;
using OneOf;
using System;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static GoogleMapsComponents.Helper;

namespace GoogleMapsComponents.Maps
{
    [JsonConverter(typeof(JsObjectRefConverter))]
    public class Marker : MVCObject //ListableEntityBase<MarkerOptions>
    {
        public static async Task<Marker> CreateAsync(IJSRuntime jsRuntime, MarkerOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.Marker",
                opts);
            var obj = new Marker(jsObjectRef);
            return obj;
        }

        internal Marker(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        public async Task<Animation?> GetAnimation()
        {
            var animation = await InvokeAsync<object>(
                "getAnimation");

            return Helper.ToNullableEnum<Animation>(animation?.ToString());
        }

        public ValueTask<bool> GetClickable()
        {
            return InvokeAsync<bool>(
                "getClickable");
        }

        public ValueTask<string> GetCursor()
        {
            return InvokeAsync<string>(
                "getCursor");
        }

        public ValueTask<bool> GetDraggable()
        {
            return InvokeAsync<bool>(
                "getDraggable");
        }

        public ValueTask<T> GetIcon<T>()
        {
            if (typeof(T) != typeof(string)
                && typeof(T) != typeof(Icon)
                && typeof(T) != typeof(Symbol))
                throw new InvalidCastException("Icon type must be string, Icon or Symbol.");

            return InvokeAsync<T>(
                "getIcon");
        }

        public ValueTask<T> GetLabel<T>()
        {
            if (typeof(T) != typeof(string)
                && typeof(T) != typeof(MarkerLabel))
                throw new InvalidCastException("label type must be string or MarkerLabel.");

            return InvokeAsync<T>(
                "getLabel");
        }

        public ValueTask<LatLngLiteral> GetPosition()
        {
            return InvokeAsync<LatLngLiteral>(
                "getPosition");
        }

        public ValueTask<MarkerShape> GetShape()
        {
            return InvokeAsync<MarkerShape>(
                "getShape");
        }

        public ValueTask<string> GetTitle()
        {
            return InvokeAsync<string>(
                "getTitle");
        }

        public ValueTask<bool> GetVisible()
        {
            return InvokeAsync<bool>(
                "getVisible");
        }

        public ValueTask<int> GetZIndex()
        {
            return InvokeAsync<int>(
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
            return this.InvokeVoidAsync(
                "setAnimation",
                animationCode);
        }

        public ValueTask SetClickable(bool flag)
        {
            return this.InvokeVoidAsync(
                "setClickable",
                flag);
        }

        public ValueTask SetCursor(string cursor)
        {
            return this.InvokeVoidAsync(
                "setCursor",
                cursor);
        }

        public ValueTask SetDraggable(bool flag)
        {
            return this.InvokeVoidAsync(
                "setDraggable",
                flag);
        }

        public ValueTask SetIcon(string icon)
        {
            return this.InvokeVoidAsync(
                "setIcon",
                icon);
        }

        public ValueTask SetIcon(Icon icon)
        {
            return this.InvokeVoidAsync(
                "setIcon",
                icon);
        }

        public ValueTask SetLabel(OneOf<string, MarkerLabel> label)
        {
            return this.InvokeVoidAsync(
                "setLabel",
                MakeArgJsFriendly(label));
        }

        public ValueTask SetOpacity(float opacity)
        {
            return this.InvokeVoidAsync(
                "setOpacity",
                opacity);
        }

        public ValueTask SetOptions(MarkerOptions options)
        {
            return this.InvokeVoidAsync(
                "setOptions",
                options);
        }

        public ValueTask SetPosition(LatLngLiteral latLng)
        {
            return this.InvokeVoidAsync(
                "setPosition",
                latLng);
        }

        public ValueTask SetShape(MarkerShape shape)
        {
            return this.InvokeVoidAsync(
                "setShape",
                shape);
        }

        public ValueTask SetTitle(string title)
        {
            return this.InvokeVoidAsync(
                "setTitle",
                title);
        }

        public ValueTask SetVisible(bool visible)
        {
            return this.InvokeVoidAsync(
                "setVisible",
                visible);
        }

        public ValueTask SetZIndex(int zIndex)
        {
            return this.InvokeVoidAsync(
                "setZIndex",
                zIndex);
        }

        public ValueTask<Map?> GetMap()
        {
            return this.InvokeWithReturnedObjectRefAsync(
                "getMap",
                objRef => new Map(objRef));
        }

        /// <summary>
        /// Renders the mao entity on the specified map or panorama. 
        /// If map is set to null, the map entity will be removed.
        /// </summary>
        /// <param name="map"></param>
        public ValueTask SetMap(Map? map)
        {
            return this.InvokeVoidAsync(
                "setMap",
                map?.Reference);
        }
    }
}