using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Editor.Scripts;
using Capstone.Engine.Graphics;
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
        }

        private void RegisterObjectives()
        {
            ObjectiveManager.AddObjective("AddPlayer", "Add a Player Object", 10);
        }

        private void SetupCamera()
        {
            _cam = new Entity();

            var c = new Camera()
                {
                    Name = "camera"
                };
            c.Setup();
            CameraManager.Instance.MakeActive("camera");
            _cam.AddComponent(c);

            var cscript = new EditorCameraScript(this)
                {
                    Name = "controlscript"
                };
            cscript.Setup();
            _cam.AddComponent(cscript);
        }

        public void PopulateObjectList()
        {
            EntityTemplateCache.Instance.Load();
        }

        internal void HandleClick(Windows.Foundation.Point point)
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
                    }
                    break;
            }
        }
    }
}
