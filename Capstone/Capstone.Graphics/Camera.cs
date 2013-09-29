using Capstone.Core;
using SharpDX;
using IComponent = Capstone.Core.IComponent;

namespace Capstone.Graphics
{
    public class Camera : IComponent
    {
        public Entity Owner { get; set; }

        public void Move(float dx, float dy)
        {
            Owner.Transform.LocalTranslation = Owner.Transform.LocalTranslation + new Vector3(dx, dy, 0);
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
