using Capstone.Core;
using SharpDX;
using System;
using IComponent = Capstone.Core.IComponent;

namespace Capstone.Graphics
{
    public class Camera : IComponent
    {
        public Entity Owner { get; set; }

        public void Move(float dx, float dy, float minX = float.MinValue, float minY = float.MinValue, float maxX = float.MaxValue, float maxY = float.MaxValue)
        {
            var pos = Owner.Transform.LocalTranslation + new Vector3(dx, dy, 0);
            if (maxX > minX)
                pos.X = Math.Max(Math.Min(pos.X, maxX), minX);
            if (maxY > minY)
                pos.Y = Math.Max(Math.Min(pos.Y, maxY), minY);
            Owner.Transform.LocalTranslation = pos;
        }

        public void SetPosition(float x, float y)
        {
            Owner.Transform.LocalTranslation = new Vector3(x, y, Owner.Transform.LocalTranslation.Z);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(Owner.Transform.Translation.X, Owner.Transform.Translation.Y);
        }

        public void Initialise()
        {
        }

        public void Destroy()
        {
        }
    }
}
