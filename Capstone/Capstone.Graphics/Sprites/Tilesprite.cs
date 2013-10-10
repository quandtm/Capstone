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
        /*
         * A sprite that is rendered out from a tilemap at runtime.
         * The sprite itself is defined by an array of tiles
         * and then uses a tilemap texture to render out the tiles.
         * The tilesprite will have its own grid, but won't snap to an external grid.
         * This allows for complex "sprites" to be built up from a tilemap and
         * rotated or positioned as needed within the world.
         * Ideally there will be a small form of culling to help reduce overdraw.
         * */

        public Entity Owner { get; set; }
        public Vector2 Origin { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        private Texture _tex;
        private int[] _map;
        private RectangleF[] _lookup;

        public TileSprite()
        {
            Origin = Vector2.Zero;
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
            var tiles = new List<RectangleF>();
            var map = new List<int>();
            var stage = TileParserStage.Unknown;
            int heightParsed = 0;

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
                                        if (parts.Length != TileWidth)
                                            continue;
                                        for (int i = 0; i < parts.Length; i++)
                                            map.Add(int.Parse(parts[1]));
                                        heightParsed++;
                                    }
                                    break;

                                case TileParserStage.Dimensions:
                                    {
                                        var parts = clean.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        var x = int.Parse(parts[0]);
                                        var y = int.Parse(parts[1]);
                                        TileWidth = x;
                                        TileHeight = y;
                                        stage = TileParserStage.Unknown;
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
                                        var rect = new RectangleF(x, y, w, h);
                                        tiles.Add(rect);
                                    }
                                    break;
                            }
                        }
                        break;
                }
            }
            if (heightParsed != TileHeight)
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
                // Todo: draw the tile sprite
            }
        }
    }
}
