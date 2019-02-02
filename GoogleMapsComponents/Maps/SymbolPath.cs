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
    [JsonConverter(typeof(StringEnumConverter))]
    public enum SymbolPath
    {
        /// <summary>
        /// A backward-pointing closed arrow.
        /// </summary>
        [EnumMember(Value = "backward_closed_arrow")]
        BACKWARD_CLOSED_ARROW,

        /// <summary>
        /// A backward-pointing open arrow.
        /// </summary>
        [EnumMember(Value = "backward_open_arrow")]
        BACKWARD_OPEN_ARROW,

        /// <summary>
        /// A circle.
        /// </summary>
        [EnumMember(Value = "circle")]
        CIRCLE,

        /// <summary>
        /// A forward-pointing closed arrow.
        /// </summary>
        [EnumMember(Value = "forward_closed_arrow")]
        FORWARD_CLOSED_ARROW,

        /// <summary>
        /// A forward-pointing open arrow.
        /// </summary>
        [EnumMember(Value = "forward_open_arrow")]
        FORWARD_OPEN_ARROW
    }
}
