using System.Collections.Generic;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Describes the image requested from the Maps Static API. A static map is a plain image: it is not
/// interactive and needs no JavaScript, which makes it a good fit for e-mails, print, thumbnails and
/// pages that must stay lightweight.
/// <para/>
/// Either <see cref="Center"/> and <see cref="Zoom"/> are required, or the viewport must be implied by
/// <see cref="Markers"/>, <see cref="Paths"/> or <see cref="Visible"/>.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#URL_Parameters
/// </summary>
public record StaticMapOptions
{
    /// <summary>
    /// The centre of the image, as coordinates or as a geocodable address.
    /// Required unless the viewport is implied by <see cref="Markers"/>, <see cref="Paths"/> or <see cref="Visible"/>.
    /// </summary>
    public StaticMapLocation? Center { get; init; }

    /// <summary>
    /// The zoom level, from 0 (the whole world) to about 21 (individual buildings).
    /// Required unless the viewport is implied by <see cref="Markers"/>, <see cref="Paths"/> or <see cref="Visible"/>.
    /// </summary>
    public int? Zoom { get; init; }

    /// <summary>
    /// The width of the image in pixels, before <see cref="Scale"/> is applied. Standard usage allows up to 640.
    /// </summary>
    public int Width { get; init; } = 600;

    /// <summary>
    /// The height of the image in pixels, before <see cref="Scale"/> is applied. Standard usage allows up to 640.
    /// </summary>
    public int Height { get; init; } = 400;

    /// <summary>
    /// Multiplies the number of returned pixels without changing the covered area, which keeps the image
    /// sharp on high resolution displays. Valid values are 1 (the default) and 2.
    /// </summary>
    public int? Scale { get; init; }

    /// <summary>
    /// The format of the returned image. Defaults to <see cref="StaticMapImageFormat.Png"/>.
    /// </summary>
    public StaticMapImageFormat? Format { get; init; }

    /// <summary>
    /// The type of map to render. Defaults to <see cref="MapTypeId.Roadmap"/>.
    /// </summary>
    public MapTypeId? MapType { get; init; }

    /// <summary>
    /// The language of the labels on the map, for example "en" or "ja". When omitted the API uses the
    /// preferred language of the country the image covers.
    /// </summary>
    public string? Language { get; init; }

    /// <summary>
    /// A two character ccTLD ("uk", "es", ...) that biases geocoding and adjusts disputed borders.
    /// </summary>
    public string? Region { get; init; }

    /// <summary>
    /// The identifier of a cloud based map style. Cannot be combined with <see cref="Styles"/>.
    /// </summary>
    public string? MapId { get; init; }

    /// <summary>
    /// Markers to draw on the map. Each entry becomes one <c>markers</c> parameter, so locations that
    /// share a style should be grouped into a single <see cref="StaticMapMarker"/>.
    /// </summary>
    public IList<StaticMapMarker>? Markers { get; init; }

    /// <summary>
    /// Polylines and polygons to draw on the map.
    /// </summary>
    public IList<StaticMapPath>? Paths { get; init; }

    /// <summary>
    /// Locations that must remain within the image, without drawing anything at them. Useful to keep a
    /// point of interest in view while the viewport is chosen automatically.
    /// </summary>
    public IList<StaticMapLocation>? Visible { get; init; }

    /// <summary>
    /// Custom styling of map features, built with <see cref="GoogleMapStyleBuilder"/> just like the styles
    /// of an interactive <see cref="GoogleMap"/>. Cannot be combined with <see cref="MapId"/>.
    /// </summary>
    public IList<MapTypeStyle>? Styles { get; init; }

    /// <summary>
    /// A usage tracking channel reported to the Google Cloud console.
    /// </summary>
    public string? Channel { get; init; }

    /// <summary>
    /// An API key that overrides the one supplied by the registered <see cref="IBlazorGoogleMapsKeyService"/>.
    /// Only needed when a page mixes keys, or when no key service is registered.
    /// </summary>
    public string? ApiKey { get; init; }
}
