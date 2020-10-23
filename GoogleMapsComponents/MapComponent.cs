using Microsoft.JSInterop;
using GoogleMapsComponents.Maps;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

#nullable enable

namespace GoogleMapsComponents
{
    //[Obsolete] //<-- No Idea why this is here.
    public class MapComponent : ComponentBase, IDisposable
    {
        [Inject]
        public IJSRuntime JsRuntime { get; protected set; } = default!;

        public Map InteropObject { get; private set; } = default!;

        public async Task InitAsync(ElementReference element, MapOptions? options = null)
        {
            InteropObject = await Map.CreateAsync(JsRuntime, element, options);
        }

        public void Dispose()
        {
            InteropObject?.Dispose();
        }
    }
}
