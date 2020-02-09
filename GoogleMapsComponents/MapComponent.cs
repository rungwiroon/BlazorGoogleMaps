using Microsoft.JSInterop;
using GoogleMapsComponents.Maps;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoogleMapsComponents
{
    //[Obsolete] //<-- No Idea why this is here.
    public class MapComponent : ComponentBase, IDisposable
    {
        [Inject]
        public IJSRuntime JsRuntime { get; protected set; }

        public Map InteropObject { get; private set; }

        public async Task InitAsync(ElementReference element, MapOptions options = null)
        {
            InteropObject = await Map.CreateAsync(JsRuntime, element, options);
        }

        public void Dispose()
        {
            InteropObject?.Dispose();
        }
    }
}
