using Capstone.Core;

namespace Capstone.Components
{
    public class ClickArea : IComponent
    {
        public Entity Owner { get; set; }


        public float BaseWidth { get; set; }
        public float BaseHeight { get; set; }

        public void Initialise()
        {
            ClickDetector.AddArea(this);
        }

        public void Destroy()
        {
            ClickDetector.RemoveArea(this);
        }

        public bool ContainsPoint(float x, float y)
        {
            var center = Owner.Transform.Translation;
            var minX = center.X - (BaseWidth / 2);
            var minY = center.Y - (BaseHeight / 2);
            var maxX = center.X + (BaseWidth / 2);
            var maxY = center.Y + (BaseWidth / 2);
            var xTest = x >= minX && x <= maxX;
            var yTest = y >= minY && y <= maxY;
            return xTest && yTest;
        }
    }
}
