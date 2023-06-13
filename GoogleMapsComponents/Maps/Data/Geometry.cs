using System.Collections;
using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A superclass for the various geometry objects.
/// </summary>
public abstract class Geometry : IEnumerable<LatLngLiteral>
{
    /// <summary>
    /// Repeatedly invokes the given function, passing a point from the geometry to the function on each invocation.
    /// </summary>
    /// <returns></returns>
    public abstract IEnumerator<LatLngLiteral> GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}