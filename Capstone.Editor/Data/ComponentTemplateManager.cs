using Capstone.Engine.Graphics;
using Capstone.Scripts;
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
            AvailableTemplates.Add(ComponentTemplate.Create(typeof(Camera)));
            AvailableTemplates.Add(ComponentTemplate.Create(typeof(Texture)));
            AvailableTemplates.Add(ComponentTemplate.Create(typeof(PlayerController)));
            AvailableTemplates.Add(ComponentTemplate.Create(typeof (EnemyController)));
        }
    }
}
