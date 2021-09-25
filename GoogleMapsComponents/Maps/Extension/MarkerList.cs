using Microsoft.JSInterop;
using OneOf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension
{
    /// <summary>
    /// A class able to manage a lot of Marker objects and get / set their
    /// properties at the same time, eventually with different values
    /// Main concept is that each Marker to can be distinguished by other ones need
    /// to have a "unique key" with a "external world mean", so not necessary it's GUID
    ///
    /// All properties should be called With a Dictionary<string, {property type}> indicating for each Marker(related to that key) the corresponding related property value
    /// </summary>
    public class MarkerList : ListableEntityListBase<Marker, MarkerOptions>
    {
        public Dictionary<string, Marker> Markers => base.BaseListableEntities;

        /// <summary>
        /// Create markers list
        /// </summary>
        /// <param name="jsRuntime"></param>
        /// <param name="opts">Dictionary of desired Marker keys and MarkerOptions values. Key as any type unique key. Not nessary Guid</param>
        /// <returns>new instance of MarkerList class will be returned with its Markers dictionary member populated with the corresponding results</returns>
        public static async Task<MarkerList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, MarkerOptions> opts)
        {
            JsObjectRef jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());

            MarkerList obj;
            Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
                jsRuntime,
                "google.maps.Marker",
                opts.ToDictionary(e => e.Key, e => (object)e.Value));

            Dictionary<string, Marker> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Marker(e.Value));
            obj = new MarkerList(jsObjectRef, objs);

            return obj;
        }

        /// <summary>
        /// Sync list over lifetime: Create and remove list depending on entity count; 
        /// entities will be removed, added or changed to mirror the given set.
        /// </summary>
        /// <param name="list">
        /// The list to manage. May be null.
        /// </param>
        /// <param name="jsRuntime"></param>
        /// <param name="opts"></param>
        /// <returns>
        /// The managed list. Assign to the variable you used as parameter.
        /// </returns>
        public static async Task<MarkerList> SyncAsync(MarkerList list, IJSRuntime jsRuntime, Dictionary<string, MarkerOptions> opts, Action<MouseEvent, string, Marker> clickCallback = null)
        {
            if (opts.Count == 0)
            {
                if (list != null)
                {
                    await list.SetMultipleAsync(opts);
                    list = null;
                }
            }
            else
            {
                if (list == null)
                {
                    list = await MarkerList.CreateAsync(jsRuntime, new Dictionary<string, MarkerOptions>());
                    if (clickCallback != null)
                    {
                        list.EntityClicked += (sender, e) =>
                        {
                            clickCallback(e.MouseEvent, e.Key, e.Entity);
                        };
                    }
                }
                await list.SetMultipleAsync(opts);
            }
            return list;
        }

        private MarkerList(JsObjectRef jsObjectRef, Dictionary<string, Marker> markers)
            : base(jsObjectRef, markers)
        {
        }

        /// <summary>
        /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public async Task SetMultipleAsync(Dictionary<string, MarkerOptions> opts)
        {
            await base.SetMultipleAsync(opts, "google.maps.Marker");
        }

        /// <summary>
        /// only keys not matching with existent Marker keys will be created
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public async Task AddMultipleAsync(Dictionary<string, MarkerOptions> opts)
        {
            await base.AddMultipleAsync(opts, "google.maps.Marker");
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

        public Task SetOpacities(Dictionary<string, float> opacities)
        {
            Dictionary<Guid, object> dictArgs = opacities.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setOpacity",
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

        public Task SetZIndexes(Dictionary<string, int> zIndexes)
        {
            Dictionary<Guid, object> dictArgs = zIndexes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setZIndex",
                dictArgs);
        }
    }
}
