using Capstone.Editor.ViewModels;
using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class EditorPage
    {
        private EditorViewModel VM
        {
            get { return (EditorViewModel) DataContext; }
        }

        public EditorPage()
        {
            InitializeComponent();
            DataContext = new EditorViewModel();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel);

            VM.PopulateObjectList();
        }

        private void OpenBuildMode(object sender, RoutedEventArgs e)
        {
        }

        private void OpenLandscapeMode(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeToolSelect(object sender, RoutedEventArgs e)
        {
            VM.Tool = EditorTool.Select;
        }

        private void ChangeToolPan(object sender, RoutedEventArgs e)
        {
            VM.Tool = EditorTool.Pan;
        }

        private void ChangeToolBuild(object sender, RoutedEventArgs e)
        {
            VM.Tool = EditorTool.Build;
        }

        private void swapPanel_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VM.HandleClick(e.GetCurrentPoint(swapPanel).Position);
        }

        private void ToggleEventEditor(object sender, RoutedEventArgs e)
        {
            VM.EventEditorVisible = !VM.EventEditorVisible;
        }
    }
}
