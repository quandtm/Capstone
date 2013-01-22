using System.Collections.ObjectModel;
using System.Threading.Tasks;

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

        }
    }
}
