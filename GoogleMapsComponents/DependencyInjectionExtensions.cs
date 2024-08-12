using Microsoft.Extensions.DependencyInjection;

namespace GoogleMapsComponents;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Adds a basic service to provide a google api key for the application when using bootstrap loader method for Google API <para/>
    /// 
    /// If implementing your own key service, be sure to register the service as the implementation of <see cref="IBlazorGoogleMapsKeyService"/>,
    /// so that the <see cref="GoogleMap"/> component can find it. For example instead of this function you might use:
    /// <code>services.AddScoped&lt;<see cref="IBlazorGoogleMapsKeyService"/>, YourServiceImplmentation&gt;();</code>
    /// </summary>
    public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection services, string key)
    {
        services.AddScoped<IBlazorGoogleMapsKeyService>(_ => new BlazorGoogleMapsKeyService(key));
        return services;
    }

    /// <inheritdoc cref="AddBlazorGoogleMaps(IServiceCollection, string)"/>
    public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection services, Maps.MapApiLoadOptions opts)
    {
        services.AddScoped<IBlazorGoogleMapsKeyService>(_ => new BlazorGoogleMapsKeyService(opts));
        return services;
    }

    public static IServiceCollection AddBlazorGoogleMaps(this IServiceCollection services, IBlazorGoogleMapsKeyService blazorGoogleMapsKeyService)
    {
        services.AddScoped(_ => blazorGoogleMapsKeyService);
        return services;
    }
}