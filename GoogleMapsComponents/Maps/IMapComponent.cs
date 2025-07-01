using System;

namespace GoogleMapsComponents.Maps;

public interface IMapComponent
{
    public Guid Guid { get; }
    public MapComponentType ComponentType { get; }
    void SetDisposed();
}

public enum MapComponentType
{
    Marker,
    Polygon,
    Heatmap
}