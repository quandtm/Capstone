using Capstone.Editor.Data.ComponentTemplates;
using System.Collections.ObjectModel;

namespace Capstone.Editor.Data
{
    public class ComponentTemplateManager
    {
        private static ComponentTemplateManager _inst;
        public static ComponentTemplateManager Instance
        {
            get
            {
                if (_inst == null)
                    _inst = new ComponentTemplateManager();
                return _inst;
            }
        }

        public ObservableCollection<ComponentTemplate> AvailableTemplates { get; private set; }

        private ComponentTemplateManager()
        {
            AvailableTemplates = new ObservableCollection<ComponentTemplate>();
            BuildTemplateList();
        }

        private void BuildTemplateList()
        {
            AvailableTemplates.Add(new TextureTemplate());
        }
    }
}
