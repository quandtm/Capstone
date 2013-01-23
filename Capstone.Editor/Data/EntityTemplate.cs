using System.Threading.Tasks;
using Capstone.Core;
using Capstone.Editor.Common;
using System;
using System.Collections.ObjectModel;
using Windows.Storage.Streams;

namespace Capstone.Editor.Data
{
    public class EntityTemplate : BindableBase
    {
        public string Name { get; set; }
        public ObservableCollection<ComponentTemplate> Components { get; private set; }

        public EntityTemplate()
        {
            Name = string.Empty;
            Components = new ObservableCollection<ComponentTemplate>();
        }

        public Entity BuildAndSetupEntity()
        {
            var e = new Entity();
            foreach (var c in Components)
            {
                var obj = (IComponent)c.CreateInstance();
                e.AddComponent(obj);
                obj.Setup();
            }
            return e;
        }

        internal void Save(DataWriter dw)
        {
            dw.WriteStringEx(Name);
            dw.WriteInt32(Components.Count);
            foreach (var c in Components)
                c.Save(dw);
        }

        internal async Task Load(DataReader dr)
        {
            Name = await dr.ReadStringEx();
            await dr.LoadAsync(sizeof(int));
            var count = dr.ReadInt32();
            for (int i = 0; i < count; i++)
            {
                var c = await ComponentTemplate.Load(dr);
                Components.Add(c);
            }
        }
    }
}
