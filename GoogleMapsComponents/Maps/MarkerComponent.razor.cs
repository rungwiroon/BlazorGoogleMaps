using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public partial class MarkerComponent : IAsyncDisposable, IMarker
{
    public MarkerComponent()
    {
        _guid = Guid.NewGuid();
        _componentId = "marker_" + _guid.ToString("N");
    }
    private readonly string _componentId;
    private bool _hasRendered = false;
    internal bool IsDisposed = false;
    private Guid _guid;

    public Guid Guid => Id ?? _guid;

    [Inject]
    private IJSRuntime Js { get; set; } = default!;

    [CascadingParameter(Name = "Map")]
    private AdvancedGoogleMap MapRef { get; set; } = default!;

    [Parameter]
    [JsonIgnore]
    public RenderFragment? ChildContent { get; set; }

    [Parameter, JsonIgnore]
    public Guid? Id { get; set; }

    /// <summary>
    /// Latitude in degrees. Values will be clamped to the range [-90, 90]. 
    /// This means that if the value specified is less than -90, it will be set to -90. 
    /// And if the value is greater than 90, it will be set to 90.
    /// </summary>
    [Parameter, JsonIgnore]
    public double Lat { get; set; }

    /// <summary>
    /// Longitude in degrees. Values outside the range [-180, 180] will be wrapped so that they fall within the range. 
    /// For example, a value of -190 will be converted to 170. A value of 190 will be converted to -170. 
    /// This reflects the fact that longitudes wrap around the globe.
    /// </summary>
    [Parameter, JsonIgnore]
    public double Lng { get; set; }

    /// <summary>
    /// An enumeration specifying how an AdvancedMarkerElement should behave when it collides with another AdvancedMarkerElement or with the basemap labels on a vector map.
    /// Note: AdvancedMarkerElement to AdvancedMarkerElement collision works on both raster and vector maps, however, AdvancedMarkerElement to base map's label collision only works on vector maps.
    /// </summary>
    [Parameter, JsonIgnore]
    public CollisionBehavior? CollisionBehavior { get; set; }

    /// <summary>
    /// If true, the AdvancedMarkerElement can be dragged.
    /// Note: AdvancedMarkerElement with altitude is not draggable.
    /// </summary>
    [Parameter, JsonIgnore]
    public bool Draggable { get; set; }

    /// <summary>
    /// This event is fired when the user stops moving the marker.
    /// </summary>
    [Parameter, JsonIgnore]
    public EventCallback<LatLngLiteral> OnMove { get; set; }

    /// <summary>
    /// If true, the AdvancedMarkerElement will be clickable and trigger the gmp-click event, and will be interactive for accessibility purposes (e.g. allowing keyboard navigation via arrow keys).
    /// </summary>
    [Parameter, JsonIgnore]
    public bool Clickable { get; set; }

    /// <summary>
    /// This event is fired when the marker is clicked.
    /// </summary>
    [Parameter, JsonIgnore]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Rollover text. If provided, an accessibility text (e.g. for use with screen readers) will be added to the
    /// </summary>
    [Parameter, JsonIgnore]
    public string? Title { get; set; }

    /// <summary>
    /// All entities are displayed on the map in order of their zIndex, with higher values displaying in front of entities with lower values. 
    /// By default, entities are displayed according to their vertical position on screen, with lower entities appearing in front of entities further up the screen.
    /// </summary>
    [Parameter, JsonIgnore]
    public int? ZIndex { get; set; }

    /// <summary>
    /// A possible override MapId, if this is unset, the markers will read their MapId from the AdvancedGoogleMap
    /// </summary>
    [Parameter, JsonIgnore]
    public Guid? MapId { get; set; }

    /// <summary>
    /// Specifies additional custom attributes that will be rendered on the "root" component of the marker.
    /// </summary>
    /// <value>The attributes.</value>
    [Parameter(CaptureUnmatchedValues = true), JsonIgnore]
    public IReadOnlyDictionary<string, object> Attributes { get; set; } = default!;

    internal async Task MarkerClicked()
    {
        await OnClick.InvokeAsync();
    }

    internal async Task MarkerDragged(LatLngLiteral position)
    {
        await OnMove.InvokeAsync(position);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            MapRef.AddMarker(this);
            _hasRendered = true;
            await UpdateOptions();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    /// <summary>
    /// Trigger a "update" of the component, by default the component will update automatically when parameters changes.
    /// </summary>
    public async Task ForceRender()
    {
        if (!_hasRendered) return;
        await UpdateOptions();
    }

    private async Task UpdateOptions()
    {
        await Js.InvokeAsync<string>("blazorGoogleMaps.objectManager.updateAdvancedComponent", Guid, new AdvancedMarkerComponentOptions()
        {
            CollisionBehavior = CollisionBehavior,
            Position = new LatLngLiteral(Lat, Lng),
            ComponentId = _componentId,
            Title = Title ?? "",
            GmpClickable = Clickable,
            GmpDraggable = Draggable,
            MapId = MapId ?? MapRef.MapId,
            ZIndex = ZIndex
        }, MapRef.CallbackRef);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (!_hasRendered)
        {
            await base.SetParametersAsync(parameters);
            return;
        }

        var optionsChanged = parameters.DidParameterChange(CollisionBehavior) ||
            parameters.DidParameterChange(Lat) ||
            parameters.DidParameterChange(Lng) ||
            parameters.DidParameterChange(ZIndex) ||
            parameters.DidParameterChange(Title) ||
            parameters.DidParameterChange(Clickable) ||
            parameters.DidParameterChange(MapId) ||
            parameters.DidParameterChange(Draggable);

        await base.SetParametersAsync(parameters);

        if (optionsChanged)
        {
            await UpdateOptions();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (IsDisposed) return;
        IsDisposed = true;
        await Js.InvokeVoidAsync("blazorGoogleMaps.objectManager.disposeAdvancedMarkerComponent", Guid);
        MapRef.RemoveMarker(this);
        GC.SuppressFinalize(this);
    }

    internal readonly struct AdvancedMarkerComponentOptions
    {
        public LatLngLiteral? Position { get; init; }
        public Guid? MapId { get; init; }
        public CollisionBehavior? CollisionBehavior { get; init; }
        public required string ComponentId { get; init; }
        public bool GmpDraggable { get; init; }
        public bool GmpClickable { get; init; }
        public string? Title { get; init; }
        public int? ZIndex { get; init; }
    }
}