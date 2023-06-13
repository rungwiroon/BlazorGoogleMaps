namespace GoogleMapsComponents.Maps;

/// <summary>
/// Reference: https://developers.google.com/maps/documentation/javascript/style-reference
/// </summary>
public static class MapStyleElements
{
    /// <summary>
    /// (default) selects all elements of the specified feature.
    /// </summary>
    public const string All = "all";
        
    /// <summary>
    /// selects all geometric elements of the specified feature
    /// </summary>
    public const string Geometry = "geometry";
    /// <summary>
    /// selects only the fill of the feature's geometry
    /// </summary>
    public const string GeometryFile = "geometry.fill";
    /// <summary>
    /// selects only the stroke of the feature's geometry
    /// </summary>
    public const string GeometryStroke = "geometry.stroke";
        
    /// <summary>
    /// selects the textual labels associated with the specified feature
    /// </summary>
    public const string Labels = "labels";
    /// <summary>
    /// selects only the icon displayed within the feature's label
    /// </summary>
    public const string LabelsIcon = "labels.icon";
    /// <summary>
    /// selects only the text of the label
    /// </summary>
    public const string LabelsText = "labels.text";
    /// <summary>
    /// selects only the fill of the label. The fill of a label is typically rendered as a colored outline that surrounds the label text
    /// </summary>
    public const string LabelsTextFill = "labels.text.fill";
    /// <summary>
    /// selects only the stroke of the label's text
    /// </summary>
    public const string LabelsTextStroke = "labels.text.stroke";
}