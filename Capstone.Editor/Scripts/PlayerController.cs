using Capstone.Engine.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Capstone.Editor.Scripts
{
    public class PlayerController : IScript
    {
        public void Initialise()
        {
            IsInitialised = true;
        }

        public bool IsInitialised
        {
            get;
            private set;
        }

        public void PointerMoved(float deltaTime, float totalTime, float x, float y)
        {
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

        public string Name { get; set; }

        public void Setup()
        {
        }
    }
}
