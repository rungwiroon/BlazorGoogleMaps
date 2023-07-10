using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

public class MapComponent : ComponentBase, IDisposable, IAsyncDisposable
{
    private bool isDisposed;

    [Inject]
    public IJSRuntime JsRuntime { get; protected set; } = default!;

    public Map InteropObject { get; private set; } = default!;

    public async Task InitAsync(ElementReference element, MapOptions? options = null)
    {
        InteropObject = await Map.CreateAsync(JsRuntime, element, options);
    }

   

    public async ValueTask DisposeAsync()
    {
        // Perform async cleanup.
        await DisposeAsyncCore();

        // Dispose of unmanaged resources.
        Dispose(false);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        //For some reasons this one is null when you refresh the page
        //Then second call it is not null and it being disposed
        if (InteropObject is not null)
            await InteropObject.DisposeAsync();

    }

    protected virtual void Dispose(bool disposing)
    {

        if (!isDisposed)
        {
            if (disposing)
            {
                InteropObject?.Dispose();                
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}