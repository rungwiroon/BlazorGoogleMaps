using Newtonsoft.Json;

namespace GoogleMapsComponents.Maps.Coordinates
{
    public class Padding
    {
        [JsonProperty("top")]
        public int Top { get; set; }

        [JsonProperty("right")]
        public int Right { get; set; }

        [JsonProperty("left")]
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
}
