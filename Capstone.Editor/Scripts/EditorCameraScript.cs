using Capstone.Core;
using Capstone.Editor.ViewModels;
using Capstone.Engine.Scripting;

namespace Capstone.Editor.Scripts
{
    public class EditorCameraScript : IScript
    {
        private float _prevX, _prevY;
        private bool _isDown;
        public EditorViewModel VM { get; set; }
        public string Name { get; set; }

        public EditorCameraScript(EditorViewModel vm)
        {
            VM = vm;
            _isDown = false;
        }

        public void Initialise()
        {
            IsInitialised = true;
        }

        public void Setup()
        {
            ScriptManager.Instance.RegisterScript(this);
        }

        public void Destroy()
        {
            ScriptManager.Instance.RemoveScript(this);
        }

        public bool IsInitialised { get; private set; }

        public void PointerMoved(float deltaTime, float totalTime, float x, float y)
        {
            if (_isDown && VM.Tool == EditorTool.Pan)
            {
                var dx = x - _prevX;
                var dy = y - _prevY;
                _prevX = x;
                _prevY = y;
                Entity.Translation.X = Entity.Translation.X - dx;
                Entity.Translation.Y = Entity.Translation.Y - dy;
            }
        }

        public void PointerPressed(float deltaTime, float totalTime, float x, float y)
        {
            _isDown = true;
            _prevX = x;
            _prevY = y;
        }

        public void PointerReleased(float deltaTime, float totalTime, float x, float y)
        {
            _isDown = false;
        }

        public void PreDrawUpdate(float deltaTime, float totalTime)
        {
        }

        public void Unload()
        {
        }

        public void Update(float deltaTime, float totalTime)
        {
        }

        public Core.Entity Entity
        {
            get;
            set;
        }
    }
}
