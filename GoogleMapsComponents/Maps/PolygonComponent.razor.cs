using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public partial class PolygonComponent :  IAsyncDisposable, IPoly
{
    public PolygonComponent()
    {
        _guid = Guid.NewGuid();
    }

    private bool _hasRendered = false;
    internal bool IsDisposed = false;
    private Guid _guid;

    public Guid Guid => Id ?? _guid;

    [Inject]
    private IJSRuntime Js { get; set; } = default!;

    [CascadingParameter(Name = "Map")]
    private AdvancedGoogleMap MapRef { get; set; } = default!;


    [Parameter, JsonIgnore]
    public Guid? Id { get; set; }

    [Parameter, JsonIgnore]
    public List<List<LatLngLiteral>> Paths { get; set; } = new(); // For multi-ring polygons

    [Parameter, JsonIgnore]
    public string StrokeColor { get; set; } = "#FF0000";

    [Parameter, JsonIgnore]
    public float StrokeOpacity { get; set; } = 0.8F;

    [Parameter, JsonIgnore]
    public int StrokeWeight { get; set; } = 2;

    [Parameter, JsonIgnore]
    public string FillColor { get; set; } = "#FF0000";

    [Parameter, JsonIgnore]
    public float FillOpacity { get; set; } = 0.35F;

    [Parameter, JsonIgnore]
    public bool Editable { get; set; } = false;

    [Parameter, JsonIgnore]
    public bool Draggable { get; set; } = false;

    [Parameter, JsonIgnore]
    public bool Clickable { get; set; } = false;

    [Parameter, JsonIgnore]
    public bool Visible { get; set; } = true;


    [Parameter, JsonIgnore]
    public EventCallback OnClick { get; set; }

    [Parameter, JsonIgnore]
    public EventCallback<List<List<LatLngLiteral>>> OnPathChanged { get; set; }

    [Parameter, JsonIgnore]
    public Guid? MapId { get; set; }


    /// <summary>
    /// Specifies additional custom attributes that will be rendered on the "root" component of the marker.
    /// </summary>
    /// <value>The attributes.</value>
    [Parameter(CaptureUnmatchedValues = true), JsonIgnore]
    public IReadOnlyDictionary<string, object> Attributes { get; set; } = default!;

    internal async Task Click()
    {
        await OnClick.InvokeAsync();
    }

    internal async Task PathChanged(List<List<LatLngLiteral>> paths)
    {
        await OnPathChanged.InvokeAsync(paths);
    }

    //internal async Task MarkerDragged(LatLngLiteral position)
    //{
    //    await OnMove.InvokeAsync(position);
    //}

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            MapRef.AddPolygon(this);
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
        await Js.InvokeAsync<string>(
            "blazorGoogleMaps.objectManager.updatePolygonComponent",
            Guid,
            new PolygonOptions
            {
                Paths = Paths,
                StrokeColor = StrokeColor,
                StrokeOpacity = (float)StrokeOpacity,
                StrokeWeight = StrokeWeight,
                FillColor = FillColor,
                FillOpacity = (float)FillOpacity,
                Clickable = Clickable,
                Editable = Editable,
                Draggable = Draggable,
                Visible = Visible,
                MapId = MapId ?? MapRef.MapId,
            },
            MapRef.CallbackRef);
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        if (!_hasRendered)
        {
            await base.SetParametersAsync(parameters);
            return;
        }

        var optionsChanged =
            parameters.DidParameterChange(Paths) ||
            parameters.DidParameterChange(StrokeColor) ||
            parameters.DidParameterChange(StrokeOpacity) ||
            parameters.DidParameterChange(StrokeWeight) ||
            parameters.DidParameterChange(FillColor) ||
            parameters.DidParameterChange(FillOpacity) ||
            parameters.DidParameterChange(Clickable) ||
            parameters.DidParameterChange(Editable) ||
            parameters.DidParameterChange(Draggable) ||
            parameters.DidParameterChange(Visible);

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
        Console.WriteLine(Guid);
        await Js.InvokeVoidAsync("blazorGoogleMaps.objectManager.disposePolygonComponent", Guid);
        MapRef.RemovePolygon(this);
        GC.SuppressFinalize(this);
    }

    internal readonly struct PolygonOptions
    {    
        public List<List<LatLngLiteral>>? Paths { get; init; }
        public string? StrokeColor { get; init; }
        public double? StrokeOpacity { get; init; }
        public int? StrokeWeight { get; init; }
        public string? FillColor { get; init; }
        public double? FillOpacity { get; init; }
        public bool? Clickable { get; init; }
        public bool? Editable { get; init; }
        public bool? Draggable { get; init;  }
        public bool? Visible { get; init; }
        public Guid? MapId { get; init; }
    }

}