using Capstone.Engine.Scripting;

namespace Capstone.Editor.Scripts
{
    public class PointerFollowScript : IScript
    {
        public void Initialise()
        {
            IsInitialised = true;
        }

        public bool IsInitialised { get; private set; }

        public void PointerMoved(float deltaTime, float totalTime, float x, float y)
        {
            Entity.Translation.X = x;
            Entity.Translation.Y = y;
        }

        public void PointerPressed(float deltaTime, float totalTime, float x, float y)
        {
        }

        public void PointerReleased(float deltaTime, float totalTime, float x, float y)
        {
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

        public Core.Entity Entity { get; set; }
    }
}
