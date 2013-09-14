using System;
using System.Collections.Generic;
using Capstone.Graphics;
using SharpDX.Direct3D;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Capstone
{
    sealed partial class App : Application
    {
        private static App _inst;
        public static XamlGraphicsDevice Device { get; private set; }

        private SwapChainBackgroundPanel _swapPanel;
        private readonly Dictionary<Type, Page> _pages;

        private Page _cur;
        private Page Current
        {
            set
            {
                if (_cur == value) return;
                if (_cur != null)
                    _swapPanel.Children.Remove(_cur);
                _cur = value;
                _swapPanel.Children.Add(_cur);
            }
        }

        public App()
        {
            _inst = this;
            InitializeComponent();
            _swapPanel = new SwapChainBackgroundPanel();
            _swapPanel.SizeChanged += OnSwapPanelSizeChanged;
            _swapPanel.Loaded += OnSwapPanelLoaded;

            _pages = new Dictionary<Type, Page>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (Window.Current.Content == null)
            {
                Window.Current.Content = _swapPanel;
                _swapPanel.Children.Add(new MainPage());

                Device = XamlGraphicsDevice.Instance;
                Device.Initialise((int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height, FeatureLevel.Level_11_1, FeatureLevel.Level_11_0);
            }
            Window.Current.Activate();
        }

        public static void NavigateTo<T>() where T : Page, new()
        {
            _inst.Navigate<T>();
        }

        private void Navigate<T>() where T : Page, new()
        {
            Page p;
            if (!_pages.TryGetValue(typeof(T), out p))
            {
                p = new T();
                _pages.Add(typeof(T), p);
            }
            Current = p;
        }

        private void OnSwapPanelSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            if (Device.HasBackgroundPanel)
                Device.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void OnSwapPanelLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!Device.HasBackgroundPanel)
                Device.BackgroundPanel = _swapPanel;
        }
    }
}
