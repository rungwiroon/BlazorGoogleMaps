using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// google.maps.MapsEventListener interface
/// An event listener, created by google.maps.event.addListener() and friends.
/// </summary>
public class MapEventListener : IJsObjectRef, IDisposable, IAsyncDisposable
{
    private readonly JsObjectRef _jsObjectRef;
    private IDisposable? _callback;
    public bool IsRemoved;
    private bool isDisposed;

    internal MapEventListener(JsObjectRef jsObjectRef, IDisposable? callback = null)
    {
        _jsObjectRef = jsObjectRef;
        _callback = callback;
    }

    /// <summary>
    /// Guid of the underlying jsObjectRef
    /// </summary>
    public Guid Guid => _jsObjectRef.Guid;
    public async Task RemoveAsync()
    {
        await _jsObjectRef.InvokeAsync("remove");
        await _jsObjectRef.DisposeAsync();
        IsRemoved = true;

        //The listener can not fire anymore, so the .NET callback behind it is released as well
        _callback?.Dispose();
        _callback = null;
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

    protected virtual async ValueTask DisposeAsyncCore() => await RemoveAsync();

    protected virtual void Dispose(bool disposing)
    {

        if (!isDisposed)
        {

            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
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