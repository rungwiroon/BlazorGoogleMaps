namespace GoogleMapsComponents.Maps.Places;

public class FindPlaceFromPhoneNumberRequest : FindPlaceFromBase
{
    /// <summary>
    /// The phone number of the place to look up. Format must be E.164.
    /// See: https://en.wikipedia.org/wiki/E.164
    /// </summary>
    public string PhoneNumber { get; set; } = default!;
}