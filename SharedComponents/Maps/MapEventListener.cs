using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    public class MapEventListener
    {
        internal Guid Guid { get; set; }

        internal MapEventListener(Guid guid)
        {
            Guid = guid;
        }

        public void Remove()
        {
            throw new NotImplementedException();
        }
    }
}
