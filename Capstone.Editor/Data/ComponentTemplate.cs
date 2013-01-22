using Capstone.Core;
using Capstone.Editor.Common;
using System;
using System.Collections.ObjectModel;
using System.Reflection;

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
            ((IComponent) obj).Name = InstanceName;
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
    }
}
