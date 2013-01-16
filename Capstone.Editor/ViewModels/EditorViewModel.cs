using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Editor.Data.ObjectTemplates;
using Capstone.Engine.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Capstone.Editor.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        private Entity _cam;

        public ObservableCollection<BaseObjectTemplate> Objects { get; private set; }
        public BaseObjectTemplate SelectedObject { get; set; }

        public int RemainingSprites { get; private set; }

        private readonly List<Entity> _entities;

        public ObservableCollection<Objective> Objectives { get; private set; }
        private readonly Dictionary<string, Objective> _objectiveLookup;

        public EditorViewModel()
        {
            Objectives = new ObservableCollection<Objective>();
            _entities = new List<Entity>();
            Objects = new ObservableCollection<BaseObjectTemplate>();
            _objectiveLookup = new Dictionary<string, Objective>();

            RemainingSprites = 500;

            SetupCamera();
            RegisterObjectives();
        }

        private void RegisterObjectives()
        {
            AddObjective("addplayer", "Add Player");
        }

        public void AddObjective(string name, string description)
        {
            if (!string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(description) && !_objectiveLookup.ContainsKey(name))
            {
                var obj = new Objective(description);
                _objectiveLookup.Add(name, obj);
                Objectives.Add(obj);
            }
        }

        public void CompleteObjective(string name)
        {
            Objective obj;
            if (_objectiveLookup.TryGetValue(name, out obj))
                obj.CompleteItem();
        }

        private void SetupCamera()
        {
            _cam = new Entity();
            var c = new Camera();
            CameraManager.Instance.AddCamera("camera", c);
            CameraManager.Instance.MakeActive("camera");
            _cam.AddComponent("camera", c);
        }

        public void PopulateObjectList()
        {
            Objects.Add(new PlayerObject());
        }

        internal void HandleClick(Windows.Foundation.Point point)
        {
            if (RemainingSprites <= 0) return;
            if (SelectedObject == null) return;

            var e = SelectedObject.CreateEntityInstance();
            if (e == null) return;
            var screenPt = new Vector2((float)point.X, (float)point.Y);
            CameraManager.Instance.ActiveCamera.ScreenToWorld(screenPt, e.Translation);
            _entities.Add(e);
            RemainingSprites = RemainingSprites - 1;

            if (SelectedObject is PlayerObject)
                CompleteObjective("addplayer");
        }
    }
}
