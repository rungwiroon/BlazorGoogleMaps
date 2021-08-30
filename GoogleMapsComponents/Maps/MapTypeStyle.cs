using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GoogleMapsComponents.Maps
{
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
        public string elementType { get; set; }

        /// <summary>
        /// The feature, or group of features, to which a styler should be applied. Optional. 
        /// If featureType is not specified, the value is assumed to be 'all'.
        /// </summary>
        public string featureType { get; set; }

        /// <summary>
        /// The style rules to apply to the selected map features and elements. 
        /// The rules are applied in the order that you specify in this array.
        /// </summary>
        public object[] stylers { get; set; }
    }

    public abstract class GoogleMapStyleElement
    {
    }

    public class GoogleMapStyleColor : GoogleMapStyleElement
    {
        public string color;

        public static explicit operator GoogleMapStyleColor(string textColor)
        {
            GoogleMapStyleColor color = new GoogleMapStyleColor();
            color.color = textColor;
            return color;
        }
    }

    public class GoogleMapStyleVisibility : GoogleMapStyleElement
    {
        public string visibility;

        public static explicit operator GoogleMapStyleVisibility(bool isVisible)
        {
            GoogleMapStyleVisibility v = new GoogleMapStyleVisibility();
            v.visibility = isVisible ? "on" : "off";
            return v;
        }
    }

    public class GoogleMapStyleLightness : GoogleMapStyleElement
    {
        public int lightness;

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
        public int saturation;

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
        public double weight;

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
        public GoogleMapStyleBuilder AddVisibility(string featureType, string elementType, bool visibility)
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
        public GoogleMapStyleBuilder AddColor(string featureType, string elementType, string colorHex)
        {
            MapTypeStyle s = new MapTypeStyle();
            s.elementType = elementType;
            s.featureType = featureType;
            s.stylers = new[] {(GoogleMapStyleColor) colorHex};
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
        public GoogleMapStyleBuilder AddLightness(string featureType, string elementType, int lightness)
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
        public GoogleMapStyleBuilder AddSaturation(string featureType, string elementType, int saturation)
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
        public GoogleMapStyleBuilder AddWeight(string featureType, string elementType, double weight)
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
        public GoogleMapStyleBuilder AddStyle(string featureType, string elementType, GoogleMapStyleElement element)
        {
            MapTypeStyle s = new MapTypeStyle();
            s.elementType = elementType;
            s.featureType = featureType;
            s.stylers = new[] {element};
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
            var dirResult = JsonConvert.DeserializeObject<dynamic>(json);
            foreach (var styleItem in dirResult)
            {
                MapTypeStyle s = new MapTypeStyle();
                s.elementType = styleItem.elementType;
                s.featureType = styleItem.featureType;
                if (IsPropertyExist(styleItem, "stylers"))
                {
                    if (IsPropertyExist(styleItem.stylers[0], "visibility"))
                    {
                        string stylerValue = styleItem.stylers[0].visibility?.ToString();
                        if (!string.IsNullOrEmpty(stylerValue))
                        {
                            s.stylers = new[] {new GoogleMapStyleVisibility() {visibility = stylerValue}};
                        }
                    }

                    if (IsPropertyExist(styleItem.stylers[0], "color"))
                    {
                        string stylerValue = styleItem.stylers[0].color?.ToString();
                        if (!string.IsNullOrEmpty(stylerValue))
                        {
                            s.stylers = new[] {new GoogleMapStyleColor() {color = stylerValue}};
                        }
                    }

                    if (IsPropertyExist(styleItem.stylers[0], "lightness"))
                    {
                        string stylerValue = styleItem.stylers[0].lightness?.ToString();
                        if (!string.IsNullOrEmpty(stylerValue) && int.TryParse(stylerValue, out int l))
                        {
                            s.stylers = new[] { new GoogleMapStyleLightness { lightness = l } };
                        }
                    }

                    if (IsPropertyExist(styleItem.stylers[0], "saturation"))
                    {
                        string stylerValue = styleItem.stylers[0].saturation?.ToString();
                        if (!string.IsNullOrEmpty(stylerValue) && int.TryParse(stylerValue, out int sat))
                        {
                            s.stylers = new[] { new GoogleMapStyleSaturation { saturation = sat } };
                        }
                    }

                    if (IsPropertyExist(styleItem.stylers[0], "weight"))
                    {
                        string stylerValue = styleItem.stylers[0].weight?.ToString();
                        if (!string.IsNullOrEmpty(stylerValue) && double.TryParse(stylerValue, out double w))
                        {
                            s.stylers = new[] { new GoogleMapStyleWeight { weight = w } };
                        }
                    }
                }

                _styles.Add(s);
            }

            return this;
        }

        /// <summary>
        /// Only works for json dynamic
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool IsPropertyExist(dynamic settings, string name)
        {
            try
            {
                var value = settings[name];
                return true;
            }
            catch (KeyNotFoundException)
            {
                return false;
            }
        }
    }
}