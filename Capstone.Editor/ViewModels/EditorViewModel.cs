using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Editor.Data.ObjectTemplates;
using Capstone.Editor.Scripts;
using Capstone.Engine.Graphics;
using Capstone.Engine.Scripting;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Windows.Foundation;

namespace Capstone.Editor.ViewModels
{
    public enum EditorTool
    {
        Select,
        Pan,
        Build
    }

    public class EditorViewModel : BindableBase
    {
        private Point _prevPoint;
        private Entity _cam;

        public ObservableCollection<BaseObjectTemplate> Objects { get; private set; }
        public BaseObjectTemplate SelectedObject { get; set; }

        public int RemainingSprites { get; private set; }

        private readonly List<Entity> _entities;

        public ObservableCollection<Objective> Objectives { get; private set; }
        private readonly Dictionary<string, Objective> _objectiveLookup;

        public EditorTool Tool { get; set; }

        public EditorViewModel()
        {
            Objectives = new ObservableCollection<Objective>();
            _entities = new List<Entity>();
            Objects = new ObservableCollection<BaseObjectTemplate>();
            _objectiveLookup = new Dictionary<string, Objective>();

            RemainingSprites = 500;

            SetupCamera();
            RegisterObjectives();

            Tool = EditorTool.Select;
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
            var cscript = new EditorCameraScript(this);
            ScriptManager.Instance.RegisterScript(cscript);
            _cam.AddComponent("controlscript", cscript);
        }

        public void PopulateObjectList()
        {
            Objects.Add(new PlayerObject());
        }

        internal void HandleClick(Windows.Foundation.Point point)
        {
            switch (Tool)
            {
                case EditorTool.Build:
                    BuildSprite(point);
                    break;
            }
        }

        private void BuildSprite(Point point)
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
