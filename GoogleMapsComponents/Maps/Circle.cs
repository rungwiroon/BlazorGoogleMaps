using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A circle on the Earth's surface; also known as a "spherical cap".
    /// </summary>
    public class Circle : IDisposable, IJsObjectRef
    {
        private Map _map;
        private readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, List<MapEventListener>> EventListeners;

        public Guid Guid => _jsObjectRef.Guid;

        /// <summary>
        /// Create a circle using the passed CircleOptions, which specify the center, radius, and style.
        /// </summary>
        /// <param name="opts"></param>
        public async static Task<Circle> CreateAsync(IJSRuntime jsRuntime, CircleOptions opts = null)
        {
            var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Circle", opts);

            var obj = new Circle(jsObjectRef, opts);

            return obj;
        }

        internal Circle(JsObjectRef jsObjectRef, CircleOptions opts = null)
        {
            _jsObjectRef = jsObjectRef;

            if (opts != null)
            {
                _map = opts.Map;
            }

            EventListeners = new Dictionary<string, List<MapEventListener>>();
        }

        public void Dispose()
        {
            foreach (string key in EventListeners.Keys)
            {
                //Probably superfluous...
                if (EventListeners[key] != null)
                {
                    foreach (MapEventListener eventListener in EventListeners[key])
                    {
                        eventListener.Dispose();
                    }

                    EventListeners[key].Clear();
                }
            }

            EventListeners.Clear();

            _jsObjectRef.Dispose();
        }

        /// <summary>
        /// Gets the LatLngBounds of this Circle.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngBoundsLiteral> GetBounds()
        {
            return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>("getBounds");
        }

        /// <summary>
        /// Returns the center of this circle.
        /// </summary>
        /// <returns></returns>
        public Task<LatLngLiteral> GetCenter()
        {
            return _jsObjectRef.InvokeAsync<LatLngLiteral>("getCenter");
        }

        /// <summary>
        /// Returns whether this circle can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetDraggable()
        {
            return _jsObjectRef.InvokeAsync<bool>("getDraggable");
        }

        /// <summary>
        /// Returns whether this circle can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetEditable()
        {
            return _jsObjectRef.InvokeAsync<bool>("getEditable");
        }

        /// <summary>
        /// Returns the map on which this circle is displayed.
        /// </summary>
        /// <returns></returns>
        public Map GetMap()
        {
            return _map;
        }

        /// <summary>
        /// Returns the radius of this circle (in meters).
        /// </summary>
        /// <returns></returns>
        public Task<double> GetRadius()
        {
            return _jsObjectRef.InvokeAsync<double>("getRadius");
        }

        /// <summary>
        /// Returns whether this circle is visible on the map.
        /// </summary>
        /// <returns></returns>
        public Task<bool> GetVisible()
        {
            return _jsObjectRef.InvokeAsync<bool>("getVisible");
        }

        /// <summary>
        /// Sets the center of this circle.
        /// </summary>
        /// <param name="center"></param>
        public Task SetCenter(LatLngLiteral center)
        {
            return _jsObjectRef.InvokeAsync("setCenter", center);
        }

        /// <summary>
        /// If set to true, the user can drag this circle over the map.
        /// </summary>
        /// <param name="draggable"></param>
        public Task SetDraggable(bool draggable)
        {
            return _jsObjectRef.InvokeAsync("setDraggable", draggable);
        }

        /// <summary>
        /// If set to true, the user can edit this circle by dragging the control points shown at the center and around the circumference of the circle.
        /// </summary>
        /// <param name="editable"></param>
        public Task SetEditable(bool editable)
        {
            return _jsObjectRef.InvokeAsync("setEditable", editable);
        }

        /// <summary>
        /// Renders the circle on the specified map. If map is set to null, the circle will be removed.
        /// </summary>
        /// <param name="map"></param>
        public Task SetMap(Map map)
        {
            _map = map;

            return _jsObjectRef.InvokeAsync(
                "setMap",
                map);
        }

        public Task SetOptions(CircleOptions options)
        {
            return _jsObjectRef.InvokeAsync("setOptions", options);
        }

        /// <summary>
        /// Sets the radius of this circle (in meters).
        /// </summary>
        /// <param name="radius"></param>
        public Task SetRadius(double radius)
        {
            return _jsObjectRef.InvokeAsync("setRadius", radius);
        }

        /// <summary>
        /// Hides this circle if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public Task SetVisible(bool visible)
        {
            return _jsObjectRef.InvokeAsync("setVisible", visible);
        }

        public async Task<MapEventListener> AddListener(string eventName, Action handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }
            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public async Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
        {
            var listenerRef = await _jsObjectRef.InvokeWithReturnedObjectRefAsync(
                "addListener", eventName, handler);
            MapEventListener eventListener = new MapEventListener(listenerRef);

            if (!EventListeners.ContainsKey(eventName))
            {
                EventListeners.Add(eventName, new List<MapEventListener>());
            }
            EventListeners[eventName].Add(eventListener);

            return eventListener;
        }

        public async Task ClearListeners(string eventName)
        {
            if (EventListeners.ContainsKey(eventName))
            {
                await _jsObjectRef.InvokeAsync("clearListeners", eventName);

                //IMHO is better preserving the knowledge that Marker had some EventListeners attached to "eventName" in the past
                //so, instead to clear the list and remove the key from dictionary, I prefer to leave the key with an empty list
                EventListeners[eventName].Clear();
            }
        }
    }
}
