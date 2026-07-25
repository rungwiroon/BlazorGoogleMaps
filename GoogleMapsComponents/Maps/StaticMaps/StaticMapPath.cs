using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// A polyline, or a filled polygon, drawn on a static map.
/// Each instance becomes a single <c>path</c> URL parameter.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#Paths
/// </summary>
public record StaticMapPath
{
    /// <summary>
    /// Creates an empty path definition. Add points to <see cref="Points"/>.
    /// </summary>
    public StaticMapPath()
    {
    }

    /// <summary>
    /// Creates a path through the given points.
    /// </summary>
    /// <param name="points">The vertices of the path, in order.</param>
    public StaticMapPath(params StaticMapLocation[] points)
    {
        Points = points;
    }

    /// <summary>
    /// The thickness of the line in pixels. Defaults to 5 when omitted.
    /// </summary>
    public int? Weight { get; init; }

    /// <summary>
    /// The line colour, either a 24 bit hex value ("0xFFFFCC"), a 32 bit hex value that includes an
    /// alpha component ("0xFFFFCC33") or one of the predefined names in <see cref="StaticMapColors"/>.
    /// </summary>
    public string? Color { get; init; }

    /// <summary>
    /// The fill colour of the area enclosed by the path. Setting it turns the path into a polygon:
    /// the API closes the path automatically, so the first and last point do not need to match.
    /// A 32 bit value such as "0xFFFF0033" lets the map show through the fill.
    /// </summary>
    public string? FillColor { get; init; }

    /// <summary>
    /// <see langword="true"/> to interpret the points as great circle segments that follow the curvature
    /// of the earth, <see langword="false"/> (the default) to render them as straight lines on the screen.
    /// </summary>
    public bool? Geodesic { get; init; }

    /// <summary>
    /// The vertices of the path, in order. At least two points are required unless
    /// <see cref="EncodedPolyline"/> is set.
    /// </summary>
    public IList<StaticMapLocation> Points { get; init; } = new List<StaticMapLocation>();

    /// <summary>
    /// When <see langword="true"/>, <see cref="Points"/> are compressed into an encoded polyline before
    /// being written to the URL. Encoding shortens the URL considerably for paths with many vertices,
    /// at the cost of about one metre of precision. Every point must be expressed as coordinates.
    /// Ignored when <see cref="EncodedPolyline"/> is set.
    /// </summary>
    public bool Encode { get; init; }

    /// <summary>
    /// An already encoded polyline, for example the <see cref="DirectionsRoute.OverviewPolyline"/> of a
    /// route returned by the Directions API. When set it is used instead of <see cref="Points"/>.
    /// Use <see cref="PolylineEncoder"/> to produce this value from coordinates yourself.
    /// </summary>
    public string? EncodedPolyline { get; init; }
}
