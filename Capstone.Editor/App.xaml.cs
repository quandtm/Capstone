using Capstone.Editor.Views;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Capstone.Editor
{
    sealed partial class App
    {
        public static App CurrentApp
        {
            get { return (App)Current; }
        }

        public App()
        {
            InitializeComponent();
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs args)
        {
            Window.Current.Content = new MainPage();
            Window.Current.Activate();
        }
    }
}
