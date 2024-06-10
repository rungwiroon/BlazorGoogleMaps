using Microsoft.JSInterop;

using OneOf;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

/// <summary>
/// <para>
/// A class able to manage a lot of AdvancedMarkerElement objects and get / set their properties at the same time, eventually with different values
/// </para>
/// <para>
/// Main concept is that for each AdvancedMarkerElement to be distinguished from other ones, it needs
/// to have a "unique key" with an "external world meaning", so not necessarily a GUID.
/// </para>
/// <para>
/// In real cases Markers are likely to be linked to places, activities, transit stops and so on -> So, what better way to choose as AdvancedMarkerElement "unique key" the "id" of the object each marker is related to?
/// A string key has been selected as type due to its implicit versatility.
/// </para>
/// <para>
/// To create Markers, simply call <c>AdvancedMarkerList.CreateAsync</c> with a Dictionary of desired AdvancedMarkerElement keys and AdvancedMarkerElementOptions values.
/// After that, a new instance of AdvancedMarkerList class will be returned with its Markers dictionary member populated with the corresponding results
/// </para>
/// <para>
/// At run-time is possible to:<br/>
/// <list type="number">
/// <item><description>add AdvancedMarkerElement to the same AdvancedMarkerList class using <c>AddMultipleAsync</c> method (only keys not matching with existent AdvancedMarkerElement keys will be created)<br/>
/// Markers dictionary will contain "union distinct" of existent AdvancedMarkerElement's keys and new keys</description>
/// </item>
/// <item><description>remove AdvancedMarkerElement from the AdvancedMarkerList class (only AdvancedMarkerElement having keys matching with existent keys will be removed)<br/>
/// Markers dictionary will contain "original - required and found" AdvancedMarkerElement's keys (eventually any is all AdvancedMarkerElement are removed)</description>
/// </item>
/// </list>
/// </para>
/// <para>
/// Each definer getter properties can be used as follows:<br/>
/// a) without parameter -> all eventually defined markers related property will be returned (if any)<br/>
/// b) with a <c>List&lt;string&gt;</c> of keys -> all eventually matching keys with Markers Dictionary keys produces related markers property extraction (if any defined)
/// </para>
/// <para>
/// Each setter properties can be used as follows:<br/>
/// With a <c>Dictionary&lt;string, {property type}&gt;</c> indicating for each AdvancedMarkerElement (related to that key) the corresponding related property value.
/// </para>
/// </summary>
public class AdvancedMarkerElementList : ListableEntityListBase<AdvancedMarkerElement, AdvancedMarkerElementOptions>
{
    public Dictionary<string, AdvancedMarkerElement> Markers => BaseListableEntities;

    /// <summary>
    /// Create markers list
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="opts">Dictionary of desired AdvancedMarkerElement keys and AdvancedMarkerElementOptions values. Key as any type unique key. Not necessarily Guid</param>
    /// <returns>New instance of AdvancedMarkerList class will be returned with its Markers dictionary member populated with the corresponding results</returns>
    public static async Task<AdvancedMarkerElementList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, AdvancedMarkerElementOptions> opts)
    {
        var jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());

        Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
            jsRuntime,
            AdvancedMarkerElement.GoogleMapAdvancedMarkerName,
            opts.ToDictionary(e => e.Key, e => (object)e.Value));

        Dictionary<string, AdvancedMarkerElement> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new AdvancedMarkerElement(e.Value));
        var obj = new AdvancedMarkerElementList(jsObjectRef, objs);

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
    /// <param name="clickCallback"></param>
    /// <returns>
    /// The managed list. Assign to the variable you used as parameter.
    /// </returns>
    public static async Task<AdvancedMarkerElementList> SyncAsync(AdvancedMarkerElementList? list,
        IJSRuntime jsRuntime,
        Dictionary<string, AdvancedMarkerElementOptions> opts,
        Action<MouseEvent, string, AdvancedMarkerElement>? clickCallback = null)
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
                list = await AdvancedMarkerElementList.CreateAsync(jsRuntime, new Dictionary<string, AdvancedMarkerElementOptions>());
                if (clickCallback != null)
                {
                    list.EntityClicked += (_, e) =>
                    {
                        clickCallback(e.MouseEvent, e.Key, e.Entity);
                    };
                }
            }
            await list.SetMultipleAsync(opts);
        }
        return list;
    }

    private AdvancedMarkerElementList(JsObjectRef jsObjectRef, Dictionary<string, AdvancedMarkerElement> markers)
        : base(jsObjectRef, markers)
    {
    }

    /// <summary>
    /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task SetMultipleAsync(Dictionary<string, AdvancedMarkerElementOptions> opts)
    {
        await base.SetMultipleAsync(opts, AdvancedMarkerElement.GoogleMapAdvancedMarkerName);
    }

    /// <summary>
    /// Only keys not matching with existent AdvancedMarkerElement keys will be created
    /// </summary>
    /// <returns></returns>
    public async Task AddMultipleAsync(Dictionary<string, AdvancedMarkerElementOptions> opts)
    {
        await base.AddMultipleAsync(opts, AdvancedMarkerElement.GoogleMapAdvancedMarkerName);
    }

    public Task<Dictionary<string, Animation>> GetAnimations(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, bool>> GetClickables(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, string>> GetCursors(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, OneOf<string, Icon, Symbol>>> GetIcons(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, string>> GetLabels(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, LatLngLiteral>> GetPositions(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, MarkerShape>> GetShapes(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, string>> GetTitles(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, int>> GetZIndexes(List<string>? filterKeys = null)
    {
        var matchingKeys = ComputeMatchingKeys(filterKeys);

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
    /// <param name="animations"></param>
    public Task SetAnimations(Dictionary<string, Animation?> animations)
    {
        var dictArgs = animations.ToDictionary(e => Markers[e.Key].Guid, e => (object?)GetAnimationCode(e.Value));
        return _jsObjectRef.InvokeMultipleAsync("setAnimation", dictArgs);
    }

    public int? GetAnimationCode(Animation? animation)
    {
        switch (animation)
        {
            case null: return null;
            case Animation.Bounce: return 1;
            case Animation.Drop: return 2;
            default: return 0;
        }
    }

    /// <summary>
    /// Sets the Clickable flag of one or more Markers to match a dictionary of marker keys and flag values.
    /// </summary>
    /// <param name="flags"></param>
    /// <returns></returns>
    public Task SetClickables(Dictionary<string, bool> flags)
    {
        var dictArgs = flags.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setClickable", dictArgs);
    }

    public Task SetCursors(Dictionary<string, string> cursors)
    {
        var dictArgs = cursors.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setCursor", dictArgs);
    }

    /// <summary>
    /// Set Icon on each AdvancedMarkerElement matching a param dictionary key to the param value with single JSInterop call.
    /// </summary>
    /// <param name="icons"></param>
    /// <returns></returns>
    public Task SetIcons(Dictionary<string, OneOf<string, Icon, Symbol>> icons)
    {
        var dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setIcon",
            dictArgs);
    }

    /// <inheritdoc cref="SetIcons(Dictionary{string, OneOf{string, Icon, Symbol}})"/>
    public Task SetIcons(Dictionary<string, string> icons)
    {
        var dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setIcon", dictArgs);
    }

    /// <inheritdoc cref="SetIcons(Dictionary{string, OneOf{string, Icon, Symbol}})"/>
    public Task SetIcons(Dictionary<string, Icon> icons)
    {
        var dictArgs = icons.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setIcon", dictArgs);
    }

    /// <summary>
    /// Set Label on each AdvancedMarkerElement matching a param dictionary key to the param value with single JSInterop call.
    /// </summary>
    /// <param name="labels"></param>
    /// <returns></returns>
    public Task SetLabels(Dictionary<string, OneOf<string, MarkerLabel>> labels)
    {
        var dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setLabel",
            dictArgs);
    }

    /// <inheritdoc cref="SetLabels(Dictionary{string, OneOf{string, MarkerLabel}})"/>
    public Task SetLabels(Dictionary<string, string> labels)
    {
        var dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setLabel",
            dictArgs);
    }

    /// <inheritdoc cref="SetLabels(Dictionary{string, OneOf{string, MarkerLabel}})"/>
    public Task SetLabels(Dictionary<string, MarkerLabel> labels)
    {
        var dictArgs = labels.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setLabel",
            dictArgs);
    }

    public Task SetOpacities(Dictionary<string, float> opacities)
    {
        var dictArgs = opacities.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setOpacity",
            dictArgs);
    }

    public Task SetPositions(Dictionary<string, LatLngLiteral> latLngs)
    {
        var dictArgs = latLngs.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setPosition", dictArgs);
    }

    public Task SetShapes(Dictionary<string, MarkerShape> shapes)
    {
        var dictArgs = shapes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setShape", dictArgs);
    }

    public Task SetTitles(Dictionary<string, string> titles)
    {
        var dictArgs = titles.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setTitle", dictArgs);
    }

    public Task SetZIndexes(Dictionary<string, int> zIndexes)
    {
        var dictArgs = zIndexes.ToDictionary(e => Markers[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync("setZIndex", dictArgs);
    }
}