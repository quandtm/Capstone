using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
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
        private int _entityCounter;
        private Point _prevPoint;
        private Entity _cam;
        private bool _pointerDown;

        public int Money { get; private set; }

        public ObservableCollection<EntityInstance> Instances { get; private set; }
        public EntityInstance SelectedInstance { get; set; }

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
                    SelectedTemplate = null;
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

        public ObservableCollection<EntityTemplate> EntityTemplates { get; private set; }
        private EntityTemplate _selectedTemplate;
        public EntityTemplate SelectedTemplate
        {
            get { return _selectedTemplate; }
            set
            {
                if (_selectedTemplate == value) return;
                _selectedTemplate = value;
                if (_selectedTemplate != null)
                    Tool = EditorTool.Build;
            }
        }

        public EditorViewModel()
        {
            Instances = new ObservableCollection<EntityInstance>();
            EntityTemplates = EntityTemplateCache.Instance.Entities;
            ObjectiveManager = new ObjectiveManager();

            _entityCounter = 0;
            Money = 500;

            SetupCamera();
            RegisterObjectives();

            Tool = EditorTool.Select;
            EventEditorVisible = false;

            ScriptManager.Instance.IsRunning = false;

            _pointerDown = false;
        }

        private void RegisterObjectives()
        {
            ObjectiveManager.AddObjective("AddPlayer", "Add an Entity with a PlayerController", 10);
        }

        private void SetupCamera()
        {
            _cam = new Entity();

            var c = new Camera()
                {
                    Name = "camera"
                };
            c.Install();
            CameraManager.Instance.MakeActive("camera");
            _cam.AddComponent(c);
        }

        public void PopulateObjectList()
        {
            EntityTemplateCache.Instance.Load();
        }

        internal void HandleReleased(Point point)
        {
            switch (Tool)
            {
                case EditorTool.Build:
                    if (_selectedTemplate != null)
                    {
                        var instance = EntityInstance.Create(_selectedTemplate);
                        Vector2 screen = new Vector2((float)point.X, (float)point.Y);
                        ((Camera)_cam.GetComponent("camera")).ScreenToWorld(screen, instance.Entity.Translation);
                        instance.Entity.Name = string.Format("entity_{0:000}", ++_entityCounter);
                        Instances.Add(instance);
                        ProcessBuildObjectives(_selectedTemplate);
                        ProcessCost(_selectedTemplate);
                    }
                    break;
            }
            _pointerDown = false;
        }

        private void ProcessCost(EntityTemplate _selectedTemplate)
        {
            Money = Money - _selectedTemplate.Cost;
        }

        internal void HandlePointerMove(Point point)
        {
            switch (Tool)
            {
                case EditorTool.Pan:
                    if (_pointerDown)
                    {
                        var dx = point.X - _prevPoint.X;
                        var dy = point.Y - _prevPoint.Y;
                        _cam.Translation.X -= (float)dx;
                        _cam.Translation.Y -= (float)dy;

                        _prevPoint = point;
                    }
                    break;
            }
        }

        internal void HandlePointerPressed(Point point)
        {
            switch (Tool)
            {
                case EditorTool.Pan:
                    _prevPoint = point;
                    break;
            }
            _pointerDown = true;
        }

        private void ProcessBuildObjectives(EntityTemplate template)
        {
            foreach (var c in template.Components)
            {
                var cTypeName = c.TemplateName;
                switch (cTypeName)
                {
                    case "PlayerController":
                        ObjectiveManager.CompleteObjective("AddPlayer");
                        break;
                }
            }
        }

        public void DeleteSelectedEntity()
        {
            var inst = SelectedInstance;
            inst.Entity.DestroyComponents();
            Instances.Remove(inst);
            SelectedInstance = null;
        }

        public void ZoomDelta(float p)
        {
            _cam.Depth += p / 1000f;
        }

        internal void ResetCamera()
        {
            _cam.Translation.X = 0;
            _cam.Translation.Y = 0;
            _cam.Depth = 1;
        }
    }
}
