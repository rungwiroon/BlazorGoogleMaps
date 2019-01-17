using Microsoft.AspNetCore.Blazor.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    public class Marker : IDisposable
    {
        private MapComponent _map;
        public Guid Guid { get; private set; }

        public Marker(MarkerOptions opt)
        {
            Guid = Guid.NewGuid();

            if (opt?.Map != null)
                _map = opt.Map;

            Helper.MyInvokeAsync<bool>(
                "googleMapMarkerJsFunctions.init",
                Guid,
                opt);
        }

        public void Dispose()
        {
            JSRuntime.Current.InvokeAsync<bool>(
                "googleMapMarkerJsFunctions.dispose",
                Guid);
        }

        public async Task<Animation> GetAnimation()
        {
            var animation = await JSRuntime.Current.InvokeAsync<string>(
                "googleMapMarkerJsFunctions.getAnimation",
                Guid);

            return Helper.ToEnum<Animation>(animation);
        }

        public async Task<bool> GetClickable()
        {
            var result = await JSRuntime.Current.InvokeAsync<bool>(
                "googleMapMarkerJsFunctions.getClickable",
                Guid);

            return result;
        }

        public async Task<string> GetCursor()
        {
            var result = await JSRuntime.Current.InvokeAsync<string>(
                "googleMapMarkerJsFunctions.getCursor",
                Guid);

            return result;
        }

        public async Task<bool> GetDraggable()
        {
            var result = await JSRuntime.Current.InvokeAsync<bool>(
                "googleMapMarkerJsFunctions.getDraggable",
                Guid);

            return result;
        }

        public async Task<object> GetIcon()
        {
            var result = await JSRuntime.Current.InvokeAsync<object>(
                "googleMapMarkerJsFunctions.getIcon",
                Guid);

            return result;
        }

        public async Task<MarkerLabel> GetLabel()
        {
            var result = await JSRuntime.Current.InvokeAsync<MarkerLabel>(
                "googleMapMarkerJsFunctions.getLabel",
                Guid);

            return result;
        }

        public MapComponent GetMap()
        {
            return _map;
        }

        public async Task<LatLngLiteral> GetPosition()
        {
            var result = await JSRuntime.Current.InvokeAsync<LatLngLiteral>(
                   "googleMapMarkerJsFunctions.getPosition",
                   Guid);

            return result;
        }

        public async Task<MarkerShape> GetShape()
        {
            var result = await JSRuntime.Current.InvokeAsync<MarkerShape>(
                   "googleMapMarkerJsFunctions.getShape",
                   Guid);

            return result;
        }

        public async Task<string> GetTitle()
        {
            var result = await JSRuntime.Current.InvokeAsync<string>(
                   "googleMapMarkerJsFunctions.getTitle",
                   Guid);

            return result;
        }

        public async Task<bool> GetVisible()
        {
            var result = await JSRuntime.Current.InvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.getVisible",
                   Guid);

            return result;
        }

        public async Task<int> GetZIndex()
        {
            var result = await JSRuntime.Current.InvokeAsync<int>(
                   "googleMapMarkerJsFunctions.getZIndex",
                   Guid);

            return result;
        }

        /// <summary>
        /// Start an animation. 
        /// Any ongoing animation will be cancelled. 
        /// Currently supported animations are: BOUNCE, DROP. 
        /// Passing in null will cause any animation to stop.
        /// </summary>
        /// <param name="animation"></param>
        public async Task SetAnimation(Animation animation)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setAnimation",
                   Guid,
                   animation);
        }

        public async Task SetClickable(bool flag)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setClickable",
                   Guid,
                   flag);
        }

        public async Task SetCursor(string cursor)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setCursor",
                   Guid,
                   cursor);
        }

        public async Task SetDraggable(bool flag)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setDraggable",
                   Guid,
                   flag);
        }

        public async Task SetIcon(string icon)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setIcon",
                   Guid,
                   icon);
        }

        public async Task SetIcon(Icon icon)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setIcon",
                   Guid,
                   icon);
        }

        public async Task SetLabel(Symbol label)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setLabel",
                   Guid,
                   label);
        }

        /// <summary>
        /// Renders the marker on the specified map or panorama. 
        /// If map is set to null, the marker will be removed.
        /// </summary>
        /// <param name="map"></param>
        public async Task SetMap(GoogleMap map)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setMap",
                   Guid,
                   map?.DivId);

            _map = map;
        }

        public async Task SetOpacity(float opacity)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setOpacity",
                   Guid,
                   opacity);
        }

        public async Task SetOptions(MarkerOptions options)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setOptions",
                   Guid,
                   options);
        }

        public async Task SetPosition(LatLngLiteral latLng)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setPosition",
                   Guid,
                   latLng);
        }

        public async Task SetShape(MarkerShape shape)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setShape",
                   Guid,
                   shape);
        }

        public async Task SetTiltle(string title)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setTitle",
                   Guid,
                   title);
        }

        public async Task SetVisible(bool visible)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setVisible",
                   Guid,
                   visible);
        }

        public async Task SetZIndex(int zIndex)
        {
            await Helper.MyInvokeAsync<bool>(
                   "googleMapMarkerJsFunctions.setZIndex",
                   Guid,
                   zIndex);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action<MapEventArgs> handler)
        {
            var guid = await MapEventJsInterop.SubscribeMarkerEvent(Guid.ToString(), eventName, (dict) => handler(MapEventArgs.Empty));
            return new MapEventListener(guid);
        }
    }
}
