using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// Identifiers used to specify the placement of controls on the map. 
    /// Controls are positioned relative to other controls in the same layout position. 
    /// Controls that are added first are positioned closer to the edge of the map. 
    /// </summary>
    public enum ControlPosition
    {
        /// <summary>
        /// Elements are positioned in the center of the bottom row.
        /// </summary>
        //[EnumMember(Value = "BOTTOM_CENTER")]
        BottomCenter = 11,

        /// <summary>
        /// Elements are positioned in the bottom left and flow towards the middle. 
        /// Elements are positioned to the right of the Google logo.
        /// </summary>
        //[EnumMember(Value = "BOTTOM_LEFT")]
        BottomLeft = 10,

        /// <summary>
        /// Elements are positioned in the bottom right and flow towards the middle. 
        /// Elements are positioned to the left of the copyrights.
        /// </summary>
        //[EnumMember(Value = "BOTTOM_RIGHT")]
        BottomRight = 12,

        /// <summary>
        /// Elements are positioned on the left, above bottom-left elements, and flow upwards.
        /// </summary>
        //[EnumMember(Value = "LEFT_BOTTOM")]
        LeftBottom = 6,

        /// <summary>
        /// Elements are positioned in the center of the left side.
        /// </summary>
        //[EnumMember(Value = "LEFT_CENTER")]
        LeftCenter = 4,

        /// <summary>
        /// Elements are positioned on the left, below top-left elements, and flow downwards.
        /// </summary>
        //[EnumMember(Value = "LEFT_TOP")]
        LeftTop = 5,

        /// <summary>
        /// Elements are positioned on the right, above bottom-right elements, and flow upwards.
        /// </summary>
        //[EnumMember(Value = "RIGHT_BOTTOM")]
        RightBottom = 9,

        /// <summary>
        /// Elements are positioned in the center of the right side.
        /// </summary>
        //[EnumMember(Value = "RIGHT_CENTER")]
        RightCenter = 8,

        /// <summary>
        /// Elements are positioned on the right, below top-right elements, and flow downwards.
        /// </summary>
        //[EnumMember(Value = "RIGHT_TOP")]
        RightTop = 7,

        /// <summary>
        /// Elements are positioned in the center of the top row.
        /// </summary>
        //[EnumMember(Value = "TOP_CENTER")]
        TopCenter = 2,

        /// <summary>
        /// Elements are positioned in the top left and flow towards the middle.
        /// </summary>
        //[EnumMember(Value = "TOP_LEFT")]
        TopLeft = 1,

        /// <summary>
        /// Elements are positioned in the top right and flow towards the middle.
        /// </summary>
        //[EnumMember(Value = "TOP_RIGHT")]
        TopRight = 3
    }
}
