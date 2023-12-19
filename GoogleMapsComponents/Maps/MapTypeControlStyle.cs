using System.Runtime.Serialization;

namespace GoogleMapsComponents.Maps;

public enum MapTypeControlStyle
{
    /// <summary>
    /// Uses the default map type control.
    /// When the DEFAULT control is shown, it will vary according to window size and other factors.
    /// The DEFAULT control may change in future versions of the API.
    /// </summary>
    [EnumMember(Value = "0")] //Docs say "DEFAULT
    Default,

    /// <summary>
    /// A dropdown menu for the screen realestate conscious.
    /// </summary>
    [EnumMember(Value = "2")] //Docs say "DROPDOWN_MENU"
    DropdownMenu,

    /// <summary>
    /// The standard horizontal radio buttons bar.
    /// </summary>
    [EnumMember(Value = "1")]   //Docs say "HORIZONTAL_BAR"
    HorizontalBar
}