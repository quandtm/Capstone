using System.Collections.Generic;
using Capstone.Editor.Common;
using Capstone.Editor.Data.ObjectTemplates;
using System.Collections.ObjectModel;
using Capstone.Core;
using Capstone.Engine.Graphics;

namespace Capstone.Editor.ViewModels
{
    public class EditorViewModel : BindableBase
    {
        private Entity _cam;

        public ObservableCollection<BaseObjectTemplate> Objects { get; private set; }
        public BaseObjectTemplate SelectedObject { get; set; }

        public int RemainingSprites { get; private set; }

        private readonly List<Entity> _entities;

        public EditorViewModel()
        {
            _entities = new List<Entity>();

            Objects = new ObservableCollection<BaseObjectTemplate>();

            RemainingSprites = 500;

            SetupCamera();
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
        }
    }
}
