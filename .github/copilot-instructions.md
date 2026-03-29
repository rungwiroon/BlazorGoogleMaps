# Copilot Instructions — BlazorGoogleMaps

## Project overview

**BlazorGoogleMaps** is a Blazor component library (`BlazorGoogleMaps` NuGet package) that wraps the Google Maps JavaScript API via JS interop. It targets `net8.0`, `net9.0`, and `net10.0` simultaneously and is consumed by two demo apps (server-side and client-side/WASM).

```
GoogleMapsComponents/          # Main library (multi-targeted net8/9/10)
Demos/
  Demo.Ui.Shared/              # Shared Razor class library (net10.0)
  ServerSideDemo/              # Blazor Server demo app
  ClientSideDemo/              # Blazor WebAssembly demo app
```

---

## Build & language settings

- **TFMs**: `net8.0;net9.0;net10.0` — never change these unless explicitly asked.
- **C# version**: `LangVersion=latest` — modern C# syntax is encouraged.
- **Nullable**: `<Nullable>enable</Nullable>` in all projects — always handle nullability.
- **Implicit usings**: enabled only in demo/shared projects, **not** in the main library. Add explicit `using` directives in `GoogleMapsComponents`.
- **Namespaces**: always use **file-scoped namespaces** (`namespace Foo.Bar;`).

---

## Core architecture patterns

### JS interop via `JsObjectRef`

Every Google Maps object (map, marker, service, etc.) holds a `JsObjectRef` that tracks a live JavaScript object by GUID.

- **Never** instantiate map objects with `new` from outside the library. Always use the `static async Task<T> CreateAsync(IJSRuntime, ...)` factory pattern.
- **Always** implement `IDisposable` (and `IAsyncDisposable` where async cleanup is needed) and call `_jsObjectRef.Dispose()`.
- Pass `IJSRuntime` directly to `CreateAsync`; do **not** inject it into constructors.

```csharp
// Correct pattern for a new map object
public class MyMapObject : IDisposable
{
    private readonly JsObjectRef _jsObjectRef;

    public static async Task<MyMapObject> CreateAsync(IJSRuntime jsRuntime, MyOptions options)
    {
        var jsObjectRef = await JsObjectRef.CreateAsync(jsRuntime, "google.maps.MyObject", options);
        return new MyMapObject(jsObjectRef);
    }

    private MyMapObject(JsObjectRef jsObjectRef)
    {
        _jsObjectRef = jsObjectRef;
    }

    public void Dispose() => _jsObjectRef.Dispose();
}
```

### Blazor components

- All map Razor components inherit from `MapComponent` (which provides `IJSRuntime JsRuntime` and `IServiceProvider ServiceProvider`).
- Use `[Parameter]` for public inputs and `EventCallback` / `EventCallback<T>` for events.
- Map initialization logic goes in `OnAfterInit` callbacks, not in `OnInitialized`.
- Razor components with significant code-behind use the partial class pattern: `MyComponent.razor` + `MyComponent.razor.cs`.

### Options / response types

- Use **`record`** types (or `class` with init-only properties) for option bags and response DTOs.
- Use **`class`** for stateful objects that wrap a `JsObjectRef`.

---

## Serialization — `System.Text.Json` only

Never use Newtonsoft.Json. All serialization goes through `System.Text.Json`.

### Enum serialization

Enums that map to Google Maps JS string values **must** use `[EnumMember(Value = "camelCase")]` and be annotated with `[JsonConverter(typeof(EnumMemberConverter<TEnum>))]` (or applied via `JsonSerializerOptions`).

```csharp
[JsonConverter(typeof(EnumMemberConverter<TravelMode>))]
public enum TravelMode
{
    [EnumMember(Value = "DRIVING")]  Driving,
    [EnumMember(Value = "WALKING")]  Walking,
}
```

### `JsObjectRef` serialization

Use `JsObjectRefConverter<T>` (already registered) when a type containing a `JsObjectRef` needs to be serialized for interop.

### Union types

Use the **`OneOf`** library for properties that accept multiple JS types (e.g., `string | LatLngLiteral`). A `OneOfConverterFactory` is already registered.

---

## Naming & style conventions

| Concern | Convention |
|---|---|
| Factory methods | `CreateAsync` |
| Async methods | Suffix `Async` |
| Private fields | `_camelCase` |
| Public members | `PascalCase` |
| JS method names | Pass as literal string matching the Google Maps JS API exactly (e.g., `"getPlacePredictions"`) |
| Namespace root | `GoogleMapsComponents` for library; `Demo.Ui.Shared`, `ClientSideDemo`, `ServerSideDemo` for demos |
| Sub-namespaces | Mirror the Google Maps API grouping: `Maps`, `Maps.Places`, `Maps.Drawing`, `Maps.Data`, `Maps.Visualization`, `Maps.Extension` |

---

## Error handling

- Use `ArgumentNullException.ThrowIfNull(x)` for null guards; `string.IsNullOrWhiteSpace` for strings.
- Prefer precise exception types (`InvalidOperationException`, `ArgumentException`).
- Do **not** swallow exceptions silently.

---

## DI registration

The library is registered via `AddBlazorGoogleMaps` extension methods in `DependencyInjectionExtensions.cs`. When adding new injectable services, add a corresponding `AddBlazorGoogleMaps` overload or integrate into the existing one rather than creating a new registration mechanism.

---

## Testing

There is currently no test project. When adding tests:

- Create a project named `GoogleMapsComponents.Tests` using **xUnit**.
- Required packages: `Microsoft.NET.Test.Sdk`, `xunit`, `xunit.runner.visualstudio`.
- Mirror source class names: `AutocompleteService` → `AutocompleteServiceTests`.
- Name tests by behavior: `WhenRequestIsNull_GetPlacePredictions_ThrowsArgumentNullException`.
- Do not test JS interop calls directly — mock `IJSRuntime`.
- Do not change member visibility (`InternalsVisibleTo`) to enable testing; test through public APIs.

---

## What to avoid

- **Do not** edit auto-generated files (`*.g.cs`, files under `obj/`, files with `// <auto-generated>`).
- **Do not** add abstractions/interfaces unless the type is an external dependency or needs to be mocked in tests.
- **Do not** add Newtonsoft.Json or any second JSON library.
- **Do not** use `sync-over-async` (`Task.Result`, `.Wait()`).
- **Do not** change `<TargetFrameworks>`, `<LangVersion>`, or SDK version unless explicitly requested.
- **Do not** use `public` unless the member is part of the library's public surface; prefer `internal` or `private`.
