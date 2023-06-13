namespace GoogleMapsComponents.Maps;

/// <summary>
/// A fare of a DirectionsRoute consisting of value and currency.
/// </summary>
public class TransitFare
{
    /// <summary>
    /// An ISO 4217 currency code indicating the currency in which the fare is expressed.
    /// </summary>
    public string currency { get; set; }

    /// <summary>
    /// The numerical value of the fare, expressed in the given currency.
    /// </summary>
    public decimal Value { get; set; }
}