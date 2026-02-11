using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A MultiLineString geometry contains a number of LineString s.
/// </summary>
public class MultiPoint : Geometry
{
    private readonly IEnumerable<LatLngLiteral> _elements;

    /// <summary>
    /// Initializes a new instance of the MultiPoint class using the specified collection of latitude and longitude
    /// points.
    /// </summary>
    /// <param name="elements">The collection of LatLngLiteral objects that define the points of the MultiPoint. This parameter cannot be null
    /// or empty.</param>
    public MultiPoint(IEnumerable<LatLngLiteral> elements)
    {
        _elements = elements;
    }

    /// <summary>
    /// GetEnumerator
    /// </summary>
    /// <returns></returns>
    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    /// <summary>
    /// Returns an array of the contained LatLngs. 
    /// A new array is returned each time getArray() is called.
    /// </summary>
    /// <returns></returns>
    public List<LatLngLiteral> GetArray()
    {
        return _elements.ToList();
    }
}