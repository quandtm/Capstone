using Axial.Graphics;
using SharpDX.Direct3D;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;

namespace Capstone
{
    sealed partial class App : Application
    {
        public static XamlGraphicsDevice Device { get; private set; }

        public App()
        {
            InitializeComponent();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (Window.Current.Content == null)
            {
                var main = new MainPage();
                Window.Current.Content = main;

                Device = new XamlGraphicsDevice();
                Device.Initialise((int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height, FeatureLevel.Level_11_1, FeatureLevel.Level_11_0);
            }
            Window.Current.Activate();
        }
    }
}
