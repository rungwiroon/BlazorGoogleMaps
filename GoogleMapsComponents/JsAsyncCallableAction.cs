using GoogleMapsComponents.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

/// <summary>
/// A callable eventcallback from JavaScript. This doesn't accept any arguments.
/// </summary>
public class JsAsyncCallableAction
{
    private readonly EventCallback eventCallback;

    public JsAsyncCallableAction(EventCallback eventCallback)
    {
        this.eventCallback = eventCallback;
    }

    [JSInvokable]
    public Task Invoke(string args, string guid)
    {
        return eventCallback.InvokeAsync();
    }
}

public class JsAsyncCallableAction<T>
{
    private readonly EventCallback<T> eventCallback;
    private readonly IJSRuntime jsRuntime;
    public JsAsyncCallableAction(IJSRuntime jsRuntime, EventCallback<T> eventCallback)
    {
        this.jsRuntime = jsRuntime;
        this.eventCallback = eventCallback;
    }

    [JSInvokable]
    public Task Invoke(string args, string guid)
    {

        var jElement = JsonDocument.Parse(args)
            .RootElement
            .EnumerateArray()
            .FirstOrDefault();

        var obj = JsonExtensions.DeSerializeObject<T>(jElement);
        if (obj is IActionArgument actionArg)
        {
            actionArg.JsObjectRef = new JsObjectRef(jsRuntime, new Guid(guid));
        }
        return eventCallback.InvokeAsync(obj);
    }
}