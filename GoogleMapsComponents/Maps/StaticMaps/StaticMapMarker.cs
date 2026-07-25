using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// A set of one or more markers that share the same style, rendered on a static map.
/// Each instance becomes a single <c>markers</c> URL parameter, so group locations that look alike
/// into one <see cref="StaticMapMarker"/> to keep the URL short.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#Markers
/// </summary>
public record StaticMapMarker
{
    /// <summary>
    /// Creates an empty marker definition. Add locations to <see cref="Locations"/>.
    /// </summary>
    public StaticMapMarker()
    {
    }

    /// <summary>
    /// Creates a marker definition for the given locations.
    /// </summary>
    /// <param name="locations">The locations that share this marker style.</param>
    public StaticMapMarker(params StaticMapLocation[] locations)
    {
        Locations = locations;
    }

    /// <summary>
    /// The size of the default marker icon. When omitted the API uses its normal sized icon.
    /// Labels are only rendered on <see cref="StaticMapMarkerSize.Mid"/> and normal sized markers.
    /// </summary>
    public StaticMapMarkerSize? Size { get; init; }

    /// <summary>
    /// The marker colour, either a 24 bit hex value ("0xFFFFCC", "#FFFFCC" and "FFFFCC" are all accepted)
    /// or one of the predefined names in <see cref="StaticMapColors"/>.
    /// Ignored when <see cref="Icon"/> is set.
    /// </summary>
    public string? Color { get; init; }

    /// <summary>
    /// A single uppercase alphanumeric character (A-Z, 0-9) rendered inside the marker.
    /// Lowercase characters are upper cased by the API. Ignored when <see cref="Icon"/> is set.
    /// </summary>
    public char? Label { get; init; }

    /// <summary>
    /// The marker scale, 1, 2 or 4. Use 2 or 4 together with <see cref="StaticMapOptions.Scale"/>
    /// to produce markers that stay sharp on high resolution displays.
    /// </summary>
    public int? Scale { get; init; }

    /// <summary>
    /// An absolute URL of a custom icon. Icons may be up to 4096 pixels in area (64x64 for square images)
    /// and must be PNG, JPEG or GIF. Up to five unique custom icons may be used per request.
    /// When set, <see cref="Size"/>, <see cref="Color"/> and <see cref="Label"/> are ignored by the API.
    /// </summary>
    public string? Icon { get; init; }

    /// <summary>
    /// The point of a custom icon that is aligned with the location, either one of the values in
    /// <see cref="StaticMapMarkerAnchors"/> or an "x,y" offset in pixels from the top left of the icon.
    /// Use <see cref="StaticMapMarkerAnchors.FromPoint"/> to build the offset form.
    /// </summary>
    public string? Anchor { get; init; }

    /// <summary>
    /// The locations that share this marker style. At least one location is required.
    /// </summary>
    public IList<StaticMapLocation> Locations { get; init; } = new List<StaticMapLocation>();
}

/// <summary>
/// The anchor values understood by the <c>markers</c> parameter of the Maps Static API.
/// </summary>
public static class StaticMapMarkerAnchors
{
    /// <summary>Anchors the icon at its top edge.</summary>
    public const string Top = "top";

    /// <summary>Anchors the icon at its bottom edge. This is the default for the built in marker icon.</summary>
    public const string Bottom = "bottom";

    /// <summary>Anchors the icon at its left edge.</summary>
    public const string Left = "left";

    /// <summary>Anchors the icon at its right edge.</summary>
    public const string Right = "right";

    /// <summary>Anchors the icon at its centre.</summary>
    public const string Center = "center";

    /// <summary>Anchors the icon at its top left corner.</summary>
    public const string TopLeft = "topleft";

    /// <summary>Anchors the icon at its top right corner.</summary>
    public const string TopRight = "topright";

    /// <summary>Anchors the icon at its bottom left corner.</summary>
    public const string BottomLeft = "bottomleft";

    /// <summary>Anchors the icon at its bottom right corner.</summary>
    public const string BottomRight = "bottomright";

    /// <summary>
    /// Builds a custom anchor expressed as an offset in pixels from the top left corner of the icon.
    /// </summary>
    /// <param name="x">Horizontal offset in pixels.</param>
    /// <param name="y">Vertical offset in pixels.</param>
    public static string FromPoint(int x, int y) => $"{x},{y}";
}
