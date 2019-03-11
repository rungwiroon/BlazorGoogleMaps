using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Data
{
    public class MultiLineString : Geometry
    {
        private readonly IEnumerable<LineString> _elements;

        public MultiLineString(IEnumerable<LineString> elements)
        {
            _elements = elements;
        }

        public MultiLineString(IEnumerable<IEnumerable<LatLngLiteral>> elements)
        {
            _elements = elements
                .Select(e => new LineString(e));
        }

        public override IEnumerator<LatLngLiteral> GetEnumerator()
        {
            return _elements
                .SelectMany(e => e)
                .GetEnumerator();
        }
    }
}
