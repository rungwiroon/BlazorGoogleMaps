using Microsoft.JSInterop;
using System;
using System.Linq;
using System.Text.Json;

namespace GoogleMapsComponents;

public class JsCallableAction
{
    private readonly Delegate _delegate;
    private readonly Type[] _argumentTypes;
    private readonly IJSRuntime _jsRuntime;

    public JsCallableAction(IJSRuntime jsRuntime, Delegate @delegate, params Type[] argumentTypes)
    {
        _jsRuntime = jsRuntime;
        _delegate = @delegate;
        _argumentTypes = argumentTypes;
    }

    [JSInvokable]
    public void Invoke(string args, string guid)
    {
        if (string.IsNullOrWhiteSpace(args) || !_argumentTypes.Any())
        {
            _delegate.DynamicInvoke();
            return;
        }

        var jArray = JsonDocument.Parse(args)
            .RootElement
            .EnumerateArray();

        var arguments = _argumentTypes.Zip(jArray, (type, jToken) => new { jToken, type })
            .Select(x =>
            {
                var obj = Helper.DeSerializeObject(x.jToken, x.type);
                if (obj is IActionArgument actionArg)
                {
                    actionArg.JsObjectRef = new JsObjectRef(_jsRuntime, new Guid(guid));
                }
				if (obj is Maps.Data.MouseEvent featureEvent
                 && x.jToken.TryGetProperty("feature", out var featureJson) 
                 && featureJson.TryGetProperty("Eg", out var featureEg)
                 && featureEg.TryGetProperty("UUID", out var uuidProp))
				{
                    string? uuidStr = uuidProp.GetString();
                    if(!string.IsNullOrWhiteSpace(uuidStr) && Guid.TryParse(uuidStr, out Guid featureUuid))
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