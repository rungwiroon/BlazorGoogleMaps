using Microsoft.AspNetCore.Components;
using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public partial class HeatmapPointComponent : ComponentBase, IAsyncDisposable
    {
        [CascadingParameter(Name = "Heatmap")] 
        public HeatMapLayerComponent Heatmap { get; set; } = default!;

        [Parameter] public LatLngLiteral Location { get; set; }
        [Parameter] public double Weight { get; set; } = 1;

        protected override void OnInitialized()
        {
            Heatmap.RegisterPoint(this);
        }

        protected override async Task OnParametersSetAsync()
        {
            await Heatmap.Refresh();
        }

        public async ValueTask DisposeAsync()
        {
            Heatmap.UnregisterPoint(this);
            await Heatmap.Refresh();
        }
    }

}
