using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps
{
    public partial class HeatmapPointComponent : ComponentBase, IAsyncDisposable
    {
        [CascadingParameter] private HeatMapLayerComponent Heatmap { get; set; } = default!;

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
