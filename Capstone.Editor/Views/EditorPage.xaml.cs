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

        private void ChangeToolPan(object sender, RoutedEventArgs e)
        {
            VM.Tool = EditorTool.Pan;
        }

        private void swapPanel_PointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            VM.HandleClick(e.GetCurrentPoint(swapPanel).Position);
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
        }

        public async Task HandleNavigationFrom()
        {
        }
    }
}
