using GoogleMapsComponents.Maps;
using Microsoft.AspNetCore.Components;
#if DEBUG
#endif

namespace Demo.Ui.Shared.Pages;

public partial class GMap
{
    [Parameter]
    public string Width { get; set; }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await InitAsync(Element, Options);
        //Debug.WriteLine("Init finished");
        await OnAfterInit.InvokeAsync(null);
    }

    protected override void OnAfterRender(bool firstRender)
    {
        base.OnAfterRender(firstRender);
    }

    public AdvancedMarkerElementOptions MeMarkerOptions;
    public async Task<AdvancedMarkerElement> AddMeMarker()
    {
        return meMarker = await AddMarker(MeMarkerOptions);
    }

    public async Task<AdvancedMarkerElement> AddMeMarker(double Lat, double Long, string Label, bool Draggable = false)
    {
        var position = new LatLngLiteral(Lat, Long);
        if (meMarker != null)
        {
            MeMarkerOptions.Title = Label;
            MeMarkerOptions.Position = position;
            MeMarkerOptions.GmpDraggable = Draggable;
            await meMarker.SetPosition(position);
            await meMarker.SetGmpDraggable(Draggable);
            await meMarker.SetContent(new PinElement { Glyph = Label });
            await meMarker.SetMap(InteropObject);
        }
        else
        {
            MeMarkerOptions = new AdvancedMarkerElementOptions
            {
                Map = InteropObject,
                Title = Label,
                GmpClickable = true,
                GmpDraggable = Draggable,
                Position = position,
                Content = new PinElement { Glyph = Label }
            };
            meMarker = await AddMarker(MeMarkerOptions);
        }
        if (Draggable)
        {
            await meMarker.AddListener<MouseEvent>("dragend", async e => await OnMakerDragEnd(meMarker, e));
        }

        return meMarker;
    }

    private async Task OnMakerDragEnd(AdvancedMarkerElement marker, MouseEvent e)
    {
        MeMarkerOptions.Position = e.LatLng;
        await MeDragEnd.InvokeAsync(e);
    }

    private AdvancedMarkerElement meMarker;
    /// <summary>
    /// Where I am
    /// </summary>
    public AdvancedMarkerElement MeMarker => meMarker;

    /// <summary>
    /// Where I want to go.
    /// </summary>
    public AdvancedMarkerElement DestMarker;
    /// <summary>
    /// Where the truck is.
    /// </summary>
    public AdvancedMarkerElement TruckMarker;

    public GMap()
    {
        Options = new MapOptions() { Zoom = 10, Center = new LatLngLiteral(54D, -105D), MapTypeId = MapTypeId.Roadmap };
    }

    protected async Task<AdvancedMarkerElement> AddMarker(AdvancedMarkerElementOptions TheMarkerOptions)
    {
        return await AdvancedMarkerElement.CreateAsync(JsRuntime, TheMarkerOptions);
    }

    protected async Task RemoveMarker(AdvancedMarkerElement TheMarker)
    {
        await TheMarker.SetMap(null);
    }

    public async Task MapInitialized()
    {
#if DEBUG
        Console.WriteLine("GMap Initialized:");
        Console.WriteLine($"\t Height: {Height}");
        if (Options != null)
        {
            Console.WriteLine($"\t Options: {Options}");

            Console.WriteLine($"\t Zoom: {Options.Zoom}");
            if (Options.Center.HasValue)
            {
                Console.WriteLine($"\t Center: ({Options.Center.Value.Lat}, {Options.Center.Value.Lng})");
            }
        }
#endif
        if (MeMarker != null)
        {
#if DEBUG
            var pos = await MeMarker.GetPosition();
            Console.WriteLine($"\t Me: ({pos.Lat}, {pos.Lng})");
#endif
            await AddMeMarker();
        }
    }

    /// <summary>
    /// Let me know when the drag of the Me Marker is done.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEvent> MeDragEnd { get; set; }

    /// <summary>
    /// Change the center of the map.
    /// </summary>
    /// <param name="TheCenter">Location of center</param>
    /// <returns>A Task to wait on.</returns>
    public async Task CenterMap(LatLngLiteral TheCenter)
    {
        Options.Center = TheCenter;
        if (InteropObject != null)
        {
            await InteropObject.SetCenter(TheCenter);
        }
    }

    /// <summary>
    /// Change the center of the map.
    /// </summary>
    /// <param name="Lat">Latitude of new center.</param>
    /// <param name="Long">Longitude of new center.</param>
    /// <returns>A Task to wait on.</returns>
    public async Task CenterMap(double Lat, double Long)
    {
        await CenterMap(new LatLngLiteral(Lat, Long));
    }
}
