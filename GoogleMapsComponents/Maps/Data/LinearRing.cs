using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A LinearRing geometry contains a number of LatLngs, representing a closed LineString. 
/// There is no need to make the first LatLng equal to the last LatLng. The LinearRing is closed implicitly.
/// </summary>
public class LinearRing : Geometry
{
    private IEnumerable<LatLngLiteral> _elements;

    public LinearRing(IEnumerable<LatLngLiteral> elements)
    {
        _elements = elements;
    }

    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        return _elements.GetEnumerator();
    }
}