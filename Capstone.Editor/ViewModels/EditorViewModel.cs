using Capstone.Editor.Common;
using Capstone.Editor.Data.ObjectTemplates;
using System.Collections.ObjectModel;

namespace Capstone.Editor.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        public ObservableCollection<BaseObjectTemplate> Objects { get; private set; }

        public EditorViewModel()
        {
            Objects = new ObservableCollection<BaseObjectTemplate>();
            PopulateObjectList();
        }

        private void PopulateObjectList()
        {
            Objects.Add(new PlayerObject());
        }
    }
}
