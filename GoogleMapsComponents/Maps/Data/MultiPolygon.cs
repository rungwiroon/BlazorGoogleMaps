using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A MultiPolygon geometry contains a number of Data.Polygon s.
/// </summary>
public class MultiPolygon : Geometry
{
    public IEnumerable<Polygon> _polygons;

    public MultiPolygon(IEnumerable<Polygon> elements)
    {
        _polygons = elements;
    }

    public MultiPolygon(IEnumerable<IEnumerable<LinearRing>> elements)
    {
        _polygons = elements
            .Select(e => new Polygon(e));
    }

    public MultiPolygon(IEnumerable<IEnumerable<IEnumerable<LatLngLiteral>>> elements)
    {
        _polygons = elements
            .Select(e => new Polygon(e.Select(ee => new LinearRing(ee))));
    }

    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        return _polygons
            .SelectMany(p => p)
            .GetEnumerator();
    }
}