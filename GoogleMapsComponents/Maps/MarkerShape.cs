using System.Collections.Generic;

namespace GoogleMapsComponents.Maps;

/// <summary>
/// This object defines the clickable region of a marker image.<br/>
/// The shape consists of two properties — type and coord — which define the non-transparent region of an image.
/// </summary>
public class MarkerShape
{
    /// <summary>
    /// The format of this attribute depends on the value of the type and follows the w3 AREA coords specification found at http://www.w3.org/TR/REC-html40/struct/objects.html#adef-coords.<br/>
    /// The coords attribute is an array of integers that specify the pixel position of the shape relative to the top-left corner of the target image.The coordinates depend on the value of type as follows:<br/>
    /// - circle: coords is [x1, y1, r] where x1, y2 are the coordinates of the center of the circle, and r is the radius of the circle.<br/>
    /// - poly: coords is [x1, y1, x2, y2...xn, yn] where each x, y pair contains the coordinates of one vertex of the polygon.<br/>
    /// - rect: coords is [x1, y1, x2, y2] where x1, y1 are the coordinates of the upper-left corner of the rectangle and x2, y2 are the coordinates of the lower-right coordinates of the rectangle.
    /// </summary>
    public IEnumerable<float> Coords { get; set; }

    /// <summary>
    /// Describes the shape's type and can be: circle, poly or rect.
    /// </summary>
    public string Type { get; set; }
}