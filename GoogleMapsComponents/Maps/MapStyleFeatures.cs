namespace GoogleMapsComponents.Maps;

/// <summary>
/// Reference: https://developers.google.com/maps/documentation/javascript/style-reference
/// </summary>
public static class MapStyleFeatures
{
    /// <summary>
    /// (default) selects all features.
    /// </summary>
    public const string All = "all";
        
    /// <summary>
    /// selects all administrative areas. Styling affects only the labels of administrative areas, not the geographical borders or fill.
    /// </summary>
    public const string Administrative = "administrative";
    /// <summary>
    /// selects countries
    /// </summary>
    public const string AdministrativeCountry = "administrative.country";
    /// <summary>
    /// selects land parcels
    /// </summary>
    public const string AdministrativeLandParcel = "administrative.land_parcel";
    /// <summary>
    /// selects localities
    /// </summary>
    public const string AdministrativeLocality = "administrative.locality";
    /// <summary>
    /// selects neighborhoods
    /// </summary>
    public const string AdministrativeNeighborhood = "administrative.neighborhood";
    /// <summary>
    /// selects provinces
    /// </summary>
    public const string AdministrativeProvince = "administrative.province";
        
    /// <summary>
    /// selects all landscapes
    /// </summary>
    public const string Landscape = "landscape";
    /// <summary>
    /// selects man-made features, such as buildings and other structures
    /// </summary>
    public const string LandscapeManMade = "landscape.man_made";
    /// <summary>
    /// selects natural features, such as mountains, rivers, deserts, and glaciers
    /// </summary>
    public const string LandscapeNatural = "landscape.natural";
    /// <summary>
    /// selects land cover features, the physical material that covers the earth's surface, such as forests, grasslands, wetlands, and bare ground
    /// </summary>
    public const string LandscapeNaturalLandcover = "landscape.natural.landcover";
    /// <summary>
    /// selects terrain features of a land surface, such as elevation, slope, and orientation
    /// </summary>
    public const string LandscapeNaturalTerrain = "landscape.natural.terrain";
        
    /// <summary>
    /// selects all points of intrest
    /// </summary>
    public const string Poi = "poi";
    /// <summary>
    /// selects tourist attractions
    /// </summary>
    public const string PoiAttraction = "poi.attraction";
    /// <summary>
    /// selects businesses
    /// </summary>
    public const string PoiBusiness = "poi.business";
    /// <summary>
    /// selects government buildings
    /// </summary>
    public const string PoiGovernment = "poi.government";
    /// <summary>
    /// selects emergency services, including hospitals, pharmacies, police, doctors, and others
    /// </summary>
    public const string PoiMedical = "poi.medical";
    /// <summary>
    /// selects parks
    /// </summary>
    public const string PoiPark = "poi.park";
    /// <summary>
    /// selects places of worship, including churches, temples, mosques, and others
    /// </summary>
    public const string PoiPlaceOfWorship = "poi.place_of_worship";
    /// <summary>
    /// selects schools
    /// </summary>
    public const string PoiSchool = "poi.school";
    /// <summary>
    /// selects sports complexes
    /// </summary>
    public const string PoiSportsComplex = "poi.sports_complex";
        
    /// <summary>
    /// selects all roads
    /// </summary>
    public const string Road = "road";
    /// <summary>
    /// selects arterial roads
    /// </summary>
    public const string RoadArterial = "road.artertial";
    /// <summary>
    /// selects highways
    /// </summary>
    public const string RoadHighway = "road.highway";
    /// <summary>
    /// selects highways with controlled access
    /// </summary>
    public const string RoadHighwayControlled = "road.highway.controlled_access";
    /// <summary>
    /// selects local roads
    /// </summary>
    public const string RoadLocal = "road.local";
        
    /// <summary>
    /// selects all transit stations and lines
    /// </summary>
    public const string Transit = "transit";
    /// <summary>
    /// selects transit lines
    /// </summary>
    public const string TransitLine = "transit.line";
    /// <summary>
    /// selects all transit stations
    /// </summary>
    public const string TransitStation = "transit.station";
    /// <summary>
    /// selects airports
    /// </summary>
    public const string TransitAirport = "transit.station.airport";
    /// <summary>
    /// selects bus stops
    /// </summary>
    public const string TransitBus = "transit.station.bus";
    /// <summary>
    /// selects rail stations
    /// </summary>
    public const string TransitRail = "transit.station.rail";
        
    /// <summary>
    /// selects bodies of water
    /// </summary>
    public const string Water = "water";
}