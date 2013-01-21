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

        public override string ToString()
        {
            return Name;
        }
    }
}
