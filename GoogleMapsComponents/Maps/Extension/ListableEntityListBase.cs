using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension;

public class ListableEntityListBase<TEntityBase, TEntityOptionsBase> : IDisposable, IAsyncDisposable
    where TEntityBase : ListableEntityBase<TEntityOptionsBase>
    where TEntityOptionsBase : IListableEntityOptionsBase
{
    // Looked up once per entity type instead of once per created entity
    private static readonly ConstructorInfo? EntityConstructor =
        typeof(TEntityBase).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault();

    protected readonly JsObjectRef _jsObjectRef;

    public readonly Dictionary<string, TEntityBase> BaseListableEntities;

    // The .NET callbacks behind the listeners added with AddListeners, by entity guid.
    // They are released together with their entity, otherwise they stay alive as long as the JS runtime.
    private readonly Dictionary<Guid, List<JsCallableAction>> _listenerCallbacks = new();
    private bool _isDisposed;

    protected ListableEntityListBase(JsObjectRef jsObjectRef, Dictionary<string, TEntityBase> baseListableEntities)
    {
        _jsObjectRef = jsObjectRef;
        BaseListableEntities = baseListableEntities;
    }

    /// <summary>
    /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
    /// </summary>
    /// <param name="opts"></param>
    /// <param name="googleMapListableEntityTypeName"></param>
    /// <returns></returns>
    public async Task SetMultipleAsync(Dictionary<string, TEntityOptionsBase> opts, string googleMapListableEntityTypeName)
    {
        var nonVisibles = new Dictionary<string, bool>();
        var lToRemove = new List<string>();
        var dictToAdd = new Dictionary<string, TEntityOptionsBase>();
        var dictToChange = new Dictionary<string, TEntityOptionsBase>();
        foreach (var sKey in BaseListableEntities.Keys)
        {
            if (!opts.ContainsKey(sKey))
            {
                lToRemove.Add(sKey);
            }
        }
        foreach (var sKey in lToRemove)
        {
            nonVisibles[sKey] = false;
        }
        foreach (var sKey in opts.Keys)
        {
            if (BaseListableEntities.ContainsKey(sKey))
            {
                dictToChange[sKey] = opts[sKey];
            }
            else
            {
                dictToAdd[sKey] = opts[sKey];
            }
        }

        var isAdvancedMarker = typeof(TEntityBase) == typeof(AdvancedMarkerElement);
        if (!isAdvancedMarker)
        {
            await SetVisibles(nonVisibles);
        }
        await RemoveMultipleAsync(lToRemove);
        await AddMultipleAsync(dictToAdd, googleMapListableEntityTypeName);

        if (!isAdvancedMarker)
        {
            await SetOptions(dictToChange);
        }
    }

    public class EntityMouseEvent
    {
        public MouseEvent MouseEvent { get; set; }
        public string Key { get; set; }
        public TEntityBase Entity { get; set; }
    }

    /// <summary>
    /// Entity clicked event containing coordinates, entity key and value.
    /// This event will be fired for entities which are being added after at least one 
    /// event handler is added to this event.
    /// Adding handlers to the event will slow down adding entities by a small amount.
    /// If no handler is added, performance is not impaired.
    /// </summary>
    public event EventHandler<EntityMouseEvent> EntityClicked;

    private void FireEvent<TEvent>(EventHandler<TEvent>? eventHandler, TEvent ea)
    {
        if (eventHandler != null)
        {
            eventHandler(this, ea);
        }
    }

    /// <summary>
    /// only keys not matching with existent listable entity keys will be created
    /// </summary>
    /// <param name="opts"></param>
    /// <param name="googleMapListableEntityTypeName"></param>
    /// <returns></returns>
    public virtual async Task AddMultipleAsync(Dictionary<string, TEntityOptionsBase> opts, string googleMapListableEntityTypeName)
    {
        if (opts.Count > 0)
        {
            Dictionary<string, JsObjectRef> jsObjectRefs = await _jsObjectRef.AddMultipleAsync(
                googleMapListableEntityTypeName,
                opts.ToDictionary(e => e.Key, e => (object)e.Value));

            Dictionary<string, TEntityBase> objs = jsObjectRefs.ToDictionary(e => e.Key, e =>
            {
                //Alternate if there are more constructors
                //var ctor = typeof(TEntityBase).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault(c => !c.GetParameters().Any());
                var ctor = EntityConstructor;
                //BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
                return (TEntityBase)ctor.Invoke(new object[] { e.Value });
                //Old version which didnt catched internal consturctors
                //return Activator.CreateInstance(typeof(TEntityBase), flags, null, e.Value) as TEntityBase;
            });

            //Someone can try to create element yet inside listable entities... really not the best approach... but manage it
            List<string> alreadyCreated = BaseListableEntities.Keys.Intersect(objs.Select(e => e.Key)).ToList();
            await RemoveMultipleAsync(alreadyCreated);

            //Now we can add all required object as NEW object
            foreach (string key in objs.Keys)
            {
                var entity = objs[key];
                BaseListableEntities.Add(key, entity);
            }
            //add event listener to the click event in one call to all added entities.
            if (this.EntityClicked != null)
            {
                await this.AddListeners<MouseEvent>(objs.Keys, "click", (mev, key) =>
                {
                    this.FireEvent(this.EntityClicked, new EntityMouseEvent { MouseEvent = mev, Key = key, Entity = BaseListableEntities[key] });
                });
            }
        }
    }

    public virtual async Task RemoveAllAsync()
    {
        await RemoveMultipleAsync(BaseListableEntities.Keys.ToList());
    }

    /// <summary>
    /// only Marker having keys matching with existent keys will be removed
    /// </summary>
    /// <param name="filterKeys"></param>
    /// <returns></returns>
    public virtual async Task RemoveMultipleAsync(List<string> filterKeys = null)
    {
        if ((filterKeys != null) && (filterKeys.Count > 0))
        {
            //One dictionary lookup per requested key, instead of comparing every entity with every key
            var foundKeys = new List<string>(filterKeys.Count);
            var foundGuids = new List<Guid>(filterKeys.Count);
            var seenKeys = new HashSet<string>();
            foreach (string key in filterKeys)
            {
                if (key != null && seenKeys.Add(key) && BaseListableEntities.TryGetValue(key, out var entity))
                {
                    foundKeys.Add(key);
                    foundGuids.Add(entity.Guid);
                }
            }

            if (foundKeys.Count > 0)
            {
                await _jsObjectRef.DisposeMultipleAsync(foundGuids);
                ReleaseListenerCallbacks(foundGuids);

                foreach (string key in foundKeys)
                {
                    //Marker object needs to dispose call due to previous DisposeMultipleAsync call
                    //Probably superfluous, but Garbage Collector may appreciate it... 
                    BaseListableEntities[key] = null;
                    BaseListableEntities.Remove(key);
                }
            }
        }
    }

    public virtual async Task RemoveMultipleAsync(List<Guid> guids)
    {
        if (guids.Count > 0)
        {
            var guidSet = new HashSet<Guid>(guids);
            var foundKeys = new List<string>();
            var foundGuids = new List<Guid>();
            foreach (var e in BaseListableEntities)
            {
                if (guidSet.Contains(e.Value.Guid))
                {
                    foundKeys.Add(e.Key);
                    foundGuids.Add(e.Value.Guid);
                }
            }

            if (foundKeys.Count > 0)
            {
                await _jsObjectRef.DisposeMultipleAsync(foundGuids);
                ReleaseListenerCallbacks(foundGuids);

                foreach (string key in foundKeys)
                {
                    //Listable entities object needs to dispose call due to previous DisposeMultipleAsync call
                    //Probably superfluous, but Garbage Collector may appreciate it... 
                    BaseListableEntities[key] = null;
                    BaseListableEntities.Remove(key);
                }
            }
        }
    }


    /// <summary>
    /// Find the eventual match between required keys (if any) and yet stored markers key (if any)
    /// If filterKeys is null or empty all keys are returned
    /// Otherwise only eventually yet stored marker keys that matches with filterKeys
    /// </summary>
    /// <param name="filterKeys"></param>
    /// <returns></returns>
    protected virtual List<string> ComputeMatchingKeys(List<string>? filterKeys = null)
    {
        List<string> matchingKeys;

        if ((filterKeys == null) || (!filterKeys.Any()))
        {
            matchingKeys = BaseListableEntities.Keys.ToList();
        }
        else
        {
            var filterSet = new HashSet<string>(filterKeys);
            matchingKeys = BaseListableEntities.Keys.Where(filterSet.Contains).ToList();
        }

        return matchingKeys;
    }

    [Obsolete("Use ComputeMatchingKeys instead. This one mistyped will be removed in future")]
    protected virtual List<string> ComputeMathingKeys(List<string>? filterKeys = null)
    {
        return ComputeMatchingKeys(filterKeys);
    }

    //Creates mapping between matching keys and markers Guid
    protected virtual Dictionary<Guid, string> ComputeInternalMapping(List<string> matchingKeys)
    {
        var keySet = new HashSet<string>(matchingKeys);
        return BaseListableEntities.Where(e => keySet.Contains(e.Key)).ToDictionary(e => e.Value.Guid, e => e.Key);
    }

    /// <summary>
    /// Creates mapping between markers Guid and empty array of parameters (getter has no parameter)
    /// </summary>
    /// <param name="matchingKeys"></param>
    /// <returns></returns>
    protected virtual Dictionary<Guid, object> ComputeDictArgs(List<string> matchingKeys)
    {
        var keySet = new HashSet<string>(matchingKeys);
        return BaseListableEntities.Where(e => keySet.Contains(e.Key)).ToDictionary(e => e.Value.Guid, _ => (object)Array.Empty<object>());
    }

    /// <summary>
    /// Create an empty result of the correct type in case of no matching keys
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    protected virtual Task<Dictionary<string, T>> ComputeEmptyResult<T>()
    {
        return Task.FromResult(new Dictionary<string, T>());
    }

    public virtual Task<Dictionary<string, Map>> GetMaps(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<Map>(
                    "getMap",
                    dictArgs)
                .ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<Map>();
        }
    }

    public virtual Task<Dictionary<string, bool>> GetDraggables(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getDraggable",
                    dictArgs)
                .ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<bool>();
        }
    }

    public virtual Task<Dictionary<string, bool>> GetVisibles(List<string>? filterKeys = null)
    {
        List<string> matchingKeys = ComputeMatchingKeys(filterKeys);

        if (matchingKeys.Any())
        {
            Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
            Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

            return _jsObjectRef.InvokeMultipleAsync<bool>(
                    "getVisible",
                    dictArgs)
                .ContinueWith(e => e.Result.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value));
        }
        else
        {
            return ComputeEmptyResult<bool>();
        }
    }

    /// <summary>
    /// Renders the listable entity on the specified map or panorama. 
    /// If map is set to null, the marker will be removed.
    /// </summary>
    /// <param name="maps"></param>
    public virtual async Task SetMaps(Dictionary<string, Map?>? maps)
    {
        maps ??= new Dictionary<string, Map?>();
        Dictionary<Guid, object?> dictArgs = maps.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object?)e.Value);
        await _jsObjectRef.InvokeMultipleAsync(
            "setMap",
            dictArgs);
    }

    public virtual Task SetDraggables(Dictionary<string, bool> draggables)
    {
        Dictionary<Guid, object> dictArgs = draggables.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setDraggable",
            dictArgs);
    }

    public virtual Task SetOptions(Dictionary<string, TEntityOptionsBase> options)
    {
        Dictionary<Guid, object> dictArgs = options.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setOptions",
            dictArgs);
    }

    public virtual Task SetVisibles(Dictionary<string, bool> visibles)
    {
        Dictionary<Guid, object> dictArgs = visibles.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
        return _jsObjectRef.InvokeMultipleAsync(
            "setVisible",
            dictArgs);
    }

    public virtual async Task AddListeners<V>(IEnumerable<string> enitityKeys, string eventName, Action<V, string> handler)
    {
        var jsRuntime = _jsObjectRef.JSRuntime;
        Dictionary<Guid, JsCallableAction> callbacks = enitityKeys.ToDictionary(
            key => BaseListableEntities[key].Guid,
            key => new JsCallableAction(jsRuntime, new Action<V>(e => handler(e, key)), typeof(V)));

        try
        {
            await _jsObjectRef.AddMultipleListenersAsync(eventName, callbacks.ToDictionary(e => e.Key, e => (object)e.Value));
        }
        catch
        {
            //No listener was added, nothing will ever call back
            foreach (var callback in callbacks.Values)
            {
                callback.Dispose();
            }

            throw;
        }

        foreach (var (guid, callback) in callbacks)
        {
            if (!_listenerCallbacks.TryGetValue(guid, out var entityCallbacks))
            {
                entityCallbacks = new List<JsCallableAction>();
                _listenerCallbacks.Add(guid, entityCallbacks);
            }

            entityCallbacks.Add(callback);
        }
    }

    private void ReleaseListenerCallbacks(IEnumerable<Guid> guids)
    {
        foreach (var guid in guids)
        {
            if (_listenerCallbacks.Remove(guid, out var entityCallbacks))
            {
                foreach (var callback in entityCallbacks)
                {
                    callback.Dispose();
                }
            }
        }
    }

    public async ValueTask DisposeAsync()
    {
        // Perform async cleanup.
        await DisposeAsyncCore();

        // Dispose of unmanaged resources.
        Dispose(false);

        // Suppress finalization.
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        if (BaseListableEntities.Count > 0)
        {
            await _jsObjectRef.DisposeMultipleAsync(BaseListableEntities.Select(e => e.Value.Guid).ToList());
            BaseListableEntities.Clear();
        }

        ReleaseListenerCallbacks(_listenerCallbacks.Keys.ToList());
    }

    protected virtual void Dispose(bool disposing)
    {

        if (!_isDisposed)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            _isDisposed = true;
        }
    }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }


}