using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places;

public class Autocomplete : EventEntityBase
{
    public static async Task<Autocomplete> CreateAsync(IJSRuntime jsRuntime, ElementReference inputField, AutocompleteOptions? opts = null)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.Autocomplete", inputField, opts);
        var obj = new Autocomplete(jsObjectRef);

        return obj;
    }

    private Autocomplete(JsObjectRef jsObjectRef) : base(jsObjectRef)
    {
    }

    /// <summary>
    /// Returns the bounds to which predictions are biased.
    /// </summary>
    public Task<LatLngBoundsLiteral> GetBounds()
    {
        return _jsObjectRef.InvokeAsync<LatLngBoundsLiteral>("getBounds");
    }

    /// <summary>
    /// Returns the fields to be included for the Place in the details response when the details
    /// are successfully retrieved. For a list of fields see PlaceResult.
    /// </summary>
    public Task<IEnumerable<string>> GetFields()
    {
        return _jsObjectRef.InvokeAsync<IEnumerable<string>>("getFields");
    }

    /// <summary>
    /// Returns the details of the Place selected by user if the details were successfully retrieved.
    /// Otherwise returns a stub Place object, with the name property set to the current value of the input field.
    /// </summary>
    public Task<PlaceResult> GetPlace()
    {
        return _jsObjectRef.InvokeAsync<PlaceResult>("getPlace");
    }

    /// <summary>
    /// Sets the preferred area within which to return Place results. Results are biased towards,
    /// but not restricted to, this area.
    /// </summary>
    public Task SetBounds(LatLngBoundsLiteral bounds)
    {
        return _jsObjectRef.InvokeAsync("setBounds", bounds);
    }

    /// <summary>
    /// Sets the component restrictions. Component restrictions are used to restrict predictions to only those
    /// within the parent component. For example, the country.
    /// </summary>
    public Task SetComponentRestrictions(ComponentRestrictions restrictions)
    {
        return _jsObjectRef.InvokeAsync("setComponentRestrictions", restrictions);
    }

    /// <summary>
    /// Sets the fields to be included for the Place in the details response when the details are successfully retrieved.
    /// For a list of fields see PlaceResult.
    /// </summary>
    public Task SetFields(IEnumerable<string> fields)
    {
        return _jsObjectRef.InvokeAsync("setFields", fields);
    }

    public Task SetOptions(AutocompleteOptions options)
    {
        return _jsObjectRef.InvokeAsync("setOptions", options);
    }

    /// <summary>
    /// Sets the types of predictions to be returned. For a list of supported types, see the developer's guide.
    /// If no type is specified, all types will be returned. The setTypes method accepts a single element array.
    /// </summary>>
    public Task SetTypes(IEnumerable<string> types)
    {
        return _jsObjectRef.InvokeAsync("setTypes", types);
    }
}