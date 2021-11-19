namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// This object is returned from various mouse events on the map and overlays.
    /// </summary>
    public class MouseEvent
    {
        /// <summary>
        /// The latitude/longitude that was below the cursor when the event occurred.
        /// </summary>
#pragma warning disable CS8618
        public LatLngLiteral LatLng { get; init; }
#pragma warning restore CS8618

        internal bool StopStatus { get; private set; } = false;

        /// <summary>
        /// Prevents this event from propagating further.
        /// </summary>
        public void Stop()
        {
            StopStatus = true;
        }
    }
}
