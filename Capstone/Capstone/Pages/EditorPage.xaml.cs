using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Capstone.Objectives;
using Capstone.Screens;
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

        public EditorPage()
        {
            InitializeComponent();
            _screen = App.ChangeScreen<EditorScreen>();
            _prevPoint = null;

            Objectives = _screen.Objectives.Objectives;

            ObjectTypes = new List<string>();
            BuildObjTypeList();

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
                        var p = new Dictionary<string, object>();
                        p.Add("BaseWidth", 320f);
                        p.Add("BaseHeight", 320f);
                        _screen.AddObject(selected as string, null, pos.X, pos.Y, 0, p);
                    }
                    break;
            }
        }

        private void HandlePointerUp(object sender, PointerRoutedEventArgs e)
        {
            _prevPoint = null;
        }

        private void HandlePointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (_prevPoint != null)
            {
                var curPt = e.GetCurrentPoint((UIElement)sender);
                if (ToolMode == EditMode.Camera)
                    _screen.Camera.Move((float)(_prevPoint.Position.X - curPt.Position.X), (float)(_prevPoint.Position.Y - curPt.Position.Y));
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
