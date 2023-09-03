namespace GoogleMapsComponents.Maps.Places;

public class PlaceGeometry
{
    /// <summary>
    /// (optional) The Place’s position.
    /// </summary>
    public LatLngLiteral? Location { get; set; }

    /// <summary>
    /// (optional) The preferred viewport when displaying this Place on a map. This property will be null
    /// if the preferred viewport for the Place is not known. Only available with PlacesService.getDetails.
    /// </summary>
    public LatLngBoundsLiteral? Viewport { get; set; }
}