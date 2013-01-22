using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;

namespace Capstone.Editor.Data
{
    public class EntityTemplateCache
    {
        private static EntityTemplateCache _inst;
        public static EntityTemplateCache Instance
        {
            get
            {
                if (_inst == null) _inst = new EntityTemplateCache();
                return _inst;
            }
        }

        public ObservableCollection<EntityTemplate> Entities { get; private set; }

        private EntityTemplateCache()
        {
            Entities = new ObservableCollection<EntityTemplate>();
        }

        public async Task Load()
        {
            // TODO: Load Templates
        }

        internal async Task Save()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync("entitytemplates", CreationCollisionOption.ReplaceExisting);
            // TODO: Save templates
        }
    }
}
