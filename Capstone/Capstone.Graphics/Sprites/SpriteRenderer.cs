using SharpDX.Toolkit.Graphics;
using System.Collections.Generic;

namespace Capstone.Graphics.Sprites
{
    public class SpriteRenderer
    {
        private static SpriteRenderer _instance;

        public static SpriteRenderer Instance
        {
            get { return _instance; }
            set { _instance = value; }
        }

        private readonly List<Sprite> _sprites;
        private SpriteBatch _sb;

        public SpriteRenderer()
        {
            _sprites = new List<Sprite>();
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
                    _sprites[i].Draw(_sb);

                _sb.End();
            }
        }
    }
}
