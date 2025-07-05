// ReSharper disable CheckNamespace

using GoogleMapsComponents.Serialization;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Object literals are accepted in place of LatLngBounds objects throughout the API.
/// These are automatically converted to LatLngBounds objects. All south, west, north and east must be set, otherwise an exception is thrown.
/// </summary>

[DebuggerDisplay("{Lat}, {Lng}, {Altitude}")]
[StructLayout(LayoutKind.Explicit, Size = sizeof(double) * 3)]
[JsonConverter(typeof(LatLngAltitudeLiteralConverter))]
public readonly struct LatLngAltitudeLiteral : IEquatable<LatLngAltitudeLiteral>
{
    /// <summary>
    /// Latitude in degrees. Values will be clamped to the range [-90, 90]. 
    /// This means that if the value specified is less than -90, it will be set to -90. 
    /// And if the value is greater than 90, it will be set to 90.
    /// </summary>
    [FieldOffset(0)]
    public readonly double Lat;

    /// <summary>
    /// Longitude in degrees. Values outside the range [-180, 180] will be wrapped so that they fall within the range. 
    /// For example, a value of -190 will be converted to 170. A value of 190 will be converted to -170. 
    /// This reflects the fact that longitudes wrap around the globe.
    /// </summary>
    [FieldOffset(sizeof(double))]
    public readonly double Lng;

    /// <summary>
    ///
    ///  </summary>
    [FieldOffset(sizeof(double))]
    public readonly double Altitude;

    /// <summary>
    /// Default constructor
    /// </summary>
    /// <param name="lat">Latitude value</param>
    /// <param name="lng">Longitude value</param>
    /// <param name="altitude">Altitude value</param>
    /// <exception cref="ArgumentException">Invoked if <paramref name="lat"/> is lower than -90 or higher than 90,
    /// or if <paramref name="lng"/> is lower than -180 or higher than 180.</exception>
    public LatLngAltitudeLiteral(double lat, double lng, double altitude)
    {
        if (lat is < -90 or > 90)
            throw new ArgumentException("Latitude values can only range from -90 to 90!", nameof(lat));

        if (lng is < -180 or > 180)
            throw new ArgumentException("Longitude values can only range from -180 to 180!", nameof(lng));

        Lat = lat;
        Lng = lng;
        Altitude = altitude;
    }

    /// <inheritdoc />
    public bool Equals(LatLngAltitudeLiteral other)
    {
        return Lat.Equals(other.Lat) && Lng.Equals(other.Lng) && Altitude.Equals(other.Altitude);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is LatLngAltitudeLiteral other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Lat, Lng, Altitude);
    }

    /// <summary>
    /// Checks if two <see cref="LatLngAltitudeLiteral"/> instances are equal
    /// </summary>
    /// <param name="left">Left hand value</param>
    /// <param name="right">Right hand value</param>
    /// <returns><see langword="true" /> if both items are equal.</returns>
    public static bool operator ==(LatLngAltitudeLiteral left, LatLngAltitudeLiteral right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two <see cref="LatLngAltitudeLiteral"/> instances are unequal
    /// </summary>
    /// <param name="left">Left hand value</param>
    /// <param name="right">Right hand value</param>
    /// <returns><see langword="true" /> if both items are unequal.</returns>
    public static bool operator !=(LatLngAltitudeLiteral left, LatLngAltitudeLiteral right)
    {
        return !left.Equals(right);
    }
}