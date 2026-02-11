using System.Collections.Generic;
using System.Linq;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// A MultiPolygon geometry contains a number of Data.Polygon s.
/// </summary>
public class MultiPolygon : Geometry
{
    /// <summary>
    /// Gets or sets the collection of polygons associated with this instance.
    /// </summary>
    /// <remarks>This field holds a collection of Polygon objects, which can be used to represent geometric
    /// shapes in a two-dimensional space. Modifications to this collection will affect the polygons
    /// represented.</remarks>
    public IEnumerable<Polygon> _polygons;

    /// <summary>
    /// Initializes a new instance of the MultiPolygon class with the specified collection of polygons.
    /// </summary>
    /// <remarks>The collection can contain any number of Polygon instances, including zero. If the collection
    /// is empty, the MultiPolygon will not contain any polygons.</remarks>
    /// <param name="elements">An enumerable collection of Polygon objects that represent the individual polygons to be included in the
    /// MultiPolygon. Cannot be null.</param>
    public MultiPolygon(IEnumerable<Polygon> elements)
    {
        _polygons = elements;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="elements"></param>
    public MultiPolygon(IEnumerable<IEnumerable<LinearRing>> elements)
    {
        _polygons = elements
            .Select(e => new Polygon(e));
    }

    /// <summary>
    /// A MultiPolygon can also be specified using an array of arrays of arrays of LatLngs. 
    /// </summary>
    /// <param name="elements"></param>
    public MultiPolygon(IEnumerable<IEnumerable<IEnumerable<LatLngLiteral>>> elements)
    {
        _polygons = elements
            .Select(e => new Polygon(e.Select(ee => new LinearRing(ee))));
    }


    /// <summary>
    /// Returns an enumerator that iterates through all latitude and longitude points contained within the collection of
    /// polygons.
    /// </summary>
    /// <remarks>This method flattens the collection of polygons into a single sequence, allowing iteration
    /// over all points in one pass.</remarks>
    /// <returns>An enumerator that provides access to each latitude and longitude point in the sequence of polygons.</returns>
    public override IEnumerator<LatLngLiteral> GetEnumerator()
    {
        return _polygons
            .SelectMany(p => p)
            .GetEnumerator();
    }
}