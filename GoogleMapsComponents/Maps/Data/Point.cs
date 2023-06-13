using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A Point geometry contains a single LatLng.
/// </summary>
public class Point : Geometry
{
    private readonly LatLngLiteral _latLng;

    public Point(LatLngLiteral latLng)
    {
        _latLng = latLng;
    }

    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        yield return _latLng;
    }

    /// <summary>
    /// Returns the contained LatLng.
    /// </summary>
    /// <returns></returns>
    public LatLngLiteral Get()
    {
        return _latLng;
    }
}