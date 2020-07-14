using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension
{
    public class MarkerList : IDisposable
    {
        private readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, Marker> Markers;

        public async static Task<MarkerList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, MarkerOptions> opts)
        {
            JsObjectRef jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());
            
            MarkerList obj;
            if (opts.Count > 0)
            {
                Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
                    jsRuntime, 
                    "google.maps.Marker", 
                    opts.ToDictionary(e => e.Key, e => (object)e.Value));
                Dictionary<string, Marker> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Marker(e.Value));
                obj = new MarkerList(jsObjectRef, objs);
            }
            else
            {
                obj = new MarkerList(jsObjectRef, null);
            }
            
            return obj;
        }

        private MarkerList(JsObjectRef jsObjectRef, Dictionary<string, Marker> markers)
        {
            _jsObjectRef = jsObjectRef;
            Markers = markers;
        }

        public void Dispose()
        {
            if (Markers.Count > 0)
            {
                _jsObjectRef.DisposeMultipleAsync(Markers.Select(e => e.Value.Guid).ToList());
                Markers.Clear();
            }
        }

        public async Task AddMultipleAsync(Dictionary<string, MarkerOptions> opts)
        {
            if (opts.Count > 0)
            {
                Dictionary<string, JsObjectRef> jsObjectRefs = await _jsObjectRef.AddMultipleAsync(
                    "google.maps.Marker",
                    opts.ToDictionary(e => e.Key, e => (object)e.Value));
                Dictionary<string, Marker> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Marker(e.Value));

                //Someone can try to create element yet inside Markers... really not the best approach... but manage it
                List<string> alreadyCreated = Markers.Keys.Intersect(objs.Select(e => e.Key)).ToList();
                await RemoveMultipleAsync(alreadyCreated);

                //Now we can add all required object as NEW object
                foreach (string key in objs.Keys)
                {
                    Markers.Add(key, objs[key]);
                }
            }            
        }        

        public async Task RemoveMultipleAsync(List<string> filterKeys = null)
        {
            if (keys.Count >0)
            {
                List<string> foundKeys = Markers.Keys.Intersect(keys).ToList();                
                if (foundKeys.Count > 0)
                {
                    List<Guid> foundGuids = Markers.Where(e => foundKeys.Contains(e.Key)).Select(e => e.Value.Guid).ToList();
                    await _jsObjectRef.DisposeMultipleAsync(foundGuids);

                    foreach (string key in foundKeys)
                    {
                        //Marker object needs to dispose call due to previous DisposeMultipleAsync call
                        //Probably superfluous, but Garbage Collector may appreciate it... 
                        Markers[key] = null;
                        Markers.Remove(key);
                    }
                }
            }
        }

        public async Task RemoveMultipleAsync(List<Guid> guids)
        {
            if (guids.Count > 0)
            {                
                List<string> foundKeys = Markers.Where(e => guids.Contains(e.Value.Guid)).Select(e => e.Key).ToList();
                if (foundKeys.Count > 0)                
                {
                    List<Guid> foundGuids = Markers.Values.Where(e => guids.Contains(e.Guid)).Select(e => e.Guid).ToList();
                    await _jsObjectRef.DisposeMultipleAsync(foundGuids);

                    foreach (string key in foundKeys)
                    {
                        //Marker object needs to dispose call due to previous DisposeMultipleAsync call
                        //Probably superfluous, but Garbage Collector may appreciate it... 
                        Markers[key] = null;
                        Markers.Remove(key);
                    }
                }
            }
        }

        //Find the eventual match between required keys (if any) and yet stored markers key (if any)
        //If filterKeys is null or empty all keys are returned
        //Otherwise only eventually yet stored marker keys that matches with filterKeys
        private List<string> ComputeMathingKeys(List<string> filterKeys = null)
        {
            List<string> matchingKeys;

            if ((filterKeys == null) || (!filterKeys.Any()))
            {
                matchingKeys = Markers.Keys.ToList();
            }
            else
            {
                matchingKeys = Markers.Keys.Where(e => filterKeys.Contains(e)).ToList();
            }

            return matchingKeys;
        }

        //Creates mapping between matching keys and markers Guid
        private Dictionary<Guid, string> ComputeInternalMapping(List<string> matchingKeys)
        {
            return Markers.Where(e => matchingKeys.Contains(e.Key)).ToDictionary(e => Markers[e.Key].Guid, e => e.Key);
        }

        //Creates mapping between markers Guid and empty array of parameters (getter has no parameter)
        private Dictionary<Guid, object> ComputeDictArgs(List<string> matchingKeys)
        {
            return Markers.Where(e => matchingKeys.Contains(e.Key)).ToDictionary(e => e.Value.Guid, e => (object)(new object[] { }));
        }

        //Create an empty result of the correct type in case of no matching keys
        private Task<Dictionary<string, T>> ComputeEmptyResult<T>()
        {
            return Task<Dictionary<string, T>>.Factory.StartNew(() => { return new Dictionary<string, T>(); });
        }

        public Task<Dictionary<string, Animation>> GetAnimations(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<string>(
                    "getAnimation",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => Helper.ToEnum<Animation>(r.Value)));
            }
            else
            {
                return ComputeEmptyResult<Animation>();
            }            
        }

        public Task<Dictionary<string, bool>> GetClickables(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getClickable",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<bool>();
            }            
        }

        public Task<Dictionary<string, string>> GetCursors(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<string>(
                    "getCursor",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<string>();
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

        public Task<Dictionary<string, OneOf<string, Icon, Symbol>>> GetIcons(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<OneOf<string, Icon, Symbol>>(
                    "getIcon",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<OneOf<string, Icon, Symbol>>();
            }           
        }

        public Task<Dictionary<string, string>> GetLabels(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<string>(
                    "getLabel",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<string>();
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
       
        public Task<Dictionary<string, LatLngLiteral>> GetPositions(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);            

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<LatLngLiteral>(
                    "getPosition",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<LatLngLiteral>();
            }
        }

        public Task<Dictionary<string, MarkerShape>> GetShapes(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<MarkerShape>(
                    "getShape",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<MarkerShape>();
            }
        }

        public Task<Dictionary<string, string>> GetTitles(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<string>(
                    "getTitle",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<string>();
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

        public Task<Dictionary<string, int>> GetZIndexes(List<string> filterKeys = null)
        {
            List<string> matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                return _jsObjectRef.InvokeMultipleAsync<int>(
                    "getZIndex",
                    dictArgs).ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
            }
            else
            {
                return ComputeEmptyResult<int>();
            }            
        }

        /// <summary>
        /// Start an animation. 
        /// Any ongoing animation will be cancelled. 
        /// Currently supported animations are: BOUNCE, DROP. 
        /// Passing in null will cause any animation to stop.
        /// </summary>
        /// <param name="animation"></param>
        public Task SetAnimations(Dictionary<string, Animation> animations)
        {
            Dictionary<Guid, object> dictArgs = animations.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setAnimation",
                dictArgs);
        }

        public Task SetClickables(Dictionary<string, bool> flags)
        {
            Dictionary<Guid, object> dictArgs = flags.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setClickable",
                dictArgs);
        }

        public Task SetCursors(Dictionary<string, string> cursors)
        {
            Dictionary<Guid, object> dictArgs = cursors.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setCursor",
                dictArgs);
        }

        public Task SetDraggables(Dictionary<string, bool> flags)
        {
            Dictionary<Guid, object> dictArgs = flags.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setDraggable",
                dictArgs);
        }

        public Task SetIcons(Dictionary<string, string> icons)
        {
            Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setIcon",
                dictArgs);
        }

        public Task SetIcons(Dictionary<string, Icon> icons)
        {
            Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setIcon",
                dictArgs);
        }

        public Task SetLabels(Dictionary<string, Symbol> labels)
        {
            Dictionary<Guid, object> dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setLabel",
                dictArgs);
        }

        /// <summary>
        /// Renders the marker on the specified map or panorama. 
        /// If map is set to null, the marker will be removed.
        /// </summary>
        /// <param name="map"></param>
        public async Task SetMaps(Dictionary<string, Map> maps)
        {
            Dictionary<Guid, object> dictArgs = maps.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            await _jsObjectRef.InvokeMultipleAsync(
                   "setMap",
                   dictArgs);
        }

        public Task SetOpacities(Dictionary<string, float> opacities)
        {
            Dictionary<Guid, object> dictArgs = opacities.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setOpacity",
                dictArgs);
        }

        public Task SetOptions(Dictionary<string, MarkerOptions> options)
        {
            Dictionary<Guid, object> dictArgs = options.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setOptions",
                dictArgs);
        }

        public Task SetPositions(Dictionary<string, LatLngLiteral> latLngs)
        {
            Dictionary<Guid, object> dictArgs = latLngs.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setPosition",
                dictArgs);
        }

        public Task SetShapes(Dictionary<string, MarkerShape> shapes)
        {
            Dictionary<Guid, object> dictArgs = shapes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setShape",
                dictArgs);
        }

        public Task SetTitles(Dictionary<string, string> titles)
        {
            Dictionary<Guid, object> dictArgs = titles.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setTitle",
                dictArgs);
        }

        public Task SetVisibles(Dictionary<string, bool> visibles)
        {            
            Dictionary<Guid, object> dictArgs = visibles.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setVisible",
                dictArgs);
        }

        public Task SetZIndexes(Dictionary<string, int> zIndexes)
        {
            Dictionary<Guid, object> dictArgs = zIndexes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setZIndex",
                dictArgs);
        }        
    }
}
