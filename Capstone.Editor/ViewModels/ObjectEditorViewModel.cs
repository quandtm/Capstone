using System.Runtime.Serialization;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Engine.Graphics;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace Capstone.Editor.ViewModels
{
    public class ObjectEditorViewModel : BindableBase
    {
        public ObservableCollection<ComponentTemplate> Available { get; private set; }

        public ComponentTemplate SelectedComponent { get; set; }

        public ObservableCollection<EntityTemplate> Entities { get; private set; }
        public EntityTemplate SelectedEntity { get; set; }

        public ObjectEditorViewModel()
        {
            Available = ComponentTemplateManager.Instance.AvailableTemplates;
            Entities = EntityTemplateCache.Instance.Entities;

            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
                SelectedComponent = ComponentTemplate.Create(typeof(Texture));
        }

        internal void AddComponent(ComponentTemplate component)
        {
            if (SelectedEntity != null)
                SelectedEntity.AddComponent(component.Clone());
        }

        internal void RemoveComponent(ComponentTemplate component)
        {
            if (SelectedEntity != null)
                SelectedEntity.RemoveComponent(component);
        }

        internal void CreateNewEntity()
        {
            var e = new EntityTemplate();
            e.Name = "New Entity";
            Entities.Add(e);
            SelectedEntity = e;
        }

        internal Task Save()
        {
            return EntityTemplateCache.Instance.Save();
        }
    }
}
