using System;
using System.Threading.Tasks;
using Windows.Storage.Pickers;
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

        private async void LoadMap(object sender, RoutedEventArgs e)
        {
            var ofd = new FileOpenPicker();
            ofd.FileTypeFilter.Add(".level");
            ofd.CommitButtonText = "Load";
            ofd.SuggestedStartLocation = PickerLocationId.Desktop;
            var file = await ofd.PickSingleFileAsync();
            if (file != null)
                App.CurrentApp.Navigate<EditorPage>().LoadLevel(file);
        }

        public void HandleNavigationTo(object parameter)
        {
        }

        public async Task HandleNavigationFrom()
        {
        }
    }
}
