using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Places
{
    public class QueryAutocompletePrediction
    {
        public string Description { get; set; }
        public PredictionSubstring[] MatchedSubstrings { get; set; }
        public string PlaceId { get; set; }
        public PredictionTerm[] Terms { get; set; }
    }
}
