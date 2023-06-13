namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// google.maps.Data.RemovePropertyEvent interface 
/// </summary>
public class RemovePropertyEvent
{
    /// <summary>
    /// The feature whose property was removed.
    /// </summary>
    public Feature Feature { get; set; }

    /// <summary>
    /// The property name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// The previous value.
    /// </summary>
    public object OldValue { get; set; }
}