using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Capstone.Core;
using Capstone.Resources;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using Windows.Storage;
using IComponent = Capstone.Core.IComponent;
using Texture = Capstone.Graphics.Resources.Texture;

namespace Capstone.Graphics.Sprites
{
    public class TileSprite : IComponent
    {
        private enum TileParserStage
        {
            Texture,
            Tiles,
            Map,
            Dimensions,
            Unknown
        }

        public enum OriginLocation
        {
            TopLeft,
            Center
        }

        public Entity Owner { get; set; }

        // Width and height in tiles
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }
        public int MapWidthPixels { get { return MapWidth * _tileWidth; } }
        public int MapHeightPixels { get { return MapHeight * _tileHeight; } }
        public OriginLocation Origin { get; set; }

        private Texture _tex;
        private int[] _map;
        public int[] Map { get { return _map; } }
        private Rectangle[] _lookup;
        // Individual tile width in pixels
        private int _tileWidth;
        private int _tileHeight;

        public TileSprite()
        {
            Origin = TileSprite.OriginLocation.Center;
        }

        public void Load(ResourceCache resources, string mapFile)
        {
            var mapThread = LoadMap(mapFile).ContinueWith(
                task =>
                {
                    var texPath = task.Result;
                    if (!string.IsNullOrWhiteSpace(texPath))
                        _tex = resources.Load<Texture>(texPath);
                });
        }

        private async Task<string> LoadMap(string mapFile)
        {
            IList<string> lines = null;
            try
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri(mapFile));
                lines = await FileIO.ReadLinesAsync(file);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to load TileMap {0} - {1}", mapFile, ex.Message);
                return string.Empty;
            }

            string texPath = string.Empty;
            var tiles = new List<Rectangle>();
            var map = new List<int>();
            var stage = TileParserStage.Unknown;
            int heightParsed = 0;
            int dimensionsParsed = 0;

            foreach (var line in lines)
            {
                var clean = line.Trim();
                if (clean.StartsWith("#") || string.IsNullOrWhiteSpace(clean))
                    continue; // Comment line, ignore

                var caps = clean.ToUpper();
                switch (caps)
                {
                    case "TEXTURE":
                        stage = TileParserStage.Texture;
                        break;

                    case "TILES":
                        stage = TileParserStage.Tiles;
                        break;

                    case "MAP":
                        stage = TileParserStage.Map;
                        break;

                    case "DIMENSIONS":
                        stage = TileParserStage.Dimensions;
                        break;

                    default:
                        {
                            switch (stage)
                            {
                                case TileParserStage.Map:
                                    {
                                        var parts = clean.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (parts.Length != MapWidth)
                                            continue;
                                        for (int i = 0; i < parts.Length; i++)
                                            map.Add(int.Parse(parts[i]));
                                        heightParsed++;
                                    }
                                    break;

                                case TileParserStage.Dimensions:
                                    {
                                        var parts = clean.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        var x = int.Parse(parts[0]);
                                        var y = int.Parse(parts[1]);
                                        if (dimensionsParsed == 0)
                                        {
                                            MapWidth = x;
                                            MapHeight = y;
                                        }
                                        else if (dimensionsParsed == 1)
                                        {
                                            _tileWidth = x;
                                            _tileHeight = y;
                                            stage = TileParserStage.Unknown;
                                        }
                                        dimensionsParsed++;
                                    }
                                    break;

                                case TileParserStage.Texture:
                                    texPath = clean;
                                    stage = TileParserStage.Unknown;
                                    break;

                                case TileParserStage.Tiles:
                                    {
                                        var parts = clean.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        if (parts.Length != 4)
                                            continue;
                                        var x = int.Parse(parts[0]);
                                        var y = int.Parse(parts[1]);
                                        var w = int.Parse(parts[2]);
                                        var h = int.Parse(parts[3]);
                                        var rect = new Rectangle(x, y, w, h);
                                        tiles.Add(rect);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            _lookup = tiles.ToArray();
            if (map.Count > 0)
                _map = map.ToArray();
            else
            {
                _map = new int[MapWidth * MapHeight];
                for (int i = 0; i < _map.Length; i++)
                    _map[i] = 0;
            }
            return texPath;
        }

        public void Initialise()
        {
            SpriteRenderer.Instance.AddTileMap(this);
        }

        public void Destroy()
        {
            SpriteRenderer.Instance.RemoveTileMap(this);
        }

        internal void Draw(SpriteBatch sb, Vector2 offset, Vector2 viewArea)
        {
            if (_tex != null && _tex.IsLoaded)
            {
                var pos3d = Owner.Transform.Translation;
                var basePos = new Vector2(pos3d.X + offset.X, pos3d.Y + offset.Y);
                if (Origin == OriginLocation.Center)
                    basePos -= (new Vector2(_tileWidth * MapWidth, _tileHeight * MapHeight) / 2.0f);

                int startX = Math.Max((int)(-offset.X / _tileWidth), 0);
                int startY = Math.Max((int)(-offset.Y / _tileHeight), 0);

                var width = Math.Min((int)(viewArea.X / _tileWidth), MapWidth - startX);
                var height = Math.Min((int)(viewArea.Y / _tileHeight) + 1, MapHeight - startY);

                for (int y = startY; y < startY + height; y++)
                {
                    for (int x = startX; x < startX + width; x++)
                    {
                        int index = (y * MapWidth) + x;
                        if (_map[index] >= 0)
                        {
                            var source = _lookup[_map[index]];
                            var pos = basePos + new Vector2(_tileWidth * x, _tileHeight * y);
                            var dest = new Rectangle((int)pos.X, (int)pos.Y, _tileWidth, _tileHeight);
                            var tex = _tex.Texture2D.ShaderResourceView[ViewType.Full, 0, 0];
                            sb.Draw(tex, dest, source, Color.White, 0, Vector2.Zero, SpriteEffects.None, pos3d.Z);
                        }
                    }
                }
            }
        }
    }
}
