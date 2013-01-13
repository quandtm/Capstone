namespace Capstone.Editor.Views
{
    public sealed partial class GamePage
    {
        public GamePage()
        {
            InitializeComponent();
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel);
        }
    }
}
