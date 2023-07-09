using GoogleMapsComponents.Serialization;
using OneOf;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.Data;

/// <summary>
/// DataOptions object used to define the properties that a developer can set on a Data object.
/// </summary>
public class DataOptions
{
    /// <summary>
    /// The position of the drawing controls on the map. The default position is TOP_LEFT.
    /// </summary>
    public ControlPosition ControlPosition { get; set; }

    /// <summary>
    /// Describes which drawing modes are available for the user to select, in the order they are displayed.
    /// This should not include the null drawing mode, which is added by default. 
    /// If null, drawing controls are disabled and not displayed. 
    /// Defaults to null. 
    /// Possible drawing modes are "Point", "LineString" or "Polygon".
    /// </summary>
    public IEnumerable<string> Controls { get; set; }

    /// <summary>
    /// The current drawing mode of the given Data layer. 
    /// A drawing mode of null means that the user can interact with the map as normal, and clicks do not draw anything.
    /// Defaults to null. 
    /// Possible drawing modes are null, "Point", "LineString" or "Polygon".
    /// </summary>
    public string DrawingMode { get; set; }

    /// <summary>
    /// When drawing is enabled and a user draws a Geometry (a Point, Line String or Polygon), this function is called with that Geometry and should return a Feature that is to be added to the Data layer.
    /// If a featureFactory is not supplied, a Feature with no id and no properties will be created from that Geometry instead.
    /// Defaults to null.
    /// </summary>
    public Func<Geometry, Feature> FeatureFactory { get; set; }

    /// <summary>
    /// Map on which to display the features in the collection.
    /// </summary>
    [JsonConverter(typeof(JsObjectRefConverter<Map>))]
    public Map Map { get; set; }

    /// <summary>
    /// Style for all features in the collection. 
    /// For more details, see the setStyle() method.
    /// </summary>
    public OneOf<Func<Feature, StyleOptions>, StyleOptions> Style { get; set; }
}