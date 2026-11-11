using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Text.Json;

namespace GoogleMapsComponents;

public class JsCallableAction : IDisposable
{
    private readonly Delegate _delegate;
    private readonly Type[] _argumentTypes;
    private readonly IJSRuntime _jsRuntime;
    private DotNetObjectReference<JsCallableAction>? _reference;

    /// <summary>
    /// When true the callback releases itself after the first call. Used for listeners that fire only once.
    /// </summary>
    internal bool DisposeAfterInvoke { get; init; }

    public JsCallableAction(IJSRuntime jsRuntime, Delegate @delegate, params Type[] argumentTypes)
    {
        _jsRuntime = jsRuntime;
        _delegate = @delegate;
        _argumentTypes = argumentTypes;
    }

    /// <summary>
    /// The reference JavaScript uses to call back. It is created once and kept,
    /// so that <see cref="Dispose"/> can release it when the callback is not needed anymore.
    /// </summary>
    internal DotNetObjectReference<JsCallableAction> GetReference()
    {
        return _reference ??= DotNetObjectReference.Create(this);
    }

    /// <summary>
    /// Releases the reference held by the JS runtime. Without it the callback, and everything
    /// its delegate captures, stays alive for as long as the JS runtime does.
    /// </summary>
    public void Dispose()
    {
        _reference?.Dispose();
        _reference = null;
        GC.SuppressFinalize(this);
    }

    [JSInvokable]
    public void Invoke(string args, string guid)
    {
        try
        {
            InvokeDelegate(args, guid);
        }
        finally
        {
            if (DisposeAfterInvoke)
            {
                Dispose();
            }
        }
    }

    private void InvokeDelegate(string args, string guid)
    {
        if (string.IsNullOrWhiteSpace(args) || !_argumentTypes.Any())
        {
            _delegate.DynamicInvoke();
            return;
        }

        var rootElement = JsonDocument.Parse(args).RootElement;
        JsonElement.ArrayEnumerator jArray = rootElement.ValueKind == JsonValueKind.Array
            ? rootElement.EnumerateArray() : [];

        var arguments = _argumentTypes.Zip(jArray, (type, jToken) => new { jToken, type })
            .Select(x =>
            {
                var obj = Helper.DeSerializeObject(x.jToken, x.type);
                if (obj is IActionArgument actionArg)
                {
                    actionArg.JsObjectRef = new JsObjectRef(_jsRuntime, new Guid(guid));
                }
                if (obj is Maps.Data.MouseEvent featureEvent)
                {
                    //null any reference that might have come from the json since it won't be usable
                    featureEvent.Feature = null;

                    if(x.jToken.TryGetProperty("featureUUID", out var featureUuidProp)
				      && Guid.TryParse(featureUuidProp.GetString(), out Guid featureUuid))
                    {
					    var featureObjectRef = new JsObjectRef(_jsRuntime, featureUuid);
                        featureEvent.Feature = new Maps.Data.Feature(featureObjectRef);
                    }
                }

                return obj;
            })
            .ToArray();

        _delegate.DynamicInvoke(arguments);
    }
}