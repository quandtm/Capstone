using Capstone.Engine.Windows;

namespace Capstone.Editor
{
    public sealed partial class MainPage
    {
        private Direct3DPanelProxy _d3d;

        public MainPage()
        {
            InitializeComponent();
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (_d3d == null)
            {
                _d3d = new Direct3DPanelProxy();
                _d3d.Initialise(swapPanel);
            }
        }
    }
}
