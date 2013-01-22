using Capstone.Core;
using Capstone.Editor.Common;
using System.Collections.ObjectModel;

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
    }
}
