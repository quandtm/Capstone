using System;
using System.Collections.Generic;

namespace Capstone.Core
{
    public sealed class Entity
    {
        // Private members
        private readonly List<Entity> _children;
        private Entity _parent;
        private readonly Dictionary<Type, IComponent> _components;

        // Properties
        public string Name { get; private set; }
        public readonly Transform2D Transform;
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
        public EntitySet Owner { get; private set; }

        internal Entity(string name, EntitySet owner)
        {
            _children = new List<Entity>();
            _components = new Dictionary<Type, IComponent>();
            Transform = new Transform2D(this);
            Name = name;
        }

        public T AddComponent<T>() where T : IComponent, new()
        {
            var type = typeof(T);
            IComponent c;
            if (_components.TryGetValue(type, out c))
                return (T)c;
            c = new T();
            c.Owner = this;
            _components.Add(type, c);
            return (T)c;
        }

        public void DestroyComponent<T>() where T : IComponent
        {
            IComponent c;
            var type = typeof(T);
            if (_components.TryGetValue(type, out c))
            {
                c.Destroy();
                _components.Remove(type);
                c.Owner = null;
            }
        }

        public void DestroyAllComponents()
        {
            foreach (var c in _components)
            {
                c.Value.Destroy();
                c.Value.Owner = null;
            }
            _components.Clear();
        }

        internal void UpdateChildTransforms()
        {
            for (int i = 0; i < _children.Count; i++)
                _children[i].Transform.UpdateWorld();
        }
    }
}
