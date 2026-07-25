using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Renders a Maps Static API image as a plain <c>img</c> element.
/// <para/>
/// Unlike <see cref="GoogleMap"/> this component loads no JavaScript and creates no interop objects, which
/// makes it suitable for thumbnails, previews, print views and pre-rendered pages. The image is a picture
/// of a map, so it cannot be panned or zoomed.
/// <para/>
/// The API key comes from the registered <see cref="IBlazorGoogleMapsKeyService"/>; set
/// <see cref="StaticMapOptions.ApiKey"/> when no key service is registered.
/// <para/>
/// <code>
/// &lt;StaticMap Options="@(new StaticMapOptions { Center = new LatLngLiteral(13.505892, 100.8162), Zoom = 13 })" /&gt;
/// </code>
/// </summary>
public partial class StaticMap : ComponentBase
{
    private IBlazorGoogleMapsKeyService? _keyService;

    /// <summary>
    /// Gets or sets the service provider used to look up the API key service.
    /// </summary>
    [Inject]
    public IServiceProvider ServiceProvider { get; protected set; } = null!;

    /// <summary>
    /// The image to request. Changing it rebuilds <see cref="Url"/> on the next render.
    /// </summary>
    [Parameter]
    [EditorRequired]
    public StaticMapOptions Options { get; set; } = null!;

    /// <summary>
    /// The alternative text of the image.
    /// </summary>
    [Parameter]
    public string? Alt { get; set; } = "Map";

    /// <summary>
    /// Attributes applied to the rendered <c>img</c> element, such as <c>class</c>, <c>style</c> or <c>loading</c>.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Raised whenever the requested image changes, with the URL that is about to be rendered.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnUrlChanged { get; set; }

    /// <summary>
    /// The URL currently rendered, or <see langword="null"/> before the first URL has been built.
    /// </summary>
    public string? Url { get; private set; }

    /// <inheritdoc />
    protected override void OnInitialized()
    {
        // Resolved from the provider rather than with [Inject] so that the component also works in
        // applications that never registered a key service and pass the key through the options.
        _keyService = ServiceProvider.GetService<IBlazorGoogleMapsKeyService>();

        base.OnInitialized();
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        ArgumentNullException.ThrowIfNull(Options, nameof(Options));

        var apiKey = Options.ApiKey;
        if (string.IsNullOrWhiteSpace(apiKey) && _keyService != null)
        {
            apiKey = (await _keyService.GetApiOptions()).Key;
        }

        var url = StaticMapUrlBuilder.Build(Options, apiKey);
        if (url == Url)
        {
            return;
        }

        Url = url;

        if (OnUrlChanged.HasDelegate)
        {
            await OnUrlChanged.InvokeAsync(url);
        }
    }
}
