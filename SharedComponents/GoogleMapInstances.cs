using Microsoft.AspNetCore.Blazor.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SharedComponents
{
    internal static class MapComponentInstances
    {
        private static Dictionary<string, MapComponent> _instances = new Dictionary<string, MapComponent>();

        internal static void Add(string id, MapComponent instance)
        {
            _instances.Add(id, instance);
        }

        internal static void Remove(string id)
        {
            _instances.Remove(id);
        }

        internal static MapComponent GetInstance(string id)
        {
            return _instances[id];
        }
    }
}
