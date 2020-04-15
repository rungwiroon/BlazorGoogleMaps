using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// Built-in symbol paths.
    /// </summary>
    public enum SymbolPath
    {
        /// <summary>
        /// A backward-pointing closed arrow.
        /// </summary>
        BACKWARD_CLOSED_ARROW = 3,

        /// <summary>
        /// A backward-pointing open arrow.
        /// </summary>
        BACKWARD_OPEN_ARROW = 4,

        /// <summary>
        /// A circle.
        /// </summary>
        CIRCLE = 0,

        /// <summary>
        /// A forward-pointing closed arrow.
        /// </summary>
        FORWARD_CLOSED_ARROW = 1,

        /// <summary>
        /// A forward-pointing open arrow.
        /// </summary>
        FORWARD_OPEN_ARROW = 2
    }
}
