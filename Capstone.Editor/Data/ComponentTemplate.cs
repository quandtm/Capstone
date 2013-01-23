using Capstone.Core;
using Capstone.Editor.Common;
using System;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Capstone.Editor.Data
{
    public sealed class ComponentTemplate : BindableBase
    {
        public string TemplateName { get; private set; }
        public string InstanceName { get; set; }
        private Type _type;

        public ObservableCollection<ComponentProperty> Properties { get; private set; }

        private ComponentTemplate(Type t)
        {
            _type = t;
            TemplateName = _type.Name;
            InstanceName = string.Empty;
            Properties = new ObservableCollection<ComponentProperty>();
        }

        public ComponentTemplate Clone()
        {
            var newType = new ComponentTemplate(_type);
            newType.InstanceName = InstanceName;
            newType._type = _type;
            foreach (var p in Properties)
                newType.Properties.Add(p.Clone());
            return newType;
        }

        public object CreateInstance()
        {
            var obj = Activator.CreateInstance(_type);
            ((IComponent)obj).Name = InstanceName;
            foreach (var p in Properties)
                p.ApplyProperty(obj);
            return obj;
        }

        public static ComponentTemplate Create(Type t)
        {
            var template = new ComponentTemplate(t);
            var props = t.GetRuntimeProperties();
            foreach (var p in props)
            {
                var item = ComponentProperty.Create(p);
                if (item != null)
                    template.Properties.Add(item);
            }
            return template;
        }

        internal async static Task<ComponentTemplate> Load(DataReader dr)
        {
            var aqn = await dr.ReadStringEx();
            var type = Type.GetType(aqn);
            var c = Create(type);
            c.InstanceName = await dr.ReadStringEx();

            await dr.LoadAsync(sizeof(int));
            var propCount = dr.ReadInt32();
            for (var i = 0; i < propCount; ++i)
            {
                var name = await dr.ReadStringEx();
                foreach (var p in c.Properties)
                {
                    if (p.Name.Equals(name))
                    {
                        await p.LoadData(dr);
                        break;
                    }
                }
            }

            return c;
        }

        internal void Save(DataWriter dw)
        {
            dw.WriteStringEx(_type.AssemblyQualifiedName);
            dw.WriteStringEx(InstanceName);
            dw.WriteInt32(Properties.Count);
            foreach (var c in Properties)
            {
                dw.WriteStringEx(c.Name);
                c.SaveData(dw);
            }
        }
    }
}
