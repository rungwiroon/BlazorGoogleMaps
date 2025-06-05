using GoogleMapsComponents.Serialization;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Object literals are accepted in place of LatLng objects, as a convenience, in many places. 
/// These are converted to LatLng objects when the Maps API encounters them.
/// </summary>
[DebuggerDisplay("{Lat}, {Lng}")]
[StructLayout(LayoutKind.Explicit, Size = sizeof(double) * 2)]
[JsonConverter(typeof(LatLngLiteralConverter))]
public readonly struct LatLngLiteral : IEquatable<LatLngLiteral>
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
    /// Default constructor
    /// </summary>
    /// <param name="lat">Latitude value</param>
    /// <param name="lng">Longitude value</param>
    /// <exception cref="ArgumentException">Invoked if <paramref name="lat"/> is lower than -90 or higher than 90,
    /// or if <paramref name="lng"/> is lower than -180 or higher than 180.</exception>
    public LatLngLiteral(double lat, double lng)
    {
        if (lat is < -90 or > 90)
            throw new ArgumentException("Latitude values can only range from -90 to 90!", nameof(lat));

        if (lng is < -180 or > 180)
            throw new ArgumentException("Longitude values can only range from -180 to 180!", nameof(lng));
        
        Lat = lat;
        Lng = lng;
    }

    #region Methods

    /// <inheritdoc />
    public bool Equals(LatLngLiteral other)
    {
        return Lat.Equals(other.Lat) && Lng.Equals(other.Lng);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return obj is LatLngLiteral other && Equals(other);
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        return HashCode.Combine(Lat, Lng);
    }

    /// <summary>
    /// Checks if two <see cref="LatLngLiteral"/> instances are equal
    /// </summary>
    /// <param name="left">Left hand value</param>
    /// <param name="right">Right hand value</param>
    /// <returns><see langword="true" /> if both items are equal.</returns>
    public static bool operator ==(LatLngLiteral left, LatLngLiteral right)
    {
        return left.Equals(right);
    }

    /// <summary>
    /// Checks if two <see cref="LatLngLiteral"/> instances are unequal
    /// </summary>
    /// <param name="left">Left hand value</param>
    /// <param name="right">Right hand value</param>
    /// <returns><see langword="true" /> if both items are unequal.</returns>
    public static bool operator !=(LatLngLiteral left, LatLngLiteral right)
    {
        return !left.Equals(right);
    }

    #endregion
}