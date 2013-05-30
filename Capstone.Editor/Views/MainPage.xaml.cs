using Capstone.Engine.Windows;
using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class MainPage
    {
        private Direct3DPanelProxy _proxy;

        public MainPage()
        {
            InitializeComponent();
            bgPanel.Loaded += InitBGPanel;
        }

        private void InitBGPanel(object sender, RoutedEventArgs e)
        {
            if (_proxy == null)
            {
                _proxy = new Direct3DPanelProxy();
                _proxy.SetPanel(bgPanel, false);
            }
        }
    }
}
