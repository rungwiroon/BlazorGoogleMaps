namespace GoogleMapsComponents.Maps.Places;

public class AutocompleteRequest
{
    /// <summary>
    /// The text string on which to search.
    /// </summary>
    public required string Input { get; set; }

    /// <summary>
    /// A token which identifies an Autocomplete session for billing purposes.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Generate a new session token via <see cref="AutocompleteSessionToken"/>. The session begins when the user 
    /// starts typing a query, and concludes when they select a place and call <see cref="PlaceService.FetchFieldsAsync"/>. 
    /// Each session can have multiple queries, followed by one fetchFields call.
    /// </para>
    /// <para>
    /// The credentials used for each request within a session must belong to the same Google Cloud Console project. 
    /// Once a session has concluded, the token is no longer valid; your app must generate a fresh token for each session.
    /// </para>
    /// <para>
    /// If the sessionToken parameter is omitted, or if you reuse a session token, the session is charged as if no 
    /// session token was provided (each request is billed separately).
    /// </para>
    /// <para>
    /// When a session token is provided in the request to <see cref="PlaceService.GetAutocompleteSuggestionsAsync"/>, 
    /// the same token will automatically be included in the first call to <see cref="PlaceService.FetchFieldsAsync"/> 
    /// on a Place returned by the resulting <see cref="AutocompleteSuggestion"/>.
    /// </para>
    /// <para>
    /// <strong>Recommended guidelines:</strong>
    /// <list type="bullet">
    /// <item><description>Use session tokens for all Place Autocomplete calls.</description></item>
    /// <item><description>Generate a fresh token for each session.</description></item>
    /// <item><description>Pass a unique session token for each new session. Using the same token for more than one session will result in each request being billed individually.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public AutocompleteSessionToken? SessionToken { get; set; }
}