namespace GoogleMapsComponents.Maps;

public class DomEvent
{
    public bool IsTrusted { get; set; }
}

/// <summary>
/// Used as await marker.AddListener<DragEndEvent>("dragend", (e) => OnDragEndDistribution(e));
/// </summary>
public class DragEndEvent
{
    public LatLngLiteral? LatLng { get; set; }
    public Point? Pixel { get; set; }
    public DomEvent? DomEvent { get; set; }
}