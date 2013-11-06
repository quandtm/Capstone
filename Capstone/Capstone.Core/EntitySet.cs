using System.Collections.Generic;

namespace Capstone.Core
{
    public class EntitySet
    {
        private readonly List<Entity> _entities;
        private readonly Dictionary<string, Entity> _lookup;
        private readonly List<Entity> _toDestroy;

        public EntitySet()
        {
            _entities = new List<Entity>();
            _lookup = new Dictionary<string, Entity>();
            _toDestroy = new List<Entity>();
        }

        public Entity Create(string name = null, Entity parent = null)
        {
            var e = new Entity(name, this);
            if (parent != null)
                e.Parent = parent;
            _entities.Add(e);
            if (!string.IsNullOrWhiteSpace(name))
                _lookup.Add(name, e);
            return e;
        }

        public Entity Find(string name)
        {
            Entity e;
            if (_lookup.TryGetValue(name, out e))
                return e;
            return null;
        }

        public void Destroy(Entity e)
        {
            _toDestroy.Remove(e);
            e.DestroyChildren();
        }

        public void Destroy(string name)
        {
            Entity e;
            if (_lookup.TryGetValue(name, out e))
                Destroy(e);
        }

        public void DestroyAllEntities()
        {
            _entities.Clear();
            _lookup.Clear();
            _toDestroy.Clear();
        }

        // Should be called each update
        public void CollectGarbage()
        {
            for (int i = 0; i < _toDestroy.Count; i++)
            {
                var e = _toDestroy[i];
                _entities.Remove(e);
                if (!string.IsNullOrWhiteSpace(e.Name))
                    _lookup.Remove(e.Name);
            }
            _toDestroy.Clear();
        }
    }
}
