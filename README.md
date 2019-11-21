# BlazorGoogleMaps
Blazor interop for GoogleMap library

## Nuget
<a href="https://www.nuget.org/packages/BlazorGoogleMaps" />
<img src="https://img.shields.io/nuget/dt/BlazorGoogleMaps" />

## Usage
1. Add google map script tag to wwwroot/index.html in Client side or _Host.cshtml in Server Side
```
<script type="text/javascript" src="https://maps.googleapis.com/maps/api/js?key=YOUR_KEY_GOES_HERE&v=3"></script>
```
For servers side also needed to link to recourse manually in preview9. This could change in future releases. Add this line into _Host.cshtml
```
<script src="_content/BlazorGoogleMaps/objectManager.js"></script>
```

2. Use component in client and server side same
```
@page "/map"
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>

<GoogleMap @ref="@map1" Id="map1" Options="@mapOptions"></GoogleMap>

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
3. Adding Direction Example
```
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>

<GoogleMap @ref="@map1" Id="map1" Options="@mapOptions" Height="350" OnAfterInit="@(async () => await OnAfterInitAsync())"></GoogleMap>
<button @onclick="AddDirections">Add Direction</button>

@code {
	private GoogleMap map1;
	private MapOptions mapOptions;	
	private DirectionsRenderer dirRend;
	
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
		await dirRend.Route(dr);
    	}		
}
```
## Known Issues
Adding map in razor page without _Host.cshtml use  RenderComponentAsync<T> to render componenent or/and try changing the Rendermode to Server in the host file

## Current status
* Map
* Marker
* InfoWindow
* Polygon, LineString, Rectangle, Circle

## Work In Progress
* Routes

## Todo
* Data 
* StreetView
* Places
