using Capstone.Editor.Views;
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
