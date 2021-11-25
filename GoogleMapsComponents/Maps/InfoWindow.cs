using Microsoft.JSInterop;
using OneOf;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static GoogleMapsComponents.Helper;

namespace GoogleMapsComponents.Maps
{
    /// <summary>
    /// A class able to manage a lot of Marker objects and get / set their properties at the same time, eventually with different values
    /// 
    /// Main concept is that each Marker to can be distinguished by other ones need to have a "unique key" with a "external world mean", so not necessary it's GUID
    /// In real cases Markers are be linked to places, activities, transit stops and so on -> So, what better way to choose as Marker "unique key" the "id" of the object each marker is related to?
    /// A string key has been selected as type due to its implicit versatility.
    /// 
    /// To create Markers, simply call MarkerList.CreateAsync with a Dictionary of desired Marker keys and MarkerOptions values
    /// After that, a new instance of MarkerList class will be returned with its Markers dictionary member populated with the corresponding results
    /// 
    /// At run-time is possible to: 
    /// 1) add Marker to the same MarketList class using AddMultipleAsync method (only keys not matching with existent Marker keys will be created)
    ///    Markers dictionary will contains "union distinct" of existent Marker's keys and new keys
    /// 2) remove Marker from the MarketList class (only Marker having keys matching with existent keys will be removed)
    ///    Markers dictionary will contains "original - required and found" Marker's keys (eventually any is all Marker are removed)
    /// 
    /// Each definer getter properties can be used as follow: 
    /// a) without parameter -> all eventually defined markers related property will be returned (if any)
    /// b) with a List<string> of keys -> all eventually mathing keys with Markers Dictionary keys produces related merkers property extracion (if any defined)
    /// 
    /// Each setter properties can be used as follow:
    /// With a Dictionary<string, {property type}> indicating for each Marker (related to that key) the corresponding related property value
    /// </summary>
    [JsonConverter(typeof(JSObjectRefConverter))]
    public class InfoWindow : MVCObject
    {
        /// <summary>
        /// Creates an info window with the given options. 
        /// An InfoWindow can be placed on a map at a particular position or above a marker, depending on what is specified in the options. 
        /// Unless auto-pan is disabled, an InfoWindow will pan the map to make itself visible when it is opened. 
        /// After constructing an InfoWindow, you must call open to display it on the map. 
        /// The user can click the close button on the InfoWindow to remove it from the map, or the developer can call close() for the same effect.
        /// </summary>
        /// <param name="opts"></param>
        public async static ValueTask<InfoWindow> CreateAsync(IJSRuntime jsRuntime, InfoWindowOptions? opts = null)
        {
            var jsObjectRef = await jsRuntime.InvokeAsync<IJSObjectReference>(
                "googleMapsObjectManager.createMVCObject",
                "google.maps.InfoWindow",
                opts);

            var obj = new InfoWindow(jsObjectRef);

            return obj;
        }

        internal InfoWindow(IJSObjectReference jsObjectRef)
            : base(jsObjectRef)
        {
        }

        /// <summary>
        /// Closes this InfoWindow by removing it from the DOM structure.
        /// </summary>
        public ValueTask Close()
        {
            return this.InvokeVoidAsync(
                "close");
        }

        public ValueTask<string> GetContent()
        {
            return this.InvokeAsync<string>(
                "getContent");
        }

        public ValueTask<LatLngLiteral> GetPosition()
        {
            return this.InvokeAsync<LatLngLiteral>(
                "getPosition");
        }

        public ValueTask<int> GetZIndex()
        {
            return this.InvokeAsync<int>(
                "getZIndex");
        }


        /// <summary>
        /// Opens this InfoWindow on the given map.
        /// </summary>
        /// <param name="map"></param>
        /// <param name="anchor"></param>
        public ValueTask Open(
            OneOf<InfoWindowOpenOptions, Map, StreetViewPanoramaMap>? options,
            MVCObject? anchor = null)
        {
            return this.InvokeVoidAsync(
                "open",
                MakeArgJsFriendly(options),
                MakeArgJsFriendly(anchor));
        }

        public ValueTask SetContent(string content)
        {
            return this.InvokeVoidAsync(
                "setContent",
                content);
        }

        public ValueTask SetPosition(LatLngLiteral position)
        {
            return this.InvokeVoidAsync(
                "setPosition",
                position);
        }

        public ValueTask SetZIndex(int zIndex)
        {
            return this.InvokeVoidAsync(
                "setZIndex",
                zIndex);
        }
    }
}
