namespace GoogleMapsComponents.Maps;

/// <summary>
/// https://developers.google.com/maps/documentation/javascript/reference/marker#MarkerLabel.className
/// These options specify the appearance of a marker label. A marker label is a single character of text which will appear inside the marker. 
/// If you are using it with a custom marker, you can reposition it with the labelOrigin property in the Icon class.
/// </summary>
public class MarkerLabel
{
    /// <summary>
    /// The color of the label text. Default color is black.
    /// </summary>
    public string Color { get; set; }

    /// <summary>
    /// The font family of the label text (equivalent to the CSS font-family property).
    /// </summary>
    public string FontFamily { get; set; }

    /// <summary>
    /// The font size of the label text (equivalent to the CSS font-size property). 
    /// Default size is 14px.
    /// </summary>
    public string FontSize { get; set; }

    /// <summary>
    /// The font weight of the label text (equivalent to the CSS font-weight property).
    /// </summary>
    public string FontWeight { get; set; }

    /// <summary>
    /// The text to be displayed in the label.
    /// </summary>
    public string Text { get; set; }
    /// <summary>
    /// The className property of the label's element (equivalent to the element's class attribute).
    /// Multiple space-separated CSS classes can be added. Default is no CSS class (an empty string).
    /// The font color, size, weight, and family can only be set via the other properties of MarkerLabel.
    /// CSS classes should not be used to change the position nor orientation of the label
    /// (e.g. using translations and rotations) if also using marker collision management.
    /// </summary>
    public string ClassName { get; set; }
}