using System.Collections.Concurrent;

namespace GoogleMapsComponents;

internal static class JsObjectRefInstances
{
    private static readonly ConcurrentDictionary<string, IJsObjectRef> Instances = new();

    internal static void Add(IJsObjectRef instance)
    {
        Instances.TryAdd(instance.Guid.ToString(), instance);
    }

    internal static void Remove(string guid)
    {
        Instances.TryRemove(guid, out _);
    }

    internal static IJsObjectRef GetInstance(string guid)
    {
        Instances.TryGetValue(guid, out var instance);
        return instance;
    }
}
