using System.Collections.Generic;

namespace GoogleMapsComponents;

internal static class JsObjectRefInstances
{
    private static readonly Dictionary<string, IJsObjectRef> Instances = new Dictionary<string, IJsObjectRef>();

    internal static void Add(IJsObjectRef instance)
    {
        Instances.Add(instance.Guid.ToString(), instance);
    }

    internal static void Remove(string guid)
    {
        Instances.Remove(guid);
    }

    internal static IJsObjectRef GetInstance(string guid)
    {
        return Instances[guid];
    }
}