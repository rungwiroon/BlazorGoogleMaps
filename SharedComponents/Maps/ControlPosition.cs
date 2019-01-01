using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// Identifiers used to specify the placement of controls on the map. 
    /// Controls are positioned relative to other controls in the same layout position. 
    /// Controls that are added first are positioned closer to the edge of the map. 
    /// </summary>
    public class ControlPosition
    {
        /// <summary>
        /// Elements are positioned in the center of the bottom row.
        /// </summary>
        public const string BOTTOM_CENTER = "BOTTOM_CENTER";

        /// <summary>
        /// Elements are positioned in the bottom left and flow towards the middle. 
        /// Elements are positioned to the right of the Google logo.
        /// </summary>
        public const string BOTTOM_LEFT = "BOTTOM_LEFT";

        /// <summary>
        /// Elements are positioned in the bottom right and flow towards the middle. 
        /// Elements are positioned to the left of the copyrights.
        /// </summary>
        public const string BOTTOM_RIGHT = "BOTTOM_RIGHT";

        /// <summary>
        /// Elements are positioned on the left, above bottom-left elements, and flow upwards.
        /// </summary>
        public const string LEFT_BOTTOM = "LEFT_BOTTOM";

        /// <summary>
        /// Elements are positioned in the center of the left side.
        /// </summary>
        public const string LEFT_CENTER = "LEFT_CENTER";

        /// <summary>
        /// Elements are positioned on the left, below top-left elements, and flow downwards.
        /// </summary>
        public const string LEFT_TOP = "LEFT_TOP";

        /// <summary>
        /// Elements are positioned on the right, above bottom-right elements, and flow upwards.
        /// </summary>
        public const string RIGHT_BOTTOM = "RIGHT_BOTTOM";

        /// <summary>
        /// Elements are positioned in the center of the right side.
        /// </summary>
        public const string RIGHT_CENTER = "RIGHT_CENTER";

        /// <summary>
        /// Elements are positioned on the right, below top-right elements, and flow downwards.
        /// </summary>
        public const string RIGHT_TOP = "RIGHT_TOP";

        /// <summary>
        /// Elements are positioned in the center of the top row.
        /// </summary>
        public const string TOP_CENTER = "TOP_CENTER";

        /// <summary>
        /// Elements are positioned in the top left and flow towards the middle.
        /// </summary>
        public const string TOP_LEFT = "TOP_LEFT";

        /// <summary>
        /// Elements are positioned in the top right and flow towards the middle.
        /// </summary>
        public const string TOP_RIGHT = "TOP_RIGHT";
    }
}
