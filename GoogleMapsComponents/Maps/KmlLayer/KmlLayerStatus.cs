namespace GoogleMapsComponents.Maps.KmlLayer;

public static class KmlLayerStatus
{
    /// <summary>
    /// The document could not be found. Most likely it is an invalid URL, or the document is not publicly available.
    /// </summary>
    public const string DOCUMENT_NOT_FOUND = "DOCUMENT_NOT_FOUND";
    /// <summary>
    /// The document exceeds the file size limits of KmlLayer.
    /// </summary>
    public const string DOCUMENT_TOO_LARGE = "DOCUMENT_TOO_LARGE";
    /// <summary>
    /// The document could not be fetched.
    /// </summary>
    public const string FETCH_ERROR = "FETCH_ERROR";
    /// <summary>
    /// The document is not a valid KML, KMZ or GeoRSS document.
    /// </summary>
    public const string INVALID_DOCUMENT = "INVALID_DOCUMENT";
    /// <summary>
    /// The KmlLayer is invalid. 
    /// </summary>
    public const string INVALID_REQUEST = "INVALID_REQUEST";
    /// <summary>
    /// The document exceeds the feature limits of KmlLayer.
    /// </summary>
    public const string LIMITS_EXCEEDED = "LIMITS_EXCEEDED";
    /// <summary>
    /// The layer loaded successfully.
    /// </summary>
    public const string OK = "OK";
    /// <summary>
    /// The document could not be loaded within a reasonable amount of time.
    /// </summary>
    public const string TIMED_OUT = "TIMED_OUT";
    /// <summary>
    /// The document failed to load for an unknown reason.
    /// </summary>
    public const string UNKNOWN = "UNKNOWN";
}