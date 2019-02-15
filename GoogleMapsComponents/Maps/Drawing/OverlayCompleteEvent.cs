using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Drawing
{
    /// <summary>
    /// The properties of an overlaycomplete event on a DrawingManager.
    /// </summary>
    public class OverlayCompleteEvent
    {
        /// <summary>
        /// The completed overlay.
        /// </summary>
        public object Overlay { get; set; }

        /// <summary>
        /// The completed overlay's type.
        /// </summary>
        public OverlayType Type { get; set; }
    }
}
