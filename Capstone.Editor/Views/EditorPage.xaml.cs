using System;
using System.Threading.Tasks;
using Capstone.Editor.ViewModels;
using Windows.UI.Input;
using Windows.UI.Xaml;
using Windows.Storage.Pickers;

namespace Capstone.Editor.Views
{
    public sealed partial class EditorPage : IView
    {
        private EditorViewModel VM
        {
            get { return (EditorViewModel)DataContext; }
        }

        private bool _ready;

        public EditorPage()
        {
            _ready = false;
            InitializeComponent();
            DataContext = new EditorViewModel();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel, false);

            VM.PopulateObjectList();
            _ready = true;
        }

        private void OpenBuildMode(object sender, RoutedEventArgs e)
        {
        }

        private void OpenLandscapeMode(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeToolPan(object sender, RoutedEventArgs e)
        {
            VM.Tool = EditorTool.Pan;
        }

        private void ToggleEventEditor(object sender, RoutedEventArgs e)
        {
            VM.EventEditorVisible = !VM.EventEditorVisible;
        }

        private void OpenEntityEditor(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.Navigate<ObjectEditorPage>();
        }

        public void HandleNavigationTo(object parameter)
        {
            if (parameter != null && parameter is Windows.Storage.StorageFile)
            {
                var result = VM.LoadLevel((Windows.Storage.StorageFile)parameter);
            }
            VM.RebuildInstances(); // TODO: Check order in which things load, might need to adjust things to ensure templates are loaded first
            VM.ProcessTemplateObjectives();
        }

        public async Task HandleNavigationFrom()
        {
        }

        private void GameAreaReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_ready)
            {
                var pt = e.GetCurrentPoint(swapPanel);
                if (pt.Properties.PointerUpdateKind == PointerUpdateKind.LeftButtonReleased)
                    VM.HandleReleased(pt.Position);
                App.CurrentApp.Direct3D.ReleasePointer(e, swapPanel);
            }
        }

        private void GameAreaMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_ready)
            {
                var pt = e.GetCurrentPoint(swapPanel);
                if (pt.Properties.IsLeftButtonPressed)
                    VM.HandlePointerMove(e.GetCurrentPoint(swapPanel).Position);
                App.CurrentApp.Direct3D.MovePointer(e, swapPanel);
            }
        }

        private void GameAreaPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_ready)
            {
                var pt = e.GetCurrentPoint(swapPanel);
                if (pt.Properties.IsLeftButtonPressed)
                    VM.HandlePointerPressed(e.GetCurrentPoint(swapPanel).Position);
                App.CurrentApp.Direct3D.PressPointer(e, swapPanel);
            }
        }

        private void DeleteEntity(object sender, RoutedEventArgs e)
        {
            VM.DeleteSelectedEntity();
        }

        private void GameAreaZooming(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VM.ZoomDelta(e.GetCurrentPoint(swapPanel).Properties.MouseWheelDelta);
        }

        private void ResetCam(object sender, RoutedEventArgs e)
        {
            VM.ResetCamera();
        }

        private async void SaveLevel(object sender, RoutedEventArgs e)
        {
            var sfd = new FileSavePicker();
            sfd.DefaultFileExtension = ".level";
            sfd.FileTypeChoices.Add("Level", new[] { ".level" });
            sfd.SuggestedStartLocation = PickerLocationId.Desktop;
            var file = await sfd.PickSaveFileAsync();
            if (file != null)
                VM.SaveLevel(file);
        }
    }
}
