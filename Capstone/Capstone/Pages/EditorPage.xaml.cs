using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Capstone.Components;
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

        private Graphics.Grid _gameGrid;
        private Core.Entity _selected;
        private Vector3 _offset;

        public EditorPage()
        {
            InitializeComponent();
            _screen = App.ChangeScreen<EditorScreen>();
            _prevPoint = null;

            Objectives = _screen.Objectives.Objectives;

            ObjectTypes = new List<string>();
            BuildObjTypeList();

            _gameGrid = new Graphics.Grid(Vector3.Zero, 32, 1000, 1000);
            _selected = null;
            _offset = Vector3.Zero;

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
                        var p = new Dictionary<string, object>();
                        p.Add("BaseWidth", 320f);
                        p.Add("BaseHeight", 320f);
                        _screen.AddObject(selected as string, null, pos.X, pos.Y, 0, p);
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
            }
        }

        private void HandlePointerUp(object sender, PointerRoutedEventArgs e)
        {
            _prevPoint = null;

            switch (ToolMode)
            {
                case EditMode.Move:
                    _selected = null;
                    _offset = Vector3.Zero;
                    break;
            }
        }

        private void HandlePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_prevPoint != null)
            {
                var curPt = e.GetCurrentPoint((UIElement)sender);
                switch (ToolMode)
                {
                    case EditMode.Camera:
                        _screen.Camera.Move((float)(_prevPoint.Position.X - curPt.Position.X), (float)(_prevPoint.Position.Y - curPt.Position.Y));
                        break;

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
                }
                _prevPoint = curPt;
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
    }
}
