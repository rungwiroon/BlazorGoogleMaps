using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class DirectionResponse
    {
        public DirectionsResult Response { get; set; }

        public DirectionsStatus Status { get; set; }
    }
}
