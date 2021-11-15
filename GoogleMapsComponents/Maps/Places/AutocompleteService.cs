using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps.Places
{
    public class AutocompleteService: IAsyncDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        public async static Task<AutocompleteService> CreateAsync(IJSRuntime jsRuntime, MarkerOptions opts = null)
        {
            //var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.places.AutocompleteService", opts);
            //var obj = new AutocompleteService(jsObjectRef);

            //return obj;

            throw new NotImplementedException();
        }

        private AutocompleteService(JsObjectRef jsObjectRef)
        {
            _jsObjectRef = jsObjectRef;
        }

        public ValueTask DisposeAsync()
        {
            return _jsObjectRef.DisposeAsync();
        }
    }
}
