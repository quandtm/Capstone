using Capstone.Editor.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Streams;

namespace Capstone.Editor.Data
{
    internal static class LevelSerializer
    {
        internal static async void Save(StorageFile file, string levelName, IList<EntityInstance> instances, IList<EntityTemplate> entityTemplates, ObjectiveManager objectives)
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
                dw.WriteInt32(objectives.CompletedObjectives.Count);
                dw.WriteInt32(usedTemplates.Count);
                dw.WriteInt32(instances.Count);

                foreach (var obj in objectives.CompletedObjectives)
                {
                    dw.WriteStringEx(obj.Name);
                    dw.WriteInt32(obj.Count);
                }

                foreach (var template in usedTemplates)
                    template.Save(dw);

                foreach (var instance in instances)
                {
                    dw.WriteStringEx(instance.Template.Name);
                    instance.Save(dw);
                }

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

        internal static async Task<bool> Load(StorageFile file, IList<EntityInstance> instances, IList<EntityTemplate> entityTemplates = null, ObjectiveManager objectives = null)
        {
            try
            {
                if (file == null) throw new ArgumentNullException("file");

                using (var stream = await file.OpenSequentialReadAsync())
                using (var dr = new DataReader(stream))
                {
                    await dr.LoadAsync(sizeof(Int32));
                    var fileVersion = dr.ReadInt32();
                    var levelName = await dr.ReadStringEx();

                    // Load objectives
                    await dr.LoadAsync(sizeof(Int32) * 3);
                    var numCompletedObjectives = dr.ReadInt32();
                    var numUsedTemplates = dr.ReadInt32();
                    var numInstances = dr.ReadInt32();

                    for (int i = 0; i < numCompletedObjectives; i++)
                    {
                        var name = await dr.ReadStringEx();
                        await dr.LoadAsync(sizeof(Int32));
                        var count = dr.ReadInt32();
                        if (objectives != null)
                        {
                            for (int j = 0; j < count; j++)
                                objectives.CompleteObjective(name);
                        }
                    }

                    var lut = new Dictionary<string, EntityTemplate>();
                    if (entityTemplates != null)
                    {
                        foreach (var t in entityTemplates)
                            lut.Add(t.Name, t);
                    }

                    for (int i = 0; i < numUsedTemplates; i++)
                    {
                        var template = new EntityTemplate();
                        await template.Load(dr);
                        if (!lut.ContainsKey(template.Name))
                        {
                            if (entityTemplates != null)
                                entityTemplates.Add(template);
                            lut.Add(template.Name, template);
                        }
                    }

                    for (int i = 0; i < numInstances; i++)
                    {
                        var name = await dr.ReadStringEx();
                        var instance = EntityInstance.Create(lut[name]);
                        await instance.Load(dr);
                        if (instances != null)
                            instances.Add(instance);
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
