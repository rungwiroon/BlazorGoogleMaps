using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

/// <summary>
/// <para>A class able to manage a lot of Polyline objects and get / set their
/// properties at the same time, eventually with different values.<br/>
/// Main concept is that for each Polyline to be distinguished from other ones, it needs
/// to have a "unique key" with an "external world meaning", so not necessarily a GUID</para>
/// <para>All properties should be called With a <c>Dictionary&lt;string, {property type}&gt;</c> indicating for each Polyline (related to that key) the corresponding related property value</para>
/// </summary>
public class PolylineList : ListableEntityListBase<Polyline, PolylineOptions>
{
    public Dictionary<string, Polyline> Polylines => base.BaseListableEntities;

    /// <summary>
    /// Create polylines list
    /// </summary>
    /// <param name="jsRuntime"></param>
    /// <param name="opts">Dictionary of desired Polyline keys and PolylineOptions values. Key as any type unique key. Not necessarily Guid</param>
    /// <returns>New instance of PolylineList class will be returned with its Polylines dictionary member populated with the corresponding results</returns>
    public static async Task<PolylineList> CreateAsync(IJSRuntime jsRuntime, Dictionary<string, PolylineOptions> opts)
    {
        JsObjectRef jsObjectRef = new JsObjectRef(jsRuntime, Guid.NewGuid());

        PolylineList obj;
        Dictionary<string, JsObjectRef> jsObjectRefs = await JsObjectRef.CreateMultipleAsync(
            jsRuntime,
            "google.maps.Polyline",
            opts.ToDictionary(e => e.Key, e => (object)e.Value));
        Dictionary<string, Polyline> objs = jsObjectRefs.ToDictionary(e => e.Key, e => new Polyline(e.Value));
        obj = new PolylineList(jsObjectRef, objs);

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
    public static async Task<PolylineList> SyncAsync(PolylineList list, IJSRuntime jsRuntime, Dictionary<string, PolylineOptions> opts, Action<MouseEvent, string, Polyline> clickCallback = null)
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
                list = await PolylineList.CreateAsync(jsRuntime, new Dictionary<string, PolylineOptions>());
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

    private PolylineList(JsObjectRef jsObjectRef, Dictionary<string, Polyline> polylines)
        : base(jsObjectRef, polylines)
    {
    }

    /// <summary>
    /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task SetMultipleAsync(Dictionary<string, PolylineOptions> opts)
    {
        await base.SetMultipleAsync(opts, "google.maps.Polyline");
    }

    /// <summary>
    /// Only keys not matching with existent Polyline keys will be created
    /// </summary>
    /// <param name="opts"></param>
    /// <returns></returns>
    public async Task AddMultipleAsync(Dictionary<string, PolylineOptions> opts)
    {
        await base.AddMultipleAsync(opts, "google.maps.Polyline");
    }

    public Task<Dictionary<string, LatLngBoundsLiteral>> GetBounds(List<string> filterKeys = null)
    {
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

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
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task<Dictionary<string, double>> GetRadiuses(List<string> filterKeys = null)
    {
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

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

    public Task SetCenters(Dictionary<string, LatLngLiteral> centers)
    {
        Dictionary<Guid, object> dictArgs = centers.ToDictionary(e => Polylines[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setCenter",
            dictArgs);
    }

    public Task SetEditables(Dictionary<string, bool> editables)
    {
        Dictionary<Guid, object> dictArgs = editables.ToDictionary(e => Polylines[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setEditable",
            dictArgs);
    }

    public Task SetRadiuses(Dictionary<string, double> radiuses)
    {
        Dictionary<Guid, object> dictArgs = radiuses.ToDictionary(e => Polylines[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setRadius",
            dictArgs);
    }
}