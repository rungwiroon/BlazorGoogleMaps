using Microsoft.Extensions.DependencyInjection;
using System;

namespace GoogleMapsComponents;

public static class DependencyInjectionExtensions
{
    internal static BlazorGoogleMapsOptions MapOptions { get; set; } = new BlazorGoogleMapsOptions();
    public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection service, Action<BlazorGoogleMapsOptions>? opts = null)
    {
        MapOptions = new BlazorGoogleMapsOptions();
        opts?.Invoke(MapOptions);

        if (MapOptions.UseBootstrapLoader)
        {
            if (MapOptions.KeyProvider is null)
            {
                throw new ArgumentNullException(nameof(MapOptions.KeyProvider), "Key is required when using bootstrap loader.");
            }

        }

        return service;
    }
}

//All options
//https://developers.google.com/maps/documentation/javascript/load-maps-js-api#required_parameters
public class BlazorGoogleMapsOptions
{
    /// <summary>
    /// Is selected option then it uses the bootstrap loader to load the google maps script.
    /// https://developers.google.com/maps/documentation/javascript/overview#Loading_the_Maps_API
    /// </summary>
    public bool UseBootstrapLoader { get; set; }

    /// <summary>
    /// Default is weekly.
    /// https://developers.google.com/maps/documentation/javascript/versions
    /// </summary>
    public string Version { get; set; } = "weekly";

    /// <summary>
    /// Comma separated list of libraries to load.
    /// https://developers.google.com/maps/documentation/javascript/libraries
    /// </summary>
    public string? Libraries { get; set; }

    public Func<string>? KeyProvider { get; set; }
}