# BlazorGoogleMaps

🗺️ **Blazor interop for Google Maps JavaScript API**

[![NuGet version (BlazorGoogleMaps)](https://img.shields.io/nuget/v/BlazorGoogleMaps)](https://www.nuget.org/packages/BlazorGoogleMaps/)
[![.NET 10](https://img.shields.io/badge/.NET-10-blue)](https://dotnet.microsoft.com/)

A powerful and easy-to-use Blazor library for integrating Google Maps into your Blazor WebAssembly and Blazor Server applications.

---

## 📑 Table of Contents

- [Features](#features)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Quick Start](#quick-start)
- [Configuration](#configuration)
- [Usage Examples](#usage-examples)
- [Live Demos](#live-demos)
- [Breaking Changes](#breaking-changes)
- [Contributing](#contributing)

---

## ✨ Features

- 🎯 **Full Google Maps API Support** - Markers, Polylines, Polygons, Circles, Info Windows, and more
- 🚀 **Blazor WebAssembly & Server** - Works seamlessly with both hosting models
- 🎨 **Advanced Markers** - Render Blazor components directly on the map
- 📍 **Marker Clustering** - Built-in support for marker clustering
- 🔥 **Heat Maps** - Visualize data density with heat map layers
- 🛣️ **Directions & Routes** - Full support for directions and route rendering
- 🎭 **Map Styling** - Customize map appearance with style options
- 📊 **Data Layers** - Support for GeoJSON and other data formats
- ⚡ **Event Handling** - Comprehensive event support for interactive maps
- 🎨 **Drawing Tools** - Built-in drawing manager for shapes and overlays

---

## 📋 Prerequisites

- .NET 8.0 or higher
- A valid Google Maps API key ([Get one here](https://developers.google.com/maps/documentation/javascript/get-api-key))

---

## 📦 Installation

Install the package via NuGet Package Manager:

```bash
dotnet add package BlazorGoogleMaps
```

Or via NuGet Package Manager Console:

```powershell
Install-Package BlazorGoogleMaps
```

---

## 🚀 Quick Start

### Step 1: Configure Your API Key

Add BlazorGoogleMaps to your `Program.cs`:

#### **Option 1: Simple Configuration (Recommended)**
```csharp
builder.Services.AddBlazorGoogleMaps("YOUR_GOOGLE_API_KEY");
```

#### **Option 2: Advanced Configuration**
```csharp
builder.Services.AddBlazorGoogleMaps(new GoogleMapsComponents.Maps.MapApiLoadOptions("YOUR_GOOGLE_API_KEY")
{
	Version = "beta",
	Libraries = "places,visualization,drawing,marker"
});
```

#### **Option 3: Custom Key Service**
For more complex scenarios (e.g., loading keys asynchronously from a database):
```csharp
builder.Services.AddScoped<IBlazorGoogleMapsKeyService, YourCustomKeyService>();
```

> ⚠️ **Legacy Method (Not Recommended):** Adding the script tag directly to your HTML is still supported but not recommended.

---

### Step 2: Add JavaScript References

Add the required JavaScript files to your `wwwroot/index.html` (Blazor WASM) or `_Host.cshtml`/`_HostLayout.cshtml` (Blazor Server):

```html
<script src="_content/BlazorGoogleMaps/js/objectManager.js"></script>
```

**Optional:** For marker clustering support, add:
```html
<script src="https://unpkg.com/@googlemaps/markerclusterer/dist/index.min.js"></script>
```

---

## 💡 Usage Examples

### Basic Map

Create a simple map component:

```razor
@page "/map"
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Google Map</h1>
<div style="height: 500px;">
	<GoogleMap @ref="@_map1" 
			   Id="map1" 
			   Options="@_mapOptions" 
			   Height="100%" 
			   OnAfterInit="@AfterMapRender">
	</GoogleMap>
</div>

@code {
	private GoogleMap? _map1;
	private MapOptions _mapOptions = default!;

	protected override void OnInitialized()
	{
		_mapOptions = new MapOptions()
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
		// Map is ready - you can perform additional initialization here
		var bounds = await LatLngBounds.CreateAsync(_map1!.JsRuntime);
	}
}
```

---

### Advanced Map with Blazor Components

Render interactive Blazor components as markers (requires Google Maps `v=beta` and a `MapId`):

```razor
@page "/advanced-map"
@using GoogleMapsComponents
@using GoogleMapsComponents.Maps

<h1>Advanced Map with Blazor Markers</h1>
<AdvancedGoogleMap @ref="@_map1" Id="map1" Options="@_mapOptions">
	@foreach (var marker in Markers)
	{
		<MarkerComponent 
			@key="marker.Id" 
			Lat="@marker.Lat" 
			Lng="@marker.Lng" 
			Clickable="@marker.Clickable" 
			Draggable="@marker.Draggable" 
			OnClick="@(() => marker.Active = !marker.Active)"
			OnMove="@(pos => marker.UpdatePosition(pos))">
			<div class="custom-marker">
				<h4>@marker.Title</h4>
				<p>Custom Blazor Content</p>
			</div>
		</MarkerComponent>
	}
</AdvancedGoogleMap>

@code {
	private AdvancedGoogleMap? _map1;
	private List<MarkerData> Markers = 
	[
		new MarkerData { Id = 1, Lat = 13.505892, Lng = 100.8162, Title = "Location 1" }
	];

	private MapOptions _mapOptions = new()
	{
		Zoom = 13,
		Center = new LatLngLiteral()
		{
			Lat = 13.505892,
			Lng = 100.8162
		},
		MapId = "DEMO_MAP_ID", // Required for advanced markers
		MapTypeId = MapTypeId.Roadmap
	};

	public class MarkerData
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
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

---

## 🎮 Live Demos

Explore interactive examples and learn more features:

- **📘 Server-Side Demo:** [GitHub Repository](https://github.com/rungwiroon/BlazorGoogleMaps/tree/master/ServerSideDemo)
- **🌐 Client-Side Demo:** [Live Demo](https://rungwiroon.github.io/BlazorGoogleMaps/mapEvents)

The server-side demos include the most up-to-date examples covering:
- Markers and Info Windows
- Polylines, Polygons, and Circles
- Heat Maps and Data Layers
- Drawing Manager
- Routes and Directions
- Event Handling
- Map Styling
- And much more!

---

## ⚠️ Breaking Changes

### Version 4.0.0
- **Migration to .NET 8** ([#286](https://github.com/rungwiroon/BlazorGoogleMaps/issues/286))
  - Minimum required version is now .NET 8.0

### Version 3.0.0
- **JSON Serialization Change**
  - Migrated from `Newtonsoft.Json` to `System.Text.Json`
  - Update any custom serialization code accordingly

### Version 2.0.0
- **LatLngLiteral Constructor Parameter Order** ([#173](https://github.com/rungwiroon/BlazorGoogleMaps/issues/176))
  - Constructor parameter order changed
  - Old: `new LatLngLiteral(lng, lat)`
  - New: `new LatLngLiteral() { Lat = lat, Lng = lng }`

---

## 🤝 Contributing

Contributions are welcome! Please feel free to submit a Pull Request. For major changes, please open an issue first to discuss what you would like to change.

---

## 📄 License

This project is licensed under the MIT License.

---

## 🙏 Acknowledgments

- Built with ❤️ for the Blazor community
- Powered by the Google Maps JavaScript API

---

**Happy Mapping! 🗺️**
