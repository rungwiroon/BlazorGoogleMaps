using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension
{
    /// <summary>
    /// A class able to manage a lot of Circle objects and get / set their
    /// properties at the same time, eventually with different values
    /// Main concept is that each Circle to can be distinguished by other ones need
    /// to have a "unique key" with a "external world mean", so not necessary it's GUID
    ///
    /// All properties should be called With a Dictionary<string, {property type}> indicating for each Circle(related to that key) the corresponding related property value
    /// </summary>
    class CircleList: IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, Circle> Circles;

        /// <summary>
        /// Create circles list
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="opts">Dictionary of desired Circle keys and CircleOptions values. Key as any type unique key. Not nessary Guid</param>
        /// <returns>new instance of CircleList class will be returned with its Circles dictionary member populated with the corresponding results</returns>
        public static async Task<CircleList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, CircleOptions> opts)
        {
            JsObjectRef jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());

            CircleList obj;
            if (opts.Count > 0)
            {
                Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
                    jsRuntime,
                    "google.maps.Circle",
                    opts.ToDictionary(e => e.Key, e => (object)e.Value));
                Dictionary<string, Circle> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Circle(e.Value));
                obj = new CircleList(jsObjectRef, objs);
            }
            else
            {
                obj = new CircleList(jsObjectRef, null);
            }

            return obj;
        }

        private CircleList(JsObjectRef jsObjectRef, Dictionary<string, Circle> circles)
        {
            _jsObjectRef = jsObjectRef;
            Circles = circles;
        }

        public void Dispose()
        {
            if (Circles.Count > 0)
            {
                _jsObjectRef.DisposeMultipleAsync(Circles.Select(e => e.Value.Guid).ToList());
                Circles.Clear();
            }
        }

        /// <summary>
        /// Only keys not matching with existent Circle keys will be created
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public async Task AddMultipleAsync(Dictionary<string, CircleOptions> opts)
        {
            if (opts.Count > 0)
            {
                Dictionary<string, JsObjectRef> jsObjectRefs = await _jsObjectRef.AddMultipleAsync(
                    "google.maps.Circle",
                    opts.ToDictionary(e => e.Key, e => (object)e.Value));
                Dictionary<string, Circle> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Circle(e.Value));

                //Someone can try to create element yet inside Circles... really not the best approach... but manage it
                List<string> alreadyCreated = Circles.Keys.Intersect(objs.Select(e => e.Key)).ToList();
                await RemoveMultipleAsync(alreadyCreated);

                //Now we can add all required object as NEW object
                foreach (string key in objs.Keys)
                {
                    Circles.Add(key, objs[key]);
                }
            }
        }

        /// <summary>
        /// Only Circle having keys matching with existent keys will be removed
        /// </summary>
        /// <param name="filterKeys"></param>
        /// <returns></returns>
        public async Task RemoveMultipleAsync(List<string> filterKeys = null)
        {
            if ((filterKeys != null) && (filterKeys.Count > 0))
            {
                List<string> foundKeys = Circles.Keys.Intersect(filterKeys).ToList();
                if (foundKeys.Count > 0)
                {
                    List<Guid> foundGuids = Circles.Where(e => foundKeys.Contains(e.Key)).Select(e => e.Value.Guid).ToList();
                    await _jsObjectRef.DisposeMultipleAsync(foundGuids);

                    foreach (string key in foundKeys)
                    {
                        //Circle object needs to dispose call due to previous DisposeMultipleAsync call
                        //Probably superfluous, but Garbage Collector may appreciate it... 
                        Circles[key] = null;
                        Circles.Remove(key);
                    }
                }
            }
        }

        public async Task RemoveMultipleAsync(List<Guid> guids)
        {
            if (guids.Count > 0)
            {
                List<string> foundKeys = Circles.Where(e => guids.Contains(e.Value.Guid)).Select(e => e.Key).ToList();
                if (foundKeys.Count > 0)
                {
                    List<Guid> foundGuids = Circles.Values.Where(e => guids.Contains(e.Guid)).Select(e => e.Guid).ToList();
                    await _jsObjectRef.DisposeMultipleAsync(foundGuids);

                    foreach (string key in foundKeys)
                    {
                        //Circle object needs to dispose call due to previous DisposeMultipleAsync call
                        //Probably superfluous, but Garbage Collector may appreciate it... 
                        Circles[key] = null;
                        Circles.Remove(key);
                    }
                }
            }
        }

        //Find the eventual match between required keys (if any) and yet stored circles key (if any)
        //If filterKeys is null or empty all keys are returned
        //Otherwise only eventually yet stored circle keys that matches with filterKeys
        private List<string> ComputeMathingKeys(List<string> filterKeys = null)
        {
            List<string> matchingKeys;

            if ((filterKeys == null) || (!filterKeys.Any()))
            {
                matchingKeys = Circles.Keys.ToList();
            }
            else
            {
                matchingKeys = Circles.Keys.Where(e => filterKeys.Contains(e)).ToList();
            }

            return matchingKeys;
        }

        //Creates mapping between matching keys and circles Guid
        private Dictionary<Guid, string> ComputeInternalMapping(List<string> matchingKeys)
        {
            return Circles.Where(e => matchingKeys.Contains(e.Key)).ToDictionary(e => Circles[e.Key].Guid, e => e.Key);
        }

        //Creates mapping between circles Guid and empty array of parameters (getter has no parameter)
        private Dictionary<Guid, object> ComputeDictArgs(List<string> matchingKeys)
        {
            return Circles.Where(e => matchingKeys.Contains(e.Key)).ToDictionary(e => e.Value.Guid, e => (object)(new object[] { }));
        }

        //Create an empty result of the correct type in case of no matching keys
        private Task<Dictionary<string, T>> ComputeEmptyResult<T>()
        {
            return Task<Dictionary<string, T>>.Factory.StartNew(() => { return new Dictionary<string, T>(); });
        }        

        public Task<Dictionary<string, LatLngBoundsLiteral>> GetBounds(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<LatLngBoundsLiteral>(
                    "getBounds",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<LatLngBoundsLiteral>();
            }
        }

        public Task<Dictionary<string, LatLngLiteral>> GetCenters(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<LatLngLiteral>(
                    "getCenter",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<LatLngLiteral>();
            }
        }

        public Task<Dictionary<string, bool>> GetDraggables(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getDraggable",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<bool>();
            }
        }

        public Task<Dictionary<string, bool>> GetEditables(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getEditable",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<bool>();
            }
        }

        public Task<Dictionary<string, Map>> GetMaps(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<Map>(
                    "getMap",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<Map>();
            }
        }

        public Task<Dictionary<string, double>> GetRadiuses(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<double>(
                    "getRadius",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<double>();
            }
        }

        public Task<Dictionary<string, bool>> GetVisibles(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getVisible",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<bool>();
            }
        }

        public Task SetCenters(Dictionary<string, LatLngLiteral> centers)
        {
            Dictionary<Guid, object> dictArgs = centers.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setCenter",
                dictArgs);
        }

        public Task SetDraggables(Dictionary<string, bool> draggables)
        {
            Dictionary<Guid, object> dictArgs = draggables.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setDraggable",
                dictArgs);
        }
        
        public Task SetEditables(Dictionary<string, bool> editables)
        {
            Dictionary<Guid, object> dictArgs = editables.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setEditable",
                dictArgs);
        }

        public async Task SetMaps(Dictionary<string, Map> maps)
        {
            Dictionary<Guid, object> dictArgs = maps.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            await _jsObjectRef.InvokeMultipleAsync(
                   "setMap",
                   dictArgs);
        }

        public Task SetOptions(Dictionary<string, CircleOptions> options)
        {
            Dictionary<Guid, object> dictArgs = options.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setOptions",
                dictArgs);
        }

        public Task SetRadiuses(Dictionary<string, double> radiuses)
        {
            Dictionary<Guid, object> dictArgs = radiuses.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setRadius",
                dictArgs);
        }
        
        public Task SetVisibles(Dictionary<string, bool> visibles)
        {
            Dictionary<Guid, object> dictArgs = visibles.ToDictionary(e => Circles[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setVisible",
                dictArgs);
        }
    }
}
