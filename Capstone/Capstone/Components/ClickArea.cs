using Capstone.Core;

namespace Capstone.Components
{
    public class ClickArea : IComponent
    {
        public Entity Owner { get; set; }

        public float BaseWidth { get; set; }
        public float BaseHeight { get; set; }
        public float OriginX { get; set; }
        public float OriginY { get; set; }

        public Graphics.Sprites.TileSprite TileSprite { get; set; }

        public ClickArea()
        {
            BaseWidth = 0;
            BaseHeight = 0;
        }

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
            var bw = TileSprite != null && TileSprite.MapWidthPixels > 0 ? TileSprite.MapWidthPixels : BaseWidth;
            var bh = TileSprite != null && TileSprite.MapHeightPixels > 0 ? TileSprite.MapHeightPixels : BaseHeight;

            if (bw == 0 || bh == 0) return false;

            var center = Owner.Transform.Translation;
            var minX = center.X - (bw / 2);
            var minY = center.Y - (bh / 2);
            var maxX = center.X + (bw / 2);
            var maxY = center.Y + (bh / 2);
            var xTest = x >= minX && x <= maxX;
            var yTest = y >= minY && y <= maxY;
            return xTest && yTest;
        }

        public bool Intersects(SharpDX.RectangleF rect)
        {
            var testRect = new SharpDX.RectangleF(Owner.Transform.Translation.X - OriginX, Owner.Transform.Translation.Y - OriginY, BaseWidth, BaseHeight);
            bool contains = false;
            rect.Contains(ref testRect, out contains);
            bool intersects = rect.Intersects(testRect);
            return contains || intersects;
        }
    }
}
