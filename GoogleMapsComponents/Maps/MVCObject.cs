using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// google.maps.MVCObject class
/// Base class implementing KVO.
/// </summary>
public class MVCObject
{
    public Task<MapEventListener> AddListener(string eventName, Action handler)
    {
        throw new NotImplementedException();
    }

    public Task<MapEventListener> AddListener<T>(string eventName, Action<T> handler)
    {
        throw new NotImplementedException();
    }
}