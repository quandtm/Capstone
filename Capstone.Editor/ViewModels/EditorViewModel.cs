using Capstone.Core;
using Capstone.Editor.Common;
using Capstone.Editor.Data;
using Capstone.Engine.Graphics;
using Capstone.Engine.Scripting;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Storage;

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
            Tool = EditorTool.Select;
            // Disable the script manager from running for the editor
            ScriptManager.Instance.IsRunning = false;
            _pointerDown = false;

            SetupCamera();
            RegisterAllObjectives();
        }

        private void RegisterAllObjectives()
        {
            ObjectiveManager.ClearObjectives();
            ObjectiveManager.AddObjective("AddPlayerComponent", "Create an Entity with a PlayerController");
            ObjectiveManager.AddObjective("AddPlayer", "Create a Player Entity");
        }

        private void ResetObjectives()
        {
            ObjectiveManager.ResetObjectives();
            ObjectiveManager.DisplayObjective("AddPlayerComponent");
        }

        private void CheckBuildObjectives()
        {
            foreach (var entityInstance in Instances)
                CheckSingleBuildObjective(entityInstance);
        }

        private void CheckSingleBuildObjective(EntityInstance instance)
        {
            // Check if the instance completes any incomplete instance objectives
            var plrObj = ObjectiveManager.Get("AddPlayer");
            if (plrObj != null && !plrObj.IsComplete)
            {
                if (plrObj.Data == instance.Template)
                    ObjectiveManager.CompleteObjective("AddPlayer");
            }
        }

        private void CheckTemplateObjectives()
        {
            foreach (var entityTemplate in EntityTemplates)
                CheckSingleTemplateObjective(entityTemplate);
        }

        private void CheckSingleTemplateObjective(EntityTemplate template)
        {
            if (template.HasComponent("PlayerController"))
            {
                var playerObj = ObjectiveManager.Get("AddPlayerComponent");
                if (playerObj != null && !playerObj.IsComplete)
                {
                    ObjectiveManager.CompleteObjective("AddPlayerComponent");
                    var o = ObjectiveManager.Get("AddPlayer");
                    if (o != null && !o.IsComplete)
                    {
                        o.Description = string.Format("Build a {0}", template.Name);
                        o.Data = template;
                        ObjectiveManager.DisplayObjective("AddPlayer");
                    }
                }
            }
        }

        // Reset the viewmodel for a new map or new load
        private void Reset()
        {
            foreach (var e in Instances)
                e.Entity.DestroyComponents();
            Instances.Clear();
            ResetObjectives();
        }

        public async Task PopulateTemplates()
        {
            await EntityTemplateCache.Instance.Load();
            CheckTemplateObjectives();
        }

        public void RebuildInstances()
        {
            foreach (var e in Instances)
                e.Rebuild();
        }

        public void SaveLevel(StorageFile file)
        {
            LevelSerializer.Save(file, "The Level", Instances, EntityTemplates, ObjectiveManager);
        }

        public async Task<bool> LoadLevel(StorageFile file)
        {
            Reset();
            var result = await LevelSerializer.LoadForEdit(file, Instances, EntityTemplates, ObjectiveManager);
            if (result)
            {
                CheckTemplateObjectives();
                CheckBuildObjectives();
            }
            return result;
        }

        public void NewLevel()
        {
            Reset();
        }

        public void HandleReleased(Point point)
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
                        CheckSingleBuildObjective(instance);
                    }
                    break;
            }
            _pointerDown = false;
        }

        public void HandlePointerMove(Point point)
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

        public void HandlePointerPressed(Point point)
        {
            switch (Tool)
            {
                case EditorTool.Pan:
                    _prevPoint = point;
                    break;
            }
            _pointerDown = true;
        }

        public void ZoomDelta(float p)
        {
            if (Tool == EditorTool.Pan)
                _cam.Depth += p / 1000f;
        }

        public void ResetCamera()
        {
            _cam.Translation.X = 0;
            _cam.Translation.Y = 0;
            _cam.Depth = 1;
        }

        public void DeleteSelectedEntity()
        {
            var inst = SelectedInstance;
            inst.Entity.DestroyComponents();
            Instances.Remove(inst);
            SelectedInstance = null;
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
    }
}
