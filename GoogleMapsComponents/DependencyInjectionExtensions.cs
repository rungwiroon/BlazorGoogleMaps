using Microsoft.Extensions.DependencyInjection;

namespace GoogleMapsComponents;

public static class DependencyInjectionExtensions
{
    public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection service)
    {
        return service;
    }
}