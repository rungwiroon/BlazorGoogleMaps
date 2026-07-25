using System;
using System.Globalization;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// A location used by the Maps Static API. Every location is either a set of coordinates
/// ("40.714728,-73.998672") or a geocodable address ("city hall, new york, ny").
/// <para/>
/// Locations are accepted by <see cref="StaticMapOptions.Center"/>, <see cref="StaticMapOptions.Visible"/>,
/// <see cref="StaticMapMarker.Locations"/> and <see cref="StaticMapPath.Points"/>.
/// <para/>
/// Both <see cref="string"/> and <see cref="LatLngLiteral"/> convert implicitly, so a location can be
/// written as <c>"Berkeley, CA"</c> or <c>new LatLngLiteral(40.714728, -73.998672)</c>.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#Locations
/// </summary>
public readonly struct StaticMapLocation : IEquatable<StaticMapLocation>
{
    private readonly string? _address;
    private readonly LatLngLiteral _latLng;
    private readonly bool _hasLatLng;

    /// <summary>
    /// Creates a location from a geocodable address, for example "1600 Amphitheatre Parkway, Mountain View, CA".
    /// </summary>
    /// <param name="address">A human readable, geocodable address.</param>
    /// <exception cref="ArgumentException">Thrown when <paramref name="address"/> is null, empty or whitespace.</exception>
    public StaticMapLocation(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
            throw new ArgumentException("Address cannot be null or whitespace.", nameof(address));

        _address = address;
        _latLng = default;
        _hasLatLng = false;
    }

    /// <summary>
    /// Creates a location from coordinates.
    /// </summary>
    /// <param name="latLng">The coordinates of the location.</param>
    public StaticMapLocation(LatLngLiteral latLng)
    {
        _address = null;
        _latLng = latLng;
        _hasLatLng = true;
    }

    /// <summary>
    /// Creates a location from a latitude and a longitude.
    /// </summary>
    /// <param name="lat">Latitude in degrees, between -90 and 90.</param>
    /// <param name="lng">Longitude in degrees, between -180 and 180.</param>
    public StaticMapLocation(double lat, double lng)
        : this(new LatLngLiteral(lat, lng))
    {
    }

    /// <summary>
    /// <see langword="true"/> when this location is expressed as coordinates,
    /// <see langword="false"/> when it is expressed as an address.
    /// </summary>
    public bool IsLatLng => _hasLatLng;

    /// <summary>
    /// The address of this location, or <see langword="null"/> when the location is expressed as coordinates.
    /// </summary>
    public string? Address => _hasLatLng ? null : _address;

    /// <summary>
    /// The coordinates of this location, or <see langword="null"/> when the location is expressed as an address.
    /// </summary>
    public LatLngLiteral? LatLng => _hasLatLng ? _latLng : null;

    /// <summary>
    /// Renders the location the way the Maps Static API expects it, without URL escaping.
    /// </summary>
    /// <exception cref="InvalidOperationException">Thrown when the location was never initialized.</exception>
    public string ToUrlValue()
    {
        if (_hasLatLng)
        {
            return string.Concat(
                _latLng.Lat.ToString("0.######", CultureInfo.InvariantCulture),
                ",",
                _latLng.Lng.ToString("0.######", CultureInfo.InvariantCulture));
        }

        if (_address == null)
            throw new InvalidOperationException("The location was not initialized with an address or with coordinates.");

        return _address;
    }

    /// <summary>
    /// Creates a location from a geocodable address.
    /// </summary>
    public static implicit operator StaticMapLocation(string address) => new(address);

    /// <summary>
    /// Creates a location from coordinates.
    /// </summary>
    public static implicit operator StaticMapLocation(LatLngLiteral latLng) => new(latLng);

    /// <inheritdoc />
    public bool Equals(StaticMapLocation other)
    {
        if (_hasLatLng != other._hasLatLng)
            return false;

        return _hasLatLng
            ? _latLng.Equals(other._latLng)
            : string.Equals(_address, other._address, StringComparison.Ordinal);
    }

    /// <inheritdoc />
    public override bool Equals(object? obj) => obj is StaticMapLocation other && Equals(other);

    /// <inheritdoc />
    public override int GetHashCode() => _hasLatLng ? _latLng.GetHashCode() : (_address?.GetHashCode() ?? 0);

    /// <summary>
    /// Checks if two <see cref="StaticMapLocation"/> instances are equal.
    /// </summary>
    public static bool operator ==(StaticMapLocation left, StaticMapLocation right) => left.Equals(right);

    /// <summary>
    /// Checks if two <see cref="StaticMapLocation"/> instances are unequal.
    /// </summary>
    public static bool operator !=(StaticMapLocation left, StaticMapLocation right) => !left.Equals(right);

    /// <inheritdoc />
    public override string ToString() => _hasLatLng || _address != null ? ToUrlValue() : string.Empty;
}
