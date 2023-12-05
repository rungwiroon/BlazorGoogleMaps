using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

[JsonConverter(typeof(Serialization.JsonStringEnumConverterEx))]
public enum StrokePosition
{
    [EnumMember(Value = "0")]
    Center = 0,
    [EnumMember(Value = "1")]
    Inside = 1,
    [EnumMember(Value = "2")]
    Outside = 2,
}