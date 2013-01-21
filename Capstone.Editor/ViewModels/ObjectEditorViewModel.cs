using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Engine.Graphics;
using System.Collections.ObjectModel;

namespace Capstone.Editor.ViewModels
{
    public class ObjectEditorViewModel : BindableBase
    {
        public ObservableCollection<ComponentTemplate> Available { get; private set; }
        public ObservableCollection<ComponentTemplate> Added { get; private set; }

        public ComponentTemplate SelectedComponent { get; set; }

        public ObjectEditorViewModel()
        {
            Available = ComponentTemplateManager.Instance.AvailableTemplates;
            Added = new ObservableCollection<ComponentTemplate>();

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                SelectedComponent = ComponentTemplate.Create(typeof(Texture));
        }

        internal void AddComponent(ComponentTemplate component)
        {
        }

        internal void RemoveComponent(ComponentTemplate component)
        {
            Added.Remove(component);
        }
    }
}
