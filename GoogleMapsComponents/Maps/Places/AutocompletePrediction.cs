namespace GoogleMapsComponents.Maps.Places
{
    public class AutocompletePrediction
    {
        public string Description { get; set; }
        public int DistanceMeters { get; set; }
        public PredictionSubstring[] MatchedSubstrings { get; set; }
        public string PlaceId { get; set; }
        public StructuredFormatting StructuredFormatting { get; set; }
        public PredictionTerm[] Terms { get; set; }
        public string[] Types { get; set; }
    }
}
