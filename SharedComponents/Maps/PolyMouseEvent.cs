using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// This object is returned from mouse events on polylines and polygons.
    /// </summary>
    public class PolyMouseEvent : MouseEventArgs
    {
        public PolyMouseEvent(string id) 
            //: base(id)
        {
        }
    }
}
