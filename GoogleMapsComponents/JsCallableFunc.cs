using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Text.Json;

namespace GoogleMapsComponents;

public class JsCallableFunc
{
    private readonly Delegate _delegate;
    private readonly Type[] _argumentTypes;

    public JsCallableFunc(Delegate @delegate, params Type[] argumentTypes)
    {
        _delegate = @delegate;
        _argumentTypes = argumentTypes;
    }

    [JSInvokable]
    public object? Invoke(string args, string guid)
    {
        if (string.IsNullOrWhiteSpace(args) || !_argumentTypes.Any())
        {
            object?[]? nullArgs = _argumentTypes.SkipLast(1).Select(x => (object?)null).ToArray();
            return _delegate.DynamicInvoke(nullArgs);
        }

        var jArray = JsonDocument.Parse(args)
            .RootElement
            .EnumerateArray();

        var arguments = _argumentTypes.SkipLast(1).Zip(jArray, (type, jToken) => new { jToken, type })
            .Select(x =>
            {
                var obj = Helper.DeSerializeObject(x.jToken, x.type);
                return obj;
            })
            .ToArray();

        return _delegate.DynamicInvoke(arguments);
    }
}
