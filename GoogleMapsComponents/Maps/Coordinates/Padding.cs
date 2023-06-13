using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Coordinates;

public class Padding
{
    [JsonPropertyName("top")]
    public int Top { get; set; }

    [JsonPropertyName("right")]
    public int Right { get; set; }

    [JsonPropertyName("left")]
    public int Left { get; set; }

    public Padding()
    {

    }

    public Padding(int padding)
    {
        Top = padding;
        Right = padding;
        Left = padding;
    }

    public Padding(int top, int right, int left)
    {
        Top = top;
        Right = right;
        Left = left;
    }

    public Padding(int top, int left)
    {
        Top = top;
        Left = left;
    }
}