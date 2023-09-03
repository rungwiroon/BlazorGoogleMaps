using OneOf;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A feature has a geometry, an id, and a set of properties
/// https://developers.google.com/maps/documentation/javascript/reference/data#Data.Feature.constructor
/// </summary>
public class Feature : IJsObjectRef
{
    private readonly JsObjectRef _jsObjectRef;
    public Guid Guid => _jsObjectRef.Guid;

    /// <summary>
    /// Needed for json serializations
    /// </summary>
    public Feature(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    public Feature()
    {

    }

    public IEnumerable<IDictionary<string, object>>? Properties { get; set; }

    /// <summary>
    /// Repeatedly invokes the given function, passing a property value and name on each invocation. 
    /// The order of iteration through the properties is undefined.
    /// </summary>
    /// <returns></returns>
    public IEnumerator<string> GetEnumerator()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the feature's geometry.
    /// </summary>
    /// <returns></returns>
    public Geometry GetGeometry()
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Returns the feature ID.
    /// </summary>
    /// <returns></returns>
    public async Task<int> GetId()
    {
        var result = await _jsObjectRef.InvokeAsync<int>("getId");
        return result;
    }

    /// <summary>
    /// Returns the value of the requested property, or undefined if the property does not exist.
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public object GetProperty(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Removes the property with the given name.
    /// </summary>
    /// <param name="name"></param>
    public void RemoveProperty(string name)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the feature's geometry.
    /// </summary>
    /// <param name="newGeometry"></param>
    public void SetGeometry(OneOf<Geometry, LatLngLiteral> newGeometry)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Sets the value of the specified property. 
    /// If newValue is undefined this is equivalent to calling removeProperty.
    /// </summary>
    /// <param name="name"></param>
    /// <param name="newValue"></param>
    public void SetProperty(string name, object newValue)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Exports the feature to a GeoJSON object.
    /// </summary>
    /// <returns></returns>
    public Task<object> ToGeoJson()
    {
        throw new NotImplementedException();
    }

    //IEnumerator IEnumerable.GetEnumerator()
    //{
    //    foreach (var keyValuePair in Properties)
    //    {
    //        yield return JsonConvert.SerializeObject(keyValuePair).Replace("\\\\", "");
    //    }
    //}

}