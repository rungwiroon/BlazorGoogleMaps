using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public partial class HeatMapLayerComponent : ComponentBase, IAsyncDisposable
    {
        private readonly List<HeatmapPointComponent> _points = new();
        private bool _hasRendered;
        private Guid _guid = Guid.NewGuid();

        [Inject] private IJSRuntime Js { get; set; } = default!;
        [CascadingParameter(Name = "Map")] private AdvancedGoogleMap MapRef { get; set; } = default!;

        [Parameter] public double Radius { get; set; } = 20;
        [Parameter] public double Opacity { get; set; } = 0.6;
        [Parameter] public bool Dissipating { get; set; } = true;

        /// <summary>
        /// A gradient array for the heatmap colors, expressed as CSS color strings.
        /// </summary>
        [Parameter] public string[]? Gradient { get; set; }

        internal void RegisterPoint(HeatmapPointComponent point)
        {
            if (!_points.Contains(point))
                _points.Add(point);
        }

        internal void UnregisterPoint(HeatmapPointComponent point)
        {
            _points.Remove(point);
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                await Js.InvokeVoidAsync("blazorGoogleMaps.heatmapManager.addHeatmap",
                    _guid, MapRef.MapId, GetOptions(), MapRef.CallbackRef);
                _hasRendered = true;
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Refresh()
        {
            if (!_hasRendered) return;
            await Js.InvokeVoidAsync("blazorGoogleMaps.heatmapManager.updateHeatmap",
                _guid, GetOptions(), MapRef.CallbackRef);
        }

        private object GetOptions() => new
        {
            Data = _points.Select(p => new { location = p.Location, weight = p.Weight }),
            Radius,
            Opacity,
            Dissipating,
            Gradient
        };

        public async ValueTask DisposeAsync()
        {
            await Js.InvokeVoidAsync("blazorGoogleMaps.heatmapManager.removeHeatmap", _guid);
        }
    }

}
