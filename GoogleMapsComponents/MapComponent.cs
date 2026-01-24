using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

public class MapComponent : ComponentBase, IDisposable, IAsyncDisposable
{
    private bool _isDisposed;

    [Inject]
    public IJSRuntime JsRuntime { get; protected set; } = default!;
    [Inject]
    public IServiceProvider ServiceProvider { get; protected set; } = default!;

    private IBlazorGoogleMapsKeyService? _keyService;
    internal event EventHandler? MapInitialized;

    protected override void OnInitialized()
    {
        // get the service from the provider instead of with [Inject] in case no 
        // service was registered. e.g. when the user loads the api with a script tag.
        _keyService = ServiceProvider.GetService<IBlazorGoogleMapsKeyService>();
        base.OnInitialized();
    }

    public Map InteropObject { get; private set; } = default!;

    public async Task InitAsync(ElementReference element, MapOptions? options = null)
    {
        MapApiLoadOptions? loadedApiOptions = options?.ApiLoadOptions;
        if (options?.ApiLoadOptions == null && _keyService != null)
        {
            bool isGoogleReady = false;
            try
            {
                isGoogleReady = await JsRuntime.InvokeAsync<bool>("blazorGoogleMaps.objectManager.isGoogleMapsReady");
            }
            catch
            {
                // Ignore JS exceptions; we'll try loading if needed.
            }

            if (!_keyService.IsApiInitialized || !isGoogleReady)
            {
                _keyService.IsApiInitialized = true;
                options ??= new MapOptions();
                loadedApiOptions = await _keyService.GetApiOptions();
                options.ApiLoadOptions = loadedApiOptions;
            }
        }

        if (options != null && string.IsNullOrWhiteSpace(options.MapId))
        {
            // If styles are set, avoid auto-applying MapId to prevent conflicts.
            if (options.Styles != null && options.Styles.Length > 0)
            {
                InteropObject = await Map.CreateAsync(JsRuntime, element, options);
                MapInitialized?.Invoke(this, EventArgs.Empty);
                return;
            }

            var mapIds = loadedApiOptions?.MapIds;
            if (mapIds == null && _keyService != null)
            {
                try
                {
                    mapIds = (await _keyService.GetApiOptions()).MapIds;
                }
                catch
                {
                    mapIds = null;
                }
            }
            if (mapIds != null && mapIds.Length == 1 && !string.IsNullOrWhiteSpace(mapIds[0]))
            {
                options.MapId = mapIds[0];
            }
        }

        InteropObject = await Map.CreateAsync(JsRuntime, element, options);
        MapInitialized?.Invoke(this, EventArgs.Empty);
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
        if (InteropObject is not null)
        {
            try
            {
                await InteropObject.DisposeAsync();
                InteropObject = null;
            }
            catch (Exception ex)
            {
                var isPossibleRefreshError = ex.HasInnerExceptionsOfType<TaskCanceledException>();
                isPossibleRefreshError |= ex.HasInnerExceptionsOfType<ObjectDisposedException>();
                //Unfortunately, JSDisconnectedException is available in dotnet >= 6.0, and not in dotnet standard.
                isPossibleRefreshError |= true;
                //If we get an exception here, we can assume that the page was refreshed. So assentialy, we swallow all exception here...
                //isPossibleRefreshError = isPossibleRefreshError || ex.HasInnerExceptionsOfType<JSDisconnectedException>();


                if (!isPossibleRefreshError)
                {
                    throw;
                }
            }
        }
    }


    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                InteropObject?.Dispose();
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}
