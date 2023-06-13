using System;
using System.Collections.Generic;
using System.Text;
using OneOf;

namespace GoogleMapsComponents.Maps.Places;

/// <summary>
/// A LocationBias represents a soft boundary or hint to use when searching for Places.
/// Results may come from outside the specified area.
/// To use the current user's IP address as a bias, the string "IP_BIAS" can be specified.
/// Note: if using a Circle the center and radius must be defined.
/// Requires the &libraries=places URL parameter.
/// </summary>
public class LocationBias : OneOfBase<LatLngLiteral, LatLngBounds, LatLngBoundsLiteral, Circle, string>
{
    public LocationBias(OneOf<LatLngLiteral, LatLngBounds, LatLngBoundsLiteral, Circle, string> value) : base(value) { }
}