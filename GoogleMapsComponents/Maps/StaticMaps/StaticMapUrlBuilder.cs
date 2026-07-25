using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Turns <see cref="StaticMapOptions"/> into a Maps Static API request URL.
/// <para/>
/// Prefer <see cref="StaticMapService"/> or the <c>StaticMap</c> component, which fill in the API key from
/// the registered <see cref="IBlazorGoogleMapsKeyService"/>; use this class directly when the key is
/// already at hand, for example on a server that renders map images into an e-mail.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start
/// </summary>
public static class StaticMapUrlBuilder
{
    /// <summary>
    /// The endpoint of the Maps Static API.
    /// </summary>
    public const string BaseUrl = "https://maps.googleapis.com/maps/api/staticmap";

    /// <summary>
    /// The longest URL the Maps Static API accepts. Longer requests are rejected by Google, so
    /// <see cref="Build"/> fails early instead of producing a URL that can never render.
    /// </summary>
    public const int MaxUrlLength = 16384;

    private static readonly HashSet<string> PredefinedColors = new(StringComparer.OrdinalIgnoreCase)
    {
        "black", "brown", "green", "purple", "yellow", "blue", "gray", "orange", "red", "white"
    };

    /// <summary>
    /// Builds the request URL for the given options.
    /// </summary>
    /// <param name="options">The image to request.</param>
    /// <param name="apiKey">
    /// The API key to authenticate with. Ignored when <see cref="StaticMapOptions.ApiKey"/> is set.
    /// </param>
    /// <returns>An absolute URL that can be used as the <c>src</c> of an image.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the options describe a request the API would reject.</exception>
    /// <exception cref="InvalidOperationException">
    /// Thrown when the resulting URL exceeds <see cref="MaxUrlLength"/> characters.
    /// </exception>
    public static string Build(StaticMapOptions options, string? apiKey = null)
    {
        ArgumentNullException.ThrowIfNull(options);

        var key = string.IsNullOrWhiteSpace(options.ApiKey) ? apiKey : options.ApiKey;
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("A Google Maps API key is required to request a static map.", nameof(apiKey));

        Validate(options);

        var parameters = new List<string>();

        if (options.Center.HasValue)
            parameters.Add("center=" + FormatLocation(options.Center.Value));

        if (options.Zoom.HasValue)
            parameters.Add("zoom=" + options.Zoom.Value.ToString(CultureInfo.InvariantCulture));

        parameters.Add(string.Format(CultureInfo.InvariantCulture, "size={0}x{1}", options.Width, options.Height));

        if (options.Scale.HasValue)
            parameters.Add("scale=" + options.Scale.Value.ToString(CultureInfo.InvariantCulture));

        if (options.Format.HasValue)
            parameters.Add("format=" + GetEnumMemberValue(options.Format.Value));

        if (options.MapType.HasValue)
            parameters.Add("maptype=" + GetEnumMemberValue(options.MapType.Value));

        if (!string.IsNullOrWhiteSpace(options.Language))
            parameters.Add("language=" + Escape(options.Language!));

        if (!string.IsNullOrWhiteSpace(options.Region))
            parameters.Add("region=" + Escape(options.Region!));

        if (!string.IsNullOrWhiteSpace(options.MapId))
            parameters.Add("map_id=" + Escape(options.MapId!));

        if (options.Styles != null)
        {
            foreach (var style in options.Styles)
            {
                var value = FormatStyle(style);
                if (value != null)
                    parameters.Add("style=" + value);
            }
        }

        if (options.Markers != null)
        {
            foreach (var marker in options.Markers)
            {
                parameters.Add("markers=" + FormatMarker(marker));
            }
        }

        if (options.Paths != null)
        {
            foreach (var path in options.Paths)
            {
                parameters.Add("path=" + FormatPath(path));
            }
        }

        if (options.Visible is { Count: > 0 })
            parameters.Add("visible=" + string.Join("|", options.Visible.Select(FormatLocation)));

        if (!string.IsNullOrWhiteSpace(options.Channel))
            parameters.Add("channel=" + Escape(options.Channel!));

        parameters.Add("key=" + Escape(key!));

        // The pipe is not a legal URI character, so it is escaped once the whole query is assembled;
        // commas and colons are left alone because Google's own examples rely on them being readable.
        var url = BaseUrl + "?" + string.Join("&", parameters).Replace("|", "%7C");

        if (url.Length > MaxUrlLength)
        {
            throw new InvalidOperationException(
                $"The static map URL is {url.Length} characters long, but the Maps Static API accepts at most {MaxUrlLength}. " +
                "Consider encoding paths by setting StaticMapPath.Encode, or reducing the number of markers.");
        }

        return url;
    }

    private static void Validate(StaticMapOptions options)
    {
        if (options.Width <= 0)
            throw new ArgumentException("Width must be greater than zero.", nameof(options));

        if (options.Height <= 0)
            throw new ArgumentException("Height must be greater than zero.", nameof(options));

        if (options.Scale is { } scale && scale != 1 && scale != 2 && scale != 4)
            throw new ArgumentException("Scale must be 1, 2 or 4.", nameof(options));

        if (options.Zoom is { } zoom && zoom is < 0 or > 22)
            throw new ArgumentException("Zoom must be between 0 and 22.", nameof(options));

        if (!string.IsNullOrWhiteSpace(options.MapId) && options.Styles is { Count: > 0 })
            throw new ArgumentException("MapId and Styles cannot be combined; a cloud based style replaces inline styles.", nameof(options));

        var hasImplicitViewport =
            options.Markers?.Any(m => m.Locations is { Count: > 0 }) == true ||
            options.Paths?.Any(p => !string.IsNullOrEmpty(p.EncodedPolyline) || p.Points is { Count: > 0 }) == true ||
            options.Visible is { Count: > 0 };

        if (!hasImplicitViewport && (!options.Center.HasValue || !options.Zoom.HasValue))
        {
            throw new ArgumentException(
                "Center and Zoom are required unless the viewport is implied by Markers, Paths or Visible.",
                nameof(options));
        }
    }

    private static string FormatLocation(StaticMapLocation location)
    {
        // "lat,lng" is left unescaped so URLs stay readable; addresses are free text and must be escaped.
        return location.IsLatLng ? location.ToUrlValue() : Escape(location.ToUrlValue());
    }

    private static string FormatMarker(StaticMapMarker marker)
    {
        ArgumentNullException.ThrowIfNull(marker);

        if (marker.Locations is not { Count: > 0 })
            throw new ArgumentException("A marker must define at least one location.", nameof(marker));

        var parts = new List<string>();

        if (marker.Size.HasValue)
            parts.Add("size:" + GetEnumMemberValue(marker.Size.Value));

        if (!string.IsNullOrWhiteSpace(marker.Color))
            parts.Add("color:" + NormalizeColor(marker.Color!));

        if (marker.Label.HasValue)
        {
            var label = char.ToUpperInvariant(marker.Label.Value);
            if (!char.IsLetterOrDigit(label) || label > 'Z')
                throw new ArgumentException("A marker label must be a single character in the range A-Z or 0-9.", nameof(marker));

            parts.Add("label:" + label);
        }

        if (marker.Scale is { } scale)
        {
            if (scale != 1 && scale != 2 && scale != 4)
                throw new ArgumentException("A marker scale must be 1, 2 or 4.", nameof(marker));

            parts.Add("scale:" + scale.ToString(CultureInfo.InvariantCulture));
        }

        if (!string.IsNullOrWhiteSpace(marker.Icon))
            parts.Add("icon:" + Escape(marker.Icon!));

        if (!string.IsNullOrWhiteSpace(marker.Anchor))
            parts.Add("anchor:" + marker.Anchor);

        parts.AddRange(marker.Locations.Select(FormatLocation));

        return string.Join("|", parts);
    }

    private static string FormatPath(StaticMapPath path)
    {
        ArgumentNullException.ThrowIfNull(path);

        var parts = new List<string>();

        if (path.Weight.HasValue)
            parts.Add("weight:" + path.Weight.Value.ToString(CultureInfo.InvariantCulture));

        if (!string.IsNullOrWhiteSpace(path.Color))
            parts.Add("color:" + NormalizeColor(path.Color!));

        if (!string.IsNullOrWhiteSpace(path.FillColor))
            parts.Add("fillcolor:" + NormalizeColor(path.FillColor!));

        if (path.Geodesic.HasValue)
            parts.Add("geodesic:" + (path.Geodesic.Value ? "true" : "false"));

        if (!string.IsNullOrEmpty(path.EncodedPolyline))
        {
            parts.Add("enc:" + Escape(path.EncodedPolyline!));
        }
        else if (path.Points is { Count: > 0 })
        {
            if (path.Points.Count < 2)
                throw new ArgumentException("A path must define at least two points.", nameof(path));

            if (path.Encode)
            {
                var coordinates = new List<LatLngLiteral>(path.Points.Count);
                foreach (var point in path.Points)
                {
                    if (point.LatLng is not { } latLng)
                        throw new ArgumentException("An encoded path can only contain coordinates, not addresses.", nameof(path));

                    coordinates.Add(latLng);
                }

                parts.Add("enc:" + Escape(PolylineEncoder.Encode(coordinates)));
            }
            else
            {
                parts.AddRange(path.Points.Select(FormatLocation));
            }
        }
        else
        {
            throw new ArgumentException("A path must define either Points or an EncodedPolyline.", nameof(path));
        }

        return string.Join("|", parts);
    }

    private static string? FormatStyle(MapTypeStyle style)
    {
        if (style.stylers is not { Length: > 0 })
            return null;

        var parts = new List<string>();

        if (!string.IsNullOrWhiteSpace(style.featureType))
            parts.Add("feature:" + style.featureType);

        if (!string.IsNullOrWhiteSpace(style.elementType))
            parts.Add("element:" + style.elementType);

        foreach (var styler in style.stylers)
        {
            switch (styler)
            {
                case GoogleMapStyleColor color when !string.IsNullOrWhiteSpace(color.color):
                    parts.Add("color:" + NormalizeColor(color.color));
                    break;
                case GoogleMapStyleHue hue when !string.IsNullOrWhiteSpace(hue.hue):
                    parts.Add("hue:" + NormalizeColor(hue.hue));
                    break;
                case GoogleMapStyleVisibility visibility when !string.IsNullOrWhiteSpace(visibility.visibility):
                    parts.Add("visibility:" + visibility.visibility);
                    break;
                case GoogleMapStyleLightness lightness:
                    parts.Add("lightness:" + lightness.lightness.ToString(CultureInfo.InvariantCulture));
                    break;
                case GoogleMapStyleSaturation saturation:
                    parts.Add("saturation:" + saturation.saturation.ToString(CultureInfo.InvariantCulture));
                    break;
                case GoogleMapStyleWeight weight:
                    parts.Add("weight:" + weight.weight.ToString(CultureInfo.InvariantCulture));
                    break;
            }
        }

        return parts.Count > 0 ? string.Join("|", parts) : null;
    }

    /// <summary>
    /// Accepts the colour spellings a developer is likely to reach for ("#RRGGBB" from CSS, "RRGGBB" from a
    /// style editor, a predefined name) and returns the "0xRRGGBB" form the Maps Static API expects.
    /// </summary>
    private static string NormalizeColor(string color)
    {
        var value = color.Trim();

        if (PredefinedColors.Contains(value))
            return value.ToLowerInvariant();

        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return "0x" + value.Substring(2);

        if (value.StartsWith("#", StringComparison.Ordinal))
            return "0x" + value.Substring(1);

        if ((value.Length == 6 || value.Length == 8) && value.All(Uri.IsHexDigit))
            return "0x" + value;

        return value;
    }

    private static string Escape(string value) => Uri.EscapeDataString(value);

    private static string GetEnumMemberValue<T>(T value) where T : struct, Enum
    {
        var name = value.ToString();
        var attribute = typeof(T).GetField(name)?.GetCustomAttribute<EnumMemberAttribute>();

        return attribute?.Value ?? name.ToLowerInvariant();
    }
}
