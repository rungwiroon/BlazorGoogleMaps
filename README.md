# BlazorGoogleMaps
Blazor interop for GoogleMap library

[![NuGet version (BlazorGoogleMaps)](https://img.shields.io/nuget/v/BlazorGoogleMaps)](https://www.nuget.org/packages/BlazorGoogleMaps/)

## Usage
1. Add google map script HEAD tag to wwwroot/index.html in Client side or _Host.cshtml in Server Side.
How to get key follow https://developers.google.com/maps/documentation/javascript/get-api-key
```
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_GOES_HERE&v=3"></script>
```
Add path to project javascript functions file in wwwroot/index.html in Client side or _Host.cshtml in Server Side.
```
<script src="_content/BlazorGoogleMaps/js/objectManager.js"></script>
```
If you want to use marker clustering in a Server Side project then add the following script to _Host.cshtml.
```
<script src="https://unpkg.com/@googlemaps/markerclustererplus/dist/index.min.js"></script>
```
If you want to use marker clustering in a Client Side project then add the following script to wwwroot/index.html.
```
<script src="https://unpkg.com/@googlemaps/markerclustererplus/dist/index.min.js"></script>
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
3. Route Direction Example
```
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>

<GoogleMap @ref="@map1" Id="map1" Options="@mapOptions" Height="350" OnAfterInit="@(async () => await OnAfterInitAsync())"></GoogleMap>
<button @onclick="AddDirections">Add Direction</button>
<p>
    Duration: @_durationTotalString <br />
    Distance: @_distanceTotalString <br />
</p>

@code {
	private GoogleMap map1;
	private MapOptions mapOptions;	
	private DirectionsRenderer dirRend;
	private string _durationTotalString;
	private string Height = 300px; //Now you can use this to dynamically set the height
    	private string _distanceTotalString;
    
	protected override void OnInitialized()
	{
		mapOptions = new MapOptions()
		{
			Zoom = 13,
			Center = new LatLngLiteral()
			{
                		Lat = 40.603629,
                		Lng = -75.472518
			},
			MapTypeId = MapTypeId.Roadmap
		};
	}

	private async Task OnAfterInitAsync()
    	{
		//Create instance of DirectionRenderer
		dirRend = await DirectionsRenderer.CreateAsync(map1.JsRuntime, new DirectionsRendererOptions()
		{
			Map = map1.InteropObject
		});
	}

	private async Task AddDirections()
    	{
		//Adding a waypoint
		var waypoints = new List<DirectionsWaypoint>();
		waypoints.Add(new DirectionsWaypoint() { Location = "Bethlehem, PA", Stopover = true } );

		//Direction Request
		DirectionsRequest dr = new DirectionsRequest();
		dr.Origin = "Allentown, PA";
		dr.Destination = "Bronx, NY";
		dr.Waypoints = waypoints;
		dr.TravelMode = TravelMode.Driving;
		
		//Calculate Route
		var directionsResult = await dirRend.Route(dr);
		foreach (var route in directionsResult.Routes.SelectMany(x => x.Legs))
		{
		    _durationTotalString += route.Duration.Text;
		    _distanceTotalString += route.Distance.Text;
		}
    	}		
}
```
## Known Issues
Adding map in razor page without _Host.cshtml use  RenderComponentAsync<T> to render componenent or/and try changing the Rendermode to Server in the host file

Server Side issue with Route DirectionsResult when using DirectionsRequestOptions all paths are included (all set to false). MaximumReceiveMessageSize reaches limit of 32kb. Then limit should be increased or set to null (unlimited)
```
In Startup.ConfigureServices
services.AddServerSideBlazor().AddHubOptions(config => config.MaximumReceiveMessageSize = 1048576);
```

## Samples
 Please check server side samples https://github.com/rungwiroon/BlazorGoogleMaps/tree/master/ServerSideDemo which are most to date

