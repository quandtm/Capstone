using System.Threading.Tasks;

namespace Capstone.Editor.Views
{
    public sealed partial class GamePage : IView
    {
        public GamePage()
        {
            InitializeComponent();
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel, true);
        }

        public async Task HandleNavigationTo(object parameter)
        {
        }

        public async Task HandleNavigationFrom()
        {
        }

        public void Load(Windows.Storage.StorageFile file)
        {

        }
    }
}
