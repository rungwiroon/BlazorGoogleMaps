using Microsoft.JSInterop;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

/// <summary>
/// Helpers for JS interop calls made while disposing.
/// </summary>
internal static class JsInteropDisposal
{
    /// <summary>
    /// Runs a JS interop call made during disposal, ignoring the failures that mean the browser side is
    /// already gone or unreachable: <see cref="JSDisconnectedException"/> (Blazor Server circuit disconnected)
    /// and <see cref="OperationCanceledException"/> (the browser did not answer in time, e.g. a suspended
    /// mobile tab). In both cases there is nothing left to clean up on the JS side, and letting the exception
    /// escape from a disposal terminates the circuit. Any other exception is propagated.
    /// </summary>
    internal static async ValueTask SwallowAsync(Func<ValueTask> jsCall)
    {
        try
        {
            await jsCall();
        }
        catch (JSDisconnectedException)
        {
            // Blazor Server: the circuit is gone, and the browser-side objects with it
        }
        catch (OperationCanceledException)
        {
            // Blazor Server: the browser didn't answer in time (e.g. a suspended mobile tab)
        }
    }
}
