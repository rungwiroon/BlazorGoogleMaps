using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GoogleMapsComponents.Maps;

public partial class MarkerComponent : IAsyncDisposable
{
    public MarkerComponent()
    {
        Guid = Guid.NewGuid();
        _componentId = "marker_" + Guid.ToString("N");
    }
    private readonly string _componentId;
    private bool hasRendered = false;
    internal bool IsDisposed = false;
    
    public Guid Guid { get; }

    [Inject] 
    private IJSRuntime JS { get; set; } = default!;
    
    [CascadingParameter(Name = "Map")] 
    private AdvancedGoogleMap MapRef { get; set; } = default!;
    
    [Parameter] 
    public RenderFragment? ChildContent { get; set; }
    
    /// <summary>
    /// Latitude in degrees. Values will be clamped to the range [-90, 90]. 
    /// This means that if the value specified is less than -90, it will be set to -90. 
    /// And if the value is greater than 90, it will be set to 90.
    /// </summary>
    [Parameter] 
    public double Lat { get; set; }
    
    /// <summary>
    /// Longitude in degrees. Values outside the range [-180, 180] will be wrapped so that they fall within the range. 
    /// For example, a value of -190 will be converted to 170. A value of 190 will be converted to -170. 
    /// This reflects the fact that longitudes wrap around the globe.
    /// </summary>
    [Parameter] 
    public double Lng { get; set; }
    
    /// <summary>
    /// An enumeration specifying how an AdvancedMarkerElement should behave when it collides with another AdvancedMarkerElement or with the basemap labels on a vector map.
    /// Note: AdvancedMarkerElement to AdvancedMarkerElement collision works on both raster and vector maps, however, AdvancedMarkerElement to base map's label collision only works on vector maps.
    /// </summary>
    [Parameter]
    public CollisionBehavior? CollisionBehavior { get; set; }

    /// <summary>
    /// If true, the AdvancedMarkerElement can be dragged.
    /// Note: AdvancedMarkerElement with altitude is not draggable.
    /// </summary>
    [Parameter]
    public bool Draggable { get; set; }

    /// <summary>
    /// This event is fired when the user stops moving the marker.
    /// </summary>
    [Parameter]
    public EventCallback<LatLngLiteral> OnMove { get; set; }
    
    /// <summary>
    /// If true, the AdvancedMarkerElement will be clickable and trigger the gmp-click event, and will be interactive for accessibility purposes (e.g. allowing keyboard navigation via arrow keys).
    /// </summary>
    [Parameter]
    public bool Clickable { get; set; }
    
    /// <summary>
    /// This event is fired when the marker is clicked.
    /// </summary>
    [Parameter]
    public EventCallback OnClick { get; set; }

    /// <summary>
    /// Rollover text. If provided, an accessibility text (e.g. for use with screen readers) will be added to the
    /// </summary>
    [Parameter]
    public string? Title { get; set; }

    /// <summary>
    /// All entities are displayed on the map in order of their zIndex, with higher values displaying in front of entities with lower values. 
    /// By default, entities are displayed according to their vertical position on screen, with lower entities appearing in front of entities further up the screen.
    /// </summary>
    [Parameter]
    public int? ZIndex { get; set; }
    
    public IMarker ToMarker()
    {
        return new MarkerComponentRef()
        {
            Guid = Guid
        };
    }
    
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
            MapRef.MapComponents[Guid] = this;
            hasRendered = true;
            await UpdateOptions();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    private async Task UpdateOptions()
    {
        await JS.InvokeAsync<string>("blazorGoogleMaps.objectManager.updateAdvancedComponent", Guid, new AdvancedMarkerComponentOptions()
        {
            CollisionBehavior = CollisionBehavior,
            Position = new LatLngLiteral(Lat, Lng),
            ComponentId = _componentId,
            Title = Title ?? "",
            GmpClickable = Clickable,
            GmpDraggable = Draggable,
            MapId = MapRef.MapId,
            ZIndex = ZIndex
        }, MapRef.callbackRef);
    }
    
    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (!hasRendered)
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
        await JS.InvokeVoidAsync("blazorGoogleMaps.objectManager.disposeAdvancedMarkerComponent", Guid);
        MapRef.MapComponents.Remove(Guid);
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

/// <summary>
/// Contains extension methods for <see cref="ParameterView" />.
/// </summary>
internal static class ParameterViewExtensions
{
    /// <summary>
    /// Checks if a parameter changed.
    /// </summary>
    /// <typeparam name="T">The value type</typeparam>
    /// <param name="parameters">The parameters.</param>
    /// <param name="parameterName">Name of the parameter.</param>
    /// <param name="parameterValue">The parameter value (SHOULD NOT BE ENTERED MANUALLY).</param>
    /// <returns><c>true</c> if the parameter value has changed, <c>false</c> otherwise.</returns>
    internal static bool DidParameterChange<T>(this ParameterView parameters, T parameterValue, [CallerArgumentExpression("parameterValue")] string parameterName = "")
    {
        if (parameters.TryGetValue(parameterName, out T? value) && value != null)
        {
            return !EqualityComparer<T>.Default.Equals(value, parameterValue);
        }

        return false;
    }
}
