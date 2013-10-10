using Capstone.Core;
using Capstone.Resources;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using IComponent = Capstone.Core.IComponent;
using Texture = Capstone.Graphics.Resources.Texture;

namespace Capstone.Graphics.Sprites
{
    public class Sprite : IComponent
    {
        public Entity Owner { get; set; }
        public Vector2 Origin { get; set; }

        private Texture _tex;

        public Rectangle? SourceRegion { get; set; }

        public Sprite()
        {
            Origin = Vector2.Zero;
            SourceRegion = null;
        }

        public void Load(ResourceCache resources, string textureName)
        {
            _tex = resources.Load<Texture>(textureName);
        }

        public void Initialise()
        {
            SpriteRenderer.Instance.AddSprite(this);
        }

        public void Destroy()
        {
            SpriteRenderer.Instance.RemoveSprite(this);
        }

        internal void Draw(SpriteBatch sb, Vector2 offset)
        {
            if (_tex != null && _tex.IsLoaded)
            {
                var pos3 = Owner.Transform.Translation;
                var pos = new Vector2(pos3.X, pos3.Y) + offset;
                var scale = Owner.Transform.Scale;
                var rot = Owner.Transform.Rotation;
                var tex = _tex.Texture2D.ShaderResourceView[ViewType.Full, 0, 0];
                sb.Draw(tex, pos, SourceRegion, Color.White, rot, Origin, scale, SpriteEffects.None, pos3.Z);
            }
        }
    }
}
