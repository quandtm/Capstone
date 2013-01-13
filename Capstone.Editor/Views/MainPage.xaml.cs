using Windows.UI.Xaml;

namespace Capstone.Editor.Views
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void OpenGame(object sender, RoutedEventArgs e)
        {
        }

        private void NewMap(object sender, RoutedEventArgs e)
        {
            App.CurrentApp.Navigate<GamePage>();
        }

        private void LoadMap(object sender, RoutedEventArgs e)
        {

        }
    }
}
