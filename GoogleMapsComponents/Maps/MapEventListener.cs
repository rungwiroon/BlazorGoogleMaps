using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MapEventListener : JsObjectRef
    {
        protected MapEventJsInterop _mapEventJsInterop;

        internal MapEventListener(IJSRuntime jsRuntime, MapEventJsInterop mapEventJsInterop, Guid guid)
            : base(jsRuntime, guid)
        {
            _mapEventJsInterop = mapEventJsInterop;
        }

        public override void Dispose()
        {
            Remove();
        }

        public Task Remove()
        {
            return _mapEventJsInterop.UnsubscribeMapEvent(Guid.ToString());
        }
    }
}
