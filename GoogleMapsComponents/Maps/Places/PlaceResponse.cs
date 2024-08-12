using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

public class PlaceResponse
{
    public PlaceResult? Results { get; set; }

    [JsonConverter(typeof(EnumMemberConverter<PlaceServiceStatus>))]
    public PlaceServiceStatus Status { get; set; }
}