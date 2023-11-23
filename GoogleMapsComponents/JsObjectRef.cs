using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents;

internal class JsObjectRef1 : IJsObjectRef
{
    protected Guid _guid;

    public Guid Guid
    {
        get { return _guid; }
    }

    public string GuidString
    {
        get { return _guid.ToString(); }
    }

    public JsObjectRef1(Guid guid)
    {
        _guid = guid;
    }

    [JsonConstructor]
    public JsObjectRef1(string guidString)
    {
        _guid = new Guid(guidString);
    }

    public override bool Equals(object obj)
    {
        var other = obj as JsObjectRef;

        if (other == null)
        {
            return false;
        }
        else
        {
            return other.Guid == _guid;
        }
    }

    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }
}

public class JsObjectRef : IJsObjectRef, IDisposable
{
    protected readonly Guid _guid;
    protected readonly IJSRuntime _jsRuntime;

    public Guid Guid
    {
        get { return _guid; }
    }

    public IJSRuntime JSRuntime
    {
        get { return _jsRuntime; }
    }

    public JsObjectRef(
        IJSRuntime jsRuntime,
        Guid guid)
    {
        _jsRuntime = jsRuntime;
        _guid = guid;
    }

    public static Task<JsObjectRef> CreateAsync(
        IJSRuntime jsRuntime,
        string constructorFunctionName,
        params object?[] args)
    {
        return CreateAsync(jsRuntime, Guid.NewGuid(), constructorFunctionName, args);
    }

    public static async Task<Dictionary<string, JsObjectRef>> CreateMultipleAsync(
        IJSRuntime jsRuntime,
        string constructorFunctionName,
        Dictionary<string, object> args)
    {
        Dictionary<string, Guid> internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        Dictionary<Guid, object> dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
            jsRuntime,
            constructorFunctionName,
            dictArgs);

        return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);
    }

    public async Task<Dictionary<string, JsObjectRef>> AddMultipleAsync(
        string constructorFunctionName,
        Dictionary<string, object> args)
    {
        Dictionary<string, Guid> internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        Dictionary<Guid, object> dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
            _jsRuntime,
            constructorFunctionName,
            dictArgs);

        return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);
    }

    public async static Task<JsObjectRef> CreateAsync(
        IJSRuntime jsRuntime,
        Guid guid,
        string functionName,
        params object?[] args)
    {
        var jsObjectRef = new JsObjectRef(jsRuntime, guid);

        await jsRuntime.MyInvokeAsync<object>(
            "blazorGoogleMaps.objectManager.createObject",
            new object[] { guid.ToString(), functionName }
                .Concat(args).ToArray()
        );

        return jsObjectRef;
    }

    public async static Task<Dictionary<Guid, JsObjectRef>> CreateMultipleAsync(
        IJSRuntime jsRuntime,
        string functionName,
        Dictionary<Guid, object> dictArgs)
    {
        Dictionary<Guid, JsObjectRef> jsObjectRefs = dictArgs.ToDictionary(e => e.Key, e => new JsObjectRef(jsRuntime, e.Key));

        await jsRuntime.MyInvokeAsync<object>(
            "blazorGoogleMaps.objectManager.createMultipleObject",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );

        return jsObjectRefs;
    }

    public virtual void Dispose()
    {
        DisposeAsync();
    }

    public ValueTask<object> DisposeAsync()
    {
        return _jsRuntime.InvokeAsync<object>(
            "blazorGoogleMaps.objectManager.disposeObject",
            _guid.ToString()
        );
    }

    public ValueTask<object> DisposeMultipleAsync(List<Guid> guids)
    {
        return _jsRuntime.InvokeAsync<object>(
            "blazorGoogleMaps.objectManager.disposeMultipleObjects",
            guids.Select(e => e.ToString()).ToList()
        );
    }

    public async Task InvokeAsync(string functionName, params object?[] args)
    {
        await _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invoke",
            new object?[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    public Task InvokeMultipleAsync(string functionName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invokeMultiple",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    public Task AddMultipleListenersAsync(string eventName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyAddListenerAsync(
            "blazorGoogleMaps.objectManager.addMultipleListeners",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), eventName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    public Task<T> InvokeAsync<T>(string functionName, params object?[] args)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    public Task<Dictionary<string, T>> InvokeMultipleAsync<T>(string functionName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyInvokeAsync<Dictionary<string, T>>(
            "blazorGoogleMaps.objectManager.invokeMultiple",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    /// <summary>
    /// Use when returned result will be one of defined types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <param name="functionName"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    public async Task<OneOf<T, U>> InvokeAsync<T, U>(string functionName, params object[] args)
    {
        var result = await _jsRuntime.MyInvokeAsync<T, U>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );

        return result;
    }

    /// <summary>
    /// Use when returned result will be one of defined types
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="U"></typeparam>
    /// <typeparam name="V"></typeparam>
    /// <param name="functionName"></param>
    /// <param name="args"></param>
    /// <returns>Discriminated union of specified types</returns>
    public Task<OneOf<T, U, V>> InvokeAsync<T, U, V>(string functionName, params object[] args)
    {
        return _jsRuntime.MyInvokeAsync<T, U, V>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    public async Task<JsObjectRef> InvokeWithReturnedObjectRefAsync(string functionName, params object[] args)
    {
        var guid = await _jsRuntime.MyInvokeAsync<string>(
            "blazorGoogleMaps.objectManager.invokeWithReturnedObjectRef",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );

        return new JsObjectRef(_jsRuntime, new Guid(guid));
    }

    //public async Task<List<JsObjectRef>> InvokeMultipleWithReturnedObjectRefAsync(string functionName, string eventname, Dictionary<Guid, object> dictArgs)
    //{
    //    List<string> guids = await _jsRuntime.MyInvokeAsync<List<string>>(
    //        "blazorGoogleMaps.objectManager.invokeMultipleWithReturnedObjectRef",
    //        new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName, eventname }
    //            .Concat(dictArgs.Values).ToArray()
    //    );

    //    return guids.Select(e => new JsObjectRef(_jsRuntime, new Guid(e))).ToList();
    //}

    public Task<T> GetValue<T>(string propertyName)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValue",
            _guid.ToString(),
            propertyName);
    }

    public async Task<JsObjectRef> GetObjectReference(string propertyName)
    {
        var guid = await _jsRuntime.MyInvokeAsync<string>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValueWithReturnedObjectRef",
            _guid.ToString(),
            propertyName);

        return new JsObjectRef(_jsRuntime, new Guid(guid));
    }

    public Task<T?> GetMappedValue<T>(string propertyName, params string[] mappedNames)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValueAndMapToArray",
            _guid.ToString(),
            propertyName, mappedNames);
    }

    public override bool Equals(object obj)
    {
        if (obj is JsObjectRef other)
        {
            return other.Guid == this.Guid;
        }
        else
        {
            return false;
        }
    }

    public override int GetHashCode()
    {
        return _guid.GetHashCode();
    }
}