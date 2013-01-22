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
        private readonly Stack<Page> _backStack;

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
            _backStack = new Stack<Page>();

            this.Suspending += App_Suspending;
        }

        private void App_Suspending(object sender, Windows.ApplicationModel.SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            if (Window.Current.Content != null && Window.Current.Content is IView)
                ((IView)Window.Current.Content).HandleNavigationFrom().Wait();
            deferral.Complete();
        }

        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Navigate<MainPage>();
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
            _backStack.Push((Page)Window.Current.Content);

            if (Window.Current.Content != null && Window.Current.Content is IView)
                ((IView)Window.Current.Content).HandleNavigationFrom();
            Window.Current.Content = p;
            if (p is IView)
                ((IView)p).HandleNavigationTo();
        }

        public void GoBack()
        {
            var back = _backStack.Pop();
            if (back != null)
            {
                if (Window.Current.Content != null && Window.Current.Content is IView)
                    ((IView)Window.Current.Content).HandleNavigationFrom();
                Window.Current.Content = back;
                if (back is IView)
                    ((IView)back).HandleNavigationTo();
            }
        }
    }
}
