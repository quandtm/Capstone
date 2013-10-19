using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Capstone.Core;
using Capstone.Resources;
using SharpDX;
using SharpDX.Toolkit.Graphics;
using Windows.Storage;
using Buffer = SharpDX.Toolkit.Graphics.Buffer;
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

        public Entity Owner { get; set; }

        // Width and height in tiles
        public int MapWidth { get; private set; }
        public int MapHeight { get; private set; }

        private Texture _tex;
        private int[] _map;
        private Rectangle[] _lookup;
        // Individual tile width in pixels
        private int _tileWidth;
        private int _tileHeight;

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
            if (heightParsed != MapHeight)
                return string.Empty; // Error out, map height is wrong
            _lookup = tiles.ToArray();
            _map = map.ToArray();
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

        internal void Draw(SpriteBatch sb, Vector2 offset)
        {
            if (_tex != null && _tex.IsLoaded)
            {
                var pos3d = Owner.Transform.Translation;
                var basePos = new Vector2(pos3d.X + offset.X, pos3d.Y + offset.Y);
                basePos -= (new Vector2(_tileWidth * MapWidth, _tileHeight * MapHeight) / 2.0f);

                int index = 0;
                for (int y = 0; y < MapHeight; y++)
                {
                    for (int x = 0; x < MapWidth; x++)
                    {
                        var source = _lookup[_map[index]];
                        index++;
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
