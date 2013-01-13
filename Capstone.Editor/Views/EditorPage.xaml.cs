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
    }
}
