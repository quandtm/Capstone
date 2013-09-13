using System.Collections.Generic;

namespace Capstone.Core
{
    public sealed class Entity
    {
        // Static members
        private static readonly List<Entity> _entities;
        private static readonly Dictionary<string, Entity> _lookup;
        private static readonly List<Entity> _toDestroy;

        // Private members
        private readonly List<Entity> _children;
        private Entity _parent;

        // Properties
        public string Name { get; private set; }

        public Entity Parent
        {
            get { return _parent; }
            set
            {
                if (_parent == value) return;
                if (_parent != null)
                    _parent._children.Remove(this);
                _parent = value;
                _parent._children.Add(this);
            }
        }


        static Entity()
        {
            _entities = new List<Entity>();
            _lookup = new Dictionary<string, Entity>();
            _toDestroy = new List<Entity>();
        }

        private Entity()
        {
            _children = new List<Entity>();
        }

        public static Entity Create(string name = null, Entity parent = null)
        {
            var e = new Entity();
            if (parent != null)
                e.Parent = parent;
            _entities.Add(e);
            if (!string.IsNullOrWhiteSpace(name))
            {
                e.Name = name;
                _lookup.Add(name, e);
            }
            return e;
        }

        public static void Destroy(Entity e)
        {
            _toDestroy.Remove(e);
        }

        public static void Destroy(string name)
        {
            Entity e;
            if (_lookup.TryGetValue(name, out e))
                Destroy(e);
        }

        public static void DestroyAllEntities()
        {
            _entities.Clear();
            _lookup.Clear();
            _toDestroy.Clear();
        }

        public static void CollectGarbage()
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
