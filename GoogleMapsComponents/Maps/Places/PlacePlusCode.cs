using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Places
{
    public class PlacePlusCode
    {
        /// <summary>
        /// It is  a 6 character or longer local code with an explicit location (CWC8+R9, Mountain View, CA, USA)
        /// </summary>
        [JsonPropertyName("compound_code")]
        public string CompoundCode { get; set; }

        /// <summary>
        /// It is a 4 character area code and 6 character or longer local code (849VCWC8+R9)
        /// </summary>
        [JsonPropertyName("global_code")]
        public string GlobalCode { get; set; }
    }
}
