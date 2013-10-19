using System;
using System.Collections.Generic;
using Capstone.Core;
using Capstone.Graphics;
using Capstone.Pages;
using SharpDX.Direct3D;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Capstone
{
    sealed partial class App : Application
    {
        private static App _inst;

        private readonly SwapChainBackgroundPanel _swapPanel;
        private readonly Dictionary<Type, Page> _pages;

        private Page _cur;
        private Page CurrentPage
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

        public GameCore Game { get; private set; }

        public App()
        {
            _inst = this;
            InitializeComponent();
            _swapPanel = new SwapChainBackgroundPanel();
            _swapPanel.SizeChanged += OnSwapPanelSizeChanged;
            _swapPanel.Loaded += OnSwapPanelLoaded;

            _pages = new Dictionary<Type, Page>();
            Game = new GameCore();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            if (Window.Current.Content == null)
            {
                Window.Current.Content = _swapPanel;
                var device = XamlGraphicsDevice.Instance;
                device.Initialise((int)Window.Current.Bounds.Width, (int)Window.Current.Bounds.Height, FeatureLevel.Level_11_1, FeatureLevel.Level_11_0);

                //Navigate<EditorPage>();
                Navigate<GamePage>();
            }
            Window.Current.Activate();
        }

        public static void NavigateTo<T>() where T : Page, INavigatable, new()
        {
            _inst.Navigate<T>();
        }

        public static T ChangeScreen<T>() where T : IScreen, new()
        {
            return _inst.Game.ChangeTo<T>();
        }

        private void Navigate<T>() where T : Page, INavigatable, new()
        {
            Page p;
            if (!_pages.TryGetValue(typeof(T), out p))
            {
                p = new T();
                _pages.Add(typeof(T), p);
            }
            if (_cur != null)
                ((INavigatable)_cur).OnNavigatedFrom();
            CurrentPage = p;
            ((INavigatable)p).OnNavigatedTo();
        }

        private void OnSwapPanelSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            var device = XamlGraphicsDevice.Instance;
            if (device.HasBackgroundPanel)
                device.Resize((int)e.NewSize.Width, (int)e.NewSize.Height);
        }

        private void OnSwapPanelLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var device = XamlGraphicsDevice.Instance;
            if (!device.HasBackgroundPanel)
                device.BackgroundPanel = _swapPanel;
        }
    }
}
