using System.Threading.Tasks;
using Capstone.Editor.Common;
using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Capstone.Editor.Data
{
    internal static class LevelSerializer
    {
        internal static async void Save(StorageFile file, string levelName, IList<EntityInstance> instances, IList<EntityTemplate> entityTemplates)
        {
            if (file == null) throw new ArgumentNullException("file");
            if (string.IsNullOrWhiteSpace(levelName)) throw new ArgumentNullException("levelName");
            if (instances == null) throw new ArgumentNullException("instances");
            if (entityTemplates == null) throw new ArgumentNullException("entityTemplates");

            var usedTemplates = FilterTemplates(entityTemplates, instances);

            using (var stream = await file.OpenTransactedWriteAsync())
            using (var dw = new DataWriter(stream.Stream))
            {
                dw.WriteInt32(1); // Version
                dw.WriteStringEx(levelName);
                dw.WriteInt32(usedTemplates.Count);
                dw.WriteInt32(instances.Count);
                foreach (var template in usedTemplates)
                    template.Save(dw);
                foreach (var instance in instances)
                    instance.Save(dw);

                await dw.StoreAsync();
                await stream.CommitAsync();
            }
        }

        private static List<EntityTemplate> FilterTemplates(IList<EntityTemplate> entityTemplates, IList<EntityInstance> instances)
        {
            var list = new List<EntityTemplate>();
            foreach (var t in entityTemplates)
            {
                foreach (var i in instances)
                {
                    if (i.Template == t)
                    {
                        list.Add(t);
                        break;
                    }
                }
            }
            return list;
        }

        internal static bool Load(StorageFile file, ref IList<EntityInstance> instances, ref IList<EntityTemplate> entityTemplates)
        {
            return false;
        }
    }
}
