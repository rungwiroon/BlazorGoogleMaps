# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**BlazorGoogleMaps** is a Blazor component library that wraps the Google Maps JavaScript API via JS interop. It's published as a NuGet package and consumed by two demo applications.

**Architecture:**
```
GoogleMapsComponents/          # Main library (multi-targeted: net8.0, net9.0, net10.0)
├── Maps/                      # Core map components and services
├── Serialization/             # Custom JSON converters (JsObjectRef, OneOf, EnumMember)
└── wwwroot/                   # Embedded JavaScript interop files

Demos/
├── Demo.Ui.Shared/            # Shared Razor class library (net10.0)
├── ServerSideDemo/            # Blazor Server demo app
└── ClientSideDemo/            # Blazor WebAssembly demo app
```

---

## Build & Common Commands

### Build
```bash
dotnet build BlazorGoogleMaps.sln
```

### Build Specific Configuration
```bash
dotnet build BlazorGoogleMaps.sln -c Release
```

### Pack the Library (NuGet)
```bash
dotnet pack GoogleMapsComponents/GoogleMapsComponents.csproj -c Release
```
Output: `GoogleMapsComponents/bin/Release/BlazorGoogleMaps.*.nupkg`

### Run Server-Side Demo
```bash
dotnet run --project Demos/ServerSideDemo/ServerSideDemo.csproj
```

### Run Client-Side (WASM) Demo
```bash
dotnet run --project Demos/ClientSideDemo/ClientSideDemo.csproj
```

### Clean Build Artifacts
```bash
dotnet clean BlazorGoogleMaps.sln
```

---

## Project Configuration

- **Target Frameworks:** `net8.0`, `net9.0`, `net10.0` — Never change these unless explicitly requested
- **C# Version:** `LangVersion=latest` — Modern C# syntax is encouraged
- **Nullable:** `<Nullable>enable</Nullable>` — Always handle nullability in the main library
- **Implicit Usings:** Enabled only in demos; add explicit `using` directives in `GoogleMapsComponents`
- **Namespaces:** Always use file-scoped namespaces (`namespace Foo.Bar;`)
- **JSON:** `System.Text.Json` only (never Newtonsoft.Json)

---

## Core Architecture Patterns

### 1. JS Interop via `JsObjectRef`

Every Google Maps object holds a `JsObjectRef` that tracks a live JavaScript object by GUID. This is the foundation of the library's interop strategy.

**Factory Pattern (Required):**
```csharp
public class MyMapObject : IDisposable
{
    private readonly JsObjectRef _jsObjectRef;

    public static async Task<MyMapObject> CreateAsync(IJSRuntime jsRuntime, MyOptions options)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.MyObject", options);
        return new MyMapObject(jsObjectRef);
    }

    private MyMapObject(JsObjectRef jsObjectRef) => _jsObjectRef = jsObjectRef;

    public void Dispose() => _jsObjectRef.Dispose();
}
```

**Key Rules:**
- Never instantiate map objects with `new` from outside the library — always use the `CreateAsync` factory
- Always implement `IDisposable` and call `_jsObjectRef.Dispose()`
- Pass `IJSRuntime` directly to `CreateAsync`; don't inject it into constructors

### 2. Blazor Components

- Inherit from `MapComponent` (provides `IJSRuntime JsRuntime` and `IServiceProvider ServiceProvider`)
- Use `[Parameter]` for inputs and `EventCallback` for outputs
- Map initialization logic goes in `OnAfterInit`, not `OnInitialized`
- Use partial class pattern for code-behind: `MyComponent.razor` + `MyComponent.razor.cs`

### 3. Options & Response Types

- Use **`record`** types for option bags and DTOs:
  ```csharp
  public record MapOptions
  {
      public int Zoom { get; init; }
      public LatLngLiteral Center { get; init; }
  }
  ```
- Use **`class`** for stateful objects that wrap a `JsObjectRef`

---

## Serialization & Custom Converters

All serialization uses `System.Text.Json`. Key patterns:

### Enum Serialization
Enums mapping to Google Maps JS strings use `[EnumMember(Value = "camelCase")]` and `[JsonConverter(...)]`:
```csharp
[JsonConverter(typeof(EnumMemberConverter<TravelMode>))]
public enum TravelMode
{
    [EnumMember(Value = "DRIVING")]  Driving,
    [EnumMember(Value = "WALKING")]  Walking,
}
```

### Union Types
Use the `OneOf` library for multi-type properties (e.g., `string | LatLngLiteral`). A `OneOfConverterFactory` is already registered.

### JsObjectRef Serialization
Use `JsObjectRefConverter<T>` when a type containing a `JsObjectRef` needs interop serialization.

Custom converters are in `GoogleMapsComponents/Serialization/`.

---

## Namespace Structure

- **Root:** `GoogleMapsComponents` for the library
- **Key Namespaces:**
  - `GoogleMapsComponents.Maps` — Core map, markers, shapes, services
  - `GoogleMapsComponents.Maps.Places` — Places API
  - `GoogleMapsComponents.Maps.Drawing` — Drawing tools
  - `GoogleMapsComponents.Maps.Data` — Data layers (GeoJSON, etc.)
  - `GoogleMapsComponents.Maps.Visualization` — Heat maps, clustering
  - `GoogleMapsComponents.Maps.Extension` — Extension methods

---

## Dependency Injection

DI registration is centralized in `GoogleMapsComponents/DependencyInjectionExtensions.cs` via `AddBlazorGoogleMaps` overloads. When adding new injectable services, integrate into the existing extension methods rather than creating new registration points.

Example (from demos):
```csharp
builder.Services.AddBlazorGoogleMaps("YOUR_API_KEY");
```

---

## Key Files to Know

| File | Purpose |
|------|---------|
| `GoogleMapsComponents/DependencyInjectionExtensions.cs` | DI registration |
| `GoogleMapsComponents/JsObjectRef.cs` | Core JS interop tracking |
| `GoogleMapsComponents/EnumMemberConverter.cs` | Enum ↔ JSON serialization |
| `GoogleMapsComponents/Serialization/` | Custom JSON converters |
| `GoogleMapsComponents/Maps/MapComponent.cs` | Base class for all map Razor components |

---

## Testing

There is currently no test project. When adding tests:

- Create `GoogleMapsComponents.Tests` using **xUnit**
- Required packages: `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`
- Mirror source names: `AutocompleteService` → `AutocompleteServiceTests`
- Name tests by behavior: `WhenRequestIsNull_GetPlacePredictions_ThrowsArgumentNullException`
- Mock `IJSRuntime` for JS interop; don't test actual JS calls
- Test through public APIs — don't change member visibility to enable testing

---

## Naming & Style Conventions

| Concern | Convention |
|---------|-----------|
| Factory methods | `CreateAsync` |
| Async methods | Suffix `Async` |
| Private fields | `_camelCase` |
| Public members | `PascalCase` |
| JS method names | Pass as literal string matching Google Maps JS API exactly |

---

## Error Handling

- Use `ArgumentNullException.ThrowIfNull(x)` for null guards
- Use `string.IsNullOrWhiteSpace` for string validation
- Prefer precise exception types (`InvalidOperationException`, `ArgumentException`)
- Do not swallow exceptions silently

---

## What to Avoid

- Do not edit auto-generated files (`*.g.cs`, `obj/`, files with `// <auto-generated>`)
- Do not add abstractions/interfaces unless the type is an external dependency or needs testing
- Do not add Newtonsoft.Json or any competing JSON library
- Do not use sync-over-async (`Task.Result`, `.Wait()`)
- Do not change `<TargetFrameworks>`, `<LangVersion>`, or SDK version without explicit request
- Do not use `public` unless the member is part of the library's public surface — prefer `internal` or `private`

---

## References

- **Full Architecture Guide:** See `.github/copilot-instructions.md` for complete patterns, DI structure, and conventions
- **Usage & Features:** See `README.md` for quick-start, examples, and feature list
- **Demo Examples:** `Demos/Demo.Ui.Shared/Pages/` contains reference implementations of all major features
