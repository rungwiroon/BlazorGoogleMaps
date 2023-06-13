using System.Collections.Generic;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// await cluster.AddListener<GoogleClusterClickEvent>("click", OnClusterLClicked)
/// https://github.com/rungwiroon/BlazorGoogleMaps/issues/233
/// </summary>
public class GoogleClusterClickEvent : MouseEvent
{
    public List<GoogleMarker> Markers { get; set; }
    public LatLngLiteral _Position { get; set; }
}

public class GoogleMarker
{
    public long DataID { get; set; }
    public LatLngLiteral Position { get; set; }
}

public class GoogleMarkerOptions : MarkerOptions
{
    public string ClusterFillColor { get; set; } = "White";
    public string ClusterTextColor { get; set; } = "Black";
    public double ClusterOpacity { get; set; } = 0.9;
    public string? ClusterTitle { get; set; }
    public int ClusterZIndex { get; set; }
    public long DataID { get; set; }
}