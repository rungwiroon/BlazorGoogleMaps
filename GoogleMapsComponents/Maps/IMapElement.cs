using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps;

public interface IMapElement
{
    Task SetMap(Map? map);
    Task<Map> GetMap();
}