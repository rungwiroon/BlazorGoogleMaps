namespace GoogleMapsComponents.Maps;

/// <summary>
/// This class represents the object for values in the `styles` array passed
/// to the <see cref="MarkerClustering"/>
/// constructor.The element in this array that is used to
/// style the cluster icon is determined by calling the `calculator` function.
/// </summary>
public class MarkerClusterIconStyle
{
    /// <summary>
    /// The URL of the cluster icon image file. If not set, img element will not be created
    /// </summary>
    public string? Url { get; set; }

    /// <summary>
    /// The name of the CSS class defining styles for the cluster markers.
    /// </summary>
    public string? ClassName { get; set; }

    /// <summary>
    /// Height The display height (in pixels) of the cluster icon. Required.
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// Width The display width (in pixels) of the cluster icon. Required.
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// The position (in pixels) from the center of the cluster icon to
    /// where the text label is to be centered and drawn.The format is `[yoffset, xoffset]`
    /// where `yoffset` increases as you go down from center and `xoffset`
    /// increases to the right of center.The default is `[0, 0]`.
    /// </summary>
    public Offset? AnchorText { get; set; }

    /// <summary>
    /// The anchor position (in pixels) of the cluster icon. This is the
    /// spot on the cluster icon that is to be aligned with the cluster position.The format is
    /// `[yoffset, xoffset]` where `yoffset` increases as you go down and
    /// `xoffset` increases to the right of the top-left corner of the icon.The default
    /// anchor position is the center of the cluster icon.
    /// </summary>
    public Offset? AnchorIcon { get; set; }

    /// <summary>
    /// The color of the label text shown on the cluster icon.
    /// @default `"black"`
    /// </summary>
    public string? TextColor { get; set; }

    /// <summary>
    /// The size (in pixels) of the label text shown on the cluster icon.
    /// @default `11`
    /// </summary>
    public int? TextSize { get; set; }

    /// <summary>
    /// The line height (in pixels) of the label text shown on the cluster icon.
    /// @default the same as cluster icon height
    /// </summary>
    public int? TextLineHeight { get; set; }

    /// <summary>
    /// The value of the CSS `text-decoration` property 
    /// for the label text shown on the cluster icon.
    /// @default `"none"`
    /// </summary>
    public string? TextDecoration { get; set; }

    /// <summary>
    /// The value of the CSS `font-weight`
    /// property for the label text shown on the cluster icon.
    /// @default `"bold"`
    /// </summary>
    public string? FontWeight { get; set; }

    /// <summary>
    /// The value of the CSS `font-style`
    /// property for the label text shown on the cluster icon.
    /// @default `"normal"`
    /// </summary>
    public string? FontStyle { get; set; }

    /// <summary>
    /// The value of the CSS `font-family`
    /// property for the label text shown on the cluster icon.
    /// @default `"Arial,sans-serif"`
    /// </summary>
    public string? FontFamily { get; set; }

    /// <summary>
    /// The position of the cluster icon image
    /// within the image defined by `url`. The format is `"xpos ypos"`
    /// (the same format as for the CSS `background-position` property). You must set
    /// this property appropriately when the image defined by `url` represents a sprite
    /// containing multiple images. Note that the position <i>must</i> be specified in px units.
    /// @default `"0 0"`
    /// </summary>
    public string? BackgroundPosition { get; set; }

}