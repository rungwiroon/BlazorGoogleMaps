using System;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Builds Maps Static API URLs using the API key of the registered <see cref="IBlazorGoogleMapsKeyService"/>.
/// Registered by <see cref="DependencyInjectionExtensions.AddBlazorGoogleMaps(Microsoft.Extensions.DependencyInjection.IServiceCollection, string)"/>
/// and its overloads, so it can simply be injected:
/// <code>@inject StaticMapService StaticMaps</code>
/// <para/>
/// The <c>StaticMap</c> component uses this service internally; inject it directly when a URL is needed
/// outside of a component, for instance to embed a map in an e-mail or a PDF.
/// </summary>
public class StaticMapService(IBlazorGoogleMapsKeyService keyService)
{
    /// <summary>
    /// Builds the URL of a static map image.
    /// </summary>
    /// <param name="options">The image to request.</param>
    /// <returns>An absolute URL that can be used as the <c>src</c> of an image.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="options"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the options describe a request the API would reject.</exception>
    public async Task<string> GetUrlAsync(StaticMapOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        var apiKey = options.ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey))
        {
            apiKey = (await keyService.GetApiOptions()).Key;
        }

        return StaticMapUrlBuilder.Build(options, apiKey);
    }

    /// <summary>
    /// Builds the URL of a static map image and signs it.
    /// <para/>
    /// Only call this from server side code: the signing secret must not be shipped to a browser.
    /// See <see cref="StaticMapUrlSigner"/>.
    /// </summary>
    /// <param name="options">The image to request.</param>
    /// <param name="signingSecret">The URL signing secret from the Google Cloud console.</param>
    /// <returns>An absolute, signed URL.</returns>
    public async Task<string> GetSignedUrlAsync(StaticMapOptions options, string signingSecret)
    {
        var url = await GetUrlAsync(options);

        return StaticMapUrlSigner.Sign(url, signingSecret);
    }
}
