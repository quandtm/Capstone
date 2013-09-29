using System;
using System.Collections.Generic;

namespace Capstone.Resources
{
    public class ResourceCache
    {
        private class ResBundle
        {
            private string _key;
            public readonly IResource Resource;
            private int _refCount;
            private bool HasReferences { get { return _refCount > 0; } }

            public ResBundle(string key, IResource res)
            {
                _key = key;
                Resource = res;
                _refCount = 0;
            }

            public void AddRef()
            {
                _refCount++;
            }

            public bool Release()
            {
                _refCount--;
                return HasReferences;
            }
        }

        private readonly Dictionary<string, ResBundle> _resources;

        /// <summary>
        /// Unload resource when it has been released by all owners. If false, resources will only be unloaded when UnloadAll is called.
        /// </summary>
        public bool AutoUnload { get; set; }

        public ResourceCache()
        {
            _resources = new Dictionary<string, ResBundle>();
            AutoUnload = true;
        }

        public T Load<T>(string path) where T : IResource, new()
        {
            ResBundle bundle;
            if (!_resources.TryGetValue(path, out bundle))
            {
                T res = new T
                {
                    ResourceKey = path
                };
                res.Load(this);
                bundle = new ResBundle(path, res);
                _resources.Add(path, bundle);
            }
            bundle.AddRef();
            return (T)bundle.Resource;
        }

        public void Release(IResource resource)
        {
            Release(resource.ResourceKey);
        }

        public void Release(string path)
        {
            ResBundle bundle;
            if (_resources.TryGetValue(path, out bundle))
            {
                if (!bundle.Release() && AutoUnload)
                {
                    // No longer has references, unload
                    bundle.Resource.Unload(this);
                    _resources.Remove(path);
                }
            }
        }

        public void UnloadAll()
        {
            foreach (var resBundle in _resources)
                resBundle.Value.Resource.Unload(this);
            _resources.Clear();
        }
    }
}
