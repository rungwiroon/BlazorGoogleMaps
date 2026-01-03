using GoogleMapsComponents;
using GoogleMapsComponents.Maps;
using GoogleMapsComponents.Maps.Places;
using Place = GoogleMapsComponents.Maps.Places.Place;

namespace Demo.Ui.Shared.Pages;

public partial class MapServices
{
    private readonly Stack<Marker> _markers = new Stack<Marker>();

    private string? _search;
    private GoogleMap _map1 = null!;
    private MapOptions _mapOptions = null!;
    private PlaceService _placesService = null!;
    private AutocompleteSessionToken? _token;
    private Geocoder? _geocoder;
    private DateTime _tokenStamp = DateTime.MinValue;

    private string? _message;

    private Place[] _suggestions = [];

    protected override void OnInitialized()
    {
        _mapOptions = new MapOptions
        {
            Zoom = 13,
            Center = new LatLngLiteral(-33.8688, 151.2195),
            MapTypeId = MapTypeId.Roadmap
        };
    }

    private async Task OnAfterMapInit()
    {
        _placesService = await PlaceService.CreateAsync(_map1.JsRuntime, new PlaceOptions()
        {
            Id = Guid.NewGuid().ToString()
        });
        _geocoder = await Geocoder.CreateAsync(_map1.JsRuntime);
    }

    private async Task<AutocompleteSessionToken> GetOrCreateTokenAsync()
    {
        // It is not officially documented how long a session token is valid for.
        // Google only says "a few minutes", but there is one mention of 3 minutes in StackOverflow
        // https://stackoverflow.com/questions/50398801/how-long-do-the-new-places-api-session-tokens-last
        if (_token is null || _tokenStamp == DateTime.MinValue || _tokenStamp.AddMinutes(3).CompareTo(DateTime.Now) < 1)
        {
            _token?.Dispose();
            _token = await AutocompleteSessionToken.CreateAsync(_map1.JsRuntime);
            _tokenStamp = DateTime.Now;
        }

        return _token;
    }

    private bool IsValidInput()
    {
        if (string.IsNullOrEmpty(_search))
        {
            _message = "Invalid input. Please enter some text and try again.";
            return false;
        }

        return true;
    }

    private async Task ClearMarkersAsync()
    {
        while (_markers.Count > 0)
        {
            var marker = _markers.Pop();
            await marker.SetMap(null);
            await marker.DisposeAsync();
        }
    }

    private async Task SearchAsync()
    {
        //This can be executed on every key stroke with 300ms debounce, but I decided to use the search button
        //These requests will be bundled together via the session token until it either expires or placesService.GetDetails is executed.
        //Every session is charged (billed). If session token is omitted/invalid/expired, every request is charged (billed).
        if (!IsValidInput())
        {
            return;
        }

        var request = new SearchByTextRequest
        {
            TextQuery = _search ?? "",
            Fields = ["displayName", "location", "businessStatus"],
            //SessionToken = await GetOrCreateTokenAsync()
        };

        var response = await _placesService.SearchByText(request);
        _suggestions = response.Places?.ToArray() ?? [];
    }

    private async Task GeocodeAsync()
    {
        if (_geocoder is null || !IsValidInput())
        {
            return;
        }

        var response = await _geocoder.Geocode(new GeocoderRequest
        {
            Address = _search
        });

        if (response.Status == GeocoderStatus.Ok)
        {
            await ClearMarkersAsync();

            var bounds = await LatLngBounds.CreateAsync(_map1.JsRuntime);

            foreach (var result in response.Results)
            {
                await RenderLocationAsync(result.FormattedAddress, result.Geometry.Location);
                await bounds.Extend(result.Geometry.Location);
            }

            await _map1.InteropObject.FitBounds(await bounds.ToJson(), 5);
        }
        else
        {
            _message = $"Your request failed with status code: {response.Status}";
        }
    }

    private async Task GetPlaceDetailAsync(string placeId)
    {
        if (_placesService is null)
        {
            return;
        }

        try
        {
            var place = await _placesService.FetchFields(new FetchFieldsRequest
            {
                //PlaceId = placeId,
                Fields = ["address_components", "formatted_address", "geometry", "name", "place_id"],
                //SessionToken = await GetOrCreateTokenAsync()
            });

            //if (place.Status == PlaceServiceStatus.Ok)
            //{
            await RenderPlaceAsync(place);
            //}
            //else
            //{
            //    _message = $"Your request failed with status code: {place.Status}";
            //}
        }
        finally
        {
            //After a request to PlacesService, the token is no longer valid
            _token?.Dispose();
            _token = null;
        }
    }

    private async Task RenderLocationAsync(string title, LatLngLiteral location)
    {
        var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
        {
            Position = location,
            Map = _map1.InteropObject,
            Title = title
        });

        _markers.Push(marker);
    }

    private async Task RenderPlaceAsync(Place? place)
    {
        //Below code borrowed from MapAutocomplete.razor "place_changed" event handler
        if (place?.Location is not null)
        {
            var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
            {
                Position = place.Location.Value,
                Map = _map1.InteropObject,
                Title = place.DisplayName ?? "No name"
            });

            _markers.Push(marker);
        }
        //if (place?.Geometry == null)
        //{
        //    _message = "No results available for " + place?.Name;
        //}
        //else if (place.Geometry.Location.HasValue)
        //{
        //    await _map1.InteropObject.SetCenter(place.Geometry.Location.Value);
        //    await _map1.InteropObject.SetZoom(13);

        //    var marker = await Marker.CreateAsync(_map1.JsRuntime, new MarkerOptions
        //    {
        //        Position = place.Geometry.Location.Value,
        //        Map = _map1.InteropObject,
        //        Title = place.FormattedAddress
        //    });

        //    _markers.Push(marker);

        //    _message = "Displaying result for " + place.Name;
        //}
        //else if (place.Geometry.Viewport != null)
        //{
        //    await _map1.InteropObject.FitBounds(place.Geometry.Viewport, 5);
        //    _message = "Displaying result for " + place.Name;
        //}
    }

    public async ValueTask DisposeAsync()
    {
        await ClearMarkersAsync();

        _token?.Dispose();
        //_placesService?.Dispose();
        _geocoder?.Dispose();
    }
}