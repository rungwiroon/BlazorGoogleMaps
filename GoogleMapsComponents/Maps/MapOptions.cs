using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// MapOptions object used to define the properties that can be set on a Map.
/// </summary>
public class MapOptions
{
    /// <summary>
    /// Color used for the background of the Map div.<br/>
    /// This color will be visible when tiles have not yet loaded as the user pans.<br/>
    /// This option can only be set when the map is initialized.
    /// </summary>
    public string BackgroundColor { get; set; }

    /// <summary>
    /// The initial Map center. Required.
    /// </summary>
    public LatLngLiteral? Center { get; set; }

    /// <summary>
    /// When false, map icons are not clickable.<br/>
    /// A map icon represents a point of interest, also known as a POI.<br/>
    /// By default map icons are clickable.
    /// </summary>
    public bool? ClickableIcons { get; set; }

    /// <summary>
    /// Enables/disables all default UI. May be overridden individually.
    /// </summary>
    public bool? DisableDefaultUI { get; set; }

    /// <summary>
    /// Enables/disables zoom and center on double click. Enabled by default.
    /// </summary>
    public bool? DisableDoubleClickZoom { get; set; }

    /// <summary>
    /// If false, prevents the map from being dragged. Dragging is enabled by default.
    /// </summary>
    public bool? Draggable { get; set; }

    /// <summary>
    /// The name or url of the cursor to display when mousing over a draggable map.<br/>
    /// This property uses the css cursor attribute to change the icon.<br/>
    /// As with the css property, you must specify at least one fallback cursor that is not a URL.<br/>
    /// For example: <c>draggableCursor: 'url(http://www.example.com/icon.png), auto;'</c>.
    /// </summary>
    public string DraggableCursor { get; set; }

    /// <summary>
    /// The name or url of the cursor to display when the map is being dragged.<br/>
    /// This property uses the css cursor attribute to change the icon.<br/>
    /// As with the css property, you must specify at least one fallback cursor that is not a URL.<br/>
    /// For example: <c>draggingCursor: 'url(http://www.example.com/icon.png), auto;'</c>.
    /// </summary>
    public string DraggingCursor { get; set; }

    /// <summary>
    /// The enabled/disabled state of the Fullscreen control.
    /// </summary>
    public bool? FullscreenControl { get; set; }

    /// <summary>
    /// The display options for the Fullscreen control.
    /// </summary>
    public FullscreenControlOptions? FullscreenControlOptions { get; set; }

    /// <summary>
    /// This setting controls how the API handles gestures on the map. Allowed values:
    /// <list type="table">
    /// <item><term>"cooperative"</term><description> Scroll events and one-finger touch gestures scroll the page, and do not zoom or pan the map. Two-finger touch gestures pan and zoom the map. Scroll events with a ctrl key or ⌘ key pressed zoom the map. In this mode the map cooperates with the page.</description></item>
    /// <item><term>"greedy"</term><description> All touch gestures and scroll events pan or zoom the map.</description></item>
    /// <item><term>"none"</term><description> The map cannot be panned or zoomed by user gestures.</description></item>
    /// <item><term>"auto"</term><description> (default) Gesture handling is either cooperative or greedy, depending on whether the page is scrollable or in an iframe.</description></item>
    /// </list>
    /// </summary>
    public string GestureHandling { get; set; }

    /// <summary>
    /// The heading for aerial imagery in degrees measured clockwise from cardinal direction North.<br/>
    /// Headings are snapped to the nearest available angle for which imagery is available.
    /// </summary>
    public int? Heading { get; set; }

    /// <summary>
    /// The heading for aerial imagery in degrees measured clockwise from cardinal direction North.<br/>
    /// Headings are snapped to the nearest available angle for which imagery is available.
    /// </summary>
    public bool? KeyboardShortcuts { get; set; }

    /// <summary>
    /// The initial enabled/disabled state of the Map type control.
    /// </summary>
    public bool? MapTypeControl { get; set; }

    /// <summary>
    /// The initial display options for the Map type control.
    /// </summary>
    public MapTypeControlOptions? MapTypeControlOptions { get; set; }

    /// <summary>
    /// The initial Map mapTypeId. Defaults to ROADMAP.
    /// </summary>
    [JsonConverter(typeof(EnumMemberConverter<MapTypeId>))]
    public MapTypeId MapTypeId { get; set; }

    /// <summary>
    /// The maximum zoom level which will be displayed on the map.<br/>
    /// If omitted, or set to null, the maximum zoom from the current map type is used instead.<br/>
    /// Valid values: Integers between zero, and up to the supported maximum zoom level.
    /// </summary>
    public int? MaxZoom { get; set; }

    /// <summary>
    /// The minimum zoom level which will be displayed on the map.<br/>
    /// If omitted, or set to null, the minimum zoom from the current map type is used instead.<br/>
    /// Valid values: Integers between zero, and up to the supported maximum zoom level.
    /// </summary>
    public int? MinZoom { get; set; }

    /// <summary>
    /// If true, do not clear the contents of the Map div.
    /// </summary>
    public bool? NoClear { get; set; }

    /// <summary>
    /// The enabled/disabled state of the Pan control.
    /// </summary>
    public bool? PanControl { get; set; }

    /// <summary>
    /// The display options for the Pan control.
    /// </summary>
    public PanControlOptions? PanControlOptions { get; set; }

    /// <summary>
    /// Defines a boundary that restricts the area of the map accessible to users.<br/>
    /// When set, a user can only pan and zoom while the camera view stays inside the limits of the boundary.
    /// </summary>
    public MapRestriction? Restriction { get; set; }

    /// <summary>
    /// The enabled/disabled state of the Rotate control.
    /// </summary>
    public bool? RotateControl { get; set; }

    /// <summary>
    /// The display options for the Rotate control.
    /// </summary>
    public RotateControlOptions? RotateControlOptions { get; set; }

    /// <summary>
    /// The initial enabled/disabled state of the Scale control.
    /// </summary>
    public bool? ScaleControl { get; set; }

    /// <summary>
    /// The initial display options for the Scale control.
    /// </summary>
    public ScaleControlOptions? ScaleControlOptions { get; set; }

    /// <summary>
    /// If false, disables zooming on the map using a mouse scroll wheel.<br/>
    /// The scrollwheel is enabled by default.
    /// </summary>
    public bool? Scrollwheel { get; set; }

    ///// <summary>
    ///// A StreetViewPanorama to display when the Street View pegman is dropped on the map.<br/>
    ///// If no panorama is specified, a default StreetViewPanorama will be displayed in the map's div when the pegman is dropped.
    ///// </summary>
    //public StreetViewPanorama streetView { get; set; }

    /// <summary>
    /// The initial enabled/disabled state of the Street View Pegman control.<br/>
    /// This control is part of the default UI, and should be set to false when displaying a map type on which the Street View road overlay should not appear (e.g. a non-Earth map type).
    /// </summary>
    public bool? StreetViewControl { get; set; }

    /// <summary>
    /// The initial display options for the Street View Pegman control.
    /// </summary>
    public StreetViewControlOptions? StreetViewControlOptions { get; set; }

    /// <summary>
    /// Styles to apply to each of the default map types.<br/>
    /// Note that for satellite/hybrid and terrain modes, these styles will only apply to labels and geometry.
    /// </summary>
    public MapTypeStyle[] Styles { get; set; }

    /// <summary>
    /// Controls the automatic switching behavior for the angle of incidence of the map.<br/>
    /// The only allowed values are 0 and 45. The value 0 causes the map to always use a 0° overhead view regardless of the zoom level and viewport.<br/>
    /// The value 45 causes the tilt angle to automatically switch to 45 whenever 45° imagery is available for the current zoom level and viewport, and
    /// switch back to 0 whenever 45° imagery is not available (this is the default behavior). 45° imagery is only available for satellite and hybrid map
    /// types, within some locations, and at some zoom levels.<br/>
    /// Note: getTilt returns the current tilt angle, not the value specified by this option.
    /// </summary>
    public int? Tilt { get; set; }

    /// <summary>
    /// The initial Map zoom level.<br/>
    /// Required.<br/>
    /// Valid values: Integers between zero, and up to the supported maximum zoom level.
    /// </summary>
    public int? Zoom { get; set; }

    /// <summary>
    /// The enabled/disabled state of the Zoom control.
    /// </summary>
    public bool? ZoomControl { get; set; }

    /// <summary>
    /// The display options for the Zoom control.
    /// </summary>
    public ZoomControlOptions? ZoomControlOptions { get; set; }

    /// <summary>
    /// Type:  string optional<br/>
    /// The unique identifier that represents a single instance of a Google Map.<br/>
    /// You can create Map IDs and update a style associated with a
    /// Map ID at any time in the Google Cloud Console Maps Management
    /// page without changing embedded JSON styling in your application code.
    /// </summary>
    public string? MapId { get; set; }
}