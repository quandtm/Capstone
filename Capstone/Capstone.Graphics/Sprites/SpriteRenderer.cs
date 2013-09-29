using System.Collections.Generic;
using SharpDX;
using SharpDX.Toolkit.Graphics;

namespace Capstone.Graphics.Sprites
{
    public class SpriteRenderer
    {
        public static SpriteRenderer Instance { get; set; }

        private readonly List<Sprite> _sprites;
        private SpriteBatch _sb;

        public Camera CurrentCamera { get; set; }

        public SpriteRenderer()
        {
            _sprites = new List<Sprite>();
            CurrentCamera = null;
        }

        public void Initialise()
        {
            var device = XamlGraphicsDevice.Instance.ToolkitDevice;
            _sb = new SpriteBatch(device);
        }

        internal void AddSprite(Sprite s)
        {
            _sprites.Add(s);
        }

        internal void RemoveSprite(Sprite s)
        {
            _sprites.Remove(s);
        }

        public void Draw()
        {
            if (_sprites.Count > 0)
            {
                _sb.Begin();

                for (var i = 0; i < _sprites.Count; i++)
                    _sprites[i].Draw(_sb, CurrentCamera == null ? Vector2.Zero : -CurrentCamera.GetPosition());

                _sb.End();
            }
        }
    }
}
