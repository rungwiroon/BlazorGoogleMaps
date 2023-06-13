using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A LinearRing geometry contains a number of LatLngs, representing a closed LineString. 
/// There is no need to make the first LatLng equal to the last LatLng. 
/// The LinearRing is closed implicitly.
/// </summary>
public class LineString : Geometry
{
    private readonly IEnumerable<LatLngLiteral> _elements;

    public LineString(IEnumerable<LatLngLiteral> elements)
    {
        _elements = elements;
    }

    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }

    public List<LatLngLiteral> GetArray()
    {
        return _elements.ToList();
    }
}