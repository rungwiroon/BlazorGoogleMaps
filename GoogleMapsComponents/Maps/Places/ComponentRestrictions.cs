using OneOf;

namespace GoogleMapsComponents.Maps.Places
{
    public class ComponentRestrictions
    {
        public OneOf<string,string[]> Country { get; set; }
    }
}
