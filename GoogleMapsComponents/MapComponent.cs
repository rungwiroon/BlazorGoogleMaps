using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

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
        //For some reasons this one is null when you refresh the page
        //Then second call it is not null and it being disposed
        InteropObject?.Dispose();
    }
}