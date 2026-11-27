using System.Text.Json;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Serialization;
using OneOf;

namespace GoogleMapsComponents.Tests;

public class OneOfConverterFactoryTests
{
    private static readonly JsonSerializerOptions Options = new()
    {
        Converters = { new OneOfConverterFactory() }
    };

    [Theory]
    [InlineData(typeof(OneOf<string, int>), true)]
    [InlineData(typeof(OneOf<string, int, bool>), true)]
    [InlineData(typeof(string), false)]
    [InlineData(typeof(LatLngLiteral), false)]
    public void CanConvert_OnlyAcceptsOneOfTypes(Type type, bool expected)
    {
        Assert.Equal(expected, new OneOfConverterFactory().CanConvert(type));
    }

    [Fact]
    public void WhenOneOf2HoldsString_Serialize_WritesString()
    {
        OneOf<string, LatLngLiteral> value = "hello";

        Assert.Equal("\"hello\"", JsonSerializer.Serialize(value, Options));
    }

    [Fact]
    public void WhenOneOf2HoldsObject_Serialize_WritesObjectWithDotnetTypeName()
    {
        OneOf<string, LatLngLiteral> value = new LatLngLiteral(1, 2);

        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value, Options));

        Assert.Equal(1, doc.RootElement.GetProperty("lat").GetDouble());
        Assert.Equal(2, doc.RootElement.GetProperty("lng").GetDouble());
        Assert.Equal(typeof(LatLngLiteral).FullName, doc.RootElement.GetProperty("dotnetTypeName").GetString());
    }

    [Fact]
    public void WhenOneOf3HoldsObject_Serialize_WritesObjectWithDotnetTypeName()
    {
        OneOf<string, int, LatLngLiteral> value = new LatLngLiteral(3, 4);

        using var doc = JsonDocument.Parse(JsonSerializer.Serialize(value, Options));

        Assert.Equal(3, doc.RootElement.GetProperty("lat").GetDouble());
        Assert.Equal(typeof(LatLngLiteral).FullName, doc.RootElement.GetProperty("dotnetTypeName").GetString());
    }

    [Theory]
    [InlineData("{\"lat\":5,\"lng\":6}")]
    [InlineData("{\"$index\":2,\"lat\":5,\"lng\":6}")]
    [InlineData("{\"$index\":-1,\"lat\":5,\"lng\":6}")]
    public void WhenTypeIndexIsMissingOrOutOfRange_Deserialize_ThrowsJsonException(string json)
    {
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<OneOf<string, LatLngLiteral>>(json, Options));
    }
}
