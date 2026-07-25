using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace GoogleMapsComponents.Maps.StaticMaps;

/// <summary>
/// Image formats supported by the Maps Static API.
/// <para/>
/// https://developers.google.com/maps/documentation/maps-static/start#ImageFormats
/// </summary>
[JsonConverter(typeof(EnumMemberConverter<StaticMapImageFormat>))]
public enum StaticMapImageFormat
{
    /// <summary>
    /// 8 bit PNG. Smaller than <see cref="Png"/> at the cost of colour fidelity.
    /// </summary>
    [EnumMember(Value = "png8")]
    Png8,

    /// <summary>
    /// 24 bit PNG. This is the default format of the Maps Static API.
    /// </summary>
    [EnumMember(Value = "png")]
    Png,

    /// <summary>
    /// 32 bit PNG.
    /// </summary>
    [EnumMember(Value = "png32")]
    Png32,

    /// <summary>
    /// GIF.
    /// </summary>
    [EnumMember(Value = "gif")]
    Gif,

    /// <summary>
    /// JPEG. Offers the greatest compression, at the cost of artifacts around sharp edges such as labels.
    /// </summary>
    [EnumMember(Value = "jpg")]
    Jpg,

    /// <summary>
    /// Non progressive JPEG, for consumers that cannot render progressive images.
    /// </summary>
    [EnumMember(Value = "jpg-baseline")]
    JpgBaseline
}
