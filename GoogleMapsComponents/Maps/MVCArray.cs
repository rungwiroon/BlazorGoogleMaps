using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MVCArray<T> : MVCObject
    {
        internal MVCArray(IJSObjectReference jsObjectRef) : base(jsObjectRef)
        {
        }
    }
}
