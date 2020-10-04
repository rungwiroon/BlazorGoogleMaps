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
        /// Use AddColor or AddVisibility. Else use custom explicit operator inheritance object
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