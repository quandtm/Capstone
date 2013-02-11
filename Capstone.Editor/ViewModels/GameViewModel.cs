using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Engine.Graphics;
using Capstone.Engine.Scripting;
using System.Collections.Generic;

namespace Capstone.Editor.ViewModels
{
    public class GameViewModel : BindableBase
    {
        private readonly List<Entity> _entities;

        public GameViewModel()
        {
            _entities = new List<Entity>();
        }

        public async void Load(Windows.Storage.StorageFile file)
        {
            ScriptManager.Instance.IsRunning = true;
            var instances = new List<EntityInstance>();
            await LevelSerializer.Load(file, instances);
            foreach (var inst in instances)
                _entities.Add(inst.Entity);
            CameraManager.Instance.MakeActive("maincamera");
        }
    }
}
