using GoogleMapsComponents.JsonConverters;
using GoogleMapsComponents.Maps.Drawing;
using Microsoft.JSInterop;
using OneOf;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// The properties of an overlaycomplete event on a DrawingManager.
    /// https://developers.google.com/maps/documentation/javascript/reference/drawing#OverlayCompleteEvent
    /// </summary>
    [JsonConverter(typeof(JSObjPropsRefConverter<OverlayCompleteEventBridge, OverlayCompleteEvent>))]
    public class OverlayCompleteEvent
    {
        /// <summary>
        /// The completed overlay.
        /// </summary>
        public OneOf<Marker, Polygon, Polyline, Rectangle, Circle> Overlay { get; init; } = default!;

        /// <summary>
        /// The completed overlay's type.
        /// </summary>
        public OverlayType Type { get; init; } = default!;
    }

    internal class OverlayCompleteEventBridge : IBridgeDto<OverlayCompleteEvent>
    {
        public IJSObjectReference OverlayReference { get; init; } = default!;

        public OverlayType Type { get; init; } = default!;

        public OverlayCompleteEvent ConvertToDestinationType()
        {
            return new OverlayCompleteEvent()
            {
                Overlay = Type switch
                {
                    OverlayType.Circle => new Circle(OverlayReference),
                    OverlayType.Marker => new Marker(OverlayReference),
                    OverlayType.Polygon => new Polygon(OverlayReference),
                    OverlayType.Polyline => new Polyline(OverlayReference),
                    OverlayType.Rectangle => new Rectangle(OverlayReference),
                    _ => throw new System.NotImplementedException(),
                },
                Type = Type,
            };
        }
    }
}
