# BlazorGoogleMaps
Blazor interop for GoogleMap library

[![NuGet version (BlazorGoogleMaps)](https://img.shields.io/nuget/v/BlazorGoogleMaps)](https://www.nuget.org/packages/BlazorGoogleMaps/)

## Usage
1. Provide your Google API key to BlazorGoogleMaps with one of the following methods. (You can get a key here: https://developers.google.com/maps/documentation/javascript/get-api-key)

Use the bootstrap loader with a key service (recommended):
```
services.AddBlazorGoogleMaps("YOUR_KEY_GOES_HERE");
```
OR specify google api libraries and/or version:
```
services.AddBlazorGoogleMaps(new GoogleMapsComponents.Maps.MapApiLoadOptions("YOUR_KEY_GOES_HERE")
    {
        Version = "beta",
        Libraries = "places,visualization,drawing,marker",
    });
```
OR to do something more complex (e.g. looking up keys asynchronously), implement a Scoped key service and add it with something like:
```
services.AddScoped<IBlazorGoogleMapsKeyService, YourServiceImplementation>();
```

OR (legacy - not recommended) Add google map script HEAD tag to wwwroot/index.html in Client side or _Host.cshtml in Server Side.
```
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_GOES_HERE&v=3"></script>
```


2. Add path to project javascript functions file in wwwroot/index.html for Blazor WASM, or in _Host.cshtml or _HostLayout.cshtml for Blazor Server.
```
<script src="_content/BlazorGoogleMaps/js/objectManager.js"></script>
```
If you want to use marker clustering add this script as well:
```
<script src="https://unpkg.com/@googlemaps/markerclusterer/dist/index.min.js"></script>
```

3. Using the component is the same for both Blazor WASM and Blazor Server
```
@page "/map"
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>
<div style="height:@Height">
<GoogleMap @ref="@_map1" Id="map1" Options="@mapOptions" Height="100%" OnAfterInit="AfterMapRender"></GoogleMap>
</div>
@functions {
	private GoogleMap _map1;
	private MapOptions mapOptions;	

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
			MapTypeId = MapTypeId.Roadmap
		};
	}

	 private async Task AfterMapRender()
	 {
	     _bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);
	 }		
}
```

OR Render markers with Blazor (currently only with `v=beta` version of google-maps, and specify a `MapId`)
```
@page "/map"
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>
<AdvancedGoogleMap @ref="@_map1" Id="map1" Options="@mapOptions">
    @foreach (var markerRef in Markers)
    {
        <MarkerComponent 
            @key="markerRef.Id" 
            Lat="@markerRef.Lat" 
            Lng="@markerRef.Lng" 
            Clickable="@markerRef.Clickable" 
            Draggable="@markerRef.Draggable" 
            OnClick="@(() => markerRef.Active = !markerRef.Active)"
            OnMove="pos => markerRef.UpdatePosition(pos)">
            <p>I am a blazor component</p>
        </MarkerComponent>
    }
</AdvancedGoogleMap>
@code {
    private List<MarkerData> Markers =
    [
        new MarkerData { Id = 1, Lat = 13.505892, Lng = 100.8162 },
    ];
	private AdvancedGoogleMap? _map1;
	private MapOptions mapOptions =new MapOptions()
	{
		Zoom = 13,
		Center = new LatLngLiteral()
		{
			Lat = 13.505892,
			Lng = 100.8162
		},
		MapId = "DEMO_MAP_ID", //required for blazor markers
		MapTypeId = MapTypeId.Roadmap
	};	

    public class MarkerData
    {
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public bool Clickable { get; set; } = true;
        public bool Draggable { get; set; }
        public bool Active { get; set; }
        
        public void UpdatePosition(LatLngLiteral position)
        {
            Lat = position.Lat;
            Lng = position.Lng;
        }
    }
}
```

## Samples
 Please check server side samples https://github.com/rungwiroon/BlazorGoogleMaps/tree/master/ServerSideDemo which are most to date
 
 ClientSide demos online
 https://rungwiroon.github.io/BlazorGoogleMaps/mapEvents

**Breaking change from 4.0.0**
Migrate to .NET 8 #286.

**Breaking change from 3.0.0**
Migrate from Newtonsoft.Json to System.Text.Json.

**Breaking change from 2.0.0**
LatLngLiteral constructor's parameters order changed #173
