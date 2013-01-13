using Capstone.Editor.Views;
using Capstone.Engine.Windows;
using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Capstone.Editor
{
    sealed partial class App
    {
        private readonly Dictionary<Type, Page> _pages;

        public Direct3DPanelProxy _d3d;
        public Direct3DPanelProxy Direct3D
        {
            get
            {
                if (_d3d == null) _d3d = new Direct3DPanelProxy();
                return _d3d;
            }
        }

        public static App CurrentApp
        {
            get { return (App)Current; }
        }

        public App()
        {
            InitializeComponent();
            _pages = new Dictionary<Type, Page>();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Window.Current.Content = new MainPage();
            Window.Current.Activate();
        }

        public void Navigate<T>() where T : Page, new()
        {
            Page p = null;
            if (!_pages.TryGetValue(typeof(T), out p))
            {
                p = new T();
                _pages.Add(typeof(T), p);
            }
            Window.Current.Content = p;
        }
    }
}
