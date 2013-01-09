using Capstone.Engine.Windows;

namespace Capstone.Editor.Views
{
    public sealed partial class GamePage
    {
        private Direct3DPanelProxy _proxy;

        public GamePage()
        {
            InitializeComponent();
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_proxy == null)
            {
                _proxy = new Direct3DPanelProxy();
                _proxy.Initialise(swapPanel);
            }
        }
    }
}
