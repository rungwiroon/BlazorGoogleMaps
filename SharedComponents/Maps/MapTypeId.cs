using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents.Maps
{
    /// <summary>
    /// Identifiers for common MapTypes
    /// </summary>
    public class MapTypeId
    {
        /// <summary>
        /// This map type displays a transparent layer of major streets on satellite images.
        /// </summary>
        public const string HYBRID = "HYBRID";

        /// <summary>
        /// This map type displays a normal street map.
        /// </summary>
        public const string ROADMAP = "ROADMAP";

        /// <summary>
        /// This map type displays satellite images.
        /// </summary>
        public const string SATELLITE = "SATELLITE";

        /// <summary>
        /// This map type displays maps with physical features such as terrain and vegetation.
        /// </summary>
        public const string TERRAIN = "TERRAIN";
    }
}
