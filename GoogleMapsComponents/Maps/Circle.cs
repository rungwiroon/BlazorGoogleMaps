using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A circle on the Earth's surface; also known as a "spherical cap".
    /// </summary>
    public class Circle : JsObjectRef
    {
        /// <summary>
        /// Create a circle using the passed CircleOptions, which specify the center, radius, and style.
        /// </summary>
        /// <param name="opts"></param>
        public async static Task<Circle> CreateAsync(IJSRuntime jsRuntime, CircleOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.Circle", opts);
            //var obj = new Circle(jsObjectRef);
            //return obj;

            throw new NotImplementedException();
        }

        internal Circle(JsObjectRef jsObjectRef)
            :base(jsObjectRef)
        {
        }        

        /// <summary>
        /// Gets the LatLngBounds of this Circle.
        /// </summary>
        /// <returns></returns>
        public ValueTask<LatLngBoundsLiteral> GetBounds()
        {
            return InvokeAsync<LatLngBoundsLiteral>("getBounds");
        }

        /// <summary>
        /// Returns the center of this circle.
        /// </summary>
        /// <returns></returns>
        public ValueTask<LatLngLiteral> GetCenter()
        {
            return InvokeAsync<LatLngLiteral>("getCenter");
        }

        /// <summary>
        /// Returns whether this circle can be dragged by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetDraggable()
        {
            return InvokeAsync<bool>("getDraggable");
        }

        /// <summary>
        /// Returns whether this circle can be edited by the user.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetEditable()
        {
            return InvokeAsync<bool>("getEditable");
        }        

        /// <summary>
        /// Returns the radius of this circle (in meters).
        /// </summary>
        /// <returns></returns>
        public ValueTask<double> GetRadius()
        {
            return InvokeAsync<double>("getRadius");
        }

        /// <summary>
        /// Returns whether this circle is visible on the map.
        /// </summary>
        /// <returns></returns>
        public ValueTask<bool> GetVisible()
        {
            return InvokeAsync<bool>("getVisible");
        }

        /// <summary>
        /// Sets the center of this circle.
        /// </summary>
        /// <param name="center"></param>
        public ValueTask SetCenter(LatLngLiteral center)
        {
            return InvokeVoidAsync("setCenter", center);
        }

        /// <summary>
        /// If set to true, the user can drag this circle over the map.
        /// </summary>
        /// <param name="draggable"></param>
        public ValueTask SetDraggable(bool draggable)
        {
            return InvokeVoidAsync("setDraggable", draggable);
        }

        /// <summary>
        /// If set to true, the user can edit this circle by dragging the control points shown at the center and around the circumference of the circle.
        /// </summary>
        /// <param name="editable"></param>
        public ValueTask SetEditable(bool editable)
        {
            return InvokeVoidAsync("setEditable", editable);
        }        

        public ValueTask SetOptions(CircleOptions options)
        {
            return InvokeVoidAsync("setOptions", options);
        }

        /// <summary>
        /// Sets the radius of this circle (in meters).
        /// </summary>
        /// <param name="radius"></param>
        public ValueTask SetRadius(double radius)
        {
            return InvokeVoidAsync("setRadius", radius);
        }

        /// <summary>
        /// Hides this circle if set to false.
        /// </summary>
        /// <param name="visible"></param>
        public ValueTask SetVisible(bool visible)
        {
            return InvokeVoidAsync("setVisible", visible);
        }        
    }
}
