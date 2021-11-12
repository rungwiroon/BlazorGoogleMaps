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

        public async ValueTask<Dictionary<string, Animation>> GetAnimations(List<string> filterKeys = null)
        {
            var matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                var res = await _jsObjectRef.InvokeMultipleAsync<string>(
                    "getAnimation",
                    dictArgs);
                    
                return res.ToDictionary(r => internalMapping[new Guid(r.Key)], r => Helper.ToEnum<Animation>(r.Value));
            }
            else
            {
                return ComputeEmptyResult<Animation>();
            }
        }

        public async ValueTask<Dictionary<string, bool>> GetClickables(List<string> filterKeys = null)
        {
            var matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                var res = await _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getClickable",
                    dictArgs);
                
                return res.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value);
            }
            else
            {
                return ComputeEmptyResult<bool>();
            }
        }

        public ValueTask<Dictionary<string, string>> GetCursors(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<string>("getCursor", filterKeys);
        }

        public ValueTask<Dictionary<string, OneOf<string, Icon, Symbol>>> GetIcons(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<OneOf<string, Icon, Symbol>>("getIcon", filterKeys);
        }

        public ValueTask<Dictionary<string, string>> GetLabels(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<string>("getLabel", filterKeys);
        }

        public ValueTask<Dictionary<string, LatLngLiteral>> GetPositions(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<LatLngLiteral>("getPosition", filterKeys);
        }

        public ValueTask<Dictionary<string, MarkerShape>> GetShapes(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<MarkerShape>("getShape", filterKeys);
        }

        public ValueTask<Dictionary<string, string>> GetTitles(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<string>("getTitle", filterKeys);
        }

        public ValueTask<Dictionary<string, int>> GetZIndexes(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<int>("getZIndex", filterKeys);
        }

        /// <summary>
        /// Start an animation. 
        /// Any ongoing animation will be cancelled. 
        /// Currently supported animations are: BOUNCE, DROP. 
        /// Passing in null will cause any animation to stop.
        /// </summary>
        /// <param name="animation"></param>
        public ValueTask SetAnimations(Dictionary<string, Animation> animations)
        {
            Dictionary<Guid, object> dictArgs = animations.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setAnimation",
                dictArgs);
        }

        public ValueTask SetClickables(Dictionary<string, bool> flags)
        {
            Dictionary<Guid, object> dictArgs = flags.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setClickable",
                dictArgs);
        }

        public ValueTask SetCursors(Dictionary<string, string> cursors)
        {
            Dictionary<Guid, object> dictArgs = cursors.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setCursor",
                dictArgs);
        }

        public ValueTask SetIcons(Dictionary<string, string> icons)
        {
            Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setIcon",
                dictArgs);
        }

        public ValueTask SetIcons(Dictionary<string, Icon> icons)
        {
            Dictionary<Guid, object> dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setIcon",
                dictArgs);
        }

        public ValueTask SetLabels(Dictionary<string, Symbol> labels)
        {
            Dictionary<Guid, object> dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setLabel",
                dictArgs);
        }

        public ValueTask SetOpacities(Dictionary<string, float> opacities)
        {
            Dictionary<Guid, object> dictArgs = opacities.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setOpacity",
                dictArgs);
        }

        public ValueTask SetPositions(Dictionary<string, LatLngLiteral> latLngs)
        {
            Dictionary<Guid, object> dictArgs = latLngs.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setPosition",
                dictArgs);
        }

        public ValueTask SetShapes(Dictionary<string, MarkerShape> shapes)
        {
            Dictionary<Guid, object> dictArgs = shapes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setShape",
                dictArgs);
        }

        public ValueTask SetTitles(Dictionary<string, string> titles)
        {
            Dictionary<Guid, object> dictArgs = titles.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setTitle",
                dictArgs);
        }

        public ValueTask SetZIndexes(Dictionary<string, int> zIndexes)
        {
            Dictionary<Guid, object> dictArgs = zIndexes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setZIndex",
                dictArgs);
        }
    }
}
