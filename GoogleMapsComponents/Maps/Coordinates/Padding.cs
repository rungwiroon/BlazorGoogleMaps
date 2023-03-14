using Newtonsoft.Json;
using System;

namespace GoogleMapsComponents.Maps.Coordinates
{
    public class Padding
    {
        [JsonProperty("top")]
        public int Top { get; set; } = 0;

        [JsonProperty("right")]
        public int Right { get; set; } = 0;

        [JsonProperty("left")]
        public int Left { get; set; } = 0;


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
}
