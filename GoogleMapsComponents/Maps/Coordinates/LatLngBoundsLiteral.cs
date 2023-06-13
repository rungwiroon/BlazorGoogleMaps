using System;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Object literals are accepted in place of LatLngBounds objects throughout the API.
/// These are automatically converted to LatLngBounds objects. All south, west, north and east must be set, otherwise an exception is thrown.
/// </summary>
public class LatLngBoundsLiteral
{
    /// <summary>
    /// Default constructor. Set East, North, South and West explicitely because here they are initialized to zero.
    /// </summary>
    public LatLngBoundsLiteral()
    {
    }

    /// <summary>
    /// Constructor with one or two given coordinate points.
    /// If the second point is null, the bounds are set to the first point.
    /// The points may be positioned arbitrarily.
    /// </summary>
    public LatLngBoundsLiteral(LatLngLiteral latLng1, LatLngLiteral? latLng2 = null)
    {
        East = latLng1.Lng;
        West = latLng1.Lng;
        South = latLng1.Lat;
        North = latLng1.Lat;
        if (latLng2 != null)
        {
            Extend(latLng2);
        }
    }

    /// <summary>
    /// Create or extend a LatLngBoundsLiteral with a given coordinate point.
    /// Using this method you can initialize a LatLngBoundsLiteral reference with null and call 
    /// subsequently this method to extend the boundaries by given points.
    /// </summary>
    public static void CreateOrExtend(ref LatLngBoundsLiteral? latLngBoundsLiteral, LatLngLiteral latLng)
    {
        if (latLngBoundsLiteral == null)
        {
            latLngBoundsLiteral = new LatLngBoundsLiteral(latLng);
        }
        else
        {
            latLngBoundsLiteral.Extend(latLng);
        }
    }

    /// <summary>
    /// East longitude in degrees. Values outside the range [-180, 180] will be wrapped to the range [-180, 180). 
    /// For example, a value of -190 will be converted to 170. 
    /// A value of 190 will be converted to -170. 
    /// This reflects the fact that longitudes wrap around the globe.
    /// </summary>
    public double East { get; set; }

    /// <summary>
    /// North latitude in degrees. Values will be clamped to the range [-90, 90]. 
    /// This means that if the value specified is less than -90, it will be set to -90. 
    /// And if the value is greater than 90, it will be set to 90.
    /// </summary>
    public double North { get; set; }

    /// <summary>
    /// South latitude in degrees. Values will be clamped to the range [-90, 90]. 
    /// This means that if the value specified is less than -90, it will be set to -90. 
    /// And if the value is greater than 90, it will be set to 90.
    /// </summary>
    public double South { get; set; }

    /// <summary>
    /// West longitude in degrees. 
    /// Values outside the range [-180, 180] will be wrapped to the range [-180, 180). 
    /// For example, a value of -190 will be converted to 170. 
    /// A value of 190 will be converted to -170. 
    /// This reflects the fact that longitudes wrap around the globe.
    /// </summary>
    public double West { get; set; }

    /// <summary>
    /// Extend these boundaries by a given coordinate point.
    /// </summary>
    public void Extend(double lng, double lat)
    {
        if (lng < West)
        {
            West = lng;
        }
        if (lng > East)
        {
            East = lng;
        }
        if (lat < South)
        {
            South = lat;
        }
        if (lat > North)
        {
            North = lat;
        }
    }

    /// <summary>
    /// Extend these boundaries by a given coordinate point.
    /// </summary>
    public void Extend(decimal lng, decimal lat)
    {
        Extend(Convert.ToDouble(lng), Convert.ToDouble(lat));
    }

    /// <summary>
    /// Extend these boundaries by a given coordinate point.
    /// </summary>
    public void Extend(LatLngLiteral latLng)
    {
        Extend(latLng.Lng, latLng.Lat);
    }

    /// <summary>
    /// Is the area zero?
    /// </summary>
    public bool IsEmpty()
    {
        return (West == East || South == North);
    }

    public override string ToString()
    {
        return $"{North} {East} {South} {West}";
    }
}