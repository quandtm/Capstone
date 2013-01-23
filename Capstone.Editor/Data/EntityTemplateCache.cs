using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

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

        public bool Loaded { get; private set; }

        private EntityTemplateCache()
        {
            Entities = new ObservableCollection<EntityTemplate>();
            Loaded = false;
        }

        public async Task Load()
        {
            if (Loaded) return;
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.GetFileAsync("entitytemplates");

            using (var f = await file.OpenSequentialReadAsync())
            using (var dr = new DataReader(f))
            {
                await dr.LoadAsync(sizeof(int));
                var entityCount = dr.ReadInt32();
                for (int i = 0; i < entityCount; i++)
                {
                    var e = new EntityTemplate();
                    await e.Load(dr);
                    Entities.Add(e);
                }
            }
            Loaded = true;
        }

        internal async Task Save()
        {
            var folder = ApplicationData.Current.LocalFolder;
            var file = await folder.CreateFileAsync("entitytemplates", CreationCollisionOption.ReplaceExisting);

            using (var f = await file.OpenTransactedWriteAsync())
            using (var dw = new DataWriter(f.Stream))
            {
                dw.WriteInt32(Entities.Count);
                foreach (var e in Entities)
                    e.Save(dw);
                await dw.StoreAsync();
                await f.CommitAsync();
            }
        }
    }
}
