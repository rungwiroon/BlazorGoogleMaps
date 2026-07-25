using System;
using System.Collections.Generic;
using System.Text;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Compresses a series of coordinates into the encoded polyline format used by the Maps Static API
/// (the <c>enc:</c> prefix of the <c>path</c> parameter) and by the Directions API.
/// <para/>
/// Encoding trades roughly one metre of precision for a much shorter URL, which matters because a
/// Maps Static API URL may not exceed <see cref="StaticMapUrlBuilder.MaxUrlLength"/> characters.
/// <para/>
/// https://developers.google.com/maps/documentation/utilities/polylinealgorithm
/// </summary>
public static class PolylineEncoder
{
    /// <summary>
    /// Encodes a series of coordinates into an encoded polyline.
    /// </summary>
    /// <param name="points">The coordinates to encode, in order.</param>
    /// <returns>The encoded polyline. Empty when <paramref name="points"/> contains no coordinates.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="points"/> is null.</exception>
    public static string Encode(IEnumerable<LatLngLiteral> points)
    {
        ArgumentNullException.ThrowIfNull(points);

        var builder = new StringBuilder();
        var previousLat = 0;
        var previousLng = 0;

        foreach (var point in points)
        {
            // Coordinates are rounded to five decimal places and encoded as an offset from the previous point.
            var lat = (int)Math.Round(point.Lat * 1e5, MidpointRounding.AwayFromZero);
            var lng = (int)Math.Round(point.Lng * 1e5, MidpointRounding.AwayFromZero);

            EncodeValue(builder, lat - previousLat);
            EncodeValue(builder, lng - previousLng);

            previousLat = lat;
            previousLng = lng;
        }

        return builder.ToString();
    }

    /// <summary>
    /// Decodes an encoded polyline back into coordinates.
    /// </summary>
    /// <param name="encoded">The encoded polyline, without the <c>enc:</c> prefix.</param>
    /// <returns>The decoded coordinates, in order.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="encoded"/> is null.</exception>
    /// <exception cref="FormatException">Thrown when <paramref name="encoded"/> is not a valid encoded polyline.</exception>
    public static IReadOnlyList<LatLngLiteral> Decode(string encoded)
    {
        ArgumentNullException.ThrowIfNull(encoded);

        var points = new List<LatLngLiteral>();
        var index = 0;
        var lat = 0;
        var lng = 0;

        while (index < encoded.Length)
        {
            lat += DecodeValue(encoded, ref index);
            lng += DecodeValue(encoded, ref index);

            points.Add(new LatLngLiteral(lat / 1e5, lng / 1e5));
        }

        return points;
    }

    private static void EncodeValue(StringBuilder builder, int value)
    {
        // Left shift by one bit, then invert every bit of negative values, so that small
        // offsets in either direction produce short output.
        var shifted = value << 1;
        if (value < 0)
        {
            shifted = ~shifted;
        }

        while (shifted >= 0x20)
        {
            builder.Append((char)((0x20 | (shifted & 0x1f)) + 63));
            shifted >>= 5;
        }

        builder.Append((char)(shifted + 63));
    }

    private static int DecodeValue(string encoded, ref int index)
    {
        var result = 0;
        var shift = 0;
        int chunk;

        do
        {
            if (index >= encoded.Length)
                throw new FormatException("The encoded polyline ended in the middle of a value.");

            chunk = encoded[index++] - 63;

            if (chunk < 0 || shift > 30)
                throw new FormatException($"The encoded polyline contains an invalid character at index {index - 1}.");

            result |= (chunk & 0x1f) << shift;
            shift += 5;
        }
        while (chunk >= 0x20);

        return (result & 1) != 0 ? ~(result >> 1) : result >> 1;
    }
}
