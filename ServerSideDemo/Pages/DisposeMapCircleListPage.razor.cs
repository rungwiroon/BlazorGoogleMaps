using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Extension;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ServerSideDemo.Pages;

public partial class DisposeMapCircleListPage : ComponentBase, IDisposable, IAsyncDisposable
{
    private GoogleMap? _map1;
    private MapOptions? _mapOptions;
    private int _bunchsize = 6000;

    private CircleList? _circleList;
    private readonly Dictionary<string, CircleOptions> _circleOptionsByRef = new Dictionary<string, CircleOptions>();
    private int _lastId;
    private string _descriptionText = "";
    private bool _isDisposed;

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions()
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
    private async Task CreateBunchOfCirclesInJs()
    {
        int howMany = _bunchsize;
        _descriptionText = $"Creating {howMany} circles with random positions...";
        //Time the creation of the circles.
        var sw = Stopwatch.StartNew();

        IJSObjectReference serverSideScripts = await _map1!.JsRuntime.InvokeAsync<IJSObjectReference>("import", "/js/serverSideScripts.js");
        await serverSideScripts.InvokeVoidAsync("createBunchOfCirclesInJs", _map1.InteropObject.Guid, howMany);

        sw.Stop();
        _descriptionText = $"Created {howMany} circles in {sw.Elapsed.TotalSeconds} seconds\n" +
                           $"Disposing will take place upon navigating to another tab";
        await InvokeAsync(StateHasChanged);
    }

    private async Task CreateBunchOfCircles()
    {
        int howMany = _bunchsize;
        var bounds = await _map1!.InteropObject.GetBounds();
        //double maxRadius = (bounds.North - bounds.South) * 111111.0 / (10 + Math.Sqrt(howMany));
        var colors = new[] { "#FFFFFF", "#9132D1", "#FFD800", "#846A00", "#AAC643", "#C96A00", "#B200FF", "#CD6A00", "#00A321", "#7F6420" };
        var rnd = new Random();

        _descriptionText = $"Creating {howMany} circles with random positions...";
        await InvokeAsync(StateHasChanged);


        for (int i = 0; i < howMany; i++)
        {
            var color = colors[rnd.Next(0, colors.Length)];
            var circleOptions = new CircleOptions
            {
                Map = _map1.InteropObject,
                Center = new LatLngLiteral
                {
                    Lat = bounds.South + rnd.NextDouble() * (bounds.North - bounds.South),
                    Lng = bounds.West + rnd.NextDouble() * (bounds.East - bounds.West)
                },
                Radius = 2,
                StrokeColor = color,
                StrokeOpacity = 0.60f,
                StrokeWeight = 2,
                FillColor = color,
                FillOpacity = 0.35f,
                Visible = true,
                ZIndex = 1000000,
            };
            _circleOptionsByRef[(++_lastId).ToString()] = circleOptions;
        }
        //Time the creation of the circles.
        var sw = Stopwatch.StartNew();
        await RefreshCircleList();
        sw.Stop();

        _descriptionText = $"Created {howMany} circles in {sw.Elapsed.TotalSeconds} seconds\n" +
            $"Disposing will take place upon navigating to another tab";
        await InvokeAsync(StateHasChanged);
    }

    private async Task RefreshCircleList()
    {
        _circleList = await CircleList.SyncAsync(_circleList, _map1!.JsRuntime, _circleOptionsByRef, async (ev, sKey, entity) =>
        {
            // Circle has been clicked --> delete it.
            _circleOptionsByRef.Remove(sKey);
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
            if (_map1 is not null)
            {
                await _map1.DisposeAsync();
            }

            if (_circleList is not null)
            {
                await _circleList.DisposeAsync();
            }
        }
        catch (Microsoft.JSInterop.JSDisconnectedException)
        {
            //Most probably page was refreshed
        }
        finally
        {
            _circleList = null;
            _map1 = null;
        }
    }

    protected virtual void Dispose(bool disposing)
    {

        if (!_isDisposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}