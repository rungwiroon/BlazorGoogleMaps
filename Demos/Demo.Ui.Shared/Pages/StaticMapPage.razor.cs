using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.StaticMaps;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Demo.Ui.Shared.Pages;

public partial class StaticMapPage
{
    private static readonly LatLngLiteral Bangkok = new(13.505892, 100.8162);

    [Inject]
    private StaticMapService StaticMaps { get; set; } = null!;

    private int _zoom = 13;
    private MapTypeId _mapType = MapTypeId.Roadmap;
    private StaticMapImageFormat _format = StaticMapImageFormat.Png;
    private int _scale = 1;

    private StaticMapOptions _interactiveOptions = null!;
    private StaticMapOptions _markerOptions = null!;
    private StaticMapOptions _pathOptions = null!;
    private StaticMapOptions _encodedOptions = null!;
    private StaticMapOptions _styledOptions = null!;

    private string? _interactiveUrl;
    private string? _serviceUrl;

    private int _encodedPointCount;
    private int _encodedLength;
    private int _unencodedLength;

    protected override void OnInitialized()
    {
        Rebuild();

        _markerOptions = new StaticMapOptions
        {
            Width = 600,
            Height = 400,
            Markers = new List<StaticMapMarker>
            {
                // Two locations in one entry: they share a style and a single markers parameter.
                new(new LatLngLiteral(13.7563, 100.5018), new LatLngLiteral(13.7367, 100.5232))
                {
                    Color = StaticMapColors.Blue,
                    Size = StaticMapMarkerSize.Mid
                },
                new("Don Mueang International Airport, Bangkok")
                {
                    Color = "#00AA55",
                    Label = 'A'
                },
                new(new LatLngLiteral(13.6900, 100.7501))
                {
                    Icon = "https://developers.google.com/maps/documentation/maps-static/images/khm_bicycle.png",
                    Anchor = StaticMapMarkerAnchors.Center
                }
            }
        };

        _pathOptions = new StaticMapOptions
        {
            Center = Bangkok,
            Zoom = 11,
            Width = 600,
            Height = 400,
            Paths = new List<StaticMapPath>
            {
                new(new LatLngLiteral(13.5600, 100.7400), new LatLngLiteral(13.4900, 100.8900), new LatLngLiteral(13.4500, 100.7700))
                {
                    Color = "#0000FF",
                    Weight = 4
                },
                new(new LatLngLiteral(13.5600, 100.7400), new LatLngLiteral(13.5800, 100.9000), new LatLngLiteral(13.4900, 100.8900))
                {
                    // A fully transparent stroke leaves only the fill visible.
                    Color = "0x00000000",
                    FillColor = "0xFF000033"
                }
            }
        };

        var circle = BuildCircle(Bangkok, 0.15, 60);
        _encodedPointCount = circle.Count;
        _encodedLength = PolylineEncoder.Encode(circle).Length;
        _unencodedLength = string.Join("|", circle.ConvertAll(p => new StaticMapLocation(p).ToUrlValue())).Length;

        _encodedOptions = new StaticMapOptions
        {
            Center = Bangkok,
            Zoom = 11,
            Width = 600,
            Height = 400,
            Paths = new List<StaticMapPath>
            {
                new()
                {
                    Points = circle.ConvertAll(p => new StaticMapLocation(p)),
                    Encode = true,
                    Color = "0xFF6600CC",
                    FillColor = "0xFF660022",
                    Weight = 3
                }
            }
        };

        _styledOptions = new StaticMapOptions
        {
            Center = Bangkok,
            Zoom = 12,
            Width = 600,
            Height = 400,
            Styles = new GoogleMapStyleBuilder()
                .AddColor(MapStyleFeatures.Water, MapStyleElements.Geometry, "#1d2c4d")
                .AddColor(MapStyleFeatures.LandscapeNatural, MapStyleElements.Geometry, "#023e58")
                .AddColor(MapStyleFeatures.Road, MapStyleElements.Geometry, "#304a7d")
                .AddVisibility(MapStyleFeatures.Poi, MapStyleElements.Labels, false)
                .AddLightness(MapStyleFeatures.AdministrativeCountry, MapStyleElements.GeometryStroke, 40)
                .Build()
        };
    }

    protected override async Task OnInitializedAsync()
    {
        _serviceUrl = await StaticMaps.GetUrlAsync(new StaticMapOptions
        {
            Center = "Grand Palace, Bangkok",
            Zoom = 15,
            Width = 300,
            Height = 200,
            Format = StaticMapImageFormat.Jpg
        });
    }

    private void Rebuild(
        int? zoom = null,
        MapTypeId? mapType = null,
        StaticMapImageFormat? format = null,
        int? scale = null)
    {
        _zoom = zoom ?? _zoom;
        _mapType = mapType ?? _mapType;
        _format = format ?? _format;
        _scale = scale ?? _scale;

        _interactiveOptions = new StaticMapOptions
        {
            Center = Bangkok,
            Zoom = _zoom,
            Width = 600,
            Height = 400,
            MapType = _mapType,
            Format = _format,
            Scale = _scale
        };
    }

    /// <summary>
    /// Builds a closed ring of coordinates around a centre, to show what encoding saves on a long path.
    /// </summary>
    private static List<LatLngLiteral> BuildCircle(LatLngLiteral center, double radiusDegrees, int points)
    {
        var ring = new List<LatLngLiteral>(points + 1);

        for (var i = 0; i <= points; i++)
        {
            var angle = 2 * Math.PI * i / points;

            ring.Add(new LatLngLiteral(
                center.Lat + (radiusDegrees * Math.Sin(angle)),
                center.Lng + (radiusDegrees * Math.Cos(angle))));
        }

        return ring;
    }
}
