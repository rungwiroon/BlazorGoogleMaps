using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

/// <summary>
/// <para>A class able to manage a lot of Polygon objects and get / set their
/// properties at the same time, eventually with different values.<br/>
/// Main concept is that for each Polygon to be distinguished from other ones, it needs
/// to have a "unique key" with an "external world meaning", so not necessarily a GUID</para>
/// <para>All properties should be called With a <c>Dictionary&lt;string, {property type}&gt;</c> indicating for each Polygon (related to that key) the corresponding related property value</para>
/// </summary>
public class PolygonList : ListableEntityListBase<Polygon, PolygonOptions>
{
    public Dictionary<string, Polygon> Polygons => base.BaseListableEntities;

    /// <summary>
    /// Create polygons list
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="opts">Dictionary of desired Polygon keys and PolygonOptions values. Key as any type unique key. Not necessarily Guid</param>
    /// <returns>New instance of PolygonList class will be returned with its Polygons dictionary member populated with the corresponding results</returns>
    public static async Task<PolygonList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, PolygonOptions> opts)
    {
        JsObjectRef jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());

        PolygonList obj;
        Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
            jsRuntime,
            "google.maps.Polygon",
            opts.ToDictionary(e => e.Key, e => (object)e.Value));
        Dictionary<string, Polygon> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Polygon(e.Value));
        obj = new PolygonList(jsObjectRef, objs);

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
    public static async Task<PolygonList> SyncAsync(PolygonList? list, IJSRuntime jsRuntime, Dictionary<string, PolygonOptions> opts, Action<MouseEvent, string, Polygon>? clickCallback = null)
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
                list = await PolygonList.CreateAsync(jsRuntime, new Dictionary<string, PolygonOptions>());
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

    private PolygonList(JsObjectRef jsObjectRef, Dictionary<string, Polygon> polygons)
        : base(jsObjectRef, polygons)
    {
    }

    /// <summary>
    /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task SetMultipleAsync(Dictionary<string, PolygonOptions> opts)
    {
        await base.SetMultipleAsync(opts, "google.maps.Polygon");
    }

    /// <summary>
    /// Only keys not matching with existent Polygon keys will be created
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task AddMultipleAsync(Dictionary<string, PolygonOptions> opts)
    {
        await base.AddMultipleAsync(opts, "google.maps.Polygon");
    }

    public Task<Dictionary<string, bool>> GetEditables(List<string> filterKeys = null)
    {
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task SetEditables(Dictionary<string, bool> editables)
    {
        Dictionary<Guid, object> dictArgs = editables.ToDictionary(e => Polygons[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setEditable",
            dictArgs);
    }
}