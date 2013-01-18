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
        private BaseObjectTemplate _selectedObj;
        public BaseObjectTemplate SelectedObject
        {
            get { return _selectedObj; }
            set
            {
                if (_selectedObj == value) return;
                _selectedObj = value;
                if (_selectedObj != null)
                    Tool = EditorTool.Build;
            }
        }

        public int Money { get; private set; }

        private readonly List<Entity> _entities;

        public ObjectiveManager ObjectiveManager { get; private set; }
        private readonly Dictionary<string, Objective> _objectiveLookup;

        private EditorTool _tool;
        public EditorTool Tool
        {
            get { return _tool; }
            set
            {
                if (_tool == value) return;
                _tool = value;
                if (_tool != EditorTool.Build)
                    SelectedObject = null;
            }
        }
        public bool IsPanTool
        {
            get { return Tool == EditorTool.Pan; }
        }
        public bool IsSelectTool
        {
            get { return Tool == EditorTool.Select; }
        }
        public bool EventEditorVisible { get; set; }

        public EditorViewModel()
        {
            ObjectiveManager = new ObjectiveManager();
            _entities = new List<Entity>();
            Objects = new ObservableCollection<BaseObjectTemplate>();

            Money = 500;

            SetupCamera();
            RegisterObjectives();

            Tool = EditorTool.Select;
            EventEditorVisible = false;
        }

        private void RegisterObjectives()
        {
            ObjectiveManager.AddObjective("AddPlayer", "Add a Player Object", 10);
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
            if (Money <= 0) return;
            if (SelectedObject == null) return;

            var e = SelectedObject.CreateEntityInstance();
            if (e == null) return;
            var screenPt = new Vector2((float)point.X, (float)point.Y);
            CameraManager.Instance.ActiveCamera.ScreenToWorld(screenPt, e.Translation);
            _entities.Add(e);
            Money = Money - SelectedObject.Cost;

            if (SelectedObject is PlayerObject)
                ObjectiveManager.CompleteObjective("AddPlayer");
        }
    }
}
