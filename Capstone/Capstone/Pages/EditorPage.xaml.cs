using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Capstone.Components;
using Capstone.Core;
using Capstone.Graphics.Sprites;
using Capstone.Objectives;
using Capstone.Screens;
using SharpDX;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Capstone.Pages
{
    public sealed partial class EditorPage : INavigatable, INotifyPropertyChanged
    {
        private readonly EditorScreen _screen;
        private PointerPoint _prevPoint;

        public ObservableCollection<Objective> Objectives { get; private set; }

        public List<string> ObjectTypes { get; private set; }

        public EditMode ToolMode { get; set; }
        public ObjectType DeleteMode { get; set; }

        private Graphics.Grid _gameGrid;
        private Core.Entity _selected;
        private Vector3 _offset;

        private TileSprite _roadSprite;
        private bool _pointerDown;

        private List<Entity> _houses = new List<Entity>();

        public EditorPage()
        {
            InitializeComponent();
            _screen = App.ChangeScreen<EditorScreen>();
            _prevPoint = null;

            Objectives = _screen.Objectives.Objectives;

            ObjectTypes = new List<string>();
            BuildObjTypeList();

            _gameGrid = new Graphics.Grid(Vector3.Zero, 32);
            _selected = null;
            _offset = Vector3.Zero;

            DeleteModeCombo.Items.Add(ObjectType.Object);
            DeleteModeCombo.Items.Add(ObjectType.RoadZone);
            DeleteModeCombo.SelectedIndex = 0;

            var roadMapEntity = _screen.CreateEntity("RoadMap", 0, 0, 1);
            _roadSprite = roadMapEntity.AddComponent<TileSprite>();
            _roadSprite.Load(_screen.Cache, "ms-appx:///Data/Road.map");
            _roadSprite.Origin = TileSprite.OriginLocation.TopLeft;

            _pointerDown = false;

            DataContext = this;
        }

        private void BuildObjTypeList()
        {
            ObjectTypes.Add("Tree");
        }

        public void OnNavigatedTo()
        {
        }

        public void OnNavigatedFrom()
        {
        }

        private void HandlePointerDown(object sender, PointerRoutedEventArgs e)
        {
            _prevPoint = e.GetCurrentPoint((UIElement)sender);
            _pointerDown = true;

            switch (ToolMode)
            {
                case EditMode.Object:
                    var selected = ObjTypeList.SelectedItem;
                    if (selected is string)
                    {
                        var pos = _screen.Camera.Owner.Transform.LocalTranslation;
                        pos.Z = 0;
                        pos.X += (float)_prevPoint.Position.X;
                        pos.Y += (float)_prevPoint.Position.Y + 100; // +100 to offset camera position
                        pos = _gameGrid.Snap(pos);
                        _selected = _screen.AddObject(selected as string, null, pos.X, pos.Y, 0, null);
                    }
                    break;

                case EditMode.Move:
                    {
                        var pos = _screen.Camera.Owner.Transform.LocalTranslation;
                        pos.X += (float)_prevPoint.Position.X;
                        pos.Y += (float)_prevPoint.Position.Y + 100; // +100 to offset camera position
                        _selected = ClickDetector.GetClicked(pos.X, pos.Y);
                        if (_selected != null)
                        {
                            _offset = _selected.Transform.Translation - pos;
                            _offset.Z = 0;
                        }
                    }
                    break;

                case EditMode.Road:
                    StampTile((float)_prevPoint.Position.X, (float)_prevPoint.Position.Y, (int)BrushSizeSlider.Value, 1);
                    break;

                case EditMode.Zone:
                    StampTile((float)_prevPoint.Position.X, (float)_prevPoint.Position.Y, (int)ZoneBrushSizeSlider.Value, 2, 0);
                    break;

                case EditMode.Delete:
                    if (DeleteMode == ObjectType.RoadZone)
                        StampTile((float)_prevPoint.Position.X, (float)_prevPoint.Position.Y, (int)DelBrushSizeSlider.Value, 0);
                    break;
            }
        }

        private void HandlePointerUp(object sender, PointerRoutedEventArgs e)
        {
            _pointerDown = false;

            switch (ToolMode)
            {
                case EditMode.Object:
                case EditMode.Move:
                    _selected = null;
                    _offset = Vector3.Zero;
                    break;

                case EditMode.Delete:
                    {
                        if (DeleteMode == ObjectType.Object)
                        {
                            var pos = _screen.Camera.Owner.Transform.LocalTranslation;
                            pos.X += (float)_prevPoint.Position.X;
                            pos.Y += (float)_prevPoint.Position.Y + 100; // +100 to offset camera position
                            var toDelete = ClickDetector.GetClicked(pos.X, pos.Y);
                            if (toDelete != null)
                                _screen.RemoveObject(toDelete);
                        }
                    }
                    break;
            }
            _prevPoint = null;
        }

        private void HandlePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_prevPoint != null)
            {
                var curPt = e.GetCurrentPoint((UIElement)sender);
                switch (ToolMode)
                {
                    case EditMode.Camera:
                        _screen.Camera.Move((float)(_prevPoint.Position.X - curPt.Position.X), (float)(_prevPoint.Position.Y - curPt.Position.Y), 0, 0);
                        break;

                    case EditMode.Object:
                    case EditMode.Move:
                        if (_selected != null)
                        {
                            var pos = _screen.Camera.Owner.Transform.LocalTranslation;
                            pos.Z = _selected.Transform.LocalTranslation.Z;
                            pos.X += (float)_prevPoint.Position.X;
                            pos.Y += (float)_prevPoint.Position.Y + 100; // +100 to offset camera position
                            pos = _gameGrid.Snap(pos + _offset);
                            _selected.Transform.LocalTranslation = pos;
                        }
                        break;

                    case EditMode.Road:
                        StampTile((float)_prevPoint.Position.X, (float)_prevPoint.Position.Y, (int)BrushSizeSlider.Value, 1);
                        break;

                    case EditMode.Zone:
                        StampTile((float)_prevPoint.Position.X, (float)_prevPoint.Position.Y, (int)ZoneBrushSizeSlider.Value, 2, 0);
                        break;

                    case EditMode.Delete:
                        if (DeleteMode == ObjectType.RoadZone)
                            StampTile((float)_prevPoint.Position.X, (float)_prevPoint.Position.Y, (int)DelBrushSizeSlider.Value, 0);
                        break;
                }
                _prevPoint = curPt;
            }
        }

        private void StampTile(float mouseX, float mouseY, int size, int val, int existingValMustBe = -2) // use -2 to stamp anywhere
        {
            var pos = _screen.Camera.Owner.Transform.LocalTranslation;
            pos.Z = 1;
            pos.X += mouseX;
            pos.Y += mouseY + 100; // +100 to offset camera position
            int cx, cy;
            _gameGrid.ToCellCoords(pos, out cx, out cy);
            var halfSize = (int)(((float)size / 2) - 0.5f);
            var xStart = Math.Max(cx - halfSize, 0);
            var yStart = Math.Max(cy - halfSize, 0);

            for (int y = yStart; y < yStart + size; y++)
            {
                for (int x = xStart; x < xStart + size; x++)
                {
                    var index = (y * _roadSprite.MapWidth) + x;
                    if (index >= 0 && index < _roadSprite.Map.Length)
                    {
                        if (existingValMustBe < -1 || existingValMustBe == _roadSprite.Map[index])
                            _roadSprite.Map[index] = val;
                    }
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void ChangeMode(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var mode = btn.Tag as string;
            EditMode m;
            Enum.TryParse<EditMode>(mode, true, out m);
            ToolMode = m;
        }

        private void GenerateBuildings(object sender, RoutedEventArgs e)
        {
            List<Tuple<RectangleF, float>> plots = new List<Tuple<RectangleF, float>>();
            FindPlots(plots);
            FillPlots(plots);
        }

        private void FillPlots(List<Tuple<RectangleF, float>> plots)
        {
            float cell = 32;
            foreach (var plot in plots)
            {
                var pos = plot.Item1.Center * cell;

                var bounds = new RectangleF(plot.Item1.Left * cell + 1, plot.Item1.Top * cell + 1, plot.Item1.Width * cell, plot.Item1.Height * cell);
                if (ClickDetector.FindIntersects(bounds) != null) continue;

                var rot = plot.Item2;
                var p = new Dictionary<string, object>();
                p.Add("Rotation", rot);
                _screen.AddObject("house", null, pos.X, pos.Y, 0.5f, p);
            }
        }

        private void FindPlots(List<Tuple<RectangleF, float>> plots)
        {
            const int plotWidth = 5;
            const int plotHeight = 5;
            int mapWidth = _roadSprite.MapWidth;
            int mapHeight = _roadSprite.MapHeight;
            const int roadValue = 1;
            const int plotValue = 2;
            var map = _roadSprite.Map;

            for (int y = 0; y < mapHeight; y++)
            {
                for (int x = 0; x < mapWidth; x++)
                {
                    int index = (y * mapWidth) + x;
                    if (map[index] == plotValue)
                    {
                        Direction dirToRoad;
                        // Check if this cell is next to a road
                        if (IsRoadAdjacent(map, index, roadValue, out dirToRoad))
                        {
                            int sx, sy, w, h;
                            float rotation = 0;
                            switch (dirToRoad)
                            {
                                case Direction.North:
                                    sx = x - plotWidth + 1;
                                    sy = y;
                                    w = plotWidth;
                                    h = 1;
                                    break;

                                case Direction.South:
                                    sx = x - plotWidth + 1;
                                    sy = y - plotHeight + 1;
                                    w = plotWidth;
                                    h = 1;
                                    rotation = MathUtil.Pi;
                                    break;

                                case Direction.East:
                                    sx = x - plotWidth + 1;
                                    sy = y - plotHeight + 1;
                                    w = 1;
                                    h = plotHeight;
                                    rotation = MathUtil.PiOverTwo;
                                    break;

                                case Direction.West:
                                    sx = x;
                                    sy = y - plotHeight + 1;
                                    w = 1;
                                    h = plotHeight;
                                    rotation = MathUtil.Pi + MathUtil.PiOverTwo;
                                    break;

                                default:
                                    throw new NotImplementedException();
                            }
                            if (sx < 0 || sy < 0 || sx >= mapWidth || sy >= mapHeight) continue;
                            w = Math.Min(sx + w, mapWidth);
                            h = Math.Min(sy + h, mapHeight);
                            // Check that the entire plot is on zoned land
                            bool placed = false;
                            for (int py = sy; py < h; py++)
                            {
                                for (int px = sx; px < w; px++)
                                {
                                    // Check each plot option until one is found or we run out of space to check
                                    if (CheckPlot(px, py, plotWidth, plotHeight, mapWidth, map, plotValue))
                                    {
                                        plots.Add(new Tuple<RectangleF, float>(new RectangleF(sx, sy, plotWidth, plotHeight), rotation));
                                        placed = true;
                                        break;
                                    }
                                }
                                if (placed)
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private bool CheckPlot(int px, int py, int plotWidth, int plotHeight, int mapWidth, int[] map, int testValue)
        {
            for (int ty = 0; ty < plotHeight; ty++)
            {
                for (int tx = 0; tx < plotWidth; tx++)
                {
                    int index = ((ty + py) * mapWidth) + (tx + px);
                    if (index < 0 || index >= map.Length) return false;
                    if (map[index] != testValue)
                        return false;
                }
            }
            return true;
        }

        private bool IsRoadAdjacent(int[] map, int index, int roadValue, out Direction dirToRoad)
        {
            if ((index + 1) < (_roadSprite.MapWidth * _roadSprite.MapHeight) && map[index + 1] == roadValue)
                dirToRoad = Direction.East;
            else if ((index - 1) >= 0 && map[index - 1] == roadValue)
                dirToRoad = Direction.West;
            else if ((index - _roadSprite.MapWidth) >= 0 && map[index - _roadSprite.MapWidth] == roadValue)
                dirToRoad = Direction.North;
            else if ((index + _roadSprite.MapWidth) < (_roadSprite.MapWidth * _roadSprite.MapHeight) && map[index + _roadSprite.MapHeight] == roadValue)
                dirToRoad = Direction.South;
            else
                dirToRoad = Direction.None;
            return dirToRoad != Direction.None;
        }
    }
}
