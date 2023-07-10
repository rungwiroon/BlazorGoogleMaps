using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class DisposeMapCircleListPage : ComponentBase, IDisposable, IAsyncDisposable
{
    private GoogleMap map1;
    private MapOptions mapOptions;
    int bunchsize = 6000;

    private CircleList circleList = null;
    private Dictionary<string, CircleOptions> circleOptionsByRef = new Dictionary<string, CircleOptions>();
    private int lastId = 0;
    private string descriptionText = "";
    bool isDisposed = false;

    protected override void OnInitialized()
    {
        mapOptions = new MapOptions()
        {
            Zoom = 13,
            Center = new LatLngLiteral()
            {
                Lat = 48.994249,
                Lng = 12.190451
            },
            MapTypeId = MapTypeId.Roadmap
        };
    }

    /// <summary>
    /// Create a bunch of circles, put them into a dictionary with reference ids and display them on the map.
    /// </summary>
    private async Task CreateBunchOfCircles()
    {
        int howMany = this.bunchsize;
        var bounds = await map1.InteropObject.GetBounds();
        double maxRadius = (bounds.North - bounds.South) * 111111.0 / (10 + Math.Sqrt(howMany));
        var colors = new string[] { "#FFFFFF", "#9132D1", "#FFD800", "#846A00", "#AAC643", "#C96A00", "#B200FF", "#CD6A00", "#00A321", "#7F6420" };
        var rnd = new Random();

        this.descriptionText = $"Creating {howMany} circles with random positions...";
        await this.InvokeAsync(this.StateHasChanged);


        for (int i = 0; i < howMany; i++)
        {            
            var color = colors[rnd.Next(0, colors.Length)];
            var circleOptions = new CircleOptions
            {
                Map = map1.InteropObject,
                Center = new LatLngLiteral { Lat = bounds.South + rnd.NextDouble() * (bounds.North - bounds.South), Lng = bounds.West + rnd.NextDouble() * (bounds.East - bounds.West) },
                Radius = 2,
                StrokeColor = color,
                StrokeOpacity = 0.60f,
                StrokeWeight = 2,
                FillColor = color,
                FillOpacity = 0.35f,
                Visible = true,
                ZIndex = 1000000,
            };
            circleOptionsByRef[(++lastId).ToString()] = circleOptions;
        }
        //Time the creation of the circles.
        var sw = Stopwatch.StartNew();
        
        await RefreshCircleList();
        sw.Stop();
        this.descriptionText = $"Created {howMany} circles in {sw.Elapsed.TotalSeconds} seconds\n" +
            $"Disposing will take place upon navigating to another tab";
        await this.InvokeAsync(this.StateHasChanged);
    }

    private async Task RefreshCircleList()
    {
        circleList = await CircleList.SyncAsync(circleList, map1.JsRuntime, circleOptionsByRef, async (ev, sKey, entity) =>
        {
            // Circle has been clicked --> delete it.
            circleOptionsByRef.Remove(sKey);
            await RefreshCircleList();
        });
    }


    public async ValueTask DisposeAsync()
    {
        // Perform async cleanup.
        await DisposeAsyncCore();

        // Dispose of unmanaged resources.
        Dispose(false);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
            try
            {
            if (this.map1 is not null)
                await this.map1.DisposeAsync();
            if (this.circleList is not null)
                await this.circleList.DisposeAsync();

            }
            catch (Microsoft.JSInterop.JSDisconnectedException)
            {
                //Most probably page was refreshed
            }
            finally
            {
                this.circleList = null;
                this.map1 = null;
            }
    }

    protected virtual void Dispose(bool disposing)
    {

        if (!isDisposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}