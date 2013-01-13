using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class EditorPage
    {
        public EditorPage()
        {
            InitializeComponent();
        }

        private void HandleLoaded(object sender, RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel);
        }

        private void OpenBuildMode(object sender, RoutedEventArgs e)
        {

        }

        private void OpenLandscapeMode(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeToolSelect(object sender, RoutedEventArgs e)
        {

        }

        private void ChangeToolPan(object sender, RoutedEventArgs e)
        {

        }
    }
}
