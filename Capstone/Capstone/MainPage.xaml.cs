using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Capstone
{
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            swapPanel.Loaded += swapPanel_Loaded;
            swapPanel.SizeChanged += swapPanel_SizeChanged;
        }

        void swapPanel_SizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (App.Device.HasBackgroundPanel)
                App.Device.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!App.Device.HasBackgroundPanel)
                App.Device.BackgroundPanel = swapPanel;
        }
    }
}
