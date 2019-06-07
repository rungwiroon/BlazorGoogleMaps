using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents
{
    internal static class JsObjectRefInstances
    {
        private static Dictionary<string, IJsObjectRef> _instances = new Dictionary<string, IJsObjectRef>();

        internal static void Add(IJsObjectRef instance)
        {
            _instances.Add(instance.Guid.ToString(), instance);
        }

        internal static void Remove(string guid)
        {
            _instances.Remove(guid);
        }

        internal static IJsObjectRef GetInstance(string guid)
        {
            return _instances[guid];
        }
    }
}
