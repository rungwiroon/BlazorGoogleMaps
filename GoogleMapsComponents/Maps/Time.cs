using System;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// A representation of time as a Date object, a localized string, and a time zone.
/// </summary>
public class Time
{
    /// <summary>
    /// A string representing the time's value. 
    /// The time is displayed in the time zone of the transit stop.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// The time zone in which this stop lies. 
    /// The value is the name of the time zone as defined in the IANA Time Zone Database, e.g. "America/New_York".
    /// </summary>
    public string? TimeZone { get; set; }

    /// <summary>
    /// The time of this departure or arrival, specified as a JavaScript Date object.
    /// </summary>
    public DateTime Value { get; set; }
}