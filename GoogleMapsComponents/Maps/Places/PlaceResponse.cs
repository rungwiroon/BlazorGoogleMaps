using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places;

public class PlaceResponse
{
    public PlaceResult[] Results { get; set; } = new PlaceResult[] { };

    public PlaceServiceStatus Status { get; set; }
}