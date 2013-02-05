using System.Threading.Tasks;
using Capstone.Editor.ViewModels;
using Windows.UI.Xaml;

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

        public void HandleNavigationTo()
        {
            VM.ProcessTemplateObjectives();
        }

        public async Task HandleNavigationFrom()
        {
        }

        private void GameAreaReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_ready)
            {
                VM.HandleReleased(e.GetCurrentPoint(swapPanel).Position);
                App.CurrentApp.Direct3D.ReleasePointer(e, swapPanel);
            }
        }

        private void GameAreaMoved(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_ready)
            {
                VM.HandlePointerMove(e.GetCurrentPoint(swapPanel).Position);
                App.CurrentApp.Direct3D.MovePointer(e, swapPanel);
            }
        }

        private void GameAreaPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            if (_ready)
            {
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
    }
}
