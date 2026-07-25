namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// The colour names understood by the <c>markers</c> and <c>path</c> parameters of the Maps Static API.
/// Anywhere a colour is accepted you may also pass a hex value such as "0xFFFFCC", "#FFFFCC" or "FFFFCC";
/// paths additionally accept a 32 bit value with an alpha component, for example "0xFFFFCC33".
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#MarkerStyles
/// </summary>
public static class StaticMapColors
{
    /// <summary>The predefined colour "black".</summary>
    public const string Black = "black";

    /// <summary>The predefined colour "brown".</summary>
    public const string Brown = "brown";

    /// <summary>The predefined colour "green".</summary>
    public const string Green = "green";

    /// <summary>The predefined colour "purple".</summary>
    public const string Purple = "purple";

    /// <summary>The predefined colour "yellow".</summary>
    public const string Yellow = "yellow";

    /// <summary>The predefined colour "blue".</summary>
    public const string Blue = "blue";

    /// <summary>The predefined colour "gray".</summary>
    public const string Gray = "gray";

    /// <summary>The predefined colour "orange".</summary>
    public const string Orange = "orange";

    /// <summary>The predefined colour "red".</summary>
    public const string Red = "red";

    /// <summary>The predefined colour "white".</summary>
    public const string White = "white";
}
