# BlazorGoogleMaps
Blazor interop for GoogleMap library

[![NuGet version (BlazorGoogleMaps)](https://img.shields.io/nuget/v/BlazorGoogleMaps)](https://www.nuget.org/packages/BlazorGoogleMaps/)

## Usage
1. Add google map script HEAD tag to wwwroot/index.html in Client side or _Host.cshtml in Server Side.
How to get key follow https://developers.google.com/maps/documentation/javascript/get-api-key
```
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_GOES_HERE&v=3"></script>
```

To use Google recommended way by using bootstrap loader use 
```
services.AddBlazorGoogleMaps(opt =>
    {
        opt.KeyProvider = () => "YOUR_KEY_GOES_HERE";
        opt.UseBootstrapLoader = true;
        opt.Version = "beta";
        opt.Libraries = "places,visualization,drawing,marker";
    });
```

Add path to project javascript functions file in wwwroot/index.html in Client side or _Host.cshtml in Server Side.
```
<script src="_content/BlazorGoogleMaps/js/objectManager.js"></script>
```
If you want to use marker clustering add the following script to _Host.cshtml.
```
<script src="https://unpkg.com/@@googlemaps/markerclusterer/dist/index.min.js"></script>
```

2. Use component in client and server side same
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

## Samples
 Please check server side samples https://github.com/rungwiroon/BlazorGoogleMaps/tree/master/ServerSideDemo which are most to date
 
 ClientSide demos online
 https://rungwiroon.github.io/BlazorGoogleMaps/mapEvents


**Breaking change from 2.0.0**
LatLngLiteral constructor's parameters order changed #173

**Breaking change from 3.0.0**
Migrate from Newtonsoft.Json to System.Text.Json.

**Breaking change from 4.0.0**
Migrate to .net 8 #286.
