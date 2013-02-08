using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class MainPage : IView
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OpenGame(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.Navigate<GamePage>();
        }

        private void NewMap(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.Navigate<EditorPage>().NewLevel();
        }

        private void LoadMap(object sender, RoutedEventArgs e)
        {

        }

        public void HandleNavigationTo(object parameter)
        {
        }

        public async Task HandleNavigationFrom()
        {
        }
    }
}
