using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GoogleMapsComponents.Maps.Extension
{
    public class ListableEntityListBase<TEntityBase, TEntityOptionsBase> : IDisposable
        where TEntityBase : ListableEntityBase<TEntityOptionsBase>
        where TEntityOptionsBase : ListableEntityOptionsBase
    {
        protected readonly JsObjectRef _jsObjectRef;

        public readonly Dictionary<string, TEntityBase> BaseListableEntities;

        protected ListableEntityListBase(JsObjectRef jsObjectRef, Dictionary<string, TEntityBase> baseListableEntities)
        {
            _jsObjectRef = jsObjectRef;
            BaseListableEntities = baseListableEntities;
        }

        public void Dispose()
        {
            if (BaseListableEntities.Count > 0)
            {
                _jsObjectRef.DisposeMultipleAsync(BaseListableEntities.Select(e => e.Value.Guid).ToList());
                BaseListableEntities.Clear();
            }
        }

        /// <summary>
        /// Set the set of entities; entities will be removed, added or changed to mirror the given set.
        /// </summary>
        /// <param name="opts"></param>
        /// <returns></returns>
        public async ValueTask SetMultipleAsync(Dictionary<string, TEntityOptionsBase> opts, string googleMapListableEntityTypeName)
        {
            var nonVisibles = new Dictionary<string, bool>();
            var lToRemove = new List<string>();
            var dictToAdd = new Dictionary<string, TEntityOptionsBase>();
            var dictToChange = new Dictionary<string, TEntityOptionsBase>();
            foreach (var sKey in this.BaseListableEntities.Keys)
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
                if (this.BaseListableEntities.ContainsKey(sKey))
                {
                    dictToChange[sKey] = opts[sKey];
                }
                else
                {
                    dictToAdd[sKey] = opts[sKey];
                }
            }
            await this.SetVisibles(nonVisibles);
            await this.RemoveMultipleAsync(lToRemove);
            await this.AddMultipleAsync(dictToAdd, googleMapListableEntityTypeName);
            await this.SetOptions(dictToChange);
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

        private void FireEvent<TEvent>(EventHandler<TEvent> eventHandler, TEvent ea)
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
                    var ctor = typeof(TEntityBase).GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic).FirstOrDefault();
                    BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance;
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

                List<string> foundKeys = BaseListableEntities.Keys.Intersect(filterKeys).ToList();
                if (foundKeys.Count > 0)
                {
                    List<Guid> foundGuids = BaseListableEntities.Where(e => foundKeys.Contains(e.Key)).Select(e => e.Value.Guid).ToList();
                    await _jsObjectRef.DisposeMultipleAsync(foundGuids);

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
                List<string> foundKeys = BaseListableEntities.Where(e => guids.Contains(e.Value.Guid)).Select(e => e.Key).ToList();
                if (foundKeys.Count > 0)
                {
                    List<Guid> foundGuids = BaseListableEntities.Values.Where(e => guids.Contains(e.Guid)).Select(e => e.Guid).ToList();
                    await _jsObjectRef.DisposeMultipleAsync(foundGuids);

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

        public async ValueTask<Dictionary<string, T>> InvokeMultipleAsync<T>(
            string functionName,
            IEnumerable<string> filterKeys)
        {
            var matchingKeys = ComputeMathingKeys(filterKeys);

            if (matchingKeys.Any())
            {
                Dictionary<Guid, string> internalMapping = ComputeInternalMapping(matchingKeys);
                Dictionary<Guid, object> dictArgs = ComputeDictArgs(matchingKeys);

                var res = await _jsObjectRef.InvokeMultipleAsync<T>(
                    functionName,
                    dictArgs);

                return res.ToDictionary(r => internalMapping[new Guid(r.Key)], r => r.Value);
            }
            else
            {
                return ComputeEmptyResult<T>();
            }
        }

        //Find the eventual match between required keys (if any) and yet stored markers key (if any)
        //If filterKeys is null or empty all keys are returned
        //Otherwise only eventually yet stored marker keys that matches with filterKeys
        protected virtual IEnumerable<string> ComputeMathingKeys(IEnumerable<string> filterKeys)
        {
            if ((filterKeys == null) || (!filterKeys.Any()))
            {
                return BaseListableEntities.Keys.AsEnumerable();
            }
            else
            {
                return BaseListableEntities.Keys.Where(e => filterKeys.Contains(e));
            }
        }

        //Creates mapping between matching keys and markers Guid
        protected virtual Dictionary<Guid, string> ComputeInternalMapping(IEnumerable<string> matchingKeys)
        {
            return BaseListableEntities.Where(e => matchingKeys.Contains(e.Key)).ToDictionary(e => BaseListableEntities[e.Key].Guid, e => e.Key);
        }

        //Creates mapping between markers Guid and empty array of parameters (getter has no parameter)
        protected virtual Dictionary<Guid, object> ComputeDictArgs(IEnumerable<string> matchingKeys)
        {
            return BaseListableEntities.Where(e => matchingKeys.Contains(e.Key)).ToDictionary(e => e.Value.Guid, e => (object)(Array.Empty<object>()));
        }

        //Create an empty result of the correct type in case of no matching keys
        protected virtual Dictionary<string, T> ComputeEmptyResult<T>()
        {
            return new Dictionary<string, T>();
        }

        public virtual ValueTask<Dictionary<string, Map>> GetMaps(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<Map>("getMap", filterKeys);
        }

        public virtual ValueTask<Dictionary<string, bool>> GetDraggables(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<bool>("getDraggable", filterKeys);
        }

        public virtual ValueTask<Dictionary<string, bool>> GetVisibles(IEnumerable<string> filterKeys)
        {
            return InvokeMultipleAsync<bool>("getVisible", filterKeys);
        }

        /// <summary>
        /// Renders the listable entity on the specified map or panorama. 
        /// If map is set to null, the marker will be removed.
        /// </summary>
        /// <param name="map"></param>
        public virtual async Task SetMaps(Dictionary<string, Map> maps)
        {
            Dictionary<Guid, object> dictArgs = maps.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
            await _jsObjectRef.InvokeMultipleAsync(
                "setMap",
                dictArgs);
        }

        public virtual ValueTask SetDraggables(Dictionary<string, bool> draggables)
        {
            Dictionary<Guid, object> dictArgs = draggables.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setDraggable",
                dictArgs);
        }

        public virtual ValueTask SetOptions(Dictionary<string, TEntityOptionsBase> options)
        {
            Dictionary<Guid, object> dictArgs = options.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setOptions",
                dictArgs);
        }

        public virtual ValueTask SetVisibles(Dictionary<string, bool> visibles)
        {
            Dictionary<Guid, object> dictArgs = visibles.ToDictionary(e => BaseListableEntities[e.Key].Guid, e => (object)e.Value);
            return _jsObjectRef.InvokeMultipleAsync(
                "setVisible",
                dictArgs);
        }

        public virtual async ValueTask AddListeners<V>(IEnumerable<string> enitityKeys, string eventName, Action<V, string> handler)
        {
            Dictionary<Guid, object> dictArgs = enitityKeys.ToDictionary(key => BaseListableEntities[key].Guid, key => (object)new Action<V>((e) =>
            {
                handler(e, key);
            }));
            await _jsObjectRef.AddMultipleListenersAsync(eventName, dictArgs);
        }
    }
}