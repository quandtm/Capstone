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

        public ObservableCollection<ComponentProperty> Properties { get; private set; }

        private ComponentTemplate(string name)
        {
            TemplateName = name;
            InstanceName = string.Empty;
            Properties = new ObservableCollection<ComponentProperty>();
        }

        public ComponentTemplate Clone()
        {
            var newType = new ComponentTemplate(TemplateName);
            newType.InstanceName = InstanceName;
            foreach (var p in Properties)
                newType.Properties.Add(p.Clone());
            return newType;
        }

        public static ComponentTemplate Create(Type t)
        {
            var template = new ComponentTemplate(t.Name);
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
