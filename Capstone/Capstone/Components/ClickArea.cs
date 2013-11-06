using Capstone.Core;

namespace Capstone.Components
{
    public class ClickArea : IComponent
    {
        public Entity Owner { get; set; }

        public bool Selectable { get; set; }

        public float BaseWidth { get; set; }
        public float BaseHeight { get; set; }

        public ClickArea()
        {
            Selectable = true;
        }

        public void Initialise()
        {
        }

        public void Destroy()
        {
        }
    }
}
