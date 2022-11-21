namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A Geocoder response returned by the <see cref="Geocoder"></see> containing the list of <see cref="GeocoderResult"></see>s.
    /// </summary>
    public class GeocoderResponse
    {
        /// <summary>
        /// The list of <see cref="GeocoderResult"></see>s
        /// </summary>
        public GeocoderResult[] Results { get; set; } = new GeocoderResult[] { };

        public GeocoderStatus Status { get; set; }
    }
}
