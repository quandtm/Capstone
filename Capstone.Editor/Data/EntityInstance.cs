using Capstone.Core;
using Capstone.Editor.Common;
using System;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Capstone.Editor.Data
{
    public class EntityInstance : BindableBase
    {
        public Entity Entity { get; private set; }
        public string Name
        {
            get { return Entity.Name; }
            set
            {
                if (Entity.Name == value) return;
                Entity.Name = value;
            }
        }

        private EntityTemplate _template;
        public EntityTemplate Template
        {
            get { return _template; }
        }

        private EntityInstance()
        {
        }

        public static EntityInstance Create(EntityTemplate template)
        {
            var instance = new EntityInstance();
            instance.Entity = template.BuildAndSetupEntity();
            instance._template = template;
            return instance;
        }

        public void Rebuild()
        {
            Entity.DestroyComponents();
            var newEntity = _template.BuildAndSetupEntity();
            newEntity.Name = Entity.Name;
            newEntity.Translation.X = Entity.Translation.X;
            newEntity.Translation.Y = Entity.Translation.Y;
            newEntity.Depth = Entity.Depth;
            newEntity.Scale = Entity.Scale;
            newEntity.Rotation = Entity.Rotation;
            Entity = newEntity;
        }

        internal void Save(DataWriter dw)
        {
            dw.WriteStringEx(Name);
            dw.WriteSingle(Entity.Translation.X);
            dw.WriteSingle(Entity.Translation.Y);
            dw.WriteSingle(Entity.Depth);
            dw.WriteSingle(Entity.Scale);
            dw.WriteSingle(Entity.Rotation);
        }

        internal async Task Load(DataReader dr)
        {
            Name = await dr.ReadStringEx();
            await dr.LoadAsync(sizeof(float) * 5);
            Entity.Translation.X = dr.ReadSingle();
            Entity.Translation.Y = dr.ReadSingle();
            Entity.Depth = dr.ReadSingle();
            Entity.Scale = dr.ReadSingle();
            Entity.Rotation = dr.ReadSingle();
        }
    }
}
