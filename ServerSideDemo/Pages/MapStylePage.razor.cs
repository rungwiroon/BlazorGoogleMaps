using System.Threading.Tasks;
using GoogleMapsComponents;
using GoogleMapsComponents.Maps;

namespace ServerSideDemo.Pages;

public partial class MapStylePage
{
    private GoogleMap map1;

    private MapOptions mapOptions;

    private LatLngBounds bounds;

    protected override void OnInitialized()
    {
        mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral()
            {
                Lat = 13.505892,
                Lng = 100.8162
            },
            MapTypeId = MapTypeId.Roadmap,
            Styles = MapStyle()
        };
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            bounds = await LatLngBounds.CreateAsync(map1.JsRuntime);
        }
    }

    public MapTypeStyle[] MapStyle()
    {
        GoogleMapStyleBuilder style = new GoogleMapStyleBuilder();

        style.AddStyle("[  {    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#f5f5f5\"      }    ]  },  {    \"elementType\": \"labels.icon\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#616161\"      }    ]  },  {    \"elementType\": \"labels.text.stroke\",    \"stylers\": [      {        \"color\": \"#f5f5f5\"      }    ]  },  {    \"featureType\": \"administrative.land_parcel\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"administrative.land_parcel\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#bdbdbd\"      }    ]  },  {    \"featureType\": \"administrative.neighborhood\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"poi\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#eeeeee\"      }    ]  },  {    \"featureType\": \"poi\",    \"elementType\": \"labels.text\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"poi\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#757575\"      }    ]  },  {    \"featureType\": \"poi.business\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"poi.park\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#e5e5e5\"      }    ]  },  {    \"featureType\": \"poi.park\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#9e9e9e\"      }    ]  },  {    \"featureType\": \"road\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#ffffff\"      }    ]  },  {    \"featureType\": \"road\",    \"elementType\": \"labels\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"road\",    \"elementType\": \"labels.icon\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"road.arterial\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"road.arterial\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#757575\"      }    ]  },  {    \"featureType\": \"road.highway\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#dadada\"      }    ]  },  {    \"featureType\": \"road.highway\",    \"elementType\": \"labels\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"road.highway\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#616161\"      }    ]  },  {    \"featureType\": \"road.local\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"road.local\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#9e9e9e\"      }    ]  },  {    \"featureType\": \"transit\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"transit.line\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#e5e5e5\"      }    ]  },  {    \"featureType\": \"transit.station\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#eeeeee\"      }    ]  },  {    \"featureType\": \"water\",    \"elementType\": \"geometry\",    \"stylers\": [      {        \"color\": \"#c9c9c9\"      }    ]  },  {    \"featureType\": \"water\",    \"elementType\": \"labels.text\",    \"stylers\": [      {        \"visibility\": \"off\"      }    ]  },  {    \"featureType\": \"water\",    \"elementType\": \"labels.text.fill\",    \"stylers\": [      {        \"color\": \"#9e9e9e\"      }    ]  }]");
        //
        return
            //style.AddStyle("", "geometry", (GoogleMapStyleColor) "#1d2c4d")
            //.AddStyle("", "labels.text.fill", (GoogleMapStyleColor) "#8ec3b9")
            //.AddStyle("", "labels.text.stroke", (GoogleMapStyleColor) "#1a3646")
            //.AddStyle("administrative", "", (GoogleMapStyleVisibility) false)
            //.AddStyle("administrative.country", "geometry.stroke", (GoogleMapStyleColor) "#4b6878")
            //.AddColor("administrative.land_parcel", "geometry.stroke", "#64779e")
            //.AddStyle("administrative.province", "geometry.stroke", (GoogleMapStyleColor) "#4b6878")
            //.AddStyle("landscape.man_made", "geometry.stroke", (GoogleMapStyleColor) "#334e87")
            //.AddStyle("landscape.natural", "geometry", (GoogleMapStyleColor) "#023e58")
            //.AddStyle("poi", "geometry", (GoogleMapStyleColor) "#283d6a")
            //.AddStyle("poi", "labels.text.fill", (GoogleMapStyleColor) "#6f9ba5")
            //.AddStyle("poi", "labels.text.stroke", (GoogleMapStyleColor) "#1d2c4d")
            //.AddStyle("poi.park", "geometry.fill", (GoogleMapStyleColor) "#1d2c4d")
            //.AddStyle("road", "geometry", (GoogleMapStyleColor) "#304a7d")
            //.AddStyle("road", "labels.text.fill", (GoogleMapStyleColor) "#98a5be")
            //.AddStyle("road", "labels.text.stroke", (GoogleMapStyleColor) "#1d2c4d")
            //.AddStyle("road.highway", "geometry", (GoogleMapStyleColor) "#2c6675")
            //.AddStyle("road.highway", "geometry.stroke", (GoogleMapStyleColor) "#255763")
            //.AddStyle("road.highway", "labels.text.fill", (GoogleMapStyleColor) "#b0d5ce")
            //.AddStyle("road.highway", "labels.text.stroke", (GoogleMapStyleColor) "#023e58")
            style.Build();
    }
}