using System.Runtime.Serialization;

namespace GoogleMapsComponents.Maps;

public enum RenderingType
{
    [EnumMember(Value = "google.maps.RenderingType.RASTER")]
    Raster,
    [EnumMember(Value = "google.maps.RenderingType.UNINITIALIZED")]
    Uninitialized,
    [EnumMember(Value = "google.maps.RenderingType.VECTOR")]
    Vector
}