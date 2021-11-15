using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    public class MapComponent : ComponentBase, IAsyncDisposable
    {
        [Inject]
        public IJSRuntime JsRuntime { get; protected set; } = default!;

        public ObjectManager ObjectManager { get; private set; } = default!;

        public Map InteropObject { get; private set; } = default!;

        public async Task InitAsync(ElementReference element, MapOptions? options = null)
        {
            ObjectManager = new ObjectManager(JsRuntime);
            InteropObject = await Map.CreateAsync(JsRuntime, element, options);
        }

        public async ValueTask DisposeAsync()
        {
            await InteropObject.DisposeAsync();
        }
    }
}
