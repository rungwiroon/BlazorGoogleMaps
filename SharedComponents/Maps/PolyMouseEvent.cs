using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// This object is returned from mouse events on polylines and polygons.
    /// </summary>
    public class PolyMouseEvent : MouseEvent
    {
        public PolyMouseEvent(Guid guid) : base(guid)
        {
        }
    }
}
