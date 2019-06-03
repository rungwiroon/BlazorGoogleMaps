using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public class MapEventListener : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        internal MapEventListener(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
        }

        public void Dispose()
        {
            _ = RemoveAsync();
        }

        public async Task RemoveAsync()
        {
            await _jsObjectRef.InvokeAsync<object>("remove");
            await _jsObjectRef.DisposeAsync();
        }
    }
}
