﻿using Capstone.Core;
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
        private EntityInstance _selectedInstance;

        public EntityInstance SelectedInstance
        {
            get { return _selectedInstance; }
            set
            {
                if (_selectedInstance == value) return;

                // Clear selection tint from previous entity by rebuilding it (resets selection tint)
                if (_selectedInstance != null)
                    _selectedInstance.Rebuild();

                SetProperty(ref _selectedInstance, value);

                HighlightEntity(_selectedInstance);

                Tool = EditorTool.Select;
            }
        }

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
                if (_tool != EditorTool.Select)
                    SelectedInstance = null;
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
                else
                    Tool = EditorTool.Select;
            }
        }

        private string _levelName;
        public string LevelName
        {
            get { return _levelName; }
            set
            {
                if (_levelName == value) return;
                SetProperty(ref _levelName, value);
                if (!string.IsNullOrWhiteSpace(value))
                    ObjectiveManager.CompleteObjective("SetLevelName");
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

            RegisterAllObjectives();
        }

        private void RegisterAllObjectives()
        {
            ObjectiveManager.ClearObjectives();
            ObjectiveManager.AddObjective("AddPlayerComponent", "Create a template with a PlayerController");
            ObjectiveManager.AddObjective("AddPlayer", "Build a {0}");
            ObjectiveManager.AddObjective("SetLevelName", "Give the level a name");
            ObjectiveManager.AddObjective("AddMainCam", "Create an Entity with a Camera named \"maincamera\"");
            ObjectiveManager.AddObjective("AddEnemyComponent", "Create a template with an EnemyController");
            ObjectiveManager.AddObjective("AddEnemy", "Build a {0}");
        }

        private void ResetObjectives()
        {
            ObjectiveManager.ResetObjectives();
            ObjectiveManager.DisplayObjective("AddPlayerComponent");
            ObjectiveManager.DisplayObjective("AddEnemyComponent");
            ObjectiveManager.DisplayObjective("AddMainCam");
            ObjectiveManager.DisplayObjective("SetLevelName");
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

            var enemyObj = ObjectiveManager.Get("AddEnemy");
            if (enemyObj != null && !enemyObj.IsComplete)
            {
                if (enemyObj.Data == instance.Template)
                    ObjectiveManager.CompleteObjective("AddEnemy");
            }
        }

        private void CheckTemplateObjectives()
        {
            foreach (var entityTemplate in EntityTemplates)
                CheckSingleTemplateObjective(entityTemplate);
        }

        private void CheckSingleTemplateObjective(EntityTemplate template)
        {
            ComponentTemplate component = null;

            if (template.HasComponent("PlayerController"))
            {
                if (ObjectiveManager.CompleteObjective("AddPlayerComponent"))
                {
                    var o = ObjectiveManager.Get("AddPlayer");
                    if (o != null && !o.IsComplete)
                    {
                        o.Description = string.Format(o.Description, template.Name);
                        o.Data = template;
                        ObjectiveManager.DisplayObjective("AddPlayer");
                    }
                }
            }

            if (template.TryGetComponent("Camera", out component) && component.InstanceName.Equals("maincamera"))
                ObjectiveManager.CompleteObjective("AddMainCam");

            if (template.HasComponent("EnemyController"))
            {
                if (ObjectiveManager.CompleteObjective("AddEnemyComponent"))
                {
                    var o = ObjectiveManager.Get("AddEnemy");
                    if (o != null && !o.IsComplete)
                    {
                        o.Description = string.Format(o.Description, template.Name);
                        o.Data = template;
                        ObjectiveManager.DisplayObjective("AddEnemy");
                    }
                }
            }
        }

        // Reset the viewmodel for a new map or new load
        private void Reset()
        {
            LevelName = string.Empty;
            foreach (var e in Instances)
                e.Entity.DestroyComponents();
            Instances.Clear();
            ResetObjectives();
            CheckTemplateObjectives();
            SetupCamera();
            ResetCamera();
        }

        public void Cleanup()
        {
            foreach (var e in Instances)
                e.Entity.DestroyComponents();
            Instances.Clear();
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

        public async Task<bool> SaveLevel(StorageFile file)
        {
            if (!string.IsNullOrWhiteSpace(LevelName))
            {
                await LevelSerializer.Save(file, LevelName, Instances, EntityTemplates, ObjectiveManager);
                return true;
            }
            return false;
        }

        public async Task<bool> LoadLevel(StorageFile file)
        {
            Reset();
            var levelName = await LevelSerializer.Load(file, Instances, EntityTemplates, ObjectiveManager);
            if (levelName != null)
            {
                CheckTemplateObjectives();
                CheckBuildObjectives();
                LevelName = levelName;
            }
            return levelName != null;
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
                        ((Camera)_cam.GetComponent("edcam")).ScreenToWorld(screen, instance.Entity.Translation);
                        instance.Entity.Name = string.Format("entity_{0:000}", ++_entityCounter);
                        Instances.Add(instance);
                        CheckSingleBuildObjective(instance);
                        ApplyCost(instance);
                    }
                    break;
            }
            _pointerDown = false;
        }

        private void ApplyCost(EntityInstance instance)
        {
            Money = Money - instance.Template.Cost;
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
            SelectedInstance = null;
            inst.Entity.DestroyComponents();
            Instances.Remove(inst);
        }

        private void SetupCamera()
        {
            if (CameraManager.Instance.GetCamera("edcam") == null)
            {
                _cam = new Entity();

                var c = new Camera()
                    {
                        Name = "edcam"
                    };
                _cam.AddComponent(c);
            }
            CameraManager.Instance.MakeActive("edcam");
        }

        private void HighlightEntity(EntityInstance highlight)
        {
            if (highlight != null)
            {
                var texLookup = highlight.Entity.GetComponentFromType(typeof(Texture).FullName);
                if (texLookup != null)
                {
                    var tex = (Texture)texLookup;
                    tex.TintRed = 0.5f;
                    tex.TintGreen = 0.5f;
                    tex.TintBlue = 0.5f;
                    tex.TintAlpha = 0.75f;
                }
            }
        }
    }
}
