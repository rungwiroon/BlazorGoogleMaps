using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MapEventListener
    {
        internal Guid Guid { get; set; }

        internal MapEventListener(Guid guid)
        {
            Guid = guid;
        }

        public Task Remove()
        {
            return MapEventJsInterop.UnsubscribeMapEvent(Guid.ToString());
        }
    }
}
