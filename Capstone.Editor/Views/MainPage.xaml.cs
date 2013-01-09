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
            App.CurrentApp.Navigate<GamePage>();
        }
    }
}
