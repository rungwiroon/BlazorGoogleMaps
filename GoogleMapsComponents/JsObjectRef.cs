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

    public override bool Equals(object? obj)
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

/// <summary>
/// Represents a reference to a live JavaScript object managed by the Google Maps JS interop layer.
/// Each instance tracks the underlying JS object by a unique <see cref="Guid"/>.
/// </summary>
public class JsObjectRef : IJsObjectRef, IDisposable
{
    protected readonly Guid _guid;
    protected readonly IJSRuntime _jsRuntime;

    /// <summary>Gets the unique identifier of the underlying JavaScript object.</summary>
    public Guid Guid
    {
        get { return _guid; }
    }

    /// <summary>Gets the <see cref="IJSRuntime"/> used for JavaScript interop calls.</summary>
    public IJSRuntime JSRuntime
    {
        get { return _jsRuntime; }
    }

    /// <summary>Initializes a <see cref="JsObjectRef"/> that wraps an existing JavaScript object.</summary>
    /// <param name="jsRuntime">The JS runtime for interop calls.</param>
    /// <param name="guid">The GUID that identifies the JS object in the object manager.</param>
    public JsObjectRef(
        IJSRuntime jsRuntime,
        Guid guid)
    {
        _jsRuntime = jsRuntime;
        _guid = guid;
    }

    /// <summary>Creates a new JavaScript object and returns a reference to it.</summary>
    /// <param name="jsRuntime">The JS runtime for interop calls.</param>
    /// <param name="constructorFunctionName">Fully-qualified JS constructor name (e.g. <c>"google.maps.Map"</c>).</param>
    /// <param name="args">Arguments forwarded to the JS constructor.</param>
    /// <returns>A <see cref="JsObjectRef"/> backed by the newly created JS object.</returns>
    public static Task<JsObjectRef> CreateAsync(
        IJSRuntime jsRuntime,
        string constructorFunctionName,
        params object?[] args)
    {
        return CreateAsync(jsRuntime, Guid.NewGuid(), constructorFunctionName, args);
    }

    /// <summary>Creates multiple JavaScript objects in a single interop call, keyed by caller-provided string identifiers.</summary>
    /// <param name="jsRuntime">The JS runtime for interop calls.</param>
    /// <param name="constructorFunctionName">Fully-qualified JS constructor name.</param>
    /// <param name="args">A dictionary mapping caller-defined keys to constructor argument objects.</param>
    /// <returns>A dictionary mapping each caller-defined key to its corresponding <see cref="JsObjectRef"/>.</returns>
    public static async Task<Dictionary<string, JsObjectRef>> CreateMultipleAsync(
        IJSRuntime jsRuntime,
        string constructorFunctionName,
        Dictionary<string, object> args)
    {
        var internalMapping = args.ToDictionary(e => e.Key, e => Guid.NewGuid());
        var dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        var result = await CreateMultipleAsync(
            jsRuntime,
            constructorFunctionName,
            dictArgs);

        return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);
    }

    /// <summary>Creates multiple JavaScript objects using this instance's runtime, keyed by caller-provided string identifiers.</summary>
    /// <param name="constructorFunctionName">Fully-qualified JS constructor name.</param>
    /// <param name="args">A dictionary mapping caller-defined keys to constructor argument objects.</param>
    /// <returns>A dictionary mapping each caller-defined key to its corresponding <see cref="JsObjectRef"/>.</returns>
    public async Task<Dictionary<string, JsObjectRef>> AddMultipleAsync(
        string constructorFunctionName,
        Dictionary<string, object> args)
    {
        var internalMapping = args.ToDictionary(e => e.Key, _ => Guid.NewGuid());
        var dictArgs = internalMapping.ToDictionary(e => e.Value, e => args[e.Key]);
        Dictionary<Guid, JsObjectRef> result = await CreateMultipleAsync(
            _jsRuntime,
            constructorFunctionName,
            dictArgs);

        return internalMapping.ToDictionary(e => e.Key, e => result[e.Value]);
    }

    /// <summary>Creates a new JavaScript object with an explicit GUID and returns a reference to it.</summary>
    /// <param name="jsRuntime">The JS runtime for interop calls.</param>
    /// <param name="guid">The GUID to assign to the new JS object in the object manager.</param>
    /// <param name="functionName">Fully-qualified JS constructor name.</param>
    /// <param name="args">Arguments forwarded to the JS constructor.</param>
    /// <returns>A <see cref="JsObjectRef"/> backed by the newly created JS object.</returns>
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

    /// <summary>Creates multiple JavaScript objects in a single interop call, keyed by their assigned GUIDs.</summary>
    /// <param name="jsRuntime">The JS runtime for interop calls.</param>
    /// <param name="functionName">Fully-qualified JS constructor name.</param>
    /// <param name="dictArgs">A dictionary mapping each new object's GUID to its constructor arguments.</param>
    /// <returns>A dictionary mapping each GUID to its corresponding <see cref="JsObjectRef"/>.</returns>
    public async static Task<Dictionary<Guid, JsObjectRef>> CreateMultipleAsync(
        IJSRuntime jsRuntime,
        string functionName,
        Dictionary<Guid, object> dictArgs)
    {
        var jsObjectRefs = dictArgs.ToDictionary(e => e.Key, e => new JsObjectRef(jsRuntime, e.Key));

        await jsRuntime.MyInvokeAsync<object>(
            "blazorGoogleMaps.objectManager.createMultipleObject",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );

        return jsObjectRefs;
    }

    /// <summary>Releases the underlying JavaScript object. Prefer <see cref="DisposeAsync"/> in async contexts.</summary>
    public virtual void Dispose()
    {
        DisposeAsync();
    }

    /// <summary>Asynchronously releases the underlying JavaScript object from the object manager.</summary>
    /// <returns>A <see cref="ValueTask{TResult}"/> that completes when the JS object has been disposed.</returns>
    public ValueTask<object> DisposeAsync()
    {
        return _jsRuntime.InvokeAsync<object>(
            "blazorGoogleMaps.objectManager.disposeObject",
            _guid.ToString()
        );
    }

    /// <summary>Asynchronously releases multiple JavaScript objects from the object manager in a single call.</summary>
    /// <param name="guids">The GUIDs of the JS objects to dispose.</param>
    /// <returns>A <see cref="ValueTask{TResult}"/> that completes when all specified JS objects have been disposed.</returns>
    public ValueTask<object> DisposeMultipleAsync(List<Guid> guids)
    {
        return _jsRuntime.InvokeAsync<object>(
            "blazorGoogleMaps.objectManager.disposeMultipleObjects",
            guids.Select(e => e.ToString()).ToList()
        );
    }

    /// <summary>Invokes a method on the underlying JavaScript object without returning a value.</summary>
    /// <param name="functionName">The method name as defined on the JS object.</param>
    /// <param name="args">Arguments forwarded to the JS method.</param>
    public async Task InvokeAsync(string functionName, params object?[] args)
    {
        await _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invoke",
            new object?[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>Invokes a method on multiple JavaScript objects in a single interop call.</summary>
    /// <param name="functionName">The method name as defined on the JS objects.</param>
    /// <param name="dictArgs">A dictionary mapping each object's GUID to its method arguments.</param>
    public Task InvokeMultipleAsync(string functionName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invokeMultiple",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), functionName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    /// <summary>Registers event listeners on multiple JavaScript objects in a single interop call.</summary>
    /// <param name="eventName">The name of the event to listen for.</param>
    /// <param name="dictArgs">A dictionary mapping each object's GUID to its listener callback.</param>
    public Task AddMultipleListenersAsync(string eventName, Dictionary<Guid, object> dictArgs)
    {
        return _jsRuntime.MyAddListenerAsync(
            "blazorGoogleMaps.objectManager.addMultipleListeners",
            new object[] { dictArgs.Select(e => e.Key.ToString()).ToList(), eventName }
                .Concat(dictArgs.Values).ToArray()
        );
    }

    /// <summary>Sets a property on the underlying JavaScript object.</summary>
    /// <param name="functionName">The property name to set.</param>
    /// <param name="args">The value(s) to pass to the property setter.</param>
    public Task InvokePropertyAsync(string functionName, params object?[] args)
    {
        return _jsRuntime.MyInvokeAsync(
            "blazorGoogleMaps.objectManager.invokeProperty",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>Gets a property value from the underlying JavaScript object.</summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="functionName">The property name to read.</param>
    /// <param name="args">Additional arguments forwarded to the property getter.</param>
    /// <returns>The property value deserialized as <typeparamref name="T"/>.</returns>
    public Task<T> InvokePropertyAsync<T>(string functionName, params object?[] args)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.invokeProperty",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>Invokes a method on the underlying JavaScript object and returns the result.</summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="functionName">The method name as defined on the JS object.</param>
    /// <param name="args">Arguments forwarded to the JS method.</param>
    /// <returns>The method result deserialized as <typeparamref name="T"/>.</returns>
    public Task<T> InvokeAsync<T>(string functionName, params object?[] args)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.invoke",
            new object[] { _guid.ToString(), functionName }
                .Concat(args).ToArray()
        );
    }

    /// <summary>Invokes a method on multiple JavaScript objects in a single interop call and returns each result.</summary>
    /// <typeparam name="T">The expected return type for each invocation.</typeparam>
    /// <param name="functionName">The method name as defined on the JS objects.</param>
    /// <param name="dictArgs">A dictionary mapping each object's GUID to its method arguments.</param>
    /// <returns>A dictionary mapping each object's GUID string to its deserialized result.</returns>
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

    /// <summary>Invokes a method on the underlying JavaScript object and wraps the returned JS object as a new <see cref="JsObjectRef"/>.</summary>
    /// <param name="functionName">The method name as defined on the JS object.</param>
    /// <param name="args">Arguments forwarded to the JS method.</param>
    /// <returns>A <see cref="JsObjectRef"/> wrapping the JS object returned by the method.</returns>
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

    /// <summary>Reads a property value directly from the underlying JavaScript object.</summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="propertyName">The name of the JS property to read.</param>
    /// <returns>The property value deserialized as <typeparamref name="T"/>.</returns>
    public Task<T> GetValue<T>(string propertyName)
    {
        return _jsRuntime.MyInvokeAsync<T>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValue",
            _guid.ToString(),
            propertyName);
    }

    /// <summary>Reads a property from the underlying JavaScript object and wraps the result as a new <see cref="JsObjectRef"/>.</summary>
    /// <param name="propertyName">The name of the JS property to read.</param>
    /// <returns>A <see cref="JsObjectRef"/> wrapping the JS object held by the property.</returns>
    public async Task<JsObjectRef> GetObjectReference(string propertyName)
    {
        var guid = await _jsRuntime.MyInvokeAsync<string>(
            "blazorGoogleMaps.objectManager.readObjectPropertyValueWithReturnedObjectRef",
            _guid.ToString(),
            propertyName);

        return new JsObjectRef(_jsRuntime, new Guid(guid));
    }

    /// <summary>Reads a property from the underlying JavaScript object and maps it to an array using the provided names.</summary>
    /// <typeparam name="T">The expected return type.</typeparam>
    /// <param name="propertyName">The name of the JS property to read.</param>
    /// <param name="mappedNames">The property names used to map the JS object to <typeparamref name="T"/>.</param>
    /// <returns>The mapped value deserialized as <typeparamref name="T"/>, or <c>null</c> if not present.</returns>
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