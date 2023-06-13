namespace GoogleMapsComponents.Maps;

/// <summary>
/// This object is sent in an event when a user clicks on an icon on the map. 
/// The place ID of this place is stored in the placeId member. To prevent the default info window from showing up, call the stop() method on this event to prevent it being propagated. 
/// Learn more about place IDs in the Places API developer guide.
/// </summary>
public class IconMouseEvent : MouseEvent
{
    public string PlaceId { get; set; }

    public IconMouseEvent()
    {

    }
}