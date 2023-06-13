using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A MultiLineString geometry contains a number of LineString s.
/// </summary>
public class MultiPoint : Geometry
{
    private readonly IEnumerable<LatLngLiteral> _elements;

    public MultiPoint(IEnumerable<LatLngLiteral> elements)
    {
        _elements = elements;
    }

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