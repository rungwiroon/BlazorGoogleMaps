namespace GoogleMapsComponents.Maps.KmlLayer;

public class KmlAuthor
{
    /// <summary>
    /// The author's e-mail address, or an empty string if not specified.
    /// </summary>
    public string Email { get; set; } = string.Empty;
    /// <summary>
    /// The author's name, or an empty string if not specified.
    /// </summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>
    /// The author's home page, or an empty string if not specified.
    /// </summary>
    public string Uri { get; set; } = string.Empty;
}