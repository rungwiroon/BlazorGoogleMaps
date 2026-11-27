using System.Text.Json;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Serialization;

namespace GoogleMapsComponents.Tests;

public class EnumConverterTests
{
    // JsonStringEnumConverterEx<T> is public, so it is used directly.
    private static readonly JsonSerializerOptions ExOptions = new()
    {
        Converters = { new JsonStringEnumConverterEx<CollisionBehavior>() }
    };

    [Theory]
    [InlineData(CollisionBehavior.REQUIRED, "google.maps.CollisionBehavior.REQUIRED")]
    [InlineData(CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL, "google.maps.CollisionBehavior.REQUIRED_AND_HIDES_OPTIONAL")]
    [InlineData(CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY, "google.maps.CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY")]
    public void WhenEnumHasEnumMember_JsonStringEnumConverterEx_WritesAndReadsMemberValue(CollisionBehavior value, string expected)
    {
        var json = JsonSerializer.Serialize(value, ExOptions);

        Assert.Equal($"\"{expected}\"", json);
        Assert.Equal(value, JsonSerializer.Deserialize<CollisionBehavior>(json, ExOptions));
    }

    [Fact]
    public void WhenJsonContainsEnumName_JsonStringEnumConverterEx_ReadsByName()
    {
        var result = JsonSerializer.Deserialize<CollisionBehavior>("\"OPTIONAL_AND_HIDES_LOWER_PRIORITY\"", ExOptions);

        Assert.Equal(CollisionBehavior.OPTIONAL_AND_HIDES_LOWER_PRIORITY, result);
    }

    [Theory]
    [InlineData("a", "a")]
    [InlineData("Abc", "abc")]
    [InlineData("", "")]
    public void ToLowerFirstChar_LowersOnlyFirstCharacter(string input, string expected)
    {
        Assert.Equal(expected, JsonStringEnumConverterEx<CollisionBehavior>.ToLowerFirstChar(input));
    }

    // EnumMemberConverter<T> is internal; it is exercised through the public
    // FullscreenControlOptions.Position property that uses it.
    private static readonly JsonSerializerOptions WebOptions = new(JsonSerializerDefaults.Web);

    [Theory]
    [InlineData(ControlPosition.BottomCenter, "BOTTOM_CENTER")]
    [InlineData(ControlPosition.LeftTop, "LEFT_TOP")]
    [InlineData(ControlPosition.RightBottom, "RIGHT_BOTTOM")]
    public void WhenPositionSerialized_EnumMemberConverter_WritesEnumMemberValue(ControlPosition position, string expected)
    {
        var json = JsonSerializer.Serialize(new FullscreenControlOptions { Position = position }, WebOptions);

        using var doc = JsonDocument.Parse(json);
        Assert.Equal(expected, doc.RootElement.GetProperty("position").GetString());
    }

    [Theory]
    [InlineData("left_top", ControlPosition.LeftTop)]
    [InlineData("RIGHT_BOTTOM", ControlPosition.RightBottom)]
    public void WhenPositionDeserialized_EnumMemberConverter_IsCaseInsensitive(string value, ControlPosition expected)
    {
        var options = JsonSerializer.Deserialize<FullscreenControlOptions>($"{{\"position\":\"{value}\"}}", WebOptions);

        Assert.Equal(expected, options!.Position);
    }

    [Fact]
    public void WhenPositionIsUnknown_EnumMemberConverter_ThrowsJsonException()
    {
        Assert.Throws<JsonException>(() =>
            JsonSerializer.Deserialize<FullscreenControlOptions>("{\"position\":\"NOWHERE\"}", WebOptions));
    }
}
