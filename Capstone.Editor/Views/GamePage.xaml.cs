﻿namespace Capstone.Editor.Views
{
    public sealed partial class GamePage
    {
        public GamePage()
        {
            InitializeComponent();
            KeyUp += GamePage_KeyUp;
        }

        void GamePage_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            // FOR TESTING
            if (e.Key == Windows.System.VirtualKey.Back)
                App.CurrentApp.Navigate<MainPage>();
        }

        private void swapPanel_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var proxy = App.CurrentApp.Direct3D;
            proxy.SetPanel(swapPanel);
        }
    }
}