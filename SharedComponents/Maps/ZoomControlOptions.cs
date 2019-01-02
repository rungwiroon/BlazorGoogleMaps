using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// Options for the rendering of the zoom control.
    /// </summary>
    public class ZoomControlOptions
    {
        /// <summary>
        /// Position id. Used to specify the position of the control on the map. 
        /// The default position is TOP_LEFT.
        /// </summary>
        public ControlPosition position { get; set; }
    }
}
