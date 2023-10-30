# BlazorGoogleMaps
Blazor interop for GoogleMap library

**Breaking change from 2.0.0**
LatLngLiteral constructor's parameters order changed #173

**Breaking change from 3.0.0**
Migrate from Newtonsoft.Json to System.Text.Json.

[![NuGet version (BlazorGoogleMaps)](https://img.shields.io/nuget/v/BlazorGoogleMaps)](https://www.nuget.org/packages/BlazorGoogleMaps/)

## Usage
1. Add google map script HEAD tag to wwwroot/index.html in Client side or _Host.cshtml in Server Side.
How to get key follow https://developers.google.com/maps/documentation/javascript/get-api-key

Do not forgot addition libraries if required *&libraries=places,visualization,drawing*

If you got 'Loading the Google Maps JavaScript API without a callback is not supported' then add *&callback=Function.prototype*
```
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_GOES_HERE&v=3"></script>
```
Add path to project javascript functions file in wwwroot/index.html in Client side or _Host.cshtml in Server Side.
```
<script src="_content/BlazorGoogleMaps/js/objectManager.js"></script>
```
If you want to use marker clustering in a Server Side project then add the following script to _Host.cshtml.
```
<script src="https://unpkg.com/@@googlemaps/markerclusterer/dist/index.min.js"></script>
```
If you want to use marker clustering in a Client Side project then add the following script to wwwroot/index.html.
```
<script src="https://unpkg.com/@googlemaps/markerclusterer/dist/index.min.js"></script>
```

2. Use component in client and server side same
```
@page "/map"
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>
<div style="height:@Height">
<GoogleMap @ref="@map1" Id="map1" Options="@mapOptions" Height="100%"></GoogleMap>
</div>
@functions {
	private GoogleMap map1;
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
}
```

## Known Issues
Adding map in razor page without _Host.cshtml use  RenderComponentAsync<T> to render component or/and try changing the Rendermode to Server in the host file

Server Side issue with Route DirectionsResult when using DirectionsRequestOptions all paths are included (all set to false). MaximumReceiveMessageSize reaches limit of 32kb. Then limit should be increased or set to null (unlimited)
```
In Startup.ConfigureServices
services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);
```

## Samples
 Please check server side samples https://github.com/rungwiroon/BlazorGoogleMaps/tree/master/ServerSideDemo which are most to date
 
 ClientSide demos. Just a few and missing code, but will be updated little by little
 https://rungwiroon.github.io/BlazorGoogleMaps/mapEvents

