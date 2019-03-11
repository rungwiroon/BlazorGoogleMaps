using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    /// <summary>
    /// A Polygon geometry contains a number of Data.LinearRings. 
    /// The first linear-ring must be the polygon exterior boundary and subsequent linear-rings must be interior boundaries, also known as holes. 
    /// See the sample polygon with a hole.
    /// </summary>
    public class Polygon : Geometry
    {
        public IEnumerable<LinearRing> _linerRings;

        public Polygon(IEnumerable<LinearRing> elements)
        {
            _linerRings = elements;
        }

        public Polygon(IEnumerable<IEnumerable<LatLngLiteral>> elements)
        {
            _linerRings = elements
                .Select(e => new LinearRing(e));
        }

        public override IEnumerator<LatLngLiteral> GetEnumerator()
        {
            return _linerRings
                .SelectMany(lr => lr)
                .GetEnumerator();
        }
    }
}
