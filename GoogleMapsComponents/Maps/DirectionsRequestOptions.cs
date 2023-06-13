namespace GoogleMapsComponents.Maps;

/// <summary>
/// By default route request strips overview_path, overview_polyline, legs.steps,legs.steps.lat_lngs, legs.steps.path since these brings overhead to resposne.
/// Clients side has no issues, but server side reaches MaximumReceiveMessageSize (Default 32kb). Removing striping pats should go along with increased MaximumReceiveMessageSize on server side
/// <br /> All strips by default true
/// <seealso cref="https://docs.microsoft.com/en-us/aspnet/core/signalr/configuration?view=aspnetcore-3.0&tabs=dotnet#configure-server-options"/>
/// </summary>
public class DirectionsRequestOptions
{
    public bool StripOverviewPath { get; set; } = true;
    public bool StripOverviewPolyline { get; set; } = true;

    /// <summary>
    /// If true then StripLegsStepsLatLngs and StripLegsStepsPath always be stripped
    /// </summary>
    public bool StripLegsSteps { get; set; } = true;

    /// <summary>
    /// If StripLegsSteps true then StripLegsStepsLatLngs and StripLegsStepsPath always be stripped
    /// </summary>
    public bool StripLegsStepsLatLngs { get; set; } = true;

    /// <summary>
    /// If StripLegsSteps true then StripLegsStepsLatLngs and StripLegsStepsPath always be stripped
    /// </summary>
    public bool StripLegsStepsPath { get; set; } = true;
        
}