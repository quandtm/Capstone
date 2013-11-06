using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct2D1.Effects;
using SharpDX.Toolkit.Graphics;

namespace Capstone.Graphics.Sprites
{
    public class SpriteRenderer
    {
        public static SpriteRenderer Instance { get; set; }

        private readonly List<Sprite> _sprites;
        private readonly List<TileSprite> _tiles;
        private SpriteBatch _sb;

        public Camera CurrentCamera { get; set; }

        public SpriteRenderer()
        {
            _sprites = new List<Sprite>();
            _tiles = new List<TileSprite>();
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

        internal void AddTileMap(TileSprite tileSprite)
        {
            _tiles.Add(tileSprite);
        }

        internal void RemoveTileMap(TileSprite tileSprite)
        {
            _tiles.Remove(tileSprite);
        }

        public void Draw()
        {
            if (_sprites.Count > 0 || _tiles.Count > 0)
            {
                var offset = CurrentCamera == null ? Vector2.Zero : -CurrentCamera.GetPosition();

                _sb.Begin();

                for (int i = 0; i < _tiles.Count; i++)
                    _tiles[i].Draw(_sb, offset);

                for (var i = 0; i < _sprites.Count; i++)
                    _sprites[i].Draw(_sb, offset);

                _sb.End();
            }
        }
    }
}
