using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Places
{
    public class Autocomplete : MVCObject
    {
        public async static Task<Autocomplete> CreateAsync(IJSRuntime jsRuntime, ElementReference inputField, AutocompleteOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.Autocomplete", inputField, opts);
            //var obj = new Autocomplete(jsObjectRef);

            //return obj;

            throw new NotImplementedException();
        }

        internal Autocomplete(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Returns the bounds to which predictions are biased.
        /// </summary>
        public ValueTask<LatLngBoundsLiteral> GetBounds()
        {
            return InvokeAsync<LatLngBoundsLiteral>("getBounds");
        }

        /// <summary>
        /// Returns the fields to be included for the Place in the details response when the details
        /// are successfully retrieved. For a list of fields see PlaceResult.
        /// </summary>
        public ValueTask<string[]> GetFields()
        {
            return InvokeAsync<string[]>("getFields");
        }

        /// <summary>
        /// Returns the details of the Place selected by user if the details were successfully retrieved.
        /// Otherwise returns a stub Place object, with the name property set to the current value of the input field.
        /// </summary>
        public ValueTask<PlaceResult> GetPlace()
        {
            return InvokeAsync<PlaceResult>("getPlace");
        }

        /// <summary>
        /// Sets the preferred area within which to return Place results. Results are biased towards,
        /// but not restricted to, this area.
        /// </summary>
        public ValueTask SetBounds(LatLngBoundsLiteral bounds)
        {
            return this.InvokeVoidAsync("setBounds", bounds);
        }

        /// <summary>
        /// Sets the component restrictions. Component restrictions are used to restrict predictions to only those
        /// within the parent component. For example, the country.
        /// </summary>
        public ValueTask SetComponentRestrictions(ComponentRestrictions restrictions)
        {
            return this.InvokeVoidAsync("setComponentRestrictions", restrictions);
        }

        /// <summary>
        /// Sets the fields to be included for the Place in the details response when the details are successfully retrieved.
        /// For a list of fields see PlaceResult.
        /// </summary>
        public ValueTask SetFields(IEnumerable<string> fields)
        {
            return this.InvokeVoidAsync("setFields", fields);
        }

        public ValueTask SetOptions(AutocompleteOptions options)
        {
            return this.InvokeVoidAsync("setOptions", options);
        }

        /// <summary>
        /// Sets the types of predictions to be returned. For a list of supported types, see the developer's guide.
        /// If no type is specified, all types will be returned. The setTypes method accepts a single element array.
        /// </summary>>
        public ValueTask SetTypes(IEnumerable<string> types)
        {
            return this.InvokeVoidAsync("setTypes", types);
        }
    }
}
