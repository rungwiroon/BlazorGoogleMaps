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
        // A set, so that registering and unregistering a point does not scan all the other points
        private readonly HashSet<HeatmapPointComponent> _points = new();
        private bool _hasRendered;
        private bool _isDisposed;
        private Task? _pendingRefresh;
        private Guid _guid = Guid.NewGuid();

        [Inject] private IJSRuntime Js { get; set; } = default!;
        [CascadingParameter(Name = "Map")] private AdvancedGoogleMap MapRef { get; set; } = default!;

        [Parameter] public double Radius { get; set; } = 20;
        [Parameter] public double Opacity { get; set; } = 0.6;
        [Parameter] public bool Dissipating { get; set; } = true;

        [Parameter] public double MaxIntensity { get; set; } = 1.0;

        [Parameter]
        public RenderFragment? ChildContent { get; set; }

        /// <summary>
        /// A gradient array for the heatmap colors, expressed as CSS color strings.
        /// </summary>
        [Parameter] public string[]? Gradient { get; set; }

        internal void RegisterPoint(HeatmapPointComponent point)
        {
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
                await Js.InvokeVoidAsync("blazorGoogleMaps.objectManager.addHeatmap",
                    _guid, MapRef.MapId, GetOptions(), MapRef.CallbackRef);
                _hasRendered = true;
            }
            await base.OnAfterRenderAsync(firstRender);
        }

        public async Task Refresh()
        {
            if (!_hasRendered || _isDisposed) return;
            await Js.InvokeVoidAsync("blazorGoogleMaps.objectManager.updateHeatmap",
                _guid, GetOptions(), MapRef.CallbackRef);
        }

        /// <summary>
        /// Used by the points. All the points that change in the same render share a single refresh,
        /// instead of each of them sending the whole data set to JavaScript.
        /// </summary>
        internal Task RequestRefresh()
        {
            if (!_hasRendered || _isDisposed) return Task.CompletedTask;

            return _pendingRefresh ??= RefreshOnceAsync();
        }

        private async Task RefreshOnceAsync()
        {
            // Runs after the current render, when every point has already reported its change
            await Task.Yield();
            _pendingRefresh = null;
            await Refresh();
        }

        private object GetOptions() => new
        {
            Data = _points.Select(p => new { location = p.Location, weight = p.Weight }),
            Radius,
            Opacity,
            Dissipating,
            MaxIntensity,
            Gradient
        };

        public async ValueTask DisposeAsync()
        {
            // The points are disposed together with the layer, there is nothing left for them to refresh
            _isDisposed = true;
            await Js.InvokeVoidAsync("blazorGoogleMaps.objectManager.removeHeatmap", _guid);
        }
    }

}
