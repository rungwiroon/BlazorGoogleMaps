using Microsoft.JSInterop;
using GoogleMapsComponents.Maps;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;

namespace GoogleMapsComponents
{
    public class MapComponent : ComponentBase, IAsyncDisposable
    {
        [Inject]
        public IJSRuntime JsRuntime { get; protected set; } = default!;

        public Map InteropObject { get; private set; } = default!;

        public async Task InitAsync(ElementReference element, MapOptions? options = null)
        {
            InteropObject = await Map.CreateAsync(JsRuntime, element, options);
        }

        public ValueTask DisposeAsync()
        {
            return InteropObject.DisposeAsync();
        }
    }
}
