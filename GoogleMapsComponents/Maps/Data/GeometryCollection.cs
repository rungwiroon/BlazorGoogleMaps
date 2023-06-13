using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A GeometryCollection contains a number of geometry objects. 
/// Any LatLng or LatLngLiteral objects are automatically converted to Data.Point geometry objects.
/// </summary>
public class GeometryCollection : Geometry
{
    public IEnumerable<Geometry> _geometries;

    public GeometryCollection(IEnumerable<Geometry> elements)
    {
        _geometries = elements;
    }

    public GeometryCollection(IEnumerable<LatLngLiteral> elements)
    {
        _geometries = elements
            .Select(e => new Point(e));
    }

    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        return _geometries
            .SelectMany(g => g)
            .GetEnumerator();
    }
}