using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Engine.Graphics;
using Capstone.Engine.Scripting;
using Capstone.Scripts;
using System.Collections.Generic;

namespace Capstone.Editor.ViewModels
{
    public class GameViewModel : BindableBase
    {
        private readonly List<Entity> _entities;

        public float Health { get; private set; }

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
            {
                _entities.Add(inst.Entity);
                InitEntity(inst.Entity);
            }

            CameraManager.Instance.MakeActive("maincamera");
        }

        private void InitEntity(Entity entity)
        {
            var playerController = entity.GetComponentFromType("Capstone.Scripts.PlayerController");
            if (playerController != null)
            {
                var pcCast = (PlayerController)playerController;
                Health = pcCast.HP;
                pcCast.HealthChanged += (s, e) => Health = pcCast.HP;
            }
        }
    }
}
