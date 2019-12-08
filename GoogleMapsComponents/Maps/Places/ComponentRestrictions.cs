using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OneOf;

namespace GoogleMapsComponents.Maps.Places
{
    public class ComponentRestrictions
    {
        public OneOf<string,string[]> Country { get; set; }
    }
}
