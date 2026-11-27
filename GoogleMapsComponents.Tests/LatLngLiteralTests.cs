using System.Text.Json;
using GoogleMapsComponents.Maps;

namespace GoogleMapsComponents.Tests;

public class LatLngLiteralTests
{
    [Theory]
    [InlineData(-90.1, 0)]
    [InlineData(90.1, 0)]
    [InlineData(0, -180.1)]
    [InlineData(0, 180.1)]
    public void WhenCoordinatesAreOutOfRange_Constructor_ThrowsArgumentException(double lat, double lng)
    {
        Assert.Throws<ArgumentException>(() => new LatLngLiteral(lat, lng));
    }

    [Fact]
    public void WhenCoordinatesAreEqual_Equals_ReturnsTrue()
    {
        var a = new LatLngLiteral(10.5, 20.5);
        var b = new LatLngLiteral(10.5, 20.5);

        Assert.True(a == b);
        Assert.Equal(a.GetHashCode(), b.GetHashCode());
    }

    [Fact]
    public void WhenSerialized_ProducesLatAndLngProperties()
    {
        var json = JsonSerializer.Serialize(new LatLngLiteral(54.68, 25.28));

        using var doc = JsonDocument.Parse(json);
        Assert.Equal(54.68, doc.RootElement.GetProperty("lat").GetDouble());
        Assert.Equal(25.28, doc.RootElement.GetProperty("lng").GetDouble());
    }

    [Fact]
    public void WhenDeserialized_RoundTripsOriginalValue()
    {
        var original = new LatLngLiteral(-33.86, 151.21);

        var result = JsonSerializer.Deserialize<LatLngLiteral>(JsonSerializer.Serialize(original));

        Assert.Equal(original, result);
    }
}
