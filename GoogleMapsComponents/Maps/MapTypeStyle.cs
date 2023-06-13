using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// Use <see cref="GoogleMapStyleBuilder"/> to build map style
/// The MapTypeStyle is a collection of selectors and stylers that define how the map should be styled. 
/// Selectors specify the map features and/or elements that should be affected, and stylers specify how those features and elements should be modified.
/// </summary>
public class MapTypeStyle
{
    /// <summary>
    /// The element to which a styler should be applied. 
    /// An element is a visual aspect of a feature on the map. 
    /// Example: a label, an icon, the stroke or fill applied to the geometry, and more. 
    /// Optional. 
    /// If elementType is not specified, the value is assumed to be 'all'.
    /// </summary>
    public string? elementType { get; set; }

    /// <summary>
    /// The feature, or group of features, to which a styler should be applied. Optional. 
    /// If featureType is not specified, the value is assumed to be 'all'.
    /// </summary>
    public string? featureType { get; set; }

    /// <summary>
    /// The style rules to apply to the selected map features and elements. 
    /// The rules are applied in the order that you specify in this array.
    /// </summary>
    public GoogleMapStyleElement[]? stylers { get; set; }
}
    
public enum MapStyleVisbility
{
    On,
    Off,
    Simplified
}

[JsonDerivedType(typeof(GoogleMapStyleColor))]
[JsonDerivedType(typeof(GoogleMapStyleHue))]
[JsonDerivedType(typeof(GoogleMapStyleSaturation))]
[JsonDerivedType(typeof(GoogleMapStyleVisibility))]
[JsonDerivedType(typeof(GoogleMapStyleLightness))]
[JsonDerivedType(typeof(GoogleMapStyleWeight))]
public abstract class GoogleMapStyleElement
{
}

public class GoogleMapStyleColor : GoogleMapStyleElement
{
    public string color { get; set; }

    public static explicit operator GoogleMapStyleColor(string textColor)
    {
        GoogleMapStyleColor color = new GoogleMapStyleColor();
        color.color = textColor;
        return color;
    }
}
    
public class GoogleMapStyleHue : GoogleMapStyleElement
{
    public string hue { get; set; }

    public static explicit operator GoogleMapStyleHue(string hueColor)
    {
        GoogleMapStyleHue hue = new GoogleMapStyleHue();
        hue.hue = hueColor;
        return hue;
    }
}

public class GoogleMapStyleVisibility : GoogleMapStyleElement
{
    public string visibility { get; set; }

    public static explicit operator GoogleMapStyleVisibility(bool isVisible)
    {
        GoogleMapStyleVisibility v = new GoogleMapStyleVisibility();
        v.visibility = isVisible ? "on" : "off";
        return v;
    }
        
    public static explicit operator GoogleMapStyleVisibility(MapStyleVisbility visbility)
    {
        GoogleMapStyleVisibility v = new GoogleMapStyleVisibility();
        v.visibility = visbility switch 
        {
            MapStyleVisbility.Off => "off",
            MapStyleVisbility.Simplified => "simplified",
            _ => "on"
        };
        return v;
    }
}

public class GoogleMapStyleLightness : GoogleMapStyleElement
{
    public int lightness { get; set; }

    public static explicit operator GoogleMapStyleLightness(int lightness)
    {
        return new GoogleMapStyleLightness
        {
            //Clamp the lightness value to be between -100 and + 100
            lightness = lightness < -100 ? -100 : lightness > 100 ? 100 : lightness
        };
    }
}

public class GoogleMapStyleSaturation : GoogleMapStyleElement
{
    public int saturation { get; set; }

    public static explicit operator GoogleMapStyleSaturation(int saturation)
    {
        return new GoogleMapStyleSaturation
        {
            //Clamp the saturation value to be between -100 and + 100
            saturation = saturation < -100 ? -100 : saturation > 100 ? 100 : saturation
        };
    }
}

public class GoogleMapStyleWeight : GoogleMapStyleElement
{
    public double weight { get; set; }

    public static explicit operator GoogleMapStyleWeight(double weight)
    {
        return new GoogleMapStyleWeight
        {
            //Clamp the weight value to be between 0 and 8
            weight = weight < 0 ? 0 : weight > 8 ? 8 : weight
        };
    }
}

/// <summary>
/// More info at <br />
/// https://developers.google.com/maps/documentation/javascript/examples/style-array?hl=fr
/// </summary>
public class GoogleMapStyleBuilder
{
    private List<MapTypeStyle> _styles = new List<MapTypeStyle>();

    public MapTypeStyle[] Build()
    {
        return _styles.ToArray();
    }

    /// <summary>
    /// AddVisibility("administrative", "", false).
    /// Color is casted into <see cref="GoogleMapStyleVisibility"/>  element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="visibility"></param>
    public GoogleMapStyleBuilder AddVisibility(string? featureType, string? elementType, bool visibility)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] { (GoogleMapStyleVisibility)visibility };
        _styles.Add(s);

        return this;
    }
        
    /// <summary>
    /// AddVisibility("administrative", "", MapStyleVisibility.Simplified).
    /// Color is casted into <see cref="GoogleMapStyleVisibility"/>  element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="visibility"></param>
    public GoogleMapStyleBuilder AddVisibility(string? featureType, string? elementType, MapStyleVisbility visibility)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] {(GoogleMapStyleVisibility) visibility};
        _styles.Add(s);

        return this;
    }

    /// <summary>
    /// For example AddColor("", "geometry", (GoogleMapStyleColor)"#1d2c4d");
    /// Color is casted into <see cref="GoogleMapStyleColor"/>  element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="colorHex"></param>
    /// <returns></returns>
    public GoogleMapStyleBuilder AddColor(string? featureType, string? elementType, string colorHex)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] { (GoogleMapStyleColor)colorHex };
        _styles.Add(s);

        return this;
    }
        
    /// <summary>
    /// For example AddHue("", "geometry", "#1d2c4d");
    /// Hue is casted into <see cref="GoogleMapStyleHue"/>  element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="hue"></param>
    /// <returns></returns>
    public GoogleMapStyleBuilder AddHue(string? featureType, string? elementType, string hue)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] {(GoogleMapStyleHue) hue};
        _styles.Add(s);

        return this;
    }

    /// <summary>
    /// AddLightness("administrative", "", 45).
    /// lightness is casted into <see cref="GoogleMapStyleLightness"/> element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="lightness">integer value between -100 and +100 : indicates the percentage change in brightness of the element.<br/>
    /// Negative values increase darkness (where -100 specifies black) while positive values increase brightness (where +100 specifies white).</param>
    public GoogleMapStyleBuilder AddLightness(string? featureType, string? elementType, int lightness)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] { (GoogleMapStyleLightness)lightness };
        _styles.Add(s);

        return this;
    }

    /// <summary>
    /// AddSaturation("administrative", "", -10).
    /// saturation is casted into <see cref="GoogleMapStyleSaturation"/> element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="saturation">integer value between -100 and +100 : indicates the percentage change in intensity of the basic color to apply to the element.<br/>
    /// E.g., -100 removes all intensity from the color of a feature, regardless of its starting color. The effect is to render the feature grayscale.</param>
    public GoogleMapStyleBuilder AddSaturation(string? featureType, string? elementType, int saturation)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] { (GoogleMapStyleSaturation)saturation };
        _styles.Add(s);

        return this;
    }

    /// <summary>
    /// AddWeight("administrative", "", 2.5).
    /// weight is casted into <see cref="GoogleMapStyleWeight"/> element
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="weight">decimal value between 0 and 8 : sets the weight of the feature, in pixels.<br/>
    /// Setting the weight to a high value may result in clipping near tile borders.</param>
    public GoogleMapStyleBuilder AddWeight(string? featureType, string? elementType, double weight)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] { (GoogleMapStyleWeight)weight };
        _styles.Add(s);

        return this;
    }

    /// <summary>
    /// Use AddColor, AddVisibility, AddLightness, AddSaturation, or AddWeight. Else use custom explicit operator inheritance object
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="element"></param>
    /// <returns></returns>
    public GoogleMapStyleBuilder AddStyle(string? featureType, string? elementType, GoogleMapStyleElement element)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = new[] { element };
        _styles.Add(s);

        return this;
    }
        
    /// <summary>
    /// Use custom explicit operator inheritance objects, possible to specify multiple object.
    /// Example: AddStyle(MapStyleFeatures.All, null, new GoogleMapsStyleHue { hue = "#e7ecf0" }, new GoogleMapsStyleWeight { weight = 2.8 })
    /// </summary>
    /// <param name="featureType"></param>
    /// <param name="elementType"></param>
    /// <param name="element"></param>
    /// <returns></returns>
    public GoogleMapStyleBuilder AddStyles(string? featureType, string? elementType, params GoogleMapStyleElement[] elements)
    {
        MapTypeStyle s = new MapTypeStyle();
        s.elementType = elementType;
        s.featureType = featureType;
        s.stylers = elements;
        _styles.Add(s);

        return this;
    }

    /// <summary>
    /// Json from https://mapstyle.withgoogle.com/
    /// </summary>
    /// <param name="json"></param>
    /// <returns></returns>
    public GoogleMapStyleBuilder AddStyle(string json)
    {
        var dirResult = JsonDocument.Parse(json);
        //Json is not a array
        if (dirResult == null || dirResult.RootElement.ValueKind != JsonValueKind.Array) return this;
            
        //Enumerate all elements and convert to MapStyle
        foreach (var style in dirResult.RootElement.EnumerateArray().Select(GetStyleType))
        {
            _styles.Add(style);
        }
        return this;
    }
        
    private static MapTypeStyle GetStyleType(JsonElement styleItem)
    {
        MapTypeStyle s = new MapTypeStyle();
        if (styleItem.TryGetProperty("elementType", out var elementType))
        {
            s.elementType = elementType.GetString();
        }
        if (styleItem.TryGetProperty("featureType", out var featureType))
        {
            s.featureType = featureType.GetString();
        }
            
        if (styleItem.TryGetProperty("stylers", out var stylersElement) && stylersElement.ValueKind == JsonValueKind.Array)
        {
            var stylers = new List<GoogleMapStyleElement>();
            foreach (var styler in stylersElement.EnumerateArray())
            {
                if (styler.TryGetProperty("visibility", out var visbility))
                {
                    var v = visbility.GetString();
                    if (!string.IsNullOrEmpty(v))
                    {
                        stylers.Add(new GoogleMapStyleVisibility {visibility = v});
                    }
                }
                    
                if (styler.TryGetProperty("color", out var color))
                {
                    var colorValue = color.GetString();
                    if (!string.IsNullOrEmpty(colorValue))
                    {
                        stylers.Add(new GoogleMapStyleColor {color = colorValue});
                    }
                }
                    
                if (styler.TryGetProperty("hue", out var hue))
                {
                    var hueValue = hue.GetString();
                    if (!string.IsNullOrEmpty(hueValue))
                    {
                        stylers.Add(new GoogleMapStyleHue { hue = hueValue});
                    }
                }

                if (styler.TryGetProperty("lightness", out var lightness) && lightness.TryGetInt32(out var lightnessValue))
                {
                    stylers.Add(new GoogleMapStyleLightness { lightness = lightnessValue});
                }

                if (styler.TryGetProperty("saturation", out var saturation) && saturation.TryGetInt32(out var saturationValue))
                {
                    stylers.Add(new GoogleMapStyleSaturation { saturation = saturationValue});
                }

                if (styler.TryGetProperty("weight", out var weight) && weight.TryGetDouble(out var weightValue))
                {
                    stylers.Add(new GoogleMapStyleWeight { weight = weightValue});
                }
            }

            s.stylers = stylers.ToArray();
        }
            
        return s;
    } 
}